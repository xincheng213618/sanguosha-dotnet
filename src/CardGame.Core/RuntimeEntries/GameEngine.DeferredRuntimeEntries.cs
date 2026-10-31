#nullable enable
namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Creating/binding a proxy stores a typed resolver; it never resolves a module.
    internal TEntry GetDeferredRuntimeEntry<TEntry, TOwner>(Func<Func<TOwner>, TEntry> create)
        where TEntry : class where TOwner : class => MatchRuntime.GetDeferredEntry<TEntry, TOwner>(create);
}
