using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Peer.Core;

public interface IModule
{
    void Register(IServiceCollection services, IConfiguration config);
}
