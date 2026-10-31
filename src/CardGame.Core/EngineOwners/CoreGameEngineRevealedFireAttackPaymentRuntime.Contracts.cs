#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRevealedFireAttackPaymentCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreRevealedFireAttackPaymentCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreRevealedFireAttackPaymentCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramVirtualOrdinaryTrickUse(long frameId);
}

internal interface ICoreRevealedFireAttackPaymentKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreRevealedFireAttackPaymentCoreNativeFireAttackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetFireAttackEffectCard(global::CardGame.Core.CardUseFrame pending);
    global::CardGame.Core.CardUseFrame SetFireAttackSelection(long frameId, global::CardGame.Core.FireAttackSelectionState? selection);
    void BeginFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.Card revealed);
}

internal interface ICoreRevealedFireAttackPaymentCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhijueZeroUse(long id);
}

internal interface ICoreRevealedFireAttackPaymentResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRevealedFireAttackPaymentCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreRevealedFireAttackPaymentCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreRevealedFireAttackPaymentKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreRevealedFireAttackPaymentCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreRevealedFireAttackPaymentCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreRevealedFireAttackPaymentCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICoreRevealedFireAttackPaymentCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTieredRoundZeroFireAttackUse(long id);
}

internal interface ICoreRevealedFireAttackPaymentCoreWushuangTacticRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWushuangTactic(int seat, int id);
}

internal interface ICoreRevealedFireAttackPaymentCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card);
}

internal interface ICoreRevealedFireAttackPaymentCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouZeroUse(long id);
}

internal interface ICoreRevealedFireAttackPaymentCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId);
}

internal interface ICoreRevealedFireAttackPaymentCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? ActiveFireAttack { get; }
}

internal interface ICoreRevealedFireAttackPaymentCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQinLangXianyingUse(long id);
}

internal interface ICoreRevealedFireAttackPaymentCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDcZeroUse(long id);
}

internal interface ICoreRevealedFireAttackPaymentCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreRevealedFireAttackPaymentCharacterZhouYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZyZeroFireUse(long id);
}

internal interface ICoreRevealedFireAttackPaymentCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRevealedFireAttackPaymentCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouXqJueyanUse(long id);
}
