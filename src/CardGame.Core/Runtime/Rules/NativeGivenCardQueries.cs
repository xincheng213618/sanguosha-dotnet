namespace CardGame.Core;

internal interface INativeGivenCardQueries : IProgramSelectionQuery
{
    IReadOnlyList<MouHqGift> GiftsForBatch(CardMovementBatchContext batch, int giver);
}

// A cross-owner read of issued Give receipts and their physical movement proof.
// CharacterState remains the receipt owner; the movement and event journals are
// read live, without another ledger or mutation of the original receipts.
internal sealed class NativeGivenCardQueries(IReadOnlyList<CharacterState> players,
    IReadOnlyList<CardMovementRecord> movements, IProgramEventHistoryQueries history) : INativeGivenCardQueries
{
    public IReadOnlyList<MouHqGift> GiftsForBatch(CardMovementBatchContext batch, int giver) =>
        Array.AsReadOnly((players[giver].MouHqActualGifts ?? []).Where(gift =>
            gift.Turn == batch.TurnNumber &&
            batch.Movements.Any(movement => movement.Sequence == gift.CompletionSequence) &&
            gift.Movements.All(movement => movements.Contains(movement)) &&
            history.Read<NativeCardsGivenEvent>().Count(fact => fact.Id == gift.Id &&
                fact.Giver == gift.Giver && fact.Recipient == gift.Recipient &&
                fact.Count == gift.Movements.Count && fact.CompletionSequence == gift.CompletionSequence) == 1).ToArray());
}
