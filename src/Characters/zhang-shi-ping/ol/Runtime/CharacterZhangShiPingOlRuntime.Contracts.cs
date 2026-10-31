#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterZhangShiPingOlCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICharacterZhangShiPingOlCoreDrawPhaseObligationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDrawPhaseObligationBoundaryFrame();
}

internal interface ICharacterZhangShiPingOlCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICharacterZhangShiPingOlCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterZhangShiPingOlProgramPhaseScheduleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule { get; }
}

internal interface ICharacterZhangShiPingOlRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICharacterZhangShiPingOlCorePublicPileSourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> PublicPileSources(int seat);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source);
    global::CardGame.Core.PublicPersistentPileSource EnsurePublicPileSource(global::CardGame.Core.ProgramSkillFrame frame, int capacity);
}

internal interface ICharacterZhangShiPingOlCoreActualDiscardRecoveryPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase();
    bool IsRecordedActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase);
    void StartActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int actor);
    void ClearActualDiscardRecoveryPhase();
}

internal interface ICharacterZhangShiPingOlCoreRecoveryReplacementDamageRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath);
}

internal interface ICharacterZhangShiPingOlCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CopyFirstReplacedWeapon(global::CardGame.Core.Card card, global::CardGame.Core.Card replaced);
}

internal interface ICharacterZhangShiPingOlCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterZhangShiPingOlCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICharacterZhangShiPingOlContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterZhangShiPingOlCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICharacterZhangShiPingOlCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterZhangShiPingOlCharacterZhangShiPingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ZhangShiPingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
    bool ZhangShiPingObserverSuffix(int root);
}

internal interface ICharacterZhangShiPingOlCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICharacterZhangShiPingOlCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasObserverSuffix(int rootIndex, global::System.Func<int, bool> edge, global::System.Func<int, global::CardGame.Core.DyingFrame, bool>? completedDyingSuffix);
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterZhangShiPingOlCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAdvancedCard(int id);
}

internal interface ICharacterZhangShiPingOlCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMovementObserverEdge(int child);
    bool IsSuspendedOrPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterZhangShiPingOlCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterZhangShiPingOlCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterZhangShiPingOlCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterZhangShiPingOlCharacterCaoXiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false);
}

internal interface ICharacterZhangShiPingOlCharacterDuFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat);
}

internal interface ICharacterZhangShiPingOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterZhangShiPingOlCoreProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation);
}

internal interface ICharacterZhangShiPingOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterZhangShiPingOlCharacterChengGongYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeChengGongYingDraw(int actorSeat);
}

internal interface ICharacterZhangShiPingOlCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterZhangShiPingOlCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterZhangShiPingOlCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICharacterZhangShiPingOlResolutionStackOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack();
}

internal interface ICharacterZhangShiPingOlCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDrawPhaseProgramWindow(global::CardGame.Core.CharacterState owner, bool skipPlayPhaseAfterDraw);
    void CompleteDetachedProgramBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
    bool ResumeScheduledProgramPhase();
    void CancelScheduledProgramPhase();
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterZhangShiPingOlCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void EnterPlayPhase(global::CardGame.Core.CharacterState current);
    void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false);
    void FinalizeEndTurn(global::CardGame.Core.CharacterState previous);
}
