#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangJiBoundaryMobileRuntime
{
    internal bool ValidBoundaryWj(ProgramSkillFrame frame)
    {
        if (frame.BoundaryWangJi is not { } receipt || receipt.Source != BoundaryWjSource(frame) || receipt.GameplayHash != frame.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || frame.InstructionIndex != 1 || !Enum.IsDefined(receipt.Stage) || receipt.Before < 0 || receipt.After < receipt.Before || receipt.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || receipt.Cursor < 0 || receipt.Cursor > receipt.Targets.Count || receipt.Targets.Distinct().Count() != receipt.Targets.Count || receipt.Targets.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || receipt.Viewed.Distinct().Count() != receipt.Viewed.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWangJiStartedEvent>().Count(entry => entry == new BoundaryWangJiStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.Op, receipt.WindowId, receipt.ActionId, receipt.Phase)) != 1)
            return false;
        if (receipt.Op == SkillProgramEffectOp.SpendBoundaryWangJiQizhi)
        {
            if (frame.SkillId != BoundaryWjQizhi || frame.ActivationId != "charge-draw" || frame.TriggerId is not null || frame.WindowContext is not null || receipt.WindowId != 0 || receipt.ActionId != 0 || receipt.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } paidPhase || paidPhase.ActorSeat != frame.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(paidPhase) || receipt.Targets is not [var recipient] || !receipt.Targets.SequenceEqual(frame.SelectedTargetSeats) || receipt.Requested != 2 || receipt.Stage is not (BoundaryWangJiStage.Drawing or BoundaryWangJiStage.DrawChildren) || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWangJiSpentEvent>().Count(entry => entry == new BoundaryWangJiSpentEvent(frame.Id, receipt.Source, receipt.GameplayHash, paidPhase, recipient)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerMarkerChangedEvent>().Count(entry => entry.ResolutionId == frame.Id && entry.PlayerSeat == frame.OwnerSeat && entry.Marker == PlayerMarkerKind.BoundaryWangJiCharge && entry.Delta == -1 && entry.Reason == BoundaryWjQizhi) != 1)
                return false;
            return receipt.Stage == BoundaryWangJiStage.Drawing && receipt.Draw is null || receipt.Stage == BoundaryWangJiStage.DrawChildren && receipt.Draw is { } activationDraw && activationDraw.RecipientSeat == recipient && activationDraw.RequestedCount == 2 && activationDraw.SequenceBefore == receipt.Before && activationDraw.SequenceAfter == receipt.After && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWangJiDrawReturnedEvent>().Count(entry => entry == new BoundaryWangJiDrawReturnedEvent(frame.Id, 2, activationDraw.ActualDrawIds.Count, activationDraw.AttemptFrameId)) == 1;
        }

        if (frame.WindowContext is not { } context || context.ParentFrameId != receipt.WindowId || frame.ActivationId != frame.TriggerId || !_resolutionStackOperations._resolutionStack.Any(parent => parent.Id == receipt.WindowId) || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(entry => entry.FrameId == frame.Id && entry.SkillId == frame.SkillId && entry.BindingId == frame.TriggerId && entry.SkillInstanceId == frame.SkillInstanceId && entry.OwnerSeat == frame.OwnerSeat && entry.Window == context.Window) != 1)
            return false;
        if (receipt.Op == SkillProgramEffectOp.ResolveBoundaryWangJiQizhi)
        {
            if (frame.SkillId != BoundaryWjQizhi || frame.TriggerId != "inspect-recipients" || context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || receipt.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != frame.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || !receipt.Targets.SequenceEqual(BoundaryWjRecipients(frame.OwnerSeat, phase)) || context.CardUse is not { } identity || identity.CardActionId != receipt.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { } action } || action.ActionId != identity.CardActionId || action.ActorSeat != frame.OwnerSeat || !BoundaryWjCard(action) || receipt.Stage is not (BoundaryWangJiStage.Inspect or BoundaryWangJiStage.DiscardChildren) || receipt.Draw is not null)
                return false;
            if (receipt.Stage == BoundaryWangJiStage.DiscardChildren)
            {
                if (receipt.Payment is not { } payment || receipt.Cursor >= receipt.Targets.Count || payment.FrameId != frame.Id || payment.Index != receipt.Cursor || payment.TargetSeat != receipt.Targets[receipt.Cursor] || payment.From != CardLocation.Hand(payment.TargetSeat) || payment.To != CardLocation.DiscardPile || !receipt.Viewed.Contains(payment.CardId) || payment.Sequence <= receipt.Before || payment.Sequence > receipt.After || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWangJiDiscardedEvent>().Count(entry => entry == payment) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(entry => entry.ParentFrameId == frame.Id && entry.InitiatorSeat == frame.OwnerSeat && entry.ExecutorSeat == frame.OwnerSeat && entry.SubjectSeat == payment.TargetSeat && entry.ActualCount == 1 && entry.HandCount == 1 && entry.SequenceBefore == receipt.Before && entry.SequenceAfter <= receipt.After && entry.Reason.Value == BoundaryWjDiscardReason && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(material => material.InvocationId == entry.InvocationId && material.CardId == payment.CardId && material.From == payment.From && material.To == payment.To && material.CompletionSequence == payment.Sequence)) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == payment.Sequence && move.CardId == payment.CardId && move.CardKind == payment.Kind && move.From == payment.From && move.To == payment.To && move.Reason.Value == BoundaryWjDiscardReason))
                    return false;
            }
            else if (receipt.Payment is not null || receipt.Viewed.Count > 0 && (receipt.Cursor >= receipt.Targets.Count || !receipt.Viewed.SequenceEqual(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.Targets[receipt.Cursor]]).Select(card => card.Id))))
                return false;
        }
        else
        {
            if (receipt.Op != SkillProgramEffectOp.ResolveBoundaryWangJiJinqu || frame.SkillId != BoundaryWjJinqu || receipt.Targets.Count != 0 || receipt.Viewed.Count != 0 || receipt.Payment is not null)
                return false;
            if (frame.TriggerId == "paired-use-charge")
            {
                if (receipt.Stage != BoundaryWangJiStage.Charge || !BoundaryWjCompletedWindow(context, out var window) || window.Action.ActionId != receipt.ActionId || window.Action.ActorSeat != frame.OwnerSeat || BoundaryWjTurn()is not { } turn || turn.OwnerSeat != frame.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWangJiUseEvent>().Count(entry => entry.FrameId == frame.Id && entry.OwnerSeat == frame.OwnerSeat && entry.TurnOrdinal == turn.Ordinal && entry.ActionId == receipt.ActionId) != 1)
                    return false;
            }
            else if (frame.TriggerId != "ending-peak" || context.Window != SkillProgramTriggerWindow.TurnEnding || receipt.Requested is < 1 or > 2 || receipt.Stage is not (BoundaryWangJiStage.Drawing or BoundaryWangJiStage.DrawChildren) || BoundaryWjTurn()is not { } endingTurn || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWangJiPeakFrozenEvent>().Count(entry => entry == new BoundaryWangJiPeakFrozenEvent(frame.Id, frame.OwnerSeat, endingTurn.Ordinal, receipt.Requested)) != 1)
                return false;
            if (receipt.Stage == BoundaryWangJiStage.DrawChildren && (receipt.Draw is not { } draw || draw.RecipientSeat != frame.OwnerSeat || draw.RequestedCount != receipt.Requested || draw.SequenceBefore != receipt.Before || draw.SequenceAfter != receipt.After || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWangJiDrawReturnedEvent>().Count(entry => entry == new BoundaryWangJiDrawReturnedEvent(frame.Id, draw.RequestedCount, draw.ActualDrawIds.Count, draw.AttemptFrameId)) != 1))
                return false;
        }

        return true;
    }
    internal bool BoundaryWjChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (!ValidBoundaryWj(frame))
            return false;
        var receipt = frame.BoundaryWangJi!;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, frame))
            return receipt.Stage == BoundaryWangJiStage.Drawing && frame.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(frame, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return frame.PendingMovementContinuation is not null && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > receipt.Before && move.Sequence <= receipt.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == frame.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(entry => entry.Change.Id == state.Id && entry.Change.ParentFrameId == frame.Id));
    }

    internal bool BoundaryWjSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame frame && this.BoundaryWjChild(frame, _resolutionStackOperations2._resolutionStack()[i]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? BoundaryWjRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => this.BoundaryWjChild(frame, child), (index) => this.BoundaryWjSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54400)]
    internal bool BoundaryWjDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && BoundaryWjRoot()is { } frame && _resolutionStackOperations._resolutionStack.FindIndex(child => child.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54400)]
    internal bool BoundaryWjRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child)
    {
        if (!ValidBoundaryWj(frame))
            return false;
        if (_coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, child, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(candidate => candidate.Id == frame.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(candidate => candidate.Id == child.Id);
        return a >= 0 && b > a + 1 && BoundaryWjChild(frame, _resolutionStackOperations._resolutionStack[a + 1]) && BoundaryWjSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 54400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 54400)]
    internal bool KeepBoundaryWj(ProgramSkillFrame frame) => frame.BoundaryWangJi is not null && ValidBoundaryWj(frame);
    [ProgramContinuation(ProgramContinuationKind.Resume, 54400)]
    internal bool ResumeBoundaryWj(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryWangJi: not null } frame || frame.Id != id)
            return false;
        ContinueBoundaryWj(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 54400)]
    internal bool ReturnBoundaryWjMovement(ProgramSkillFrame frame) => frame.PendingMovementContinuation is not null && ResumeBoundaryWj(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54400)]
    internal bool AdvanceBoundaryWjChild() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryWjRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
