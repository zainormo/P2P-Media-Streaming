using Microsoft.AspNetCore.SignalR;

namespace Tracker.Modules.Peers;

public class PeerHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var peerId = Context.GetHttpContext()?.Request.Query["peerId"].ToString();
        if (!string.IsNullOrEmpty(peerId)) await Groups.AddToGroupAsync(Context.ConnectionId, peerId);
        await base.OnConnectedAsync();
    }

    public Task Signal(string toPeerId, string payload) =>
        Clients.Group(toPeerId).SendAsync("signal", Context.ConnectionId, payload);
}
