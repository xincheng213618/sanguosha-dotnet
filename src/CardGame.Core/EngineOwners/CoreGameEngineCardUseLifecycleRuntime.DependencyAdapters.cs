#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime))]
internal sealed class CoreCardUseLifecycleCoreNativeTrickUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreNativeTrickUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> _targetFactory = targetFactory;
    public void CompleteAlcoholUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card alcohol, long resolutionId) => this._targetFactory().CompleteAlcoholUse(source, alcohol, resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed class CoreCardUseLifecycleCharacterZhugeLiangMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterZhugeLiangMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangMouRuntime> _targetFactory = targetFactory;
    public bool ContinueMouZhijueSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueMouZhijueSimpleUse(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime))]
internal sealed class CoreCardUseLifecycleCoreSoleIncomingEffectRepeatAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreSoleIncomingEffectRepeatOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> _targetFactory = targetFactory;
    public bool IsSoleIncomingExtraRound(long id) => this._targetFactory().IsSoleIncomingExtraRound(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreCardUseLifecycleCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => this._targetFactory().GetTrickRepresentation(frameId, cardId, requireProcessing);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed class CoreCardUseLifecycleCharacterHuangQuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterHuangQuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _targetFactory = targetFactory;
    public void RecordJianjiOrdinaryAlcoholUse(long id) => this._targetFactory().RecordJianjiOrdinaryAlcoholUse(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDongZhuoMouOlRuntime))]
internal sealed class CoreCardUseLifecycleCharacterDongZhuoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhuoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterDongZhuoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhuoMouOlRuntime> _targetFactory = targetFactory;
    public bool ContinueMouDongZhuoOlWine(long id, global::CardGame.Core.ProgramSimpleCardContinuation c) => this._targetFactory().ContinueMouDongZhuoOlWine(id, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime))]
internal sealed class CoreCardUseLifecycleCharacterHuanShuJunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterHuanShuJunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanShuJunOlRuntime> _targetFactory = targetFactory;
    public bool ContinueHuanpeiSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation c) => this._targetFactory().ContinueHuanpeiSimpleUse(id, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime))]
internal sealed class CoreCardUseLifecycleCharacterXiaoQiaoMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterXiaoQiaoMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoMouOlRuntime> _targetFactory = targetFactory;
    public bool ContinueMouXqJueyanSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation c) => this._targetFactory().ContinueMouXqJueyanSimpleUse(id, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreCardUseLifecycleCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public bool ContinueGuanduIntoxicationUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueGuanduIntoxicationUse(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed class CoreCardUseLifecycleCoreNativeRecoveryCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreNativeRecoveryCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _targetFactory = targetFactory;
    public void CompleteRecoveryCardUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, long resolutionId, global::CardGame.Core.CardKind playedCardKind, int recoveryAmount, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource> recoveryPolicySources) => this._targetFactory().CompleteRecoveryCardUse(source, target, card, resolutionId, playedCardKind, recoveryAmount, recoveryPolicySources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime))]
internal sealed class CoreCardUseLifecycleCoreDrawFundedDistinctBasicUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreDrawFundedDistinctBasicUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> _targetFactory = targetFactory;
    public bool ContinueDrawFundedDistinctBasicSimpleUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueDrawFundedDistinctBasicSimpleUse(frameId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed class CoreCardUseLifecycleCharacterGuoTuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterGuoTuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime> _targetFactory = targetFactory;
    public bool ContinueWeijieSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation c) => this._targetFactory().ContinueWeijieSimpleUse(id, c);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreCardUseLifecycleCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public bool ContinueHappyWeiYanIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueHappyWeiYanIneffectiveSimple(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreCardUseLifecycleCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool ContinueDiouSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueDiouSimpleUse(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime))]
internal sealed class CoreCardUseLifecycleCharacterHuangZhongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterHuangZhongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool ContinueAnniversaryHzIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueAnniversaryHzIneffectiveSimple(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime))]
internal sealed class CoreCardUseLifecycleCoreVirtualBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreVirtualBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineVirtualBasicOffersRuntime> _targetFactory = targetFactory;
    public bool ContinueVirtualBasicEffect(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueVirtualBasicEffect(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreCardUseLifecycleCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public bool ContinueTieredRoundZeroSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueTieredRoundZeroSimpleUse(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreCardUseLifecycleCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime))]
