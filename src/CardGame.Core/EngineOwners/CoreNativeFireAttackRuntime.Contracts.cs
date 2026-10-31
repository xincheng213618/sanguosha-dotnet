#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeFireAttackCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? ActiveFireAttack { get; }
}

internal interface ICoreNativeFireAttackCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreNativeFireAttackCoreRevealedFireAttackPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FireAttackPaymentIsReveal(int seat);
    void ContinueRevealedFireAttackPayment(global::CardGame.Core.CardUseFrame use);
    void CompleteNativeFireAttackPayment(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.Card revealed, global::CardGame.Core.Card payment, bool revealInstead);
}

internal interface ICoreNativeFireAttackCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeFireAttackCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeFireAttackCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardMoveReason reason, long? parentFrameId = default);
    void CompleteNativeLogicalDiscardInvocation(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>? prepaidMaterials = default);
}

internal interface ICoreNativeFireAttackCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeFireAttackCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreNativeFireAttackCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginPublicHandRevealWindow(long parentId, global::CardGame.Core.PublicHandRevealReturnKind returned);
    bool TryBeginPublicHandRevealWindow(long parentId, string key);
}

internal interface ICoreNativeFireAttackCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeFireAttackCoreColorFireAttacksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginColorFireAttackReveal(global::CardGame.Core.CardUseFrame pending);
    bool TryBeginColorFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.Card revealed);
    bool TryResolveColorFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId);
}

internal interface ICoreNativeFireAttackCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreNativeFireAttackCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginWenhePoisonFireAttackPayment(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card revealed, global::CardGame.Core.Card cost, int sequenceBefore);
}

internal interface ICoreNativeFireAttackResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreNativeFireAttackCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreNativeFireAttackKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreNativeFireAttackCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreNativeFireAttackCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouOperation(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramEffectOp op);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> MouZhitianTop(int seat);
    bool TryResolveMouZhitianFireDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId);
}

internal interface ICoreNativeFireAttackCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreNativeFireAttackCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreNativeFireAttackCoreSequentialTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveFinishedTrickCard(long frameId, global::CardGame.Core.Card card);
}
