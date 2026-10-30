using Tracker.Core;

namespace Tracker.Modules.Auth;

public class AuthModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration config) => services.AddScoped<AuthService>();
    public void Map(IEndpointRouteBuilder app) => app.MapAuth();
}
