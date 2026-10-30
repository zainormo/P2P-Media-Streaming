using System.Net.Http.Json;
using Common.Contracts.Peers;

namespace Peer.Modules.Tracker;

public class TrackerClient(HttpClient http)
{
    public Task RegisterAsync(PeerInfo peer, CancellationToken ct) =>
        http.PostAsJsonAsync("api/peers", peer, ct);

    public Task AnnounceAsync(AnnounceRequest request, CancellationToken ct) =>
        http.PostAsJsonAsync("api/swarm/announce", request, ct);

    public async Task<List<AnnounceRequest>> SwarmAsync(string mediaId, CancellationToken ct) =>
        await http.GetFromJsonAsync<List<AnnounceRequest>>($"api/swarm/{mediaId}", ct) ?? new();
}
