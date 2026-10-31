#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengGongYingOlRuntime
{

    internal bool IsChengGongYingIssuedMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.ChengGongYing!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || r.Recipient is not { } payer)
            return false;
        return r.Stage switch
        {
            ChengGongYingStage.DrawChildren => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(payer) && m.Reason.Value == KuangxiangDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle && r.Reshuffles.Any(s => s.ParentFrameId == f.Id && s.Movements.Contains(m)),
            ChengGongYingStage.DiscardChildren => r.Selected.Contains(m.CardId) && m.From == CardLocation.Hand(payer) && m.To == CardLocation.DiscardPile && m.Reason.Value == KuangxiangDiscardReason,
            _ => false
        };
    }

    internal bool ChengGongYingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ChengGongYing is not { } r || f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsChengGongYingIssuedMovement(f, m)) && (moved.Batch.Movements.All(m => m.Reason != CardMoveReasons.Reshuffle) || moved.Batch.ParentBatchId is null && r.Reshuffles.Any(s => s.BatchId == moved.Batch.Id && s.ParentFrameId == moved.Batch.ParentFrameId && s.Movements.SequenceEqual(moved.Batch.Movements)));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? ChengGongYingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.ChengGongYingFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (c) => _coreObserverSubtreesOperations.IsMovementObserverEdge(c) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(c) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[c], _resolutionStackOperations2._resolutionStack()[c - 1]), (c, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(c, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27100)]
    internal bool AdvanceChengGongYingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ChengGongYingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
