#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreVirtualBasicOffersResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreVirtualBasicOffersCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreVirtualBasicOffersCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreVirtualBasicOffersCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    bool TryMarkProgramUseCommitted(long frameId);
    void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreVirtualBasicOffersCharacterRuanJiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactRuanJiVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramVirtualBasicDraft draft);
}

internal interface ICoreVirtualBasicOffersCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreVirtualBasicOffersCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreVirtualBasicOffersCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreVirtualBasicOffersCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreVirtualBasicOffersCoreHellKingProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactHellPreparationAlcoholDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramVirtualBasicDraft draft);
}

internal interface ICoreVirtualBasicOffersCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryQueueRecoveryReplacement(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.RecoveryAttemptCompletion completion, global::CardGame.Core.HappyQiaoRecoverToOrigin? happyQiaoRecoverToOrigin = default);
}

internal interface ICoreVirtualBasicOffersCharacterZhangFenAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactDagongCheBasicDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramVirtualBasicDraft d);
}

internal interface ICoreVirtualBasicOffersCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordMouZhaJiangAwareAlcoholDebit(long useId, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreVirtualBasicOffersCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreVirtualBasicOffersCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreVirtualBasicOffersCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreVirtualBasicOffersCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICoreVirtualBasicOffersCharacterZhouYuOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MouGuquPhysicalUseForbidden(int actor, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreVirtualBasicOffersCoreNativeRecoveryFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount);
}

internal interface ICoreVirtualBasicOffersCoreGuanduGeneralNativeCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind? GuanduChoulueFrozenBasicKind(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreVirtualBasicOffersCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopFinishedCardUse(long frameId);
}

internal interface ICoreVirtualBasicOffersCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreVirtualBasicOffersContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreVirtualBasicOffersCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreVirtualBasicOffersCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
}

internal interface ICoreVirtualBasicOffersCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreVirtualBasicOffersCoreCardRecoveryModifiersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ApplyIssuedCardRecoveryModifiers(long useId, int actorSeat, int targetSeat, int baseAmount);
}

internal interface ICoreVirtualBasicOffersCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreVirtualBasicOffersCoreAdvancedPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNextUnlimitedCard(global::CardGame.Core.CharacterState owner);
    bool HasFriendQihuiAllowance(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreVirtualBasicOffersCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreVirtualBasicOffersKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreVirtualBasicOffersCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreVirtualBasicOffersCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreVirtualBasicOffersCoreForcedPhaseAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactPhaseAlcoholVirtualDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramVirtualBasicDraft draft);
}

internal interface ICoreVirtualBasicOffersKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreVirtualBasicOffersCharacterLiuLiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactAnniversaryLiuLiBasicDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramVirtualBasicDraft draft);
}

internal interface ICoreVirtualBasicOffersCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouWujingAllowance(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreVirtualBasicOffersCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordYingboCardUse(long resolutionId, int sourceSeat, global::CardGame.Core.CardKind cardKind);
}

internal interface ICoreVirtualBasicOffersCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreVirtualBasicOffersCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryContinueDesignatedVirtualBasicUse(long id);
}

internal interface ICoreVirtualBasicOffersCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreVirtualBasicOffersCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreVirtualBasicOffersCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksPlayCardHistory { get; }
}

internal interface ICoreVirtualBasicOffersCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreVirtualBasicOffersCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreVirtualBasicOffersCoreExtraCardEffectRoundsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyExtraCapableAlcoholEffect(long id, global::CardGame.Core.CharacterState target);
    bool TryBeginExtraCardEffectRound(long id, bool afterAttack = false);
}

internal interface ICoreVirtualBasicOffersCoreTurnTargetCardQuotaAllowancesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetCardQuotaAllowance(int actor, int target);
}

internal interface ICoreVirtualBasicOffersCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreVirtualBasicOffersCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordSlashUseDebit(long cardUseFrameId, int actorSeat);
}

internal interface ICoreVirtualBasicOffersCoreProvenanceVirtualAlcoholOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactProvenanceVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramVirtualBasicDraft draft);
}

internal interface ICoreVirtualBasicOffersCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFirstActualPlayUseDistance(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreVirtualBasicOffersCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreVirtualBasicOffersCoreZhaoguMirrorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhaoguVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramVirtualBasicDraft draft);
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardKind Kind, int Target)> ZhaoguVirtualBasicOptions(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.ProgramSkillFrame CompleteZhaoguVirtualBasicReceipt(global::CardGame.Core.ProgramSkillFrame frame, long childId);
}

internal interface ICoreVirtualBasicOffersCharacterWeiYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMobileShiZhuangshiQuota(int seat);
}

internal interface ICoreVirtualBasicOffersCharacterYangBiaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactAnniversaryYangBiaoBasicDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramVirtualBasicDraft draft);
}
