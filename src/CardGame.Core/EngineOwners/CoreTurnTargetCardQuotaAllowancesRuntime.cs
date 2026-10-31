#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreTurnTargetCardQuotaAllowancesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly ICoreTurnTargetCardQuotaAllowancesCoreProgramTargetRulesOperations _coreProgramTargetRulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    internal CoreTurnTargetCardQuotaAllowancesRuntime(global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, ICoreTurnTargetCardQuotaAllowancesCoreProgramTargetRulesOperations coreProgramTargetRulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess)
    {
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreProgramTargetRulesOperations = coreProgramTargetRulesOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
    }
}
