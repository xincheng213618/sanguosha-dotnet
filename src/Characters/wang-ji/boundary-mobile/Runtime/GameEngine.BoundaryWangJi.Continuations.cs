namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool ValidBoundaryWj(ProgramSkillFrame frame)
    {
        if (frame.BoundaryWangJi is not { } receipt || receipt.Source != BoundaryWjSource(frame) || receipt.GameplayHash != frame.GameplayHash ||
            _contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || frame.InstructionIndex != 1 ||
            !Enum.IsDefined(receipt.Stage) || receipt.Before < 0 || receipt.After < receipt.Before || receipt.After > _movementSequence ||
            receipt.Cursor < 0 || receipt.Cursor > receipt.Targets.Count || receipt.Targets.Distinct().Count() != receipt.Targets.Count || receipt.Targets.Any(seat => !IsValidPlayerSeat(seat)) ||
            receipt.Viewed.Distinct().Count() != receipt.Viewed.Count ||
            ProgramEventHistory<BoundaryWangJiStartedEvent>().Count(entry => entry == new BoundaryWangJiStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.Op, receipt.WindowId, receipt.ActionId, receipt.Phase)) != 1) return false;
        if (receipt.Op == SkillProgramEffectOp.SpendBoundaryWangJiQizhi)
        {
            if (frame.SkillId != BoundaryWjQizhi || frame.ActivationId != "charge-draw" || frame.TriggerId is not null || frame.WindowContext is not null || receipt.WindowId != 0 || receipt.ActionId != 0 ||
                receipt.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } paidPhase || paidPhase.ActorSeat != frame.OwnerSeat || !IsRecordedActualDiscardRecoveryPhase(paidPhase) ||
                receipt.Targets is not [var recipient] || !receipt.Targets.SequenceEqual(frame.SelectedTargetSeats) || receipt.Requested != 2 || receipt.Stage is not (BoundaryWangJiStage.Drawing or BoundaryWangJiStage.DrawChildren) ||
                ProgramEventHistory<BoundaryWangJiSpentEvent>().Count(entry => entry == new BoundaryWangJiSpentEvent(frame.Id, receipt.Source, receipt.GameplayHash, paidPhase, recipient)) != 1 ||
                ProgramEventHistory<PlayerMarkerChangedEvent>().Count(entry => entry.ResolutionId == frame.Id && entry.PlayerSeat == frame.OwnerSeat && entry.Marker == PlayerMarkerKind.BoundaryWangJiCharge && entry.Delta == -1 && entry.Reason == BoundaryWjQizhi) != 1) return false;
            return receipt.Stage == BoundaryWangJiStage.Drawing && receipt.Draw is null || receipt.Stage == BoundaryWangJiStage.DrawChildren && receipt.Draw is { } activationDraw && activationDraw.RecipientSeat == recipient && activationDraw.RequestedCount == 2 &&
                activationDraw.SequenceBefore == receipt.Before && activationDraw.SequenceAfter == receipt.After && ProgramEventHistory<BoundaryWangJiDrawReturnedEvent>().Count(entry => entry == new BoundaryWangJiDrawReturnedEvent(frame.Id, 2, activationDraw.ActualDrawIds.Count, activationDraw.AttemptFrameId)) == 1;
        }
        if (frame.WindowContext is not { } context || context.ParentFrameId != receipt.WindowId || frame.ActivationId != frame.TriggerId || !_resolutionStack.Any(parent => parent.Id == receipt.WindowId) ||
            ProgramEventHistory<ProgramBindingStartedEvent>().Count(entry => entry.FrameId == frame.Id && entry.SkillId == frame.SkillId && entry.BindingId == frame.TriggerId && entry.SkillInstanceId == frame.SkillInstanceId && entry.OwnerSeat == frame.OwnerSeat && entry.Window == context.Window) != 1) return false;
        if (receipt.Op == SkillProgramEffectOp.ResolveBoundaryWangJiQizhi)
        {
            if (frame.SkillId != BoundaryWjQizhi || frame.TriggerId != "inspect-recipients" || context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized ||
                receipt.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != frame.OwnerSeat || !IsRecordedActualDiscardRecoveryPhase(phase) ||
                !receipt.Targets.SequenceEqual(BoundaryWjRecipients(frame.OwnerSeat, phase)) || context.CardUse is not { } identity || identity.CardActionId != receipt.ActionId ||
                LifecycleCardUse(identity.ParentCardUseFrameId) is not { Action: { } action } || action.ActionId != identity.CardActionId || action.ActorSeat != frame.OwnerSeat || !BoundaryWjCard(action) ||
                receipt.Stage is not (BoundaryWangJiStage.Inspect or BoundaryWangJiStage.DiscardChildren) || receipt.Draw is not null) return false;
            if (receipt.Stage == BoundaryWangJiStage.DiscardChildren)
            {
                if (receipt.Payment is not { } payment || receipt.Cursor >= receipt.Targets.Count || payment.FrameId != frame.Id || payment.Index != receipt.Cursor || payment.TargetSeat != receipt.Targets[receipt.Cursor] ||
                    payment.From != CardLocation.Hand(payment.TargetSeat) || payment.To != CardLocation.DiscardPile || !receipt.Viewed.Contains(payment.CardId) || payment.Sequence <= receipt.Before || payment.Sequence > receipt.After ||
                    ProgramEventHistory<BoundaryWangJiDiscardedEvent>().Count(entry => entry == payment) != 1 ||
                    !ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(entry => entry.ParentFrameId == frame.Id && entry.InitiatorSeat == frame.OwnerSeat && entry.ExecutorSeat == frame.OwnerSeat && entry.SubjectSeat == payment.TargetSeat && entry.ActualCount == 1 && entry.HandCount == 1 && entry.SequenceBefore == receipt.Before && entry.SequenceAfter <= receipt.After && entry.Reason.Value == BoundaryWjDiscardReason &&
                        ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(material => material.InvocationId == entry.InvocationId && material.CardId == payment.CardId && material.From == payment.From && material.To == payment.To && material.CompletionSequence == payment.Sequence)) ||
                    !_cardMovements.Any(move => move.Sequence == payment.Sequence && move.CardId == payment.CardId && move.CardKind == payment.Kind && move.From == payment.From && move.To == payment.To && move.Reason.Value == BoundaryWjDiscardReason)) return false;
            }
            else if (receipt.Payment is not null || receipt.Viewed.Count > 0 && (receipt.Cursor >= receipt.Targets.Count || !receipt.Viewed.SequenceEqual(GetHand(_players[receipt.Targets[receipt.Cursor]]).Select(card => card.Id)))) return false;
        }
        else
        {
            if (receipt.Op != SkillProgramEffectOp.ResolveBoundaryWangJiJinqu || frame.SkillId != BoundaryWjJinqu || receipt.Targets.Count != 0 || receipt.Viewed.Count != 0 || receipt.Payment is not null) return false;
            if (frame.TriggerId == "paired-use-charge")
            {
                if (receipt.Stage != BoundaryWangJiStage.Charge || !BoundaryWjCompletedWindow(context, out var window) || window.Action.ActionId != receipt.ActionId || window.Action.ActorSeat != frame.OwnerSeat ||
                    BoundaryWjTurn() is not { } turn || turn.OwnerSeat != frame.OwnerSeat || ProgramEventHistory<BoundaryWangJiUseEvent>().Count(entry => entry.FrameId == frame.Id && entry.OwnerSeat == frame.OwnerSeat && entry.TurnOrdinal == turn.Ordinal && entry.ActionId == receipt.ActionId) != 1) return false;
            }
            else if (frame.TriggerId != "ending-peak" || context.Window != SkillProgramTriggerWindow.TurnEnding || receipt.Requested is < 1 or > 2 ||
                receipt.Stage is not (BoundaryWangJiStage.Drawing or BoundaryWangJiStage.DrawChildren) || BoundaryWjTurn() is not { } endingTurn ||
                ProgramEventHistory<BoundaryWangJiPeakFrozenEvent>().Count(entry => entry == new BoundaryWangJiPeakFrozenEvent(frame.Id, frame.OwnerSeat, endingTurn.Ordinal, receipt.Requested)) != 1) return false;
            if (receipt.Stage == BoundaryWangJiStage.DrawChildren && (receipt.Draw is not { } draw || draw.RecipientSeat != frame.OwnerSeat || draw.RequestedCount != receipt.Requested ||
                draw.SequenceBefore != receipt.Before || draw.SequenceAfter != receipt.After || ProgramEventHistory<BoundaryWangJiDrawReturnedEvent>().Count(entry => entry == new BoundaryWangJiDrawReturnedEvent(frame.Id, draw.RequestedCount, draw.ActualDrawIds.Count, draw.AttemptFrameId)) != 1)) return false;
        }
        return true;
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 54400)]
    private bool BoundaryWjMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation continuation) =>
        effect is not null && ValidBoundaryWj(frame) && frame.BoundaryWangJi is { } receipt && effect.Op == receipt.Op &&
        continuation == frame.PendingMovementContinuation && continuation.BeforeCount == 0 && continuation.CoverageResultBind is null &&
        (receipt.Stage == BoundaryWangJiStage.DiscardChildren && continuation.SubjectSeat == receipt.Targets[receipt.Cursor] || receipt.Stage == BoundaryWangJiStage.DrawChildren && continuation.SubjectSeat == receipt.Draw!.RecipientSeat);
    private bool BoundaryWjChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (!ValidBoundaryWj(frame)) return false; var receipt = frame.BoundaryWangJi!;
        if (NativeDrawAttemptRidesOn(child, frame)) return receipt.Stage == BoundaryWangJiStage.Drawing && frame.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery) return ProgramRecoveryReplacementReceiptMatches(frame, recovery, CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp) return ProgramHpReturnMatches(frame, hp);
        if (child is CardsMovedTriggerWindowFrame moved) return frame.PendingMovementContinuation is not null && ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(move => _cardMovements.Contains(move) && move.Sequence > receipt.Before && move.Sequence <= receipt.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == frame.Id &&
            (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram ||
             state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && ProgramEventHistory<CharacterStateChangedEvent>().Any(entry => entry.Change.Id == state.Id && entry.Change.ParentFrameId == frame.Id));
    }
    private bool BoundaryWjSuffix(int index) => HasObserverSuffix(index, static (engine, i) => engine.IsMovementObserverEdge(i) || engine.IsSameNameHandObserverEdge(i) || engine.IsYanPuDyingExitEdge(i) || engine.TypedDamagePostEventEdge(engine._resolutionStack[i - 1], engine._resolutionStack[i]) ||
        engine._resolutionStack[i - 1] is ProgramSkillFrame frame && engine.BoundaryWjChild(frame, engine._resolutionStack[i]) || NativeDrawAttemptRidesOn(engine._resolutionStack[i], engine._resolutionStack[i - 1]) || engine.RecoveryReplacementFrameRidesOn(engine._resolutionStack[i], engine._resolutionStack[i - 1]), static (engine, i, dying) => engine.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    private ProgramSkillFrame? BoundaryWjRoot() => FindProgramObserverRoot(static (engine, frame, child) => engine.BoundaryWjChild(frame, child), static (engine, index) => engine.BoundaryWjSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 54400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 54400)]
    private bool BoundaryWjDamage(long id) => BoundaryWjRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == id && child is DamageTriggerWindowFrame) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 54400)]
    private bool BoundaryWjBeforeDamage(long id) => BoundaryWjRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == id && child is BeforeDamageProgramWindowFrame) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54400)]
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 54400)]
    private bool BoundaryWjDying() => ActiveDying is { } dying && BoundaryWjRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == dying.Id) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54400)]
    private bool BoundaryWjRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child)
    {
        if (!ValidBoundaryWj(frame)) return false;
        if (ProgramRecoveryReplacementReceiptMatches(frame, child, CompleteProgramEventHistory().ToArray())) return true;
        var a = _resolutionStack.FindIndex(candidate => candidate.Id == frame.Id); var b = _resolutionStack.FindIndex(candidate => candidate.Id == child.Id);
        return a >= 0 && b > a + 1 && BoundaryWjChild(frame, _resolutionStack[a + 1]) && BoundaryWjSuffix(a) && RecoveryReplacementFrameRidesOn(child, _resolutionStack[b - 1]);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 54400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 54400)]
    private bool KeepBoundaryWj(ProgramSkillFrame frame) => frame.BoundaryWangJi is not null && ValidBoundaryWj(frame);
    [ProgramContinuation(ProgramContinuationKind.Resume, 54400)]
    private bool ResumeBoundaryWj(long id)
    { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryWangJi: not null } frame || frame.Id != id) return false; ContinueBoundaryWj(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 54400)]
    private bool ReturnBoundaryWjMovement(ProgramSkillFrame frame) => frame.PendingMovementContinuation is not null && ResumeBoundaryWj(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54400)]
    private bool AdvanceBoundaryWjChild() => _pendingDecision is null && BoundaryWjRoot() is not null && TryAdvanceObserverSubtree(true);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 54400)]
    private void AssertBoundaryWj(ProgramSkillFrame frame)
    { if (frame.BoundaryWangJi is not null && !ValidBoundaryWj(frame)) throw new InvalidOperationException("Wang Ji lost its original frame, designated Use or native payment proof."); }
}
