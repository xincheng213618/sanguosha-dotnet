#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed partial class CoreNativeRuleQueriesEntries
{
    private readonly Func<EngineOwners.CoreNativeRuleQueriesRuntime> _resolve;
    private CoreNativeRuleQueriesEntries(Func<EngineOwners.CoreNativeRuleQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeRuleQueriesEntries Create(Func<EngineOwners.CoreNativeRuleQueriesRuntime> resolve) => new(resolve);
}
