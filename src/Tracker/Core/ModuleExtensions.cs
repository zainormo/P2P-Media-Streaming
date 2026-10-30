namespace Tracker.Core;

public static class ModuleExtensions
{
    static readonly List<IModule> Modules = typeof(IModule).Assembly.GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IModule).IsAssignableFrom(t))
        .Select(t => (IModule)Activator.CreateInstance(t)!)
        .ToList();

    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration config)
    {
        Modules.ForEach(m => m.Register(services, config));
        return services;
    }

    public static IEndpointRouteBuilder MapModules(this IEndpointRouteBuilder app)
    {
        Modules.ForEach(m => m.Map(app));
        return app;
    }
}
