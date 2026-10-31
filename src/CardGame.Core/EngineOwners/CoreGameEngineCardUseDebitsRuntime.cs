#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardUseDebitsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries _coreBoundDiscardSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries _corePlayPhaseSkillGrantsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly ICoreCardUseDebitsCoreProgramCardUsePaymentsOperations _coreProgramCardUsePaymentsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess _coreCurrentTurnStatisticsSlashCountThisTurnAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess _coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess;
    internal CoreGameEngineCardUseDebitsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries coreBoundDiscardSlashBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries corePlayPhaseSkillGrantsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, ICoreCardUseDebitsCoreProgramCardUsePaymentsOperations coreProgramCardUsePaymentsOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess coreCurrentTurnStatisticsSlashCountThisTurnAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess)
    {
        this._characterCaoChunOlOperations = characterCaoChunOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreBoundDiscardSlashBenefitsOperations = coreBoundDiscardSlashBenefitsOperations;
        this._corePlayPhaseSkillGrantsOperations = corePlayPhaseSkillGrantsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreProgramCardUsePaymentsOperations = coreProgramCardUsePaymentsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreCurrentTurnStatisticsSlashCountThisTurnAccess = coreCurrentTurnStatisticsSlashCountThisTurnAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess = coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess;
    }
}
