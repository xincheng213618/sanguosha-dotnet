namespace CardGame.Core;

internal interface ICurrentHandEntryQueries : IProgramSelectionQuery
{
    long? CurrentHandEntry(int owner, int cardId);
}

// Residency is a physical movement fact, independent of whether a rule calls
// the return a new acquisition or remembers an earlier public reveal.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class CurrentHandEntryQueries(ICardZoneQueries cards,
    ICardMovementHistoryQueries history) : ICurrentHandEntryQueries
{
    public long? CurrentHandEntry(int owner, int cardId)
    {
        if (cards.GetLocation(cardId) != CardLocation.Hand(owner))
            return null;
        var movements = history.Read();
        for (var index = movements.Count - 1; index >= 0; index--)
        {
            var movement = movements[index];
            if (movement.CardId == cardId && movement.From != movement.To)
                return movement.To == CardLocation.Hand(owner) ? movement.Sequence : null;
        }
        return 0;
    }
}
