namespace CardGame.Core;

/// <summary>A physical entity occupies a delayed-card slot without becoming that card kind.</summary>
public sealed record JudgmentCardAttachedEvent(long ProducerFrameId, CardConversionSource Source,
    int CardId, CardKind Slot, long ArrivalSequence) : IGameEvent;

internal interface IJudgmentAttachmentQueries : IProgramSelectionQuery
{
    CardKind? Slot(int cardId);
}

internal interface IJudgmentAttachmentOperations : IProgramRuntimeOperations
{
    void Attach(long producerFrameId, CardConversionSource source, CardKind slot,
        CardMovementRecord actualArrival);
}
