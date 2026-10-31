namespace CardGame.Core;

// A carrier represents the continuing equipment/judgment placement of one
// genuine zero-material Use. It never becomes a new reusable physical card.
public sealed record DiouCarrierDestroyedEvent(long OriginalProgramFrameId, CardConversionSource Source,
    string GameplayHash, int CardId, CardKind Kind, long ParentFrameId, int MovementSequence,
    CardLocation From, CardMoveReason Reason) : IGameEvent;
