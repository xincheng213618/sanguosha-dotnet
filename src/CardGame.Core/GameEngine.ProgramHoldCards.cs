namespace CardGame.Core;

public sealed record ProgramHoldCardsPlacedEvent(
    long FrameId, string SkillId, string BindingId, int OwnerSeat, int HolderSeat,
    string ResultBind, IReadOnlyList<int> CardIds) : IGameEvent;

public sealed record PojunHoldReturnedEvent(int HolderSeat, IReadOnlyList<int> CardIds) : IGameEvent;
