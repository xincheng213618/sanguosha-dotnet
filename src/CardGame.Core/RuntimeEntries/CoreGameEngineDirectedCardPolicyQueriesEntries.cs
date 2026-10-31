#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDirectedCardPolicyQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _resolve;
    private CoreGameEngineDirectedCardPolicyQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDirectedCardPolicyQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> resolve) => new(resolve);
}
