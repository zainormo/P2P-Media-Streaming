using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Peer.Core;

namespace Peer.Modules.Transport;

public class TransportModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration config) =>
        services.AddHostedService<PeerListener>();
}
