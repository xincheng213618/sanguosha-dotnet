#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBaOlRuntime
{
    internal bool LiuBaMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.LiuBa!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == LiuBaStage.DrawChildren)
            return m.Reason == CardMoveReasons.RecastDraw && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Recipient) || m.Reason == CardMoveReasons.Reshuffle && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiuBaEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Step == r.Step && e.Sequence == m.Sequence && e.CardId == m.CardId && e.From == m.From && e.To == m.To))
            return true;
        return r.Grain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(r.Recipient) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard;
    }

    internal bool LiuBaLion(ProgramSkillFrame f, int seat) => f.LiuBa is { Paid: true } r && r.Stage != LiuBaStage.DrawChildren && r.Entities.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(seat)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiuBaEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Step == r.Step && e.Kind == CardKind.SilverLion && e.From == CardLocation.Equipment(seat));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 140)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 130)]
    internal bool IsLiuBaRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.LiuBa is { Paid: true } && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && LiuBaLion(f, recovery.Attempt.TargetSeat) && recovery.Attempt.Completion.MoveReason?.Value == (f.LiuBa.Stage == LiuBaStage.RecastChildren ? CardMoveReasons.RecastDiscard.Value : f.LiuBa.Stage == LiuBaStage.ReturnChildren ? TongduReturnReason : LiuBaExchangeReason);
    internal bool LiuBaFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LiuBa is not { Paid: true } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => LiuBaMovementMatches(f, m));
        if (child is RecoveryReplacementFrame recovery)
            return IsLiuBaRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && hp.Change.Amount > 0 && LiuBaLion(f, hp.Change.TargetSeat);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return false;
    }

    internal ProgramSkillFrame? LiuBaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.LiuBaFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27390)]
    internal bool AdvanceLiuBaChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuBaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
