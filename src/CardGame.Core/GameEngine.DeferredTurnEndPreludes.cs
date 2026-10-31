namespace CardGame.Core;

public sealed record DeferredTurnEndPreludeChild(long FrameId, CardMovementBatchContext Batch);
public sealed record DeferredTurnEndPrelude(bool Completed = false, DeferredTurnEndPreludeChild? CurrentChild = null);
public sealed record DeferredTurnEndPreludeReturn(long ParentFrameId, int OwnerSeat, int TurnNumber, long BatchId);
