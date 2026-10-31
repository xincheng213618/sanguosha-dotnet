#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSayingTieredSuppliesCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreSayingTieredSuppliesCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreSayingTieredSuppliesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreSayingTieredSuppliesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}

internal interface ICoreSayingTieredSuppliesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSayingTieredSuppliesCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule, string Hash)> TieredRoundZeroRules(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind? output = default, bool responseUse = false, bool dyingUse = false);
}

internal interface ICoreSayingTieredSuppliesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreSayingTieredSuppliesKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreSayingTieredSuppliesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreSayingTieredSuppliesCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreSayingTieredSuppliesCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICoreSayingTieredSuppliesCharacterWoLongFengChuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEquipmentSlotNamedSkill(string skill);
    global::CardGame.Core.EquipmentSlotNamedUsePayment? UnconsumedEquipmentSlotPayment(global::CardGame.Core.CardConversionSource source);
    bool EquipmentSlotNamedChoiceIsPayable(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.EquipmentSlotNamedUseIntent intent, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.EquipmentSlot slot);
    void BeginEquipmentSlotNamedUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::CardGame.Core.EquipmentSlotNamedUseIntent intent, global::CardGame.Core.PendingDecision original, global::CardGame.Core.LegalAction? action, global::CardGame.Core.PromptChoice? choice);
}

internal interface ICoreSayingTieredSuppliesCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreSayingTieredSuppliesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreSayingTieredSuppliesCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreSayingTieredSuppliesCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void NotifyAiOfSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
}

internal interface ICoreSayingTieredSuppliesCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool TryGetSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
    void PrepareSayingSuppliedSlash(global::CardGame.Core.ProgramSkillFrame supplied, int actorSeat, int providerSeat, int targetSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversionSource, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreSayingTieredSuppliesCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreSayingTieredSuppliesCoreCurrentCardEnhancementsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAdjustedSlashBaseDamage(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreSayingTieredSuppliesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryMarkProgramUseCommitted(long frameId);
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreSayingTieredSuppliesContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreSayingTieredSuppliesCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreSayingTieredSuppliesCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreSayingTieredSuppliesCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}
