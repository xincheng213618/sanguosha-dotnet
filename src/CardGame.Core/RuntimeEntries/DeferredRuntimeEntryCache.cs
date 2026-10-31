#nullable enable
namespace CardGame.Core;
// Actual per-match entry identity owner, not gameplay/pending state.
internal sealed class DeferredRuntimeEntryCache
{
    private readonly global::System.Collections.Generic.Dictionary<global::System.Type, object> _entries = new();
    internal bool TryGet<TEntry>(out TEntry entry)
        where TEntry : class
    {
        if (_entries.TryGetValue(typeof(TEntry), out var value))
        {
            entry = (TEntry)value;
            return true;
        }

        entry = default !;
        return false;
    }

    internal TEntry Add<TEntry>(TEntry entry)
        where TEntry : class
    {
        _entries.Add(typeof(TEntry), entry);
        return entry;
    }
}
