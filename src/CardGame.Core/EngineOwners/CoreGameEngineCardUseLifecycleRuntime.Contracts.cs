#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardUseLifecycleCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAlcoholUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card alcohol, long resolutionId);
}

internal interface ICoreCardUseLifecycleCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueMouZhijueSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreSoleIncomingEffectRepeatOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleIncomingExtraRound(long id);
}

internal interface ICoreCardUseLifecycleCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreCardUseLifecycleCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordJianjiOrdinaryAlcoholUse(long id);
}

internal interface ICoreCardUseLifecycleCharacterDongZhuoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueMouDongZhuoOlWine(long id, global::CardGame.Core.ProgramSimpleCardContinuation c);
}

internal interface ICoreCardUseLifecycleCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueHuanpeiSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation c);
}

internal interface ICoreCardUseLifecycleCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueMouXqJueyanSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation c);
}

internal interface ICoreCardUseLifecycleCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueGuanduIntoxicationUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreNativeRecoveryCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteRecoveryCardUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, long resolutionId, global::CardGame.Core.CardKind playedCardKind, int recoveryAmount, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource> recoveryPolicySources);
}

internal interface ICoreCardUseLifecycleCoreDrawFundedDistinctBasicUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueDrawFundedDistinctBasicSimpleUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueWeijieSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation c);
}

internal interface ICoreCardUseLifecycleCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueHappyWeiYanIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueDiouSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCharacterHuangZhongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueAnniversaryHzIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreVirtualBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueVirtualBasicEffect(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueTieredRoundZeroSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreCardUseLifecycleCoreDyingSuitsPeachProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueDyingSuitsPeach(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCharacterChengYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueMouCyIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCharacterCaoZhenBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueBoundarySidiIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueQinLangXianyingSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZixiExtraEffectRound(long id);
}

internal interface ICoreCardUseLifecycleCoreFinalizedCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginFinalizedSimpleCardPrograms(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreShanheSourceMouDongZhuoWineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueShanheSourceMouWine(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardUseLifecycleCharacterLiuYouAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureAnniversaryLiuYouNextSlash(long cardUseFrameId);
}

internal interface ICoreCardUseLifecycleCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreCardUseLifecycleCharacterAHuiNanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueAHuiNanJuemanSimpleUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCharacterZhouYuOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueMouZuifengAlcoholUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreCardUseLifecycleCoreNativeEquipmentUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteEquipmentUse(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment, long resolutionId);
}

internal interface ICoreCardUseLifecycleCoreProgramCardUseNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSimpleBeforeTargetPrograms(long useId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryPauseRecoveryPaidCardUse(long useId, global::CardGame.Core.RecoveryPaidCardUseContinuation continuation);
}

internal interface ICoreCardUseLifecycleCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreCardUseLifecycleCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueDesignatedVirtualBasicEffect(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreCardUseLifecycleCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCardUseLifecycleCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreCardUseLifecycleCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCaoChunSimpleDebt(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}
