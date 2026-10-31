#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanHappyGodRuntime
{
    internal bool HappySunQuanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.HappySunQuan is not { } r || effect?.Op != r.Operation || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || r.Stage is not (HappySunQuanStage.MovementChildren or HappySunQuanStage.RecoveryChildren))
            return false;
        return r.Operation == SkillProgramEffectOp.ResolveHappySunQuanShehu ? r.Payments.Count(p => p.Index == r.Index && p.CardId == r.ChosenCard && p.Target == r.Target) == 1 : r.Operation == SkillProgramEffectOp.ResolveHappySunQuanDingli && (r.Stage == HappySunQuanStage.RecoveryChildren && r.Branch == 1 || r.Draw is { } d && r.Branch == 2 && d.RecipientSeat == f.OwnerSeat && d.RequestedCount == r.Amount && d.SequenceBefore == r.Before && d.SequenceAfter == r.After && d.ActualDrawIds.SequenceEqual(r.Paid.Select(m => m.CardId)) && r.Paid.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == HappyDingliDraw));
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 40940)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 40940)]
    internal bool HappySunQuanRecovery(ProgramSkillFrame f, RecoveryReplacementFrame child) => f.HappySunQuan is { Operation: SkillProgramEffectOp.ResolveHappySunQuanDingli, Stage: HappySunQuanStage.RecoveryChildren, Branch: 1, Amount: 1 } && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f) && child.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && child.Attempt.SourceSeat == f.OwnerSeat && child.Attempt.TargetSeat == f.OwnerSeat && child.Attempt.Amount == 1 && child.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && child.Attempt.Completion.InstructionIndex == f.InstructionIndex;
    internal bool HappySunQuanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HappySunQuan is not { } r)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return HappySunQuanRecovery(f, recovery);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && HappySunQuanMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(r.Paid.Contains);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && (r.Stage == HappySunQuanStage.RecoveryChildren && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == f.OwnerSeat || r.Stage == HappySunQuanStage.HpLoss && hp.Continuation is PostEventContinuation.Program or PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == f.OwnerSeat);
        return child is DyingFrame dying && r.Stage == HappySunQuanStage.HpLoss && r.Branch == 2 && dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.Continuation == DyingContinuationKind.ProgramSkill;
    }

    internal ProgramSkillFrame? HappySunQuanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterSunQuanHappyGodOperations.HappySunQuanFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 40940)]
    internal bool HappySunQuanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && HappySunQuanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 40940)]
    internal bool AdvanceHappySunQuanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HappySunQuanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40940)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40940)]
    internal bool RetainHappySunQuanPaid(ProgramSkillFrame f)
    {
        if (f.HappySunQuan is { } r && (r.Operation == SkillProgramEffectOp.ResolveHappySunQuanLearning || r.Operation == SkillProgramEffectOp.GrantHappySunQuanLearning && r.Targets.Count > 0 || r.Payments.Count > 0 || r.Draw is not null))
        {
            return true;
        }

        return f.SkillId == HappySunQuanComposition.Learning && f.TriggerId == "resolve-study" && f.InstructionIndex == 0 && f.WindowContext is { } w && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId) == 1 && IsIssuedHappyLearning(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), w);
    }
}
