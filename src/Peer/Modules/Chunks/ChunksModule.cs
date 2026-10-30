using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peer.Core;

namespace Peer.Modules.Chunks;

public class ChunksModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<ChunkStore>();
        services.AddHostedService<SeedService>();
        services.AddHostedService<ChunkDownloader>();
    }
}
