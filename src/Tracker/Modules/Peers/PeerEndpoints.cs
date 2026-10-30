using Common.Contracts.Peers;

namespace Tracker.Modules.Peers;

public static class PeerEndpoints
{
    public static void MapPeers(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/peers");

        group.MapPost("/", (PeerInfo peer, PeerRegistry registry) =>
        {
            registry.Upsert(peer);
            return Results.NoContent();
        });

        group.MapGet("/", (PeerRegistry registry) => registry.Active());
    }
}
