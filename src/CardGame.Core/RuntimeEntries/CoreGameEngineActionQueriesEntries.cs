#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActionQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _resolve;
    private CoreGameEngineActionQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActionQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> resolve) => new(resolve);
}
