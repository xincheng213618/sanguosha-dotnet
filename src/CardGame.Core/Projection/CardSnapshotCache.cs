using System.Diagnostics.CodeAnalysis;

namespace CardGame.Core;

internal interface ICardSnapshotCache : IProgramRuntimeOperations
{
    bool TryGet(int cardId, [NotNullWhen(true)] out CardSnapshot? snapshot);
    void Store(int cardId, CardSnapshot snapshot);
}

// Derived projections belong to one match. No suspended rule state or viewer
// access decision is retained in this cache.
internal sealed class CardSnapshotCache : ICardSnapshotCache
{
    private readonly Dictionary<int, CardSnapshot> _snapshots = [];
    public bool TryGet(int cardId, [NotNullWhen(true)] out CardSnapshot? snapshot) =>
        _snapshots.TryGetValue(cardId, out snapshot);
    public void Store(int cardId, CardSnapshot snapshot) => _snapshots[cardId] = snapshot;
}
