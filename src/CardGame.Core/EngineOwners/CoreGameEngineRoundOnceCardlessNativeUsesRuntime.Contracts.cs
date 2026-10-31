#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player);
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreRedAdditionalTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTargetAdjustmentAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreQinglongInsertionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SuspendContinuationForQinglongFollowup(long outerResolutionId, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? followupRequest = default);
}

internal interface ICoreRoundOnceCardlessNativeUsesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordSlashUseDebit(long cardUseFrameId, int actorSeat);
}

internal interface ICoreRoundOnceCardlessNativeUsesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreRoundOnceCardlessNativeUsesCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryMaTieSlashQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreRoundOnceCardlessNativeUsesKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreRoundOnceCardlessNativeUsesKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description);
    void SelectUseConversion(global::CardGame.Core.LegalAction action);
    bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
    void CaptureSelectedResponseConversion(global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeMultiTargetActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0);
}

internal interface ICoreRoundOnceCardlessNativeUsesCharacterKongRongMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouKrLiwenUseQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreRoundOnceCardlessBasicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseRoundOnceCardlessBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool forResponse);
    global::CardGame.Core.RoundOnceCardlessBasicPaymentReceipt ConsumeRoundOnceCardlessBasic(global::CardGame.Core.CardConversionSource source, bool forResponse);
    global::CardGame.Core.RoundOnceCardlessBasicUseReceipt IssueRoundOnceCardlessBasicUse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource source);
    bool IsIssuedRoundOnceCardlessBasicUse(global::CardGame.Core.RoundOnceCardlessBasicUseReceipt? receipt, global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreRoundOnceCardlessNativeUsesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreActualTurnDamageClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesDeclaredActualDamageUse(global::CardGame.Core.CardUseFrame use, int actor, int provider);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseQinglongCrescentBladeTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, bool? effectiveColor = default, bool allowAnyColor = true, global::CardGame.Core.Suit? physicalSuit = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddNextActualUseAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertCurrentSlashFirePolicy(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreRoundOnceCardlessNativeUsesCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKeBiNengPingduanBasicQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreRoundOnceCardlessNativeUsesKernelCoreMainOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    bool TryMarkProgramUseCommitted(long frameId);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void NotifyAiOfSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
}

internal interface ICoreRoundOnceCardlessNativeUsesCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLuSuUseQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreRoundOnceCardlessNativeUsesCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddNextCardTargetAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor);
}
