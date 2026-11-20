#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanXiangAnniversaryRuntime
{
    internal bool YanXiangOriginal(ProgramSkillFrame f, SkillProgramEffectOp operation)
    {
        if (_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        if (operation == SkillProgramEffectOp.ResolveAnniversaryYanXiangYuzheng)
            return f.SkillId == AnniversaryYanXiangComposition.Yuzheng && f.ActivationId == "admonish" && f.TriggerId is null && f.WindowContext is null && f.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && f.SelectedTargetSeats is [var actor] && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor);
        if (f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || f.ActivationId != f.TriggerId)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1)
            return false;
        if (operation == SkillProgramEffectOp.ResolveAnniversaryYanXiangYuzhengReward)
            return f.SkillId == AnniversaryYanXiangComposition.Yuzheng && f.TriggerId is "issued-use-reward" or "issued-response-reward" && w.Window is SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.CardResponseCompleted && _resolutionStackOperations._resolutionStack[index - 1] is ProgramCardTriggerWindowFrame p && p.Id == w.ParentFrameId && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(p.Candidates[p.CandidateIndex]));
        return operation == SkillProgramEffectOp.ResolveAnniversaryYanXiangSuishi && f.SkillId == AnniversaryYanXiangComposition.Suishi && f.TriggerId is "own-ending-damage-name" or "other-ending-damage-name" && w.Window == SkillProgramTriggerWindow.TurnEnding && _resolutionStackOperations._resolutionStack[index - 1] is TurnEndingBoundaryFrame ending && ending.Id == w.ParentFrameId && ending.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && ending.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && w.SourceSeat == ending.OwnerSeat && w.TargetSeat == ending.OwnerSeat && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) && w.OccurrenceIndex == candidate.OccurrenceIndex && w.Facts == (ending.Items[ending.ItemIndex].Facts ?? ending.Facts);
    }

    internal bool ValidYanXiang(ProgramSkillFrame f)
    {
        if (f.AnniversaryYanXiang is not { } r || f.InstructionIndex != 1 || !YanXiangOriginal(f, r.Operation) || r.Source != YanXiangSource(f) || r.Hash != f.GameplayHash || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || r.Phase != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Actor) || !Enum.IsDefined(r.Stage) || f.SelectedCardIds.Count != 0 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Any(id => id <= 0) || r.Cards.Distinct().Count() != r.Cards.Count || r.Required < 0 || r.Cards.Count > r.Required || r.Requested < 0 || r.Cursor < 0 || r.RewardLeases.Distinct().Count() != r.RewardLeases.Count || r.NativeBatches.Distinct().Count() != r.NativeBatches.Count || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYanXiangStartedEvent>().Count(e => e == new AnniversaryYanXiangStartedEvent(f.Id, r.Source, r.Hash, r.Operation, r.ParentId, r.Turn, r.Round, r.Phase, r.Actor)) != 1)
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryYanXiangYuzheng)
        {
            if (f.SelectedTargetSeats is not [var actor] || actor != r.Actor || r.Option is < 0 or > 2 || r.Plan is not null || r.Name is not null || r.RewardLeases.Count != 0 || r.UseId != 0 || r.ActionId != 0)
                return false;
            if (r.Stage == AnniversaryYanXiangStage.Option)
                return r.Option == 0 && r.Cards.Count == 0 && f.NativeOperationDrawReturns.Count == 0 && YanXiangAvailableOptions(f.OwnerSeat, f.SkillInstanceId).Length > 0;
            if (r.Option == 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYanXiangOptionChosenEvent>().Count(e => e == new AnniversaryYanXiangOptionChosenEvent(f.Id, r.Source, r.Hash, r.Turn, r.Phase, r.Actor, r.Option)) != 1)
                return false;
            if (r.Stage is not (AnniversaryYanXiangStage.HandCards or AnniversaryYanXiangStage.Drawing or AnniversaryYanXiangStage.MovementChildren or AnniversaryYanXiangStage.Completed))
                return false;
            if (r.Option == 1 && (f.NativeOperationDrawReturns.Count != 0 || r.Requested != 0 || r.Stage == AnniversaryYanXiangStage.Drawing))
                return false;
            if (r.Option == 2 && (r.Cards.Count != 0 || r.Required != 0 || r.Moves.Count != 0 || r.Requested > 5))
                return false;
        }
        else if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryYanXiangYuzhengReward)
        {
            var action = YanXiangWindowAction(f.WindowContext);
            if (f.SelectedTargetSeats.Count != 0 || action is null || r.Actor != action.ActorSeat || r.RewardAction != action.ActionId || r.Requested != 2 || r.RewardLeases.Count == 0 || r.Cursor >= r.RewardLeases.Count || r.Option != 0 || r.Cards.Count != 0 || r.Plan is not null || r.Name is not null || r.Stage is not (AnniversaryYanXiangStage.Drawing or AnniversaryYanXiangStage.MovementChildren or AnniversaryYanXiangStage.Completed) || r.RewardLeases.Any(id => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYanXiangLeaseEvent>().Any(l => l.Id == id && l.Option == 1 && l.Actor == r.Actor && l.Source.OwnerSeat == f.OwnerSeat && l.Source.SkillInstanceId == f.SkillInstanceId && l.Round == r.Round && YanXiangLeaseProof(l)) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYanXiangRewardRequestedEvent>().Count(e => e == new AnniversaryYanXiangRewardRequestedEvent(f.Id, id, r.RewardAction)) != 1))
                return false;
        }
        else
        {
            if (f.SelectedTargetSeats.Count != 0 || r.Actor != f.WindowContext?.SourceSeat || r.Option != 0 || r.Cards.Count != 0 || f.NativeOperationDrawReturns.Count != 0 || r.Requested != 0 || r.RewardLeases.Count != 0 || r.Stage is AnniversaryYanXiangStage.Option or AnniversaryYanXiangStage.HandCards or AnniversaryYanXiangStage.Drawing or AnniversaryYanXiangStage.MovementChildren)
                return false;
            if (r.Stage != AnniversaryYanXiangStage.DamageName && (r.Name is not { } name || !YanXiangDamageNames().Contains(name) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYanXiangNameDeclaredEvent>().Count(e => e == new AnniversaryYanXiangNameDeclaredEvent(f.Id, r.Actor, name)) != 1))
                return false;
            if (r.Plan is { } p && (p.CardId <= 0 || p.From.OwnerSeat != r.Actor || p.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(p.Kind) != r.Name || p.Targets.Distinct().Count() != p.Targets.Count || p.Targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t))))
                return false;
            if (r.Stage is AnniversaryYanXiangStage.IssuingUse or AnniversaryYanXiangStage.NativeUse or AnniversaryYanXiangStage.NativeTail && r.Plan is null)
                return false;
            if (r.Stage is AnniversaryYanXiangStage.NativeUse or AnniversaryYanXiangStage.NativeTail && (r.UseId <= f.Id || r.ActionId <= 0 || r.Plan is not { } p2 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYanXiangUseIssuedEvent>().Count(e => e == new AnniversaryYanXiangUseIssuedEvent(f.Id, r.UseId, r.ActionId, r.Actor, p2.CardId, p2.Kind)) != 1))
                return false;
            if (r.Returned && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYanXiangUseReturnedEvent>().Count(e => e == new AnniversaryYanXiangUseReturnedEvent(f.Id, r.UseId, r.ActionId)) != 1)
                return false;
        }

        if (r.Moves.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.From != CardLocation.Hand(r.Actor) || !r.Cards.Contains(m.CardId) || m.Reason.Value != YanXiangDiscardReason || m.Sequence <= r.Before || m.Sequence > r.After || m.To != CardLocation.DiscardPile && !_coreProgramAtomicCardMovesOperations.IsRedirectedPaidDiscardMovement(m, m.From, new(YanXiangDiscardReason))))
            return false;
        // During the next reward's Drawing stage, the previous cursor can have a completed receipt.
        if (r.Requested == 0 || r.Stage is not (AnniversaryYanXiangStage.MovementChildren or AnniversaryYanXiangStage.Completed))
            return true;

        return YanXiangCompletedDraw(f, r) is { } draw &&
            _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryYanXiangDrawReturnedEvent>().Count(e => e ==
                new AnniversaryYanXiangDrawReturnedEvent(f.Id, r.Cursor, draw.RecipientSeat, draw.RequestedCount, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter)) == 1;
    }

    private NativeDrawOutcome? YanXiangCompletedDraw(ProgramSkillFrame f, AnniversaryYanXiangReceipt r) =>
        NativeDrawProducerRuntime.CompletedOperationDraw(f, f.InstructionIndex, r.Operation, new(YanXiangDrawReason)) is { } draw &&
        draw.RecipientSeat == r.Actor && draw.RequestedCount == r.Requested && draw.SequenceBefore >= r.Before &&
        draw.SequenceAfter >= draw.SequenceBefore && draw.SequenceAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence ? draw : null;

    internal bool YanXiangMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.AnniversaryYanXiang is { Stage: AnniversaryYanXiangStage.MovementChildren or AnniversaryYanXiangStage.NativeTail } r && effect?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidYanXiang(f);
    internal bool YanXiangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidYanXiang(f) || f.AnniversaryYanXiang is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == AnniversaryYanXiangStage.NativeUse && IsYanXiangNativeUse(use);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryYanXiangStage.Drawing && CoreGameEngineNativeDrawAttemptsRuntime.MatchesProgramDrawAttempt(f, draw, r.Actor, r.Requested);
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (f.PendingMovementContinuation is not { } p || !YanXiangMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) ||
                !CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved))
                return false;

            var (before, after) = (r.Before, r.After);
            if (r.Requested > 0)
            {
                if (YanXiangCompletedDraw(f, r) is not { } outcome)
                    return false;
                (before, after) = (outcome.SequenceBefore, outcome.SequenceAfter);
            }

            return moved.Batch.Movements.All(m => m.Sequence > before && m.Sequence <= after &&
                _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        }
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal ProgramSkillFrame? YanXiangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.YanXiangFirstChild(f, c), (i) => this.YanXiangObserverSuffix(i));
    internal bool YanXiangObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, dying));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 46800)]
    internal bool YanXiangDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && YanXiangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 46800)]
    internal bool AdvanceYanXiangChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && YanXiangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 46800)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 46800)]
    internal bool YanXiangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryYanXiang is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent && YanXiangFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && (child == parent + 1 || YanXiangObserverSuffix(parent)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }
}
