namespace CardGame.Core;

internal interface ICardMovementHistoryQueries : IProgramSelectionQuery
{
    IReadOnlyList<CardMovementRecord> Read();
    CardMovementRecord? Last(Func<CardMovementRecord, bool>? predicate = null);
}

// A live read of the one physical movement journal. No second ledger is kept.
internal sealed class CardMovementHistoryQueries(IReadOnlyList<CardMovementRecord> movements) : ICardMovementHistoryQueries
{
    private readonly IReadOnlyList<CardMovementRecord> _view = new MovementHistoryView(movements);
    public IReadOnlyList<CardMovementRecord> Read() => _view;
    public CardMovementRecord? Last(Func<CardMovementRecord, bool>? predicate = null) =>
        predicate is null ? movements.LastOrDefault() : movements.LastOrDefault(predicate);

    private sealed class MovementHistoryView(IReadOnlyList<CardMovementRecord> source) : IReadOnlyList<CardMovementRecord>
    {
        public int Count => source.Count;
        public CardMovementRecord this[int index] => source[index];
        public IEnumerator<CardMovementRecord> GetEnumerator() => source.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
