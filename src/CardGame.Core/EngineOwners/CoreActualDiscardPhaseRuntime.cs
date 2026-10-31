#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreActualDiscardPhaseRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries _characterDuFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuXiOlEntries _characterDuXiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries _characterHuaTuoAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuMengMobileEntries _characterLuMengMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQiaoZhouOlEntries _characterQiaoZhouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries _coreActualDiscardRecoveryPhasesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries _coreNativeLogicalDiscardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries _corePhaseHandSeizureOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeRuleQueriesEntries _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess _coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess;
    internal CoreActualDiscardPhaseRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries characterDuFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDuXiOlEntries characterDuXiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries characterHuaTuoAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterLuMengMobileEntries characterLuMengMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterQiaoZhouOlEntries characterQiaoZhouOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries coreActualDiscardRecoveryPhasesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries coreNativeLogicalDiscardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries corePhaseHandSeizureOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreNativeRuleQueriesEntries coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess)
    {
        this._characterCaoChunOlOperations = characterCaoChunOlOperations;
        this._characterDuFuRenOlOperations = characterDuFuRenOlOperations;
        this._characterDuXiOlOperations = characterDuXiOlOperations;
        this._characterHuaTuoAnniversaryGodOperations = characterHuaTuoAnniversaryGodOperations;
        this._characterLuMengMobileOperations = characterLuMengMobileOperations;
        this._characterLvQianOlOperations = characterLvQianOlOperations;
        this._characterQiaoZhouOlOperations = characterQiaoZhouOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._corePhaseHandSeizureOperations = corePhaseHandSeizureOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess = coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess = coreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess;
        this._coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess = coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
        this._coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess = coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess;
    }
}
