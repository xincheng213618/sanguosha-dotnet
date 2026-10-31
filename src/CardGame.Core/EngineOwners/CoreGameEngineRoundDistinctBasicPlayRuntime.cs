#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRoundDistinctBasicPlayRuntime
{
    private readonly RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly RuntimeEntries.CharacterHuangGaiMouMobileEntries _characterHuangGaiMouMobileOperations;
    private readonly RuntimeEntries.CharacterHuangQuanOlEntries _characterHuangQuanOlOperations;
    private readonly RuntimeEntries.CharacterKeBiNengOlEntries _characterKeBiNengOlOperations;
    private readonly RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedPoliciesEntries _coreAdvancedPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineHandComparisonEntries _coreHandComparisonOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundDistinctBasicUseEntries _coreRoundDistinctBasicUseOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundGainedSourceUsesEntries _coreRoundGainedSourceUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries _coreTurnRedSlashBenefitsOperations;
    private readonly RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CoreNativeRecoveryCardsEntries _coreNativeRecoveryCardsOperations;
    private readonly RuntimeEntries.CoreNativeSlashUseEntries _coreNativeSlashUseOperations;
    private readonly RuntimeEntries.CoreNativeTargetLegalityEntries _coreNativeTargetLegalityOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly RuntimeEntries.CoreTurnTargetCardQuotaAllowancesEntries _coreTurnTargetCardQuotaAllowancesOperations;
    private readonly IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly IKernelAdvanceToHumanBoundaryOperations _kernelCoreGameEngineRuntimeOperations;
    private readonly IKernelAcceptAndRejectOperations _kernelCoreMainOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRoundDistinctBasicPlayRuntime(RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, RuntimeEntries.CharacterHuangGaiMouMobileEntries characterHuangGaiMouMobileOperations, RuntimeEntries.CharacterHuangQuanOlEntries characterHuangQuanOlOperations, RuntimeEntries.CharacterKeBiNengOlEntries characterKeBiNengOlOperations, RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, RuntimeEntries.CoreGameEngineAdvancedPoliciesEntries coreAdvancedPoliciesOperations, RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations, RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations, RuntimeEntries.CoreGameEngineHandComparisonEntries coreHandComparisonOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineRoundDistinctBasicUseEntries coreRoundDistinctBasicUseOperations, RuntimeEntries.CoreGameEngineRoundGainedSourceUsesEntries coreRoundGainedSourceUsesOperations, RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries coreTurnRedSlashBenefitsOperations, RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations, RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations, RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, RuntimeEntries.CoreNativeRecoveryCardsEntries coreNativeRecoveryCardsOperations, RuntimeEntries.CoreNativeSlashUseEntries coreNativeSlashUseOperations, RuntimeEntries.CoreNativeTargetLegalityEntries coreNativeTargetLegalityOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, RuntimeEntries.CoreTurnTargetCardQuotaAllowancesEntries coreTurnTargetCardQuotaAllowancesOperations, IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, IKernelAdvanceToHumanBoundaryOperations kernelCoreGameEngineRuntimeOperations, IKernelAcceptAndRejectOperations kernelCoreMainOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterHuangGaiMouMobileOperations = characterHuangGaiMouMobileOperations;
        this._characterHuangQuanOlOperations = characterHuangQuanOlOperations;
        this._characterKeBiNengOlOperations = characterKeBiNengOlOperations;
        this._characterZhugeLiangMouOperations = characterZhugeLiangMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAdvancedPoliciesOperations = coreAdvancedPoliciesOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreHandComparisonOperations = coreHandComparisonOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRoundDistinctBasicUseOperations = coreRoundDistinctBasicUseOperations;
        this._coreRoundGainedSourceUsesOperations = coreRoundGainedSourceUsesOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreTurnRedSlashBenefitsOperations = coreTurnRedSlashBenefitsOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeRecoveryCardsOperations = coreNativeRecoveryCardsOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreTurnTargetCardQuotaAllowancesOperations = coreTurnTargetCardQuotaAllowancesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreGameEngineRuntimeOperations = kernelCoreGameEngineRuntimeOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
