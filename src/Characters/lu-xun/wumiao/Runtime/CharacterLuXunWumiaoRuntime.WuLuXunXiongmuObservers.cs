#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal bool WuXiongmuPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.Xiongmu is not { } r || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == XiongmuStage.InsertChildren)
            return r.PutMovements.Contains(m) || r.WoodenOxSpill.Contains(m);
        if (r.Stage == XiongmuStage.ObtainChildren)
            return r.ObtainMovements.Contains(m);
        return r.Stage == XiongmuStage.DrawChildren && r.DrawOutcome is { } draw && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && (draw.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == XiongmuDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    }

    internal bool WuXiongmuSilverLion(ProgramSkillFrame f) => f.Xiongmu is { Stage: XiongmuStage.InsertChildren } r && r.PutMovements.Any(m => m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(f.OwnerSeat) && m.Reason.Value == XiongmuPutReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
    internal bool WuXiongmuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Xiongmu is not { } r || !ValidWuXiongmuStarted(f))
            return false;
        if (r.Stage == XiongmuStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is RecoveryReplacementFrame recovery)
            return IsWuXiongmuRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == XiongmuStage.InsertChildren && WuXiongmuSilverLion(f) && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window && e.Change.TargetSeat == state.OwnerSeat);
        return child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => WuXiongmuPaidMovement(f, m));
    }

    internal bool WuXiongmuLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame window && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]) && f.WindowContext is { } context && context.ParentFrameId == window.Id && context.Window == window.Window;
    internal bool WuXiongmuObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _characterLuXunWumiaoOperations.WuXiongmuLifecycleEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: { Paid: true } } replacement && _characterSunHongOlOperations.ZenrunFirstChild(replacement, _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? WuXiongmuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterLuXunWumiaoOperations.WuXiongmuFirstChild(f, child), (i) => _characterLuXunWumiaoOperations.WuXiongmuObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1210)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1320)]
    internal bool HasWuXiongmuDamageObserver(long id) => WuXiongmuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1350)]
    internal bool HasWuXiongmuBeforeDamageObserver(long id) => WuXiongmuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 440)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 430)]
    internal bool IsWuXiongmuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Xiongmu is null)
            return false;
        if (WuXiongmuSilverLion(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == XiongmuPutReason)
            return true;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && WuXiongmuFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && WuXiongmuObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29000)]
    internal bool AdvanceWuXiongmuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuXiongmuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
