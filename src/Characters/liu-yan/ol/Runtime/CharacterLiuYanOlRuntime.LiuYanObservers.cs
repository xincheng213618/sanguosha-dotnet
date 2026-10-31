#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYanOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 870)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 810)]
    internal bool IsLiuYanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.LiuYan is { Stage: LiuYanStage.LimuRecoveryChildren, RecoveryIssued: true } && ValidLiuYan(f) && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == f.InstructionIndex && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1;
    internal bool LiuYanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LiuYan is not { } r || !ValidLiuYan(f))
            return false;
        if (child is CardUseFrame use)
            return r.Stage == LiuYanStage.LimuNative && use.LiuYanLimu is { } o && MatchesLiuYanLimu(f, o) && IsLiuYanLimuUse(use.Id);
        if (child is RecoveryReplacementFrame recovery)
            return IsLiuYanRecoveryParent(f, recovery);
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.ResumeProgramFrameId != f.Id || moved.Batch.Movements.Count == 0)
                return false;
            if (r.Stage == LiuYanStage.TusheDrawChildren)
                return moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == LiuYanDrawReason || m.Reason == CardMoveReasons.Reshuffle));
            if (r.Stage is LiuYanStage.LimuReturnedChildren or LiuYanStage.LimuRecoveryChildren && r.Use is { } origin)
                return moved.Batch.ParentFrameId is { } parent && (parent == f.Id || parent == origin.CardUseFrameId) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.CardId == origin.CardId && m.CardKind == origin.PrintedKind && (m.From == origin.From && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use || m.From == CardLocation.Processing && (m.To == CardLocation.Judgment(origin.ActorSeat) && m.Reason == CardMoveReasons.DelayedCardPlace || m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished)) || origin.PrintedKind == CardKind.WoodenOx && origin.From == CardLocation.Equipment(origin.ActorSeat) && r.Auxiliaries.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind && c.From == m.From) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
            return false;
        }

        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == LiuYanStage.LimuRecoveryChildren && r.RecoveryIssued && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount > 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiuYanLimuRecoveryIssuedEvent>().Count(e => e.FrameId == f.Id) == 1;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? LiuYanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.LiuYanFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27230)]
    internal bool AdvanceLiuYanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuYanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
