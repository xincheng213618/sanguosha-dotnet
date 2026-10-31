#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanduGeneralChoicesRuntime
{
    private readonly RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelMatchRandomAccess _randomOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly ICoreGuanduGeneralChoicesCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly RuntimeEntries.CoreGameEngineGuanduActivitiesEntries _coreGuanduActivitiesOperations;
    private readonly RuntimeEntries.CoreGameEngineGuanduGeneralBenefitsEntries _coreGuanduGeneralBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineGuanduGeneralQualificationEntries _coreGuanduGeneralQualificationOperations;
    private readonly RuntimeEntries.CoreGameEngineGuanduGeneralsEntries _coreGuanduGeneralsOperations;
    private readonly RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries _coreNativeLogicalDiscardsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineGuanduGeneralChoicesRuntime(RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelMatchRandomAccess randomOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations, ICoreGuanduGeneralChoicesCoreEquipmentContentOperations coreEquipmentContentOperations, RuntimeEntries.CoreGameEngineGuanduActivitiesEntries coreGuanduActivitiesOperations, RuntimeEntries.CoreGameEngineGuanduGeneralBenefitsEntries coreGuanduGeneralBenefitsOperations, RuntimeEntries.CoreGameEngineGuanduGeneralQualificationEntries coreGuanduGeneralQualificationOperations, RuntimeEntries.CoreGameEngineGuanduGeneralsEntries coreGuanduGeneralsOperations, RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries coreNativeLogicalDiscardsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreGuanduActivitiesOperations = coreGuanduActivitiesOperations;
        this._coreGuanduGeneralBenefitsOperations = coreGuanduGeneralBenefitsOperations;
        this._coreGuanduGeneralQualificationOperations = coreGuanduGeneralQualificationOperations;
        this._coreGuanduGeneralsOperations = coreGuanduGeneralsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
