namespace Tracker.Core;

public interface IModule
{
    void Register(IServiceCollection services, IConfiguration config);
    void Map(IEndpointRouteBuilder app);
}
