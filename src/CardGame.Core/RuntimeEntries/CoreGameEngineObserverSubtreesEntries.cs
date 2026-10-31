#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed partial class CoreGameEngineObserverSubtreesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _resolve;
    private CoreGameEngineObserverSubtreesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineObserverSubtreesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> resolve) => new(resolve);
}
