#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{

    internal bool CanContinueIssuedZhaoZhong(ProgramSkillFrame f)
    {
        if (f.ZhaoZhong is { Paid: true })
        {
            return true;
        }

        if (f.ZhaoZhong is not null || f.WindowContext is not { } c || f.TriggerId is null)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (IsBiriTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)))
            return IsIssuedBiriCandidate(candidate, c);
        return f.TriggerId == "issued-next-chain" && IsIssuedDiciChainCandidate(candidate, c);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28110)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28110)]
    internal bool PermitIssuedZhaoZhong(ProgramSkillFrame f) => CanContinueIssuedZhaoZhong(f);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 170)]
    internal bool IsZhaoZhongMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ZhaoZhong is { Paid: true } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is ZhaoZhongStage.GiftChildren or ZhaoZhongStage.RecoveryChildren or ZhaoZhongStage.NextGiftChildren or ZhaoZhongStage.DrawChildren;
    internal bool ZhaoZhongPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.ZhaoZhong!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<DiciGiftEntityEvent>().Any(e => e.FrameId == f.Id && e.LeaseId == r.LeaseId && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence))
            return true;
        return r.DrawOutcome is { } o && m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BiriDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 330)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 320)]
    internal bool IsZhaoZhongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => f.ZhaoZhong is { Paid: true, Stage: ZhaoZhongStage.RecoveryChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f) && child.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && child.Attempt.SourceSeat == f.OwnerSeat && child.Attempt.TargetSeat == r.TargetSeat && child.Attempt.Amount == 1 && child.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && child.Attempt.Completion.InstructionIndex == f.InstructionIndex;
    internal bool ZhaoZhongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhaoZhong is not { Paid: true } r)
            return false;
        if (r.Stage == ZhaoZhongStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is RecoveryReplacementFrame recovery)
            return IsZhaoZhongRecoveryParent(f, recovery);
        if (child is DamageFrame damage)
            return r.Stage == ZhaoZhongStage.NextDamageChildren && damage.ParentFrameId == f.Id && f.AttackAttempt is { } a && damage.SourceSeat == f.OwnerSeat && damage.TargetSeat == a.TargetSeat && damage.Amount == a.DamageAmount;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == ZhaoZhongStage.NextDamageChildren && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id && f.AttackAttempt is not null;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => ZhaoZhongPaidMovement(f, m));
    }

    internal bool ZhaoZhongLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame w && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && f.WindowContext is { } c && c.ParentFrameId == w.Id && c.Window == w.Window;
    internal bool ZhaoZhongObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _characterZhaoZhongOlOperations.ZhaoZhongLifecycleEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: not null } z && _characterSunHongOlOperations.ZenrunFirstChild(z, _resolutionStackOperations2._resolutionStack()[n]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? ZhaoZhongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhaoZhongOlOperations.ZhaoZhongFirstChild(f, child), (i) => _characterZhaoZhongOlOperations.ZhaoZhongObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1210)]
    internal bool HasZhaoZhongDamageObserver(long id) => ZhaoZhongObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1240)]
    internal bool HasZhaoZhongBeforeDamageObserver(long id) => ZhaoZhongObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28110)]
    internal bool AdvanceZhaoZhongChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhaoZhongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
