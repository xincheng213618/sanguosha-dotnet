#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAlternativeViewAsCostsRuntime
{
    private readonly RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly RuntimeEntries.CharacterDongZhuoMouOlEntries _characterDongZhuoMouOlOperations;
    private readonly RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly RuntimeEntries.CharacterJuShouMouEntries _characterJuShouMouOperations;
    private readonly RuntimeEntries.CharacterKongShuOlEntries _characterKongShuOlOperations;
    private readonly RuntimeEntries.CharacterLiuChengMouMobileEntries _characterLiuChengMouMobileOperations;
    private readonly RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly RuntimeEntries.CharacterSimaYiMouEntries _characterSimaYiMouOperations;
    private readonly RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly RuntimeEntries.CharacterXunXuAnniversaryEntries _characterXunXuAnniversaryOperations;
    private readonly RuntimeEntries.CharacterZhangFeiWumiaoEntries _characterZhangFeiWumiaoOperations;
    private readonly RuntimeEntries.CharacterZhaoTongZhaoGuangAnniversaryEntries _characterZhaoTongZhaoGuangAnniversaryOperations;
    private readonly RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries _coreChainedStateBasicReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries _coreDrawFundedDistinctBasicReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries _coreExtraCardEffectRoundsOperations;
    private readonly RuntimeEntries.CoreGameEngineGuanduEquipmentEntries _coreGuanduEquipmentOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundDistinctBasicPlayEntries _coreRoundDistinctBasicPlayOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries _coreShanheSourceMouDongZhuoWineOperations;
    private readonly RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangShopCardsEntries _coreWushuangShopCardsOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly EngineStates.Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess _coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess;
    internal CoreGameEngineAlternativeViewAsCostsRuntime(RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations, RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, RuntimeEntries.CharacterDongZhuoMouOlEntries characterDongZhuoMouOlOperations, RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, RuntimeEntries.CharacterJuShouMouEntries characterJuShouMouOperations, RuntimeEntries.CharacterKongShuOlEntries characterKongShuOlOperations, RuntimeEntries.CharacterLiuChengMouMobileEntries characterLiuChengMouMobileOperations, RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations, RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations, RuntimeEntries.CharacterSimaYiMouEntries characterSimaYiMouOperations, RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, RuntimeEntries.CharacterXunXuAnniversaryEntries characterXunXuAnniversaryOperations, RuntimeEntries.CharacterZhangFeiWumiaoEntries characterZhangFeiWumiaoOperations, RuntimeEntries.CharacterZhaoTongZhaoGuangAnniversaryEntries characterZhaoTongZhaoGuangAnniversaryOperations, RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries coreChainedStateBasicReturnsOperations, RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries coreDrawFundedDistinctBasicReturnsOperations, RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries coreExtraCardEffectRoundsOperations, RuntimeEntries.CoreGameEngineGuanduEquipmentEntries coreGuanduEquipmentOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, RuntimeEntries.CoreGameEngineRoundDistinctBasicPlayEntries coreRoundDistinctBasicPlayOperations, RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries coreShanheSourceMouDongZhuoWineOperations, RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations, RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations, RuntimeEntries.CoreGameEngineWushuangShopCardsEntries coreWushuangShopCardsOperations, RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, EngineStates.Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess)
    {
        this._characterCaoZhiLeOperations = characterCaoZhiLeOperations;
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterDongZhuoMouOlOperations = characterDongZhuoMouOlOperations;
        this._characterGuoTuOlOperations = characterGuoTuOlOperations;
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._characterJuShouMouOperations = characterJuShouMouOperations;
        this._characterKongShuOlOperations = characterKongShuOlOperations;
        this._characterLiuChengMouMobileOperations = characterLiuChengMouMobileOperations;
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._characterMiHengLeOperations = characterMiHengLeOperations;
        this._characterSimaYiMouOperations = characterSimaYiMouOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._characterXunXuAnniversaryOperations = characterXunXuAnniversaryOperations;
        this._characterZhangFeiWumiaoOperations = characterZhangFeiWumiaoOperations;
        this._characterZhaoTongZhaoGuangAnniversaryOperations = characterZhaoTongZhaoGuangAnniversaryOperations;
        this._characterZhugeLiangMouOperations = characterZhugeLiangMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreChainedStateBasicReturnsOperations = coreChainedStateBasicReturnsOperations;
        this._coreDrawFundedDistinctBasicReturnsOperations = coreDrawFundedDistinctBasicReturnsOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreExtraCardEffectRoundsOperations = coreExtraCardEffectRoundsOperations;
        this._coreGuanduEquipmentOperations = coreGuanduEquipmentOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRoundDistinctBasicPlayOperations = coreRoundDistinctBasicPlayOperations;
        this._coreShanheSourceMouDongZhuoWineOperations = coreShanheSourceMouDongZhuoWineOperations;
        this._coreTieredRoundZeroReturnsOperations = coreTieredRoundZeroReturnsOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreWushuangShopCardsOperations = coreWushuangShopCardsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess = coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess;
    }
}
