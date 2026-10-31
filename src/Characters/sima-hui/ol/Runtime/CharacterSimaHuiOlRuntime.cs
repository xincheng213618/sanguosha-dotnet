#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterSimaHuiOlRuntime : global::CardGame.Core.ISimaHuiProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries _coreDamagePreventionProgramsOperations;
    private readonly ICharacterSimaHuiOlCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries _coreSameNameHandDiscardOrDamageObserversOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterSimaHuiOlRuntime(global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations,global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations,global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries coreDamagePreventionProgramsOperations,ICharacterSimaHuiOlCoreEquipmentContentOperations coreEquipmentContentOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations,global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries coreSameNameHandDiscardOrDamageObserversOperations,global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterBeiMiHuOlOperations = characterBeiMiHuOlOperations;
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreSameNameHandDiscardOrDamageObserversOperations = coreSameNameHandDiscardOrDamageObserversOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
