#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CoreTurnTargetCardQuotaAllowancesCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICoreTurnTargetCardQuotaAllowancesCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
