#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhouYuLeRuntime : global::CardGame.Core.ILeZhouYuGuYinHost, global::CardGame.Core.ILeZhouYuPingluHost
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess _randomOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries _coreActualDiscardRecoveryPhasesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeUsedDiscardOriginsEntries _coreNativeUsedDiscardOriginsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntityObserversEntries _coreOriginalHandEntityObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries _corePrivateOfferReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramContinuationsEntries _coreProgramContinuationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries _coreProgramDiscardOriginsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramNativeDrawEntries _coreProgramNativeDrawOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries _coreSkillOwnershipWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly ICharacterZhouYuLeCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunHongOlEntries _characterSunHongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouYuLeEntries _characterZhouYuLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery _resolutionStackOperations2;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries _coreDamageObserverCursorQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries _coreSameNameHandDiscardOrDamageObserversOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    internal CharacterZhouYuLeRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess randomOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries coreActualDiscardRecoveryPhasesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeUsedDiscardOriginsEntries coreNativeUsedDiscardOriginsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntityObserversEntries coreOriginalHandEntityObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries corePrivateOfferReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramContinuationsEntries coreProgramContinuationsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries coreProgramDiscardOriginsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramNativeDrawEntries coreProgramNativeDrawOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries coreSkillOwnershipWindowsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,ICharacterZhouYuLeCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations,global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.RuntimeEntries.CharacterSunHongOlEntries characterSunHongOlOperations,global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations,global::CardGame.Core.RuntimeEntries.CharacterZhouYuLeEntries characterZhouYuLeOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery resolutionStackOperations2,global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries coreDamageObserverCursorQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries coreSameNameHandDiscardOrDamageObserversOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeUsedDiscardOriginsOperations = coreNativeUsedDiscardOriginsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreOriginalHandEntityObserversOperations = coreOriginalHandEntityObserversOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramContinuationsOperations = coreProgramContinuationsOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._characterSunHongOlOperations = characterSunHongOlOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._characterZhouYuLeOperations = characterZhouYuLeOperations;
        this._resolutionStackOperations2 = resolutionStackOperations2;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreSameNameHandDiscardOrDamageObserversOperations = coreSameNameHandDiscardOrDamageObserversOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
    }
}
