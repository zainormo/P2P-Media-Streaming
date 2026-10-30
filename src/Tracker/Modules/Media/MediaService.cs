using System.Collections.Concurrent;
using Common.Contracts.Media;

namespace Tracker.Modules.Media;

public class MediaService
{
    readonly ConcurrentDictionary<string, MediaItem> items = new();

    public IEnumerable<MediaItem> All() => items.Values;

    public void Add(MediaItem item) => items[item.Id] = item;
}
