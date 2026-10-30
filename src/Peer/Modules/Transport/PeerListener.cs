using System.Net;
using System.Net.Sockets;
using Common.Protocol;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Peer.Modules.Chunks;

namespace Peer.Modules.Transport;

public class PeerListener(IConfiguration config, ChunkStore store, ILogger<PeerListener> log) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var listener = new TcpListener(IPAddress.Any, config.GetValue<int>("Peer:Port"));
        listener.Start();
        while (!ct.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync(ct);
            _ = Task.Run(() => HandleAsync(client, ct), ct);
        }
    }

    async Task HandleAsync(TcpClient client, CancellationToken ct)
    {
        using (client)
        {
            try
            {
                client.ReceiveTimeout = 5000;
                var stream = client.GetStream();
                using var reader = new BinaryReader(stream);
                using var writer = new BinaryWriter(stream);

                if ((MessageType)reader.ReadByte() != MessageType.ChunkRequest) return;
                var mediaId = reader.ReadString();
                var index = reader.ReadInt32();
                var data = await store.ReadAsync(mediaId, index, ct);

                writer.Write((byte)MessageType.ChunkResponse);
                writer.Write(data?.Length ?? -1);
                if (data is not null) writer.Write(data);
                writer.Flush();
            }
            catch (Exception e) when (e is IOException or SocketException or EndOfStreamException)
            {
                log.LogDebug("Peer connection closed: {Message}", e.Message);
            }
        }
    }
}
