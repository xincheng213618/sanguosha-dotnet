#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaXiongMouOlRuntime
{
    internal bool ValidMouHxBojue(ProgramSkillFrame f)
    {
        if (f.MouHxBojue is not { } r || f.SkillId != MouHuaXiongOlComposition.Bojue || f.ActivationId != "simultaneous-choice" || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var peer] || peer != r.Peer || peer == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(peer) || r.Source != MouHxBojueSource(f) || r.GameplayHash != f.GameplayHash || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveMouHuaXiongBojue || !Enum.IsDefined(r.Stage) || r.Cursor is < 0 or > 2 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHxBojueStartedEvent>().Count(e => e == new MouHxBojueStartedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, peer, r.InitialHandSum)) != 1)
            return false;
        if (r.Votes.Count > 2 || r.Votes.Where((v, i) => v.Seat != (i == 0 ? f.OwnerSeat : peer)).Any())
            return false;
        if (r.Stage == MouHxBojueStage.Commit)
            return r.Cursor == 0 && r.Delta is null && f.PendingMovementContinuation is null;
        if (r.Votes.Count != 2)
            return false;
        var revealed = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHxBojueVoteRevealedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (revealed.Length != 2 || revealed.Where((e, i) => e.Index != i || e.Seat != r.Votes[i].Seat || e.Draw != r.Votes[i].Draw).Any())
            return false;
        if (r.Delta is { } delta && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHxBojueDeltaEvent>().Count(e => e.FrameId == f.Id && e.Before == r.InitialHandSum && e.AbsoluteChange == delta && Math.Abs(e.After - e.Before) == delta) != 1)
            return false;
        if (r.Stage is MouHxBojueStage.Children or MouHxBojueStage.NativeChildren)
        {
            if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat)
                return false;
            if (r.Stage == MouHxBojueStage.NativeChildren)
                return r.Returned && r.UseId > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHxBojueSlashReturnedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1;
            if (r.DrawOutcome is { } o)
                return r.Delta is null && r.PaidCard is null && o.RecipientSeat == MouHxBojueActor(f) && o.RequestedCount == 1 && r.Before == o.SequenceBefore && r.After == o.SequenceAfter && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHxBojueDrawReturnedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.Recipient == o.RecipientSeat && e.Actual == o.ActualDrawIds.Count && e.Replacement == o.ReplacementGainedIds.Count && e.AttemptId == o.AttemptFrameId) == 1;
            return r.PaidCard is { } id && r.PaidBy == MouHxBojueActor(f) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHxBojueDiscardedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.CardId == id && e.Executor == r.PaidBy && e.Followup == (r.Delta is not null) && e.Sequence > r.Before && e.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == id && m.From == e.From && m.To == e.To && m.Reason.Value == MouHxBojueDiscardReason)) == 1;
        }

        return f.PendingMovementContinuation is null;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 33300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 33300)]
    internal bool ContinuePaidMouHxBojue(ProgramSkillFrame f) => f.MouHxBojue is { Votes.Count: 2 } && ValidMouHxBojue(f);
    internal bool MouHxBojueLionPayment(ProgramSkillFrame f, int seat, CardMoveReason? reason) => f.MouHxBojue is { Stage: MouHxBojueStage.Children, PaidCard: { } id } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(seat) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason == reason);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 33300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 33300)]
    internal bool IsMouHxBojueRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => ValidMouHxBojue(f) && f.PendingMovementContinuation is not null && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && MouHxBojueLionPayment(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason);
    internal bool MouHxBojueFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouHxBojue is not { } r || !ValidMouHxBojue(f))
            return false;
        if (child is CardUseFrame native)
            return r.Stage == MouHxBojueStage.NativeUse && IsMouHxBojueUse(native);
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouHxBojueStage.Drawing && f.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return IsMouHxBojueRecovery(f, recovery);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (r.Stage == MouHxBojueStage.NativeChildren ? moved.Batch.ParentFrameId == r.UseId : m.Sequence > r.Before && m.Sequence <= r.After));
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && MouHxBojueLionPayment(f, hp.Change.TargetSeat, new(MouHxBojueDiscardReason));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouHxBojueNativeEdge(int i)
    {
        if (_resolutionStackOperations._resolutionStack[i - 1] is CardUseFrame { MouHxBojueSlash: not null } use && IsMouHxBojueUse(use) && _resolutionStackOperations._resolutionStack[i] is ProgramCardTriggerWindowFrame w)
            return w.ParentFrameId == use.Id && w.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[i - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { MouHxBojueSlash: not null } native && IsMouHxBojueUse(native) && _resolutionStackOperations._resolutionStack[i] is ProgramSkillFrame f && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, c);
        }

        return false;
    }

    internal bool MouHxBojueObserverEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || MouHxBojueNativeEdge(i) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouHxBojueFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal ProgramSkillFrame? MouHxBojueObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MouHxBojueFirstChild(f, child), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => this.MouHxBojueObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 33300)]
    internal bool IsMouHxBojueDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } d && MouHxBojueObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 33300)]
    internal bool AdvanceMouHxBojueChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouHxBojueObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
