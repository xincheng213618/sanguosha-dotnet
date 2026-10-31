#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWenhePoisonCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HpChangeContext? RecordHpChange(long? parentFrameId, int? sourceSeat, int targetSeat, int before, int after, global::CardGame.Core.HpChangeKind kind);
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreWenhePoisonCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTieredRoundZeroFireAttackUse(long id);
}

internal interface ICoreWenhePoisonKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreWenhePoisonCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreWenhePoisonCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQinLangXianyingUse(long id);
}

internal interface ICoreWenhePoisonCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResponseCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreWenhePoisonCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> BuildDyingResponderSeats(int victimSeat);
    void ExposeHumanDyingPrompt();
}

internal interface ICoreWenhePoisonKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreWenhePoisonCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhijueZeroUse(long id);
}

internal interface ICoreWenhePoisonCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreWenhePoisonPendingCardsMovedBatchesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

internal interface ICoreWenhePoisonCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreWenhePoisonCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CardSupplyCompletionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICoreWenhePoisonCharacterZhouYuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZyZeroFireUse(long id);
}

internal interface ICoreWenhePoisonCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreWenhePoisonCoreActivityBattleRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IWenheBattleRulesFeature? WenheRules { get; }
}

internal interface ICoreWenhePoisonCoreDeferredTurnEndPreludesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardsMovedTriggerWindowFrame? CreateCardsMovedProgramWindow(global::CardGame.Core.CardMovementBatchContext batch, long? resumeProgramFrameId = default);
}

internal interface ICoreWenhePoisonCoreDyingEntryProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreWenhePoisonCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreWenhePoisonCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card);
}

internal interface ICoreWenhePoisonCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetCardUsePhysicalCards(long resolutionId);
}

internal interface ICoreWenhePoisonCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void AdvanceRuntimeFrame(long frameId);
}

internal interface ICoreWenhePoisonCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidCardUseMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement);
}

internal interface ICoreWenhePoisonCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreWenhePoisonCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMandatorySelfDyingProgram(global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreWenhePoisonCoreNativeFireAttackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetFireAttackEffectCard(global::CardGame.Core.CardUseFrame pending);
}

internal interface ICoreWenhePoisonResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreWenhePoisonCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreWenhePoisonCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreWenhePoisonCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreWenhePoisonCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreWenhePoisonCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}
