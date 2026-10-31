#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime))]
internal sealed partial class CoreGameEnginePhaseStartControlPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime> _resolve;
    private CoreGameEnginePhaseStartControlPoliciesEntries(Func<EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseStartControlPoliciesEntries Create(Func<EngineOwners.CoreGameEnginePhaseStartControlPoliciesRuntime> resolve) => new(resolve);
}
