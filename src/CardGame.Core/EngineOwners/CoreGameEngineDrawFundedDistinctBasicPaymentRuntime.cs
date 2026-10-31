#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicPaymentRuntime
{
    private readonly RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly RuntimeEntries.CharacterKeBiNengOlEntries _characterKeBiNengOlOperations;
    private readonly RuntimeEntries.CharacterKongRongMouOlEntries _characterKongRongMouOlOperations;
    private readonly RuntimeEntries.CharacterLiYiOlEntries _characterLiYiOlOperations;
    private readonly RuntimeEntries.CharacterLuSuMouEntries _characterLuSuMouOperations;
    private readonly RuntimeEntries.CharacterLvKuangLvXiangOlEntries _characterLvKuangLvXiangOlOperations;
    private readonly RuntimeEntries.CharacterMaTieAnniversaryEntries _characterMaTieAnniversaryOperations;
    private readonly RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly RuntimeEntries.CharacterWuJingOlEntries _characterWuJingOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries _coreDrawFundedDistinctBasicOffersOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries _coreDrawFundedDistinctBasicReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries _coreDrawFundedDistinctBasicUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineWenyiPeachSuppliesEntries _coreWenyiPeachSuppliesOperations;
    private readonly RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly RuntimeEntries.CoreNativeDyingEntries _coreNativeDyingOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CoreNativeQinglongEntries _coreNativeQinglongOperations;
    private readonly RuntimeEntries.CoreNativeResponseWindowsEntries _coreNativeResponseWindowsOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly IKernelAdvanceToHumanBoundaryOperations _kernelCoreGameEngineRuntimeOperations;
    private readonly IKernelAcceptAndRejectOperations _kernelCoreMainOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDrawFundedDistinctBasicPaymentRuntime(RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations, RuntimeEntries.CharacterKeBiNengOlEntries characterKeBiNengOlOperations, RuntimeEntries.CharacterKongRongMouOlEntries characterKongRongMouOlOperations, RuntimeEntries.CharacterLiYiOlEntries characterLiYiOlOperations, RuntimeEntries.CharacterLuSuMouEntries characterLuSuMouOperations, RuntimeEntries.CharacterLvKuangLvXiangOlEntries characterLvKuangLvXiangOlOperations, RuntimeEntries.CharacterMaTieAnniversaryEntries characterMaTieAnniversaryOperations, RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations, RuntimeEntries.CharacterWuJingOlEntries characterWuJingOlOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations, RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries coreDrawFundedDistinctBasicOffersOperations, RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries coreDrawFundedDistinctBasicReturnsOperations, RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries coreDrawFundedDistinctBasicUsesOperations, RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineWenyiPeachSuppliesEntries coreWenyiPeachSuppliesOperations, RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, RuntimeEntries.CoreNativeBorrowedSwordEntries coreNativeBorrowedSwordOperations, RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, RuntimeEntries.CoreNativeDyingEntries coreNativeDyingOperations, RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, RuntimeEntries.CoreNativeQinglongEntries coreNativeQinglongOperations, RuntimeEntries.CoreNativeResponseWindowsEntries coreNativeResponseWindowsOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, IKernelAdvanceToHumanBoundaryOperations kernelCoreGameEngineRuntimeOperations, IKernelAcceptAndRejectOperations kernelCoreMainOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess, EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoChunOlOperations = characterCaoChunOlOperations;
        this._characterKeBiNengOlOperations = characterKeBiNengOlOperations;
        this._characterKongRongMouOlOperations = characterKongRongMouOlOperations;
        this._characterLiYiOlOperations = characterLiYiOlOperations;
        this._characterLuSuMouOperations = characterLuSuMouOperations;
        this._characterLvKuangLvXiangOlOperations = characterLvKuangLvXiangOlOperations;
        this._characterMaTieAnniversaryOperations = characterMaTieAnniversaryOperations;
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._characterWuJingOlOperations = characterWuJingOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreDrawFundedDistinctBasicOffersOperations = coreDrawFundedDistinctBasicOffersOperations;
        this._coreDrawFundedDistinctBasicReturnsOperations = coreDrawFundedDistinctBasicReturnsOperations;
        this._coreDrawFundedDistinctBasicUsesOperations = coreDrawFundedDistinctBasicUsesOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreWenyiPeachSuppliesOperations = coreWenyiPeachSuppliesOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeBorrowedSwordOperations = coreNativeBorrowedSwordOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeDyingOperations = coreNativeDyingOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeQinglongOperations = coreNativeQinglongOperations;
        this._coreNativeResponseWindowsOperations = coreNativeResponseWindowsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreGameEngineRuntimeOperations = kernelCoreGameEngineRuntimeOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
