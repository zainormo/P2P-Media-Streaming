namespace Common.Contracts.Peers;

public record AnnounceRequest(string PeerId, string Address, string MediaId, int TotalChunks, int[] Chunks);
