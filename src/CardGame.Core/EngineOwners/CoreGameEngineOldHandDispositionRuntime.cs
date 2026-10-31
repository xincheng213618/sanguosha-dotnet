#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineOldHandDispositionRuntime : global::CardGame.Core.IOldHandDispositionHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualHandGainProgramsEntries _coreActualHandGainProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries _coreCardDistributionPresentationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFireTargetBenefitsEntries _coreFireTargetBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGainGiftProgramsEntries _coreGainGiftProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries _coreHalfHandPhaseDebtProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries _coreNativeLogicalDiscardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries _coreProgramCardPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess _coreActualTurnProgressPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineOldHandDispositionRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineActualHandGainProgramsEntries coreActualHandGainProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDistributionPresentationEntries coreCardDistributionPresentationOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineFireTargetBenefitsEntries coreFireTargetBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineGainGiftProgramsEntries coreGainGiftProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries coreHalfHandPhaseDebtProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries coreNativeLogicalDiscardsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries coreProgramCardPaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess coreActualTurnProgressPhaseTurnNumberAccess,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess,global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualHandGainProgramsOperations = coreActualHandGainProgramsOperations;
        this._coreCardDistributionPresentationOperations = coreCardDistributionPresentationOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreFireTargetBenefitsOperations = coreFireTargetBenefitsOperations;
        this._coreGainGiftProgramsOperations = coreGainGiftProgramsOperations;
        this._coreHalfHandPhaseDebtProgramsOperations = coreHalfHandPhaseDebtProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressPhaseTurnNumberAccess = coreActualTurnProgressPhaseTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess = coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
