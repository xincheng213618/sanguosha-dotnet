#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed partial class CoreGameEngineRuleQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _resolve;
    private CoreGameEngineRuleQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRuleQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> resolve) => new(resolve);
}
