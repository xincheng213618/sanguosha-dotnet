namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Constructor draft data is deterministic from the seed. Queue its public
    // facts only after Start, when the command session can retain them.
    private sealed record ShenwuDraftSetup(IReadOnlyList<string> SharedGods, string LeaderId);
    private ShenwuDraftSetup? _shenwuDraft;
    private int ShenwuSeat(int offset) => (Math.Max(0, _options.HumanSeat) + offset) % _playerCount;
    private int ShenwuSeatOffset(int seat) => (seat - Math.Max(0, _options.HumanSeat) + _playerCount) % _playerCount;
    private IReadOnlyList<int> ShenwuPlayerSlots => _modeDefinition.ShenwuActivity!.PlayerSeatOffsets.Select(ShenwuSeat).ToArray();
    private int ShenwuLeaderSeat => ShenwuSeat(_modeDefinition.ShenwuActivity!.LeaderSeatOffset);
    private bool IsShenwuPlayerSlot(int seat) => _modeDefinition.ShenwuActivity is { } activity &&
        activity.PlayerSeatOffsets.Contains(ShenwuSeatOffset(seat));

}
