#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePlayCardHistoryRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries _characterHuangfuSongWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunQuanAnniversaryGodEntries _characterSunQuanAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries _characterWeiYanMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityOrdinaryTricksEntries _coreActivityOrdinaryTricksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnViewAsEntries _coreActualTurnViewAsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries _coreBoundDiscardSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedUseConversionsEntries _coreNamedUseConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseRulesEntries _coreProgramCardUseRulesOperations;
    private readonly ICorePlayCardHistoryCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedSourceUsesEntries _coreRoundGainedSourceUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGuanduEventProgramsEntries _coreGuanduEventProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeMultiTargetActionsEntries _coreNativeMultiTargetActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramTieredRoundConversionEntries _coreProgramTieredRoundConversionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTurnTargetCardQuotaAllowancesEntries _coreTurnTargetCardQuotaAllowancesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess _coreGameEngineSkillProgramsProgramPhaseUsesAccess;
    internal CoreGameEnginePlayCardHistoryRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries characterHuangfuSongWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations, global::CardGame.Core.RuntimeEntries.CharacterSunQuanAnniversaryGodEntries characterSunQuanAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries characterWeiYanMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityOrdinaryTricksEntries coreActivityOrdinaryTricksOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnViewAsEntries coreActualTurnViewAsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries coreBoundDiscardSlashBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedUseConversionsEntries coreNamedUseConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseRulesEntries coreProgramCardUseRulesOperations, ICorePlayCardHistoryCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedSourceUsesEntries coreRoundGainedSourceUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations, global::CardGame.Core.RuntimeEntries.CoreGuanduEventProgramsEntries coreGuanduEventProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.RuntimeEntries.CoreNativeMultiTargetActionsEntries coreNativeMultiTargetActionsOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreProgramTieredRoundConversionEntries coreProgramTieredRoundConversionOperations, global::CardGame.Core.RuntimeEntries.CoreTurnTargetCardQuotaAllowancesEntries coreTurnTargetCardQuotaAllowancesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess coreGameEngineSkillProgramsProgramPhaseUsesAccess)
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
