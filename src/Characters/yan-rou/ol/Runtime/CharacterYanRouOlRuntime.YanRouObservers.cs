#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanRouOlRuntime
{

    internal bool IsYanRouIssuedMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.YanRou!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == YanRouStage.DrawChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Payer) && m.Reason.Value == YanRouDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle && r.Reshuffles.Any(s => s.ParentFrameId == f.Id && s.Movements.Contains(m));
        if (r.Stage != YanRouStage.DiscardChildren)
            return false;
        return m.CardId == r.CardId && m.From == r.From && m.To == (m.CardKind == CardKind.GeneralWeapon && m.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile) && m.Reason.Value == YanRouDiscardReason || r.WoodenOxGrainIds.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(r.Payer) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(ox => ox.Sequence > r.Before && ox.Sequence <= r.After && ox.CardId == r.CardId && ox.CardKind == CardKind.WoodenOx && ox.From == CardLocation.Equipment(r.Payer) && ox.Reason.Value == YanRouDiscardReason);
    }

    internal bool YanRouSilverLion(ProgramSkillFrame f) => f.YanRou is { Stage: YanRouStage.DiscardChildren } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => IsYanRouIssuedMovement(f, m) && m.CardId == r.CardId && m.From == CardLocation.Equipment(r.Payer) && m.CardKind == CardKind.SilverLion);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 720)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 660)]
    internal bool IsYanRouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.YanRou is not { } r || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement)
            return false;
        return r.Stage == YanRouStage.RecoveryChildren && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.Target && recovery.Attempt.Amount == r.Amount && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1 || r.Stage == YanRouStage.DiscardChildren && YanRouSilverLion(f) && recovery.Attempt.SourceSeat == r.Payer && recovery.Attempt.TargetSeat == r.Payer && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == YanRouDiscardReason;
    }

    internal bool YanRouFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.YanRou is not { } r || f.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsYanRouIssuedMovement(f, m)) && (moved.Batch.Movements.All(m => m.Reason != CardMoveReasons.Reshuffle) || moved.Batch.ParentBatchId is null && r.Reshuffles.Any(s => s.BatchId == moved.Batch.Id && s.ParentFrameId == moved.Batch.ParentFrameId && s.Movements.SequenceEqual(moved.Batch.Movements)));
        if (child is RecoveryReplacementFrame recovery)
            return IsYanRouRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && (r.Stage == YanRouStage.RecoveryChildren && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == r.Target && hp.Change.Amount > 0 && hp.Change.Amount <= r.Amount || YanRouSilverLion(f) && hp.Change.SourceSeat == r.Payer && hp.Change.TargetSeat == r.Payer && hp.Change.Amount == 1);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? YanRouObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.YanRouFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (c) => _coreObserverSubtreesOperations.IsMovementObserverEdge(c) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(c) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[c], _resolutionStackOperations2._resolutionStack()[c - 1]), (c, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(c, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26800)]
    internal bool AdvanceYanRouSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && YanRouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
