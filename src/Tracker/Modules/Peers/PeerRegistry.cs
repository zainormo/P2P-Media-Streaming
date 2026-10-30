using System.Collections.Concurrent;
using Common.Contracts.Peers;

namespace Tracker.Modules.Peers;

public class PeerRegistry
{
    readonly ConcurrentDictionary<string, (PeerInfo Info, DateTime Seen)> peers = new();

    public void Upsert(PeerInfo peer) => peers[peer.PeerId] = (peer, DateTime.UtcNow);

    public IEnumerable<PeerInfo> Active()
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-2);
        return peers.Values.Where(p => p.Seen > cutoff).Select(p => p.Info).ToList();
    }
}
