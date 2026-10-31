#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineObserverSubtreesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _resolve;
    private CoreGameEngineObserverSubtreesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineObserverSubtreesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> resolve) => new(resolve);
}
