#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeRuleQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _resolve;
    private CoreNativeRuleQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeRuleQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> resolve) => new(resolve);
}
