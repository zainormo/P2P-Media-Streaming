using System.Collections.Concurrent;
using Common.Contracts.Peers;

namespace Tracker.Modules.Swarm;

public class SwarmService
{
    readonly ConcurrentDictionary<string, ConcurrentDictionary<string, AnnounceRequest>> swarms = new();

    public void Announce(AnnounceRequest request) =>
        swarms.GetOrAdd(request.MediaId, _ => new())[request.PeerId] = request;

    public IEnumerable<AnnounceRequest> Peers(string mediaId) =>
        swarms.TryGetValue(mediaId, out var swarm) ? swarm.Values : Array.Empty<AnnounceRequest>();
}
