using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peer.Core;

namespace Peer.Modules.Playback;

public class PlaybackModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration config) =>
        services.AddHostedService<StreamServer>();
}
