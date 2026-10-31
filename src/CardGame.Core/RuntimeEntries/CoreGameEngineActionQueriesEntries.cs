#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed partial class CoreGameEngineActionQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _resolve;
    private CoreGameEngineActionQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActionQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> resolve) => new(resolve);
}
