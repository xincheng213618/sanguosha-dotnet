namespace CardGame.Core;

// The journal owns both the attachment and its physical lifetime. Moving between
// judgment areas preserves the slot; leaving those areas ends this attachment.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class JudgmentAttachmentRuntime(IProgramEventHistoryQueries history,
    ICardMovementHistoryQueries movements, ICardZoneQueries cards, IProgramRuntimeEvents events)
    : IJudgmentAttachmentQueries, IJudgmentAttachmentOperations
{
    public CardKind? Slot(int cardId)
    {
        if (cardId <= 0 ||
            history.Read<JudgmentCardAttachedEvent>().LastOrDefault(fact => fact.CardId == cardId) is not { } attached ||
            cards.GetLocation(cardId).Zone != CardZoneKind.Judgment)
            return null;
        return movements.Read().Any(move => move.CardId == cardId &&
            move.Sequence > attached.ArrivalSequence && move.To.Zone != CardZoneKind.Judgment)
                ? null : attached.Slot;
    }

    public void Attach(long producerFrameId, CardConversionSource source, CardKind slot,
        CardMovementRecord actualArrival)
    {
        if (producerFrameId <= 0 || slot is not (CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning) ||
            actualArrival.CardId <= 0 || actualArrival.Sequence <= 0 || actualArrival.From == actualArrival.To ||
            actualArrival.To is not { Zone: CardZoneKind.Judgment, OwnerSeat: not null } ||
            movements.Last(move => move.CardId == actualArrival.CardId) != actualArrival ||
            cards.GetLocation(actualArrival.CardId) != actualArrival.To)
            throw new InvalidOperationException("A judgment attachment requires a legal slot and its actual current judgment-area arrival.");

        var previous = history.Read<JudgmentCardAttachedEvent>()
            .LastOrDefault(fact => fact.CardId == actualArrival.CardId);
        if (previous?.ArrivalSequence == actualArrival.Sequence)
        {
            if (previous.ProducerFrameId != producerFrameId || previous.Source != source || previous.Slot != slot)
                throw new InvalidOperationException("An issued judgment attachment cannot change its source or slot.");
            return;
        }
        if (Slot(actualArrival.CardId) is not null)
            throw new InvalidOperationException("An entity cannot replace its active judgment attachment.");

        events.AdvanceFact(new JudgmentCardAttachedEvent(producerFrameId, source,
            actualArrival.CardId, slot, actualArrival.Sequence));
    }
}
