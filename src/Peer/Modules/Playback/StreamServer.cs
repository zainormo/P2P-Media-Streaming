using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Peer.Modules.Chunks;

namespace Peer.Modules.Playback;

public class StreamServer(ChunkStore store, IConfiguration config) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{config["Peer:StreamPort"]}/");
        listener.Start();
        ct.Register(listener.Stop);

        while (!ct.IsCancellationRequested)
        {
            HttpListenerContext context;
            try
            {
                context = await listener.GetContextAsync();
            }
            catch (Exception e) when (e is HttpListenerException or ObjectDisposedException)
            {
                break;
            }
            _ = HandleAsync(context, ct);
        }
    }

    async Task HandleAsync(HttpListenerContext context, CancellationToken ct)
    {
        var mediaId = context.Request.Url!.AbsolutePath.Trim('/');
        var total = store.Total(mediaId);
        context.Response.Headers["Access-Control-Allow-Origin"] = "*";

        if (total == 0 || Enumerable.Range(0, total).Any(i => !store.Has(mediaId, i)))
        {
            context.Response.StatusCode = 404;
            context.Response.Close();
            return;
        }

        context.Response.ContentType = "video/mp4";
        for (var i = 0; i < total; i++)
            await context.Response.OutputStream.WriteAsync((await store.ReadAsync(mediaId, i, ct))!, ct);
        context.Response.Close();
    }
}