internal sealed class CoreCardUseLifecycleCoreDyingSuitsPeachProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreDyingSuitsPeachProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime> _targetFactory = targetFactory;
    public bool ContinueDyingSuitsPeach(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueDyingSuitsPeach(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime))]
internal sealed class CoreCardUseLifecycleCharacterChengYuMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterChengYuMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengYuMouRuntime> _targetFactory = targetFactory;
    public bool ContinueMouCyIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueMouCyIneffectiveSimple(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime))]
internal sealed class CoreCardUseLifecycleCharacterCaoZhenBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterCaoZhenBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool ContinueBoundarySidiIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueBoundarySidiIneffectiveSimple(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime))]
internal sealed class CoreCardUseLifecycleCharacterQinLangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterQinLangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _targetFactory = targetFactory;
    public bool ContinueQinLangXianyingSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueQinLangXianyingSimpleUse(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime))]
internal sealed class CoreCardUseLifecycleCharacterDaQiaoLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterDaQiaoLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> _targetFactory = targetFactory;
    public bool IsZixiExtraEffectRound(long id) => this._targetFactory().IsZixiExtraEffectRound(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime))]
internal sealed class CoreCardUseLifecycleCoreFinalizedCardTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreFinalizedCardTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFinalizedCardTargetsRuntime> _targetFactory = targetFactory;
    public bool TryBeginFinalizedSimpleCardPrograms(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().TryBeginFinalizedSimpleCardPrograms(frameId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime))]
internal sealed class CoreCardUseLifecycleCoreShanheSourceMouDongZhuoWineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreShanheSourceMouDongZhuoWineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceMouDongZhuoWineRuntime> _targetFactory = targetFactory;
    public bool ContinueShanheSourceMouWine(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueShanheSourceMouWine(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuYouAnniversaryRuntime))]
internal sealed class CoreCardUseLifecycleCharacterLiuYouAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYouAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterLiuYouAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYouAnniversaryRuntime> _targetFactory = targetFactory;
    public void CaptureAnniversaryLiuYouNextSlash(long cardUseFrameId) => this._targetFactory().CaptureAnniversaryLiuYouNextSlash(cardUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreCardUseLifecycleCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime))]
internal sealed class CoreCardUseLifecycleCharacterAHuiNanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterAHuiNanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> _targetFactory = targetFactory;
    public bool ContinueAHuiNanJuemanSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueAHuiNanJuemanSimpleUse(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime))]
internal sealed class CoreCardUseLifecycleCharacterZhouYuOlMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterZhouYuOlMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouYuOlMouRuntime> _targetFactory = targetFactory;
    public bool ContinueMouZuifengAlcoholUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueMouZuifengAlcoholUse(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime))]
internal sealed class CoreCardUseLifecycleCoreNativeCardUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreNativeCardUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _targetFactory = targetFactory;
    public void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().FinishCardUse(frameId, card, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime))]
internal sealed class CoreCardUseLifecycleCoreNativeEquipmentUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreNativeEquipmentUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> _targetFactory = targetFactory;
    public void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId) => this._targetFactory().CompleteEquipmentUse(source, equipment, resolutionId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime))]
internal sealed class CoreCardUseLifecycleCoreProgramCardUseNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreProgramCardUseNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseNullificationRuntime> _targetFactory = targetFactory;
    public bool TryBeginSimpleBeforeTargetPrograms(long useId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().TryBeginSimpleBeforeTargetPrograms(useId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreCardUseLifecycleCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryPauseRecoveryPaidCardUse(long useId, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation) => this._targetFactory().TryPauseRecoveryPaidCardUse(useId, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime))]
internal sealed class CoreCardUseLifecycleCoreNativeNullificationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreNativeNullificationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime> _targetFactory = targetFactory;
    public void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().BeginJizhiOrNullificationWindow(resolutionId, effectCard, sourceSeat, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime))]
internal sealed class CoreCardUseLifecycleCoreDesignatedExtraTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreDesignatedExtraTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDesignatedExtraTargetsRuntime> _targetFactory = targetFactory;
    public bool ContinueDesignatedVirtualBasicEffect(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().ContinueDesignatedVirtualBasicEffect(id, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreCardUseLifecycleCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime))]
internal sealed class CoreCardUseLifecycleCharacterCaoChunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardUseLifecycleCharacterCaoChunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime> _targetFactory = targetFactory;
    public bool TryBeginCaoChunSimpleDebt(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => this._targetFactory().TryBeginCaoChunSimpleDebt(id, continuation);
}
