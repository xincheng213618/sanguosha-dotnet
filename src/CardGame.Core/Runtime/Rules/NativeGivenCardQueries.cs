namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeGivenCardQueries(IProgramEventHistoryQueries history,
    IAdvancingRuleFacts advancing, INativeGiveScopeQueries scopes) : INativeGivenCardQueries
{
    // A Before observer can reenter before the issued fact reaches the journal.
    public IReadOnlyList<NativeGiveFact> Read() => Array.AsReadOnly(
        history.Read<NativeCardsGivenEvent>().Concat(advancing.Facts.OfType<NativeCardsGivenEvent>().Reverse())
            .Select(fact => fact.Receipt).OfType<NativeGiveFact>().DistinctBy(fact => fact.Id).ToArray());

    public IReadOnlyList<NativeGiveFact> GiftsForCurrentScope(int giver, int turn, int turnOwner)
    {
        var cutoff = scopes.CurrentCutoff(turn, turnOwner);
        return Array.AsReadOnly(Read().Where(gift => gift.Giver == giver && gift.Turn == turn &&
            gift.TurnOwner == turnOwner && gift.ScopeCutoffAtIssue == cutoff).ToArray());
    }

    public IReadOnlyList<NativeGiveFact> GiftsForBatch(CardMovementBatchContext batch, int giver) =>
        Array.AsReadOnly(Read().Where(gift => gift.Giver == giver && gift.Turn == batch.TurnNumber &&
            gift.ScopeCutoffAtIssue == scopes.CurrentCutoff(gift.Turn, gift.TurnOwner) &&
            batch.Movements.Any(movement => movement.Sequence == gift.CompletionSequence)).ToArray());
}
