#nullable enable
namespace CardGame.Core;
// Actual per-match entry identity owner, not gameplay/pending state.
internal sealed class DeferredRuntimeEntryCache
{
    private readonly Dictionary<Type, object> _entries = new();
    internal object GetOrAdd(ProgramRuntimeEntryRegistration registration, Func<Type, object> resolve)
    {
        if (_entries.TryGetValue(registration.Type, out var existing)) return existing;
        var entry = registration.Create(resolve);
        _entries.Add(registration.Type, entry);
        return entry;
    }

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
