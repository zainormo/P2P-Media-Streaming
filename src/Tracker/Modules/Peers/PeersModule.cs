using Tracker.Core;

namespace Tracker.Modules.Peers;

public class PeersModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<PeerRegistry>();
        services.AddSignalR();
    }

    public void Map(IEndpointRouteBuilder app)
    {
        app.MapPeers();
        app.MapHub<PeerHub>("/hubs/peers");
    }
}
