#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterGaoGanOlRuntime
{
    private readonly ICharacterGaoGanOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterGaoGanOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterGaoGanOlCoreActualDiscardRecoveryPhasesOperations _coreActualDiscardRecoveryPhasesOperations;
    private readonly ICharacterGaoGanOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterGaoGanOlCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterGaoGanOlCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICharacterGaoGanOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterGaoGanOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CharacterGaoGanOlRuntime(ICharacterGaoGanOlContentRegistryOperations contentRegistryOperations, ICharacterGaoGanOlResolutionStackOperations resolutionStackOperations, ICharacterGaoGanOlCoreActualDiscardRecoveryPhasesOperations coreActualDiscardRecoveryPhasesOperations, ICharacterGaoGanOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterGaoGanOlCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterGaoGanOlCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICharacterGaoGanOlCoreRuntimeOperations coreRuntimeOperations, ICharacterGaoGanOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
