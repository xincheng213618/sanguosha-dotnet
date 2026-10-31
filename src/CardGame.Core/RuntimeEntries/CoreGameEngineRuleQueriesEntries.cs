#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed partial class CoreGameEngineRuleQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRuleQueriesRuntime> _resolve;
    private CoreGameEngineRuleQueriesEntries(Func<EngineOwners.CoreGameEngineRuleQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRuleQueriesEntries Create(Func<EngineOwners.CoreGameEngineRuleQueriesRuntime> resolve) => new(resolve);
}
