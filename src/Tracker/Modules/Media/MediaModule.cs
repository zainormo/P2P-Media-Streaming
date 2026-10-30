using Tracker.Core;

namespace Tracker.Modules.Media;

public class MediaModule : IModule
{
    public void Register(IServiceCollection services, IConfiguration config) => services.AddSingleton<MediaService>();
    public void Map(IEndpointRouteBuilder app) => app.MapMedia();
}
