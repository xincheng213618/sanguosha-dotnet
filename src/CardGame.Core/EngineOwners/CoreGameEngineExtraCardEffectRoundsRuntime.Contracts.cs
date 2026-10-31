#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreExtraCardEffectRoundsCharacterDaQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZixiExtraEffectRound(long id);
    bool HasUnstartedZixiExtraEffect(long id);
    void CaptureZixiExtraEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void CompleteZixiExtraEffectRound(long id);
    bool TryBeginZixiExtraEffectRound(long id, bool afterAttack);
    void AssertZixiExtraEffectRounds();
}

internal interface ICoreExtraCardEffectRoundsCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreExtraCardEffectRoundsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreExtraCardEffectRoundsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreExtraCardEffectRoundsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICoreExtraCardEffectRoundsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreExtraCardEffectRoundsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreExtraCardEffectRoundsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateCardContinuations(long id, global::System.Func<global::CardGame.Core.CardResolutionContinuations, global::CardGame.Core.CardResolutionContinuations> update);
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreExtraCardEffectRoundsCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ExtraCardEffectState? NativeExtraEffectState(global::CardGame.Core.CardUseFrame use);
    void UpdateNativeExtraEffectState(long id, global::System.Func<global::CardGame.Core.ExtraCardEffectState, global::CardGame.Core.ExtraCardEffectState> update);
    bool IsFeiYiExtraEffectRound(long id);
    bool HasUnstartedFeiYiExtraEffect(long id);
    bool TryBeginFeiYiExtraEffectRound(long id, bool afterAttack);
}

internal interface ICoreExtraCardEffectRoundsCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouBingcaiExtraRound(long id);
    bool HasUnstartedMouBingcaiExtraEffect(long id);
    void CaptureMouBingcaiExtraEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void CompleteMouBingcaiExtraEffectRound(long id);
    bool TryBeginMouBingcaiExtraEffectRound(long id, bool afterAttack);
    bool ContinueMouBingcaiExtraFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreExtraCardEffectRoundsCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreExtraCardEffectRoundsCoreSoleIncomingRangeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSoleIncomingEffectRounds(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreExtraCardEffectRoundsCoreGuanduExtraEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GuanduRepeatTargetsRemainLegal(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreExtraCardEffectRoundsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreExtraCardEffectRoundsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueSlashDefense(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreExtraCardEffectRoundsCoreShanheSchoolTrickOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnstartedShanheSchoolTrick(long id);
    void CompleteShanheSchoolTrick(long id);
    bool TryBeginShanheSchoolTrick(long id, bool afterAttack);
    void AssertShanheSchoolTrickRounds();
}

internal interface ICoreExtraCardEffectRoundsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
    void ContinueSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreExtraCardEffectRoundsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreExtraCardEffectRoundsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreExtraCardEffectRoundsCharacterJiangJiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJjEffectRound(long id);
    bool HasUnstartedMouJjEffect(long id);
    void CompleteMouJjEffect(long id);
    bool TryBeginMouJjEffect(long id, bool afterAttack);
    void AssertMouJjEffects();
}

internal interface ICoreExtraCardEffectRoundsCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHqRound(long id);
    bool HasUnstartedMouHqRound(long id);
    void CaptureMouHqAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void CompleteMouHqRound(long id);
    bool TryBeginMouHqRound(long id, bool afterAttack);
    bool ContinueMouHqFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void AssertMouHqRounds();
}

internal interface ICoreExtraCardEffectRoundsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreExtraCardEffectRoundsCoreNamedAmbushInvalidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNamedAmbushUseIneffective(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreExtraCardEffectRoundsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreExtraCardEffectRoundsCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRemainingAdjustedSimpleTargets(long frameId);
}

internal interface ICoreExtraCardEffectRoundsCoreSoleIncomingEffectRepeatOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSoleIncomingExtraRound(long id);
    bool HasUnstartedSoleIncomingEffect(long id);
    void CaptureSoleIncomingAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void CompleteSoleIncomingExtraRound(long id);
    bool TryBeginSoleIncomingExtraRound(long id, bool afterAttack);
    bool ContinueSoleIncomingFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreExtraCardEffectRoundsCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRemainingSequentialTrickTargets(long frameId);
}

internal interface ICoreExtraCardEffectRoundsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICoreExtraCardEffectRoundsCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMoQuanyuExtraRound(long id);
    bool HasUnstartedMoQuanyuExtraRound(long id);
    void CaptureMoQuanyuExtraAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void CompleteMoQuanyuExtraRound(long id);
    bool TryBeginMoQuanyuExtraRound(long id, bool afterAttack);
    bool ContinueMoQuanyuFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}
