using Common.Contracts.Peers;
using Common.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Peer.Modules.Chunks;

namespace Peer.Modules.Tracker;

public class HeartbeatService(TrackerClient tracker, ChunkStore store, IConfiguration config, ILogger<HeartbeatService> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var info = new PeerInfo(config["Peer:Id"]!, $"{config["Peer:Host"]}:{config["Peer:Port"]}");
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Constants.HeartbeatSeconds));
        do
        {
            try
            {
                await tracker.RegisterAsync(info, ct);
                foreach (var id in store.MediaIds())
                    await tracker.AnnounceAsync(new AnnounceRequest(info.PeerId, info.Address, id, store.Total(id), store.Available(id)), ct);
            }
            catch (HttpRequestException e)
            {
                log.LogWarning("Tracker unreachable: {Message}", e.Message);
            }
        } while (await timer.WaitForNextTickAsync(ct));
    }
}
