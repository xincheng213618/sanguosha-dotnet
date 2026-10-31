#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardUseLifecycleRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterAHuiNanOlEntries _characterAHuiNanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries _characterCaoZhenBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengYuMouEntries _characterChengYuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries _characterDaQiaoLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongZhuoMouOlEntries _characterDongZhuoMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanOlEntries _characterHuangQuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries _characterHuangZhongAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuYouAnniversaryEntries _characterLiuYouAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouYuOlMouEntries _characterZhouYuOlMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries _coreDesignatedExtraTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries _coreDrawFundedDistinctBasicUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsPeachProgramsEntries _coreDyingSuitsPeachProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalizedCardTargetsEntries _coreFinalizedCardTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduCardsEntries _coreGuanduCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries _coreProgramCardUseNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries _coreShanheSourceMouDongZhuoWineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries _coreSoleIncomingEffectRepeatOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries _coreTieredRoundZeroUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualBasicOffersEntries _coreVirtualBasicOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentUseEntries _coreNativeEquipmentUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryCardsEntries _coreNativeRecoveryCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries _coreNativeTrickUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCardUseLifecycleRuntime(global::CardGame.Core.RuntimeEntries.CharacterAHuiNanOlEntries characterAHuiNanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries characterCaoZhenBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterChengYuMouEntries characterChengYuMouOperations, global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries characterDaQiaoLeOperations, global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterDongZhuoMouOlEntries characterDongZhuoMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangQuanOlEntries characterHuangQuanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries characterHuangZhongAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuYouAnniversaryEntries characterLiuYouAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouYuOlMouEntries characterZhouYuOlMouOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries coreDesignatedExtraTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries coreDrawFundedDistinctBasicUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsPeachProgramsEntries coreDyingSuitsPeachProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalizedCardTargetsEntries coreFinalizedCardTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduCardsEntries coreGuanduCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries coreProgramCardUseNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries coreShanheSourceMouDongZhuoWineOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries coreSoleIncomingEffectRepeatOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries coreTieredRoundZeroUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualBasicOffersEntries coreVirtualBasicOffersOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentUseEntries coreNativeEquipmentUseOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeRecoveryCardsEntries coreNativeRecoveryCardsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries coreNativeTrickUseOperations, global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterAHuiNanOlOperations = characterAHuiNanOlOperations;
        this._characterCaoChunOlOperations = characterCaoChunOlOperations;
        this._characterCaoZhenBoundaryMobileOperations = characterCaoZhenBoundaryMobileOperations;
        this._characterChengYuMouOperations = characterChengYuMouOperations;
        this._characterDaQiaoLeOperations = characterDaQiaoLeOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterDongZhuoMouOlOperations = characterDongZhuoMouOlOperations;
        this._characterGuoTuOlOperations = characterGuoTuOlOperations;
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._characterHuangQuanOlOperations = characterHuangQuanOlOperations;
        this._characterHuangZhongAnniversaryGodOperations = characterHuangZhongAnniversaryGodOperations;
        this._characterLiuYouAnniversaryOperations = characterLiuYouAnniversaryOperations;
        this._characterQinLangOlOperations = characterQinLangOlOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._characterZhouYuOlMouOperations = characterZhouYuOlMouOperations;
        this._characterZhugeLiangMouOperations = characterZhugeLiangMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreDesignatedExtraTargetsOperations = coreDesignatedExtraTargetsOperations;
        this._coreDrawFundedDistinctBasicUsesOperations = coreDrawFundedDistinctBasicUsesOperations;
        this._coreDyingSuitsPeachProgramsOperations = coreDyingSuitsPeachProgramsOperations;
        this._coreFinalizedCardTargetsOperations = coreFinalizedCardTargetsOperations;
        this._coreGuanduCardsOperations = coreGuanduCardsOperations;
        this._coreProgramCardUseNullificationOperations = coreProgramCardUseNullificationOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShanheSourceMouDongZhuoWineOperations = coreShanheSourceMouDongZhuoWineOperations;
        this._coreSoleIncomingEffectRepeatOperations = coreSoleIncomingEffectRepeatOperations;
        this._coreTieredRoundZeroUsesOperations = coreTieredRoundZeroUsesOperations;
        this._coreVirtualBasicOffersOperations = coreVirtualBasicOffersOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeEquipmentUseOperations = coreNativeEquipmentUseOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeRecoveryCardsOperations = coreNativeRecoveryCardsOperations;
        this._coreNativeTrickUseOperations = coreNativeTrickUseOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
