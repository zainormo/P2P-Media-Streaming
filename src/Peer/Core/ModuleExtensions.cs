using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Peer.Core;

public static class ModuleExtensions
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration config)
    {
        typeof(IModule).Assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IModule).IsAssignableFrom(t))
            .Select(t => (IModule)Activator.CreateInstance(t)!)
            .ToList()
            .ForEach(m => m.Register(services, config));
        return services;
    }
}
