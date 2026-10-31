namespace CardGame.Core;

public sealed record BoundaryPangDeDiscardPaidEvent(long FrameId, string Instance, string Hash, long UseId, long ActionId,
    int Owner, int Target, int CardId, CardKind Kind, CardLocation From, CardLocation To, long MovementSequence) : IGameEvent;
