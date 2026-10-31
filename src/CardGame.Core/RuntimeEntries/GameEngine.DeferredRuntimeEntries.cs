#nullable enable
namespace CardGame.Core;
public sealed partial class GameEngine
{
    private readonly DeferredRuntimeEntryCache _deferredRuntimeEntries = new();
    // Creating/binding a proxy stores a typed resolver; it never resolves a module.
    internal TEntry GetDeferredRuntimeEntry<TEntry, TOwner>(global::System.Func<global::System.Func<TOwner>, TEntry> create)
        where TEntry : class where TOwner : class
    {
        if (_deferredRuntimeEntries.TryGet<TEntry>(out var existing))
            return existing;
        return _deferredRuntimeEntries.Add(create(GetRuntimeModule<TOwner>));
    }
}
