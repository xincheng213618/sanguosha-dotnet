#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime))]
internal sealed partial class CoreGameEngineHealthQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> _resolve;
    private CoreGameEngineHealthQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHealthQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHealthQueriesRuntime> resolve) => new(resolve);
}
