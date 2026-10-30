using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peer.Core;

namespace Peer.Modules.Tracker;

public class TrackerModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient<TrackerClient>(h => h.BaseAddress = new Uri(config["Tracker:Url"]!));
        services.AddHostedService<HeartbeatService>();
    }
}
