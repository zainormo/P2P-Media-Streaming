using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Peer.Modules.Tracker;
using Peer.Modules.Transport;

namespace Peer.Modules.Chunks;

public class ChunkDownloader(TrackerClient tracker, ChunkStore store, IConfiguration config, ILogger<ChunkDownloader> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var ids = config.GetSection("Peer:Download").Get<string[]>() ?? Array.Empty<string>();
        foreach (var id in ids) await DownloadAsync(id, ct);
    }

    async Task DownloadAsync(string mediaId, CancellationToken ct)
    {
        var self = config["Peer:Id"];
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var swarm = (await tracker.SwarmAsync(mediaId, ct)).Where(p => p.PeerId != self).ToList();
                var total = swarm.Select(p => p.TotalChunks).DefaultIfEmpty(0).Max();
                if (total > 0)
                {
                    store.SetTotal(mediaId, total);
                    var missing = Enumerable.Range(0, total).Where(i => !store.Has(mediaId, i)).ToList();
                    if (missing.Count == 0) return;

                    foreach (var index in missing)
                    {
                        foreach (var peer in swarm.Where(p => p.Chunks.Contains(index)))
                        {
                            var data = await TryFetchAsync(peer.Address, mediaId, index, ct);
                            if (data is null) continue;
                            await store.WriteAsync(mediaId, index, data, ct);
                            break;
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                log.LogWarning("Tracker unreachable: {Message}", e.Message);
            }
            await Task.Delay(TimeSpan.FromSeconds(5), ct);
        }
    }

    static async Task<byte[]?> TryFetchAsync(string address, string mediaId, int index, CancellationToken ct)
    {
        try
        {
            return await PeerConnection.FetchChunkAsync(address, mediaId, index, ct);
        }
        catch (Exception e) when (e is IOException or SocketException)
        {
            return null;
        }
    }
}
