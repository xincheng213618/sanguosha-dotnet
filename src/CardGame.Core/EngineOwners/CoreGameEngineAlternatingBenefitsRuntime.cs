#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineAlternatingBenefitsRuntime : global::CardGame.Core.IAlternatingBenefitsProgramHost
{
    private readonly ICoreAlternatingBenefitsCardZonesOperations _cardZonesOperations;
    private readonly ICoreAlternatingBenefitsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreAlternatingBenefitsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreAlternatingBenefitsTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreAlternatingBenefitsCoreActualDiscardRecoveryPhasesOperations _coreActualDiscardRecoveryPhasesOperations;
    private readonly ICoreAlternatingBenefitsCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreAlternatingBenefitsCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreAlternatingBenefitsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreAlternatingBenefitsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreAlternatingBenefitsCoreRedAdditionalTargetsOperations _coreRedAdditionalTargetsOperations;
    private readonly ICoreAlternatingBenefitsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreAlternatingBenefitsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreAlternatingBenefitsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAlternatingBenefitsRuntime(ICoreAlternatingBenefitsCardZonesOperations cardZonesOperations, ICoreAlternatingBenefitsContentRegistryOperations contentRegistryOperations, ICoreAlternatingBenefitsResolutionStackOperations resolutionStackOperations, ICoreAlternatingBenefitsTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreAlternatingBenefitsCoreActualDiscardRecoveryPhasesOperations coreActualDiscardRecoveryPhasesOperations, ICoreAlternatingBenefitsCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreAlternatingBenefitsCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreAlternatingBenefitsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreAlternatingBenefitsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreAlternatingBenefitsCoreRedAdditionalTargetsOperations coreRedAdditionalTargetsOperations, ICoreAlternatingBenefitsCoreRuntimeOperations coreRuntimeOperations, ICoreAlternatingBenefitsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreAlternatingBenefitsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRedAdditionalTargetsOperations = coreRedAdditionalTargetsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
