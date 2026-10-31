#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardUseLifecycleRuntime
{
    private readonly RuntimeEntries.CharacterAHuiNanOlEntries _characterAHuiNanOlOperations;
    private readonly RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries _characterCaoZhenBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterChengYuMouEntries _characterChengYuMouOperations;
    private readonly RuntimeEntries.CharacterDaQiaoLeEntries _characterDaQiaoLeOperations;
    private readonly RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly RuntimeEntries.CharacterDongZhuoMouOlEntries _characterDongZhuoMouOlOperations;
    private readonly RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly RuntimeEntries.CharacterHuangQuanOlEntries _characterHuangQuanOlOperations;
    private readonly RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries _characterHuangZhongAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterLiuYouAnniversaryEntries _characterLiuYouAnniversaryOperations;
    private readonly RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly RuntimeEntries.CharacterZhouYuOlMouEntries _characterZhouYuOlMouOperations;
    private readonly RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries _coreDesignatedExtraTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries _coreDrawFundedDistinctBasicUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineDyingSuitsPeachProgramsEntries _coreDyingSuitsPeachProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineFinalizedCardTargetsEntries _coreFinalizedCardTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineGuanduCardsEntries _coreGuanduCardsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries _coreProgramCardUseNullificationOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries _coreShanheSourceMouDongZhuoWineOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries _coreSoleIncomingEffectRepeatOperations;
    private readonly RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries _coreTieredRoundZeroUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineVirtualBasicOffersEntries _coreVirtualBasicOffersOperations;
    private readonly RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly RuntimeEntries.CoreNativeEquipmentUseEntries _coreNativeEquipmentUseOperations;
    private readonly RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly RuntimeEntries.CoreNativeRecoveryCardsEntries _coreNativeRecoveryCardsOperations;
    private readonly RuntimeEntries.CoreNativeTrickUseEntries _coreNativeTrickUseOperations;
    private readonly RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineCardUseLifecycleRuntime(RuntimeEntries.CharacterAHuiNanOlEntries characterAHuiNanOlOperations, RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations, RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries characterCaoZhenBoundaryMobileOperations, RuntimeEntries.CharacterChengYuMouEntries characterChengYuMouOperations, RuntimeEntries.CharacterDaQiaoLeEntries characterDaQiaoLeOperations, RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, RuntimeEntries.CharacterDongZhuoMouOlEntries characterDongZhuoMouOlOperations, RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, RuntimeEntries.CharacterHuangQuanOlEntries characterHuangQuanOlOperations, RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries characterHuangZhongAnniversaryGodOperations, RuntimeEntries.CharacterLiuYouAnniversaryEntries characterLiuYouAnniversaryOperations, RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations, RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, RuntimeEntries.CharacterZhouYuOlMouEntries characterZhouYuOlMouOperations, RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries coreDesignatedExtraTargetsOperations, RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicUsesEntries coreDrawFundedDistinctBasicUsesOperations, RuntimeEntries.CoreGameEngineDyingSuitsPeachProgramsEntries coreDyingSuitsPeachProgramsOperations, RuntimeEntries.CoreGameEngineFinalizedCardTargetsEntries coreFinalizedCardTargetsOperations, RuntimeEntries.CoreGameEngineGuanduCardsEntries coreGuanduCardsOperations, RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries coreProgramCardUseNullificationOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries coreShanheSourceMouDongZhuoWineOperations, RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries coreSoleIncomingEffectRepeatOperations, RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries coreTieredRoundZeroUsesOperations, RuntimeEntries.CoreGameEngineVirtualBasicOffersEntries coreVirtualBasicOffersOperations, RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, RuntimeEntries.CoreNativeEquipmentUseEntries coreNativeEquipmentUseOperations, RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, RuntimeEntries.CoreNativeRecoveryCardsEntries coreNativeRecoveryCardsOperations, RuntimeEntries.CoreNativeTrickUseEntries coreNativeTrickUseOperations, RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
