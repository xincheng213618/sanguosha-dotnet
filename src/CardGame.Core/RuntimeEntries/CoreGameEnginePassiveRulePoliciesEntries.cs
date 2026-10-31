#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePassiveRulePoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> _resolve;
    private CoreGameEnginePassiveRulePoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePassiveRulePoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePassiveRulePoliciesRuntime> resolve) => new(resolve);
}
