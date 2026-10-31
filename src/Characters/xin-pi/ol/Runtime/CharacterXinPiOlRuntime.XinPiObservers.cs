#nullable enable

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
    internal ProgramSkillFrame? XinPiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.XinPiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 400)]
    internal bool TryAdvanceXinPiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XinPiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
