#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseStartControlPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime> _resolve;
    private CoreGameEnginePhaseStartControlPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseStartControlPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime> resolve) => new(resolve);
}
