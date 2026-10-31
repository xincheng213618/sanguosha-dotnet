#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinPiOlRuntime
{
    internal bool XinPiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.XinPi is not { } r || !CanContinueXinPi(root))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (r.Stage == XinPiStage.RecoveryChildren || moved.Batch.Id != moved.Id || moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId != root.Id || moved.ResumeProgramFrameId is not null || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements.Count == 0)
                return false;
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (r.Stage == XinPiStage.ClaimChildren ? moved.Batch.Id == r.BatchId && r.Entities.Any(id => id == m.CardId && m.From == r.From[r.Entities.ToList().IndexOf(id)]) && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == ChijieClaimReason : m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Stage == XinPiStage.RecipientDrawChildren ? r.TargetSeat!.Value : root.OwnerSeat) && m.Reason.Value == YinjuDrawReason));
        }

        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (r.Stage != XinPiStage.RecoveryChildren || r.Relation is not { } relation)
            return false;
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<YinjuRecoveryPaidEvent>().Single(e => e.FrameId == root.Id);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == relation.TargetSeat && hp.Change.HpBefore == paid.HpBefore && hp.Change.Amount == paid.RequestedRecovery;
        return child is RecoveryReplacementFrame recovery && recovery.Return.ResumeFrameId == root.Id && recovery.Return.Continuation == PostEventContinuation.Program && IsXinPiRecoveryCompletion(root, recovery.Attempt);
    }

    internal bool IsXinPiRecoveryCompletion(ProgramSkillFrame f, RecoveryAttempt attempt) => f.XinPi is { Stage: XinPiStage.RecoveryChildren, Relation: { } relation } r && attempt.SourceSeat == f.OwnerSeat && attempt.TargetSeat == relation.TargetSeat && attempt.Amount == r.Requested && attempt.Completion.Producer == RecoveryAttemptProducer.Program && attempt.Completion.InstructionIndex == f.InstructionIndex && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<YinjuRecoveryPaidEvent>().Count(e => e.FrameId == f.Id && e.Relation == relation && e.TargetSeat == attempt.TargetSeat && e.SourceSeat == attempt.SourceSeat && e.HpBefore == attempt.HpBefore && e.RequestedRecovery == attempt.Amount && e.DamageAttemptId == r.DamageAttemptId) == 1;
    internal ProgramSkillFrame? XinPiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterXinPiOlOperations.XinPiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    internal bool HasXinPiCardObserver(long useId)
    {
        if (XinPiObserverRoot()is not { XinPi: { UseId: { } nativeId, ActionId: { } actionId } r } root || nativeId != useId || root.WindowContext?.ParentFrameId != r.WindowId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.WindowId)is not ProgramCardTriggerWindowFrame window || window.ParentFrameId != useId || window.Action.ActionId != actionId || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { } a } || a.ActionId != actionId)
            return false;
        return _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
    }

    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 290)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 400)]
    internal bool HasXinPiDamageObserver(long id) => XinPiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 490)]
    internal bool HasXinPiBeforeDamageObserver(long id) => XinPiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 260)]
    internal bool IsXinPiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && XinPiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 400)]
    internal bool TryAdvanceXinPiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XinPiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
