using Common.Contracts.Peers;

namespace Tracker.Modules.Swarm;

public static class SwarmEndpoints
{
    public static void MapSwarm(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/swarm");

        group.MapPost("/announce", (AnnounceRequest request, SwarmService service) =>
        {
            service.Announce(request);
            return Results.NoContent();
        });

        group.MapGet("/{mediaId}", (string mediaId, SwarmService service) => service.Peers(mediaId));
    }
}
