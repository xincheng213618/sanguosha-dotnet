#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePlayCardHistoryRuntime
{
    private readonly RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly RuntimeEntries.CharacterHuangfuSongWumiaoEntries _characterHuangfuSongWumiaoOperations;
    private readonly RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly RuntimeEntries.CharacterSunQuanAnniversaryGodEntries _characterSunQuanAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterWeiYanMobileShiEntries _characterWeiYanMobileShiOperations;
    private readonly RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineActivityOrdinaryTricksEntries _coreActivityOrdinaryTricksOperations;
    private readonly RuntimeEntries.CoreGameEngineActualTurnViewAsEntries _coreActualTurnViewAsOperations;
    private readonly RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries _coreBoundDiscardSlashBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly RuntimeEntries.CoreGameEngineNamedUseConversionsEntries _coreNamedUseConversionsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardUseRulesEntries _coreProgramCardUseRulesOperations;
    private readonly ICorePlayCardHistoryCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundGainedSourceUsesEntries _coreRoundGainedSourceUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    private readonly RuntimeEntries.CoreGuanduEventProgramsEntries _coreGuanduEventProgramsOperations;
    private readonly RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly RuntimeEntries.CoreNativeMultiTargetActionsEntries _coreNativeMultiTargetActionsOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreProgramTieredRoundConversionEntries _coreProgramTieredRoundConversionOperations;
    private readonly RuntimeEntries.CoreTurnTargetCardQuotaAllowancesEntries _coreTurnTargetCardQuotaAllowancesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess _coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    internal CoreGameEnginePlayCardHistoryRuntime(RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, RuntimeEntries.CharacterHuangfuSongWumiaoEntries characterHuangfuSongWumiaoOperations, RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations, RuntimeEntries.CharacterSunQuanAnniversaryGodEntries characterSunQuanAnniversaryGodOperations, RuntimeEntries.CharacterWeiYanMobileShiEntries characterWeiYanMobileShiOperations, RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, RuntimeEntries.CoreGameEngineActivityOrdinaryTricksEntries coreActivityOrdinaryTricksOperations, RuntimeEntries.CoreGameEngineActualTurnViewAsEntries coreActualTurnViewAsOperations, RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries coreBoundDiscardSlashBenefitsOperations, RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, RuntimeEntries.CoreGameEngineNamedUseConversionsEntries coreNamedUseConversionsOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineProgramCardUseRulesEntries coreProgramCardUseRulesOperations, ICorePlayCardHistoryCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, RuntimeEntries.CoreGameEngineRoundGainedSourceUsesEntries coreRoundGainedSourceUsesOperations, RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations, RuntimeEntries.CoreGuanduEventProgramsEntries coreGuanduEventProgramsOperations, RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, RuntimeEntries.CoreNativeMultiTargetActionsEntries coreNativeMultiTargetActionsOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CoreProgramTieredRoundConversionEntries coreProgramTieredRoundConversionOperations, RuntimeEntries.CoreTurnTargetCardQuotaAllowancesEntries coreTurnTargetCardQuotaAllowancesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess coreGameEngineSkillProgramsProgramPhaseUsesAccess)
    {
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterHuangfuSongWumiaoOperations = characterHuangfuSongWumiaoOperations;
        this._characterMiHengLeOperations = characterMiHengLeOperations;
        this._characterSunQuanAnniversaryGodOperations = characterSunQuanAnniversaryGodOperations;
        this._characterWeiYanMobileShiOperations = characterWeiYanMobileShiOperations;
        this._characterZhangFeiAnniversaryGodOperations = characterZhangFeiAnniversaryGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreActivityOrdinaryTricksOperations = coreActivityOrdinaryTricksOperations;
        this._coreActualTurnViewAsOperations = coreActualTurnViewAsOperations;
        this._coreBoundDiscardSlashBenefitsOperations = coreBoundDiscardSlashBenefitsOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreNamedUseConversionsOperations = coreNamedUseConversionsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramCardUseRulesOperations = coreProgramCardUseRulesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRoundGainedSourceUsesOperations = coreRoundGainedSourceUsesOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreGuanduEventProgramsOperations = coreGuanduEventProgramsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeMultiTargetActionsOperations = coreNativeMultiTargetActionsOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramTieredRoundConversionOperations = coreProgramTieredRoundConversionOperations;
        this._coreTurnTargetCardQuotaAllowancesOperations = coreTurnTargetCardQuotaAllowancesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineSkillProgramsProgramPhaseUsesAccess = coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    }
}
