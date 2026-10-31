#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoLeRuntime
{
    internal bool WeiwanLionRemoval(ProgramSkillFrame f, int seat) => f.Weiwan is { } r && (r.Stage == WeiwanStage.CostChildren && r.PrintedKind == CardKind.SilverLion && r.CostFrom == CardLocation.Equipment(seat) && r.CostTo != r.CostFrom || r.Stage == WeiwanStage.ObtainChildren && r.Samples.Any(s => s.PrintedKind == CardKind.SilverLion && s.From == CardLocation.Equipment(seat)));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 380)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 370)]
    internal bool IsWeiwanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Weiwan is not { } r || !HasPaidWeiwanSelection(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement)
            return r.Stage is WeiwanStage.CostChildren or WeiwanStage.ObtainChildren && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason == new CardMoveReason(r.Stage == WeiwanStage.CostChildren ? WeiwanCostReason : WeiwanObtainReason) && WeiwanLionRemoval(f, recovery.Attempt.TargetSeat);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == recovery.Id);
        return index >= 0 && child > index + 1 && WeiwanFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && WeiwanObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    internal bool WeiwanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Weiwan is not { } r || !HasPaidWeiwanSelection(f))
            return false;
        AssertWeiwan(f);
        if (child is IdentityAppearanceFrame appearance)
            return r.Stage == WeiwanStage.HealthIssuing && r.Branch == WeiwanBranch.LoseHp && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.ParentFrameId == f.Id && appearance.Receipt.OwnerSeat == r.TargetSeat && appearance.Receipt.RequestedDeduction == 1 && appearance.Receipt.OriginalInstructionIndex == f.InstructionIndex;
        if (child is DyingFrame dying)
            return r.Stage == WeiwanStage.HealthChildren && r.Branch == WeiwanBranch.LoseHp && r.HpLossCaptured && dying.ParentFrameId == f.Id && dying.VictimSeat == r.TargetSeat && dying.KillerSeat is null && dying.Continuation == DyingContinuationKind.ProgramSkill;
        if (child is RecoveryReplacementFrame recovery)
            return IsWeiwanRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && (r.Stage == WeiwanStage.HealthChildren && r.Branch == WeiwanBranch.LoseHp && r.HpLossCaptured && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == r.TargetSeat && hp.Change.SourceSeat is null && hp.Change.Amount == 1 || r.Stage is WeiwanStage.CostChildren or WeiwanStage.ObtainChildren && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && hp.Change.Amount == 1 && WeiwanLionRemoval(f, hp.Change.TargetSeat));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && (moved.Batch.Id == (r.Stage == WeiwanStage.CostChildren ? r.CostBatchId : r.Stage == WeiwanStage.ObtainChildren ? r.ObtainBatchId : 0) && moved.Batch.Movements.All(m => WeiwanPrimaryMovement(f, m)) || WeiwanOxSpill(f, moved.Batch));
    }

    internal bool WeiwanLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame w && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && f.WindowContext is { } context && context.ParentFrameId == w.Id && context.Window == w.Window;
    internal bool WeiwanObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(next) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(next) || _characterXiaoQiaoLeOperations.WeiwanLifecycleEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || _resolutionStackOperations2._resolutionStack()[next - 1] is ProgramSkillFrame { Zenrun: not null } zenrun && _characterSunHongOlOperations.ZenrunFirstChild(zenrun, _resolutionStackOperations2._resolutionStack()[next]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(next, dying));
    internal ProgramSkillFrame? WeiwanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterXiaoQiaoLeOperations.WeiwanFirstChild(f, child), (index) => _characterXiaoQiaoLeOperations.WeiwanObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1150)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1260)]
    internal bool HasWeiwanDamageObserver(long id) => WeiwanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1290)]
    internal bool HasWeiwanBeforeDamageObserver(long id) => WeiwanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1120)]
    internal bool IsWeiwanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && WeiwanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28620)]
    internal bool AdvanceWeiwanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WeiwanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
