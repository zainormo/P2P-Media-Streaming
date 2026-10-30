using Tracker.Core;

namespace Tracker.Modules.Swarm;

public class SwarmModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration config) => services.AddSingleton<SwarmService>();
    public void Map(IEndpointRouteBuilder app) => app.MapSwarm();
}
