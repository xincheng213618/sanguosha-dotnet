#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterKongRongMouOlRuntime : IMouKongRongFacts, IMouKongRongNative
{
    private readonly RuntimeEntries.CharacterWoLongFengChuOlEntries _characterWoLongFengChuOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries _coreAfterTurnEndedProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries _coreChainedStateBasicConversionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries _coreDamagePreventionProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries _coreDrawFundedDistinctBasicPaymentOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries _corePrepPoisonOutcomesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries _coreShownEntityTurnPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries _coreSkillOwnershipWindowsOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineStrategicProgramsEntries _coreStrategicProgramsOperations;
    private readonly RuntimeEntries.CoreNativeActionDispatchEntries _coreNativeActionDispatchOperations;
    private readonly RuntimeEntries.CoreNativeLegalActionsEntries _coreNativeLegalActionsOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterKongRongMouOlCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICharacterKongRongMouOlMouKongRongOperations _mouKongRongOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterKongRongMouOlRuntime(RuntimeEntries.CharacterWoLongFengChuOlEntries characterWoLongFengChuOlOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries coreAfterTurnEndedProgramsOperations, RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries coreChainedStateBasicConversionsOperations, RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries coreDamagePreventionProgramsOperations, RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicPaymentEntries coreDrawFundedDistinctBasicPaymentOperations, RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries corePrepPoisonOutcomesOperations, RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries coreShownEntityTurnPoliciesOperations, RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries coreSkillOwnershipWindowsOperations, RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, RuntimeEntries.CoreGameEngineStrategicProgramsEntries coreStrategicProgramsOperations, RuntimeEntries.CoreNativeActionDispatchEntries coreNativeActionDispatchOperations, RuntimeEntries.CoreNativeLegalActionsEntries coreNativeLegalActionsOperations, RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, ICharacterKongRongMouOlCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICharacterKongRongMouOlMouKongRongOperations mouKongRongOperations, EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterWoLongFengChuOlOperations = characterWoLongFengChuOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreChainedStateBasicConversionsOperations = coreChainedStateBasicConversionsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._coreDrawFundedDistinctBasicPaymentOperations = coreDrawFundedDistinctBasicPaymentOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePrepPoisonOutcomesOperations = corePrepPoisonOutcomesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShownEntityTurnPoliciesOperations = coreShownEntityTurnPoliciesOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreStrategicProgramsOperations = coreStrategicProgramsOperations;
        this._coreNativeActionDispatchOperations = coreNativeActionDispatchOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._mouKongRongOperations = mouKongRongOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
