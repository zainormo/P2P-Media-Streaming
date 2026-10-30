using Common.Contracts.Auth;

namespace Tracker.Modules.Auth;

public static class AuthEndpoints
{
    public static void MapAuth(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", async (RegisterRequest request, AuthService service) =>
            await service.RegisterAsync(request) ? Results.Ok() : Results.Conflict());

        group.MapPost("/login", async (LoginRequest request, AuthService service) =>
        {
            var response = await service.LoginAsync(request);
            return response is null ? Results.Unauthorized() : Results.Ok(response);
        });
    }
}
