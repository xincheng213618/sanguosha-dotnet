namespace CardGame.Core;

internal interface ICurrentHandEntryQueries : IProgramSelectionQuery
{
    long? CurrentHandEntry(int owner, int cardId);
}

internal interface ICardZoneEntryQueries : IProgramSelectionQuery
{
    long? CurrentEntry(CardLocation location, int cardId);
}

// Residency is a physical movement fact, independent of whether a rule calls
// the return a new acquisition or remembers an earlier public reveal.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class CurrentHandEntryQueries(ICardZoneQueries cards,
    ICardMovementHistoryQueries history) : ICurrentHandEntryQueries, ICardZoneEntryQueries
{
    public long? CurrentHandEntry(int owner, int cardId) => CurrentEntry(CardLocation.Hand(owner), cardId);

    public long? CurrentEntry(CardLocation location, int cardId)
    {
        if (cards.GetLocation(cardId) != location)
            return null;
        var movements = history.Read();
        for (var index = movements.Count - 1; index >= 0; index--)
        {
            var movement = movements[index];
            if (movement.CardId == cardId && movement.From != movement.To)
                return movement.To == location ? movement.Sequence : null;
        }
        return 0;
    }
}
