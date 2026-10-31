#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreTurnTargetCardQuotaAllowancesRuntime
{
    private readonly IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly ICoreTurnTargetCardQuotaAllowancesCoreProgramTargetRulesOperations _coreProgramTargetRulesOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    internal CoreTurnTargetCardQuotaAllowancesRuntime(IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, ICoreTurnTargetCardQuotaAllowancesCoreProgramTargetRulesOperations coreProgramTargetRulesOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess)
    {
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreProgramTargetRulesOperations = coreProgramTargetRulesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
    }
}
