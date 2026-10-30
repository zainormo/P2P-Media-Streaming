using Common.Protocol;
using Microsoft.Extensions.Configuration;

namespace Peer.Modules.Chunks;

public class ChunkStore(IConfiguration config)
{
    readonly string root = Path.GetFullPath(config["Peer:DataDir"] ?? "data");

    string Dir(string mediaId) => Path.Combine(root, Path.GetFileName(mediaId));

    string ChunkPath(string mediaId, int index) => Path.Combine(Dir(mediaId), $"{index}.chunk");

    public IEnumerable<string> MediaIds() =>
        Directory.Exists(root) ? Directory.GetDirectories(root).Select(d => Path.GetFileName(d)) : Array.Empty<string>();

    public int Total(string mediaId)
    {
        var file = Path.Combine(Dir(mediaId), "total");
        return File.Exists(file) ? int.Parse(File.ReadAllText(file)) : 0;
    }

    public void SetTotal(string mediaId, int total)
    {
        Directory.CreateDirectory(Dir(mediaId));
        File.WriteAllText(Path.Combine(Dir(mediaId), "total"), total.ToString());
    }

    public bool Has(string mediaId, int index) => index >= 0 && File.Exists(ChunkPath(mediaId, index));

    public int[] Available(string mediaId) =>
        Directory.Exists(Dir(mediaId))
            ? Directory.GetFiles(Dir(mediaId), "*.chunk").Select(f => int.Parse(Path.GetFileNameWithoutExtension(f))).Order().ToArray()
            : Array.Empty<int>();

    public async Task<byte[]?> ReadAsync(string mediaId, int index, CancellationToken ct) =>
        Has(mediaId, index) ? await File.ReadAllBytesAsync(ChunkPath(mediaId, index), ct) : null;

    public async Task WriteAsync(string mediaId, int index, byte[] data, CancellationToken ct)
    {
        Directory.CreateDirectory(Dir(mediaId));
        await File.WriteAllBytesAsync(ChunkPath(mediaId, index), data, ct);
    }

    public async Task ImportAsync(string mediaId, string file, CancellationToken ct)
    {
        await using var stream = File.OpenRead(file);
        var buffer = new byte[Constants.ChunkSize];
        var index = 0;
        int read;
        while ((read = await stream.ReadAtLeastAsync(buffer, buffer.Length, false, ct)) > 0)
            await WriteAsync(mediaId, index++, buffer[..read], ct);
        SetTotal(mediaId, index);
    }
}
