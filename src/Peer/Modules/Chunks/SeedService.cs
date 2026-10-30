using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Peer.Modules.Chunks;

public class SeedService(ChunkStore store, IConfiguration config) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        foreach (var seed in config.GetSection("Peer:Seed").GetChildren())
            await store.ImportAsync(seed["Id"]!, seed["File"]!, ct);
    }
}
