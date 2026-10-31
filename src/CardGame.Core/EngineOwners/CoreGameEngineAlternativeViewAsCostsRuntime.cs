#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAlternativeViewAsCostsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongZhuoMouOlEntries _characterDongZhuoMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterJuShouMouEntries _characterJuShouMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterKongShuOlEntries _characterKongShuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries _characterLiuChengMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries _characterSimaYiMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunXuAnniversaryEntries _characterXunXuAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries _characterZhangFeiWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhaoTongZhaoGuangAnniversaryEntries _characterZhaoTongZhaoGuangAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries _coreChainedStateBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries _coreDrawFundedDistinctBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries _coreExtraCardEffectRoundsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduEquipmentEntries _coreGuanduEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundDistinctBasicPlayEntries _coreRoundDistinctBasicPlayOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries _coreShanheSourceMouDongZhuoWineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries _coreWushuangShopCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess _coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess;
    internal CoreGameEngineAlternativeViewAsCostsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations, global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterDongZhuoMouOlEntries characterDongZhuoMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterJuShouMouEntries characterJuShouMouOperations, global::CardGame.Core.RuntimeEntries.CharacterKongShuOlEntries characterKongShuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuChengMouMobileEntries characterLiuChengMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries characterSimaYiMouOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXunXuAnniversaryEntries characterXunXuAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries characterZhangFeiWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterZhaoTongZhaoGuangAnniversaryEntries characterZhaoTongZhaoGuangAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries coreChainedStateBasicReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries coreDrawFundedDistinctBasicReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries coreExtraCardEffectRoundsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduEquipmentEntries coreGuanduEquipmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundDistinctBasicPlayEntries coreRoundDistinctBasicPlayOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries coreShanheSourceMouDongZhuoWineOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries coreWushuangShopCardsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess coreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess)
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
