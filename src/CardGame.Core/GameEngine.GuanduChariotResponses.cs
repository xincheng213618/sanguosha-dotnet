namespace CardGame.Core;

/// <summary>A real response payment destroyed its equipped entity in the same movement.</summary>
public sealed record GuanduChariotResponsePaidEvent(long ActionId, long ParentFrameId, int CardId,
    int MovementSequence, long BatchId, ProgramCardContinuation Continuation) : IGameEvent;
