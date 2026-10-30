using Common.Contracts.Media;

namespace Tracker.Modules.Media;

public static class MediaEndpoints
{
    public static void MapMedia(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/media").RequireAuthorization();

        group.MapGet("/", (MediaService service) => service.All());

        group.MapPost("/", (MediaItem item, MediaService service) =>
        {
            service.Add(item);
            return Results.Created($"/api/media/{item.Id}", item);
        });
    }
}
