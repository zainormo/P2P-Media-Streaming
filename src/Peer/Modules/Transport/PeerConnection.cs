using System.Net.Sockets;
using System.Text;
using Common.Protocol;

namespace Peer.Modules.Transport;

public static class PeerConnection
{
    public static async Task<byte[]?> FetchChunkAsync(string address, string mediaId, int index, CancellationToken ct)
    {
        var parts = address.Split(':');
        using var client = new TcpClient();
        await client.ConnectAsync(parts[0], int.Parse(parts[1]), ct);

        var stream = client.GetStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);

        writer.Write((byte)MessageType.ChunkRequest);
        writer.Write(mediaId);
        writer.Write(index);
        writer.Flush();

        if ((MessageType)reader.ReadByte() != MessageType.ChunkResponse) return null;
        var length = reader.ReadInt32();
        return length is < 0 or > Constants.ChunkSize ? null : reader.ReadBytes(length);
    }
}
