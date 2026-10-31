#nullable enable
namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Creating/binding a proxy stores a typed resolver; it never resolves a module.
    internal TEntry GetDeferredRuntimeEntry<TEntry, TOwner>(global::System.Func<global::System.Func<TOwner>, TEntry> create)
        where TEntry : class where TOwner : class => MatchRuntime.GetDeferredEntry<TEntry, TOwner>(create);
}
