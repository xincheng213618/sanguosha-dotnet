#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterFeiYiOlRuntime : global::CardGame.Core.IFeiYiHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnDamageClaimsEntries _coreActualTurnDamageClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries _coreNamedAmbushInvalidationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries _coreNativeLogicalDiscardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries _coreNextCardTargetAdjustmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramContinuationsEntries _coreProgramContinuationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries _coreSnapshotValueProjectionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries _characterFeiYiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery _resolutionStackOperations2;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CharacterFeiYiOlRuntime(global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnDamageClaimsEntries coreActualTurnDamageClaimsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries coreNamedAmbushInvalidationOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries coreNativeLogicalDiscardsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries coreNextCardTargetAdjustmentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramContinuationsEntries coreProgramContinuationsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreSnapshotValueProjectionEntries coreSnapshotValueProjectionOperations,global::CardGame.Core.RuntimeEntries.CharacterFeiYiOlEntries characterFeiYiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery resolutionStackOperations2,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreActualTurnDamageClaimsOperations = coreActualTurnDamageClaimsOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreNamedAmbushInvalidationOperations = coreNamedAmbushInvalidationOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreNextCardTargetAdjustmentsOperations = coreNextCardTargetAdjustmentsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreSnapshotValueProjectionOperations = coreSnapshotValueProjectionOperations;
        this._characterFeiYiOlOperations = characterFeiYiOlOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._resolutionStackOperations2 = resolutionStackOperations2;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
