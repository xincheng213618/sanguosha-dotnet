namespace CardGame.Core;
public sealed partial class GameEngine
{
    private bool ValidBoundaryZf(ProgramSkillFrame frame)
    {
        if (frame.BoundaryZhouFei is not { } receipt || receipt.Source != BoundaryZfSource(frame) || receipt.Hash != frame.GameplayHash ||
            _contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || frame.InstructionIndex != 1 || frame.WindowContext is not { } context || receipt.WindowId != context.ParentFrameId ||
            frame.ActivationId != frame.TriggerId || !Enum.IsDefined(receipt.Stage) || !IsValidPlayerSeat(receipt.Holder) || receipt.Before < 0 || receipt.After < receipt.Before || receipt.After > _movementSequence ||
            receipt.Selected.Distinct().Count() != receipt.Selected.Count || receipt.Deposits.Distinct().Count() != receipt.Deposits.Count || receipt.Cursor < 0 || receipt.Cursor > receipt.Deposits.Count ||
            receipt.Payment.Select(material => material.CardId).Distinct().Count() != receipt.Payment.Count ||
            !_resolutionStack.Any(parent => parent.Id == receipt.WindowId) ||
            ProgramEventHistory<BoundaryZhouFeiStartedEvent>().Count(entry => entry == new BoundaryZhouFeiStartedEvent(frame.Id, receipt.Source, receipt.Hash, receipt.Op, receipt.WindowId, receipt.Round, receipt.Holder, receipt.Op == SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin && context.Window == SkillProgramTriggerWindow.CardsMoved ? receipt.Direction : -1)) != 1 ||
            ProgramEventHistory<ProgramBindingStartedEvent>().Count(entry => entry.FrameId == frame.Id && entry.SkillId == frame.SkillId && entry.BindingId == frame.TriggerId && entry.SkillInstanceId == frame.SkillInstanceId && entry.OwnerSeat == frame.OwnerSeat && entry.Window == context.Window) != 1) return false;
        if (receipt.Op == SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng)
        {
            if (frame.SkillId != BoundaryZfKongsheng || frame.TriggerId is not ("prepare-store" or "health-store") || receipt.Location != new CardLocation(CardZoneKind.PrivatePersistentPile, receipt.Holder, $"boundary-zhou-fei:{frame.Id}") ||
                context.Window is not (SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.AfterHealthChanged) ||
                context.Window == SkillProgramTriggerWindow.AfterHealthChanged && (context.HpChange is not { Kind: HpChangeKind.Loss or HpChangeKind.Recovery, Amount: > 0 } change || change.TargetSeat != frame.OwnerSeat) ||
                receipt.Stage is not (BoundaryZhouFeiStage.Store or BoundaryZhouFeiStage.StoreChildren)) return false;
            if (receipt.Stage == BoundaryZhouFeiStage.Store)
                return receipt.Payment.Count == 0 && receipt.Selected.All(id => _cardZones.GetLocation(id).OwnerSeat == receipt.Holder && _cardZones.GetLocation(id).Zone is CardZoneKind.Hand or CardZoneKind.Equipment);
            return receipt.Payment.Count == receipt.Selected.Count && receipt.Payment.All(material => material.From.OwnerSeat == receipt.Holder && material.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment &&
                _cardMovements.Count(move => move.Sequence > receipt.Before && move.Sequence <= receipt.After && move.CardId == material.CardId && move.From == material.From &&
                    move.To == NormalizeEphemeralCardDestination(GetAdvancedCard(material.CardId), material.From, receipt.Location!.Value) && move.Reason.Value == BoundaryZfStoreReason) == 1);
        }
        if (receipt.Op == SkillProgramEffectOp.ReturnBoundaryZhouFeiKongsheng)
        {
            if (frame.SkillId != BoundaryZfKongsheng || frame.TriggerId != "return-held" || context.Window != SkillProgramTriggerWindow.TurnEnding || context.TargetSeat != receipt.Holder ||
                receipt.Deposits.Any(id => { var deposit = BoundaryZfDeposit(id); return deposit.Holder != receipt.Holder || deposit.Source.OwnerSeat != frame.OwnerSeat || deposit.Source.SkillInstanceId != frame.SkillInstanceId || deposit.Hash != frame.GameplayHash; }) ||
                receipt.Stage is not (BoundaryZhouFeiStage.UseChoice or BoundaryZhouFeiStage.IssuingUse or BoundaryZhouFeiStage.NativeUse or BoundaryZhouFeiStage.Used or BoundaryZhouFeiStage.ReturnChildren)) return false;
            if (receipt.Stage == BoundaryZhouFeiStage.ReturnChildren)
                return receipt.Cursor < receipt.Deposits.Count && ProgramEventHistory<BoundaryZhouFeiDepositReturnedEvent>().Count(entry => entry.FrameId == frame.Id && entry.DepositId == receipt.Deposits[receipt.Cursor] && entry.Holder == receipt.Holder && entry.Count == receipt.Payment.Count && entry.Before == receipt.Before && entry.After == receipt.After) == 1 &&
                    receipt.Payment.All(material => material.From == BoundaryZfDeposit(receipt.Deposits[receipt.Cursor]).Location && _cardMovements.Any(move => move.Sequence > receipt.Before && move.Sequence <= receipt.After && move.CardId == material.CardId && move.From == material.From && move.Reason.Value == BoundaryZfReturnReason && move.To == receipt.Location));
            if (receipt.Stage == BoundaryZhouFeiStage.UseChoice) return receipt.Native is null;
            return receipt.Native is { } native && receipt.Cursor < receipt.Deposits.Count && native.ParentId == frame.Id && native.Source == receipt.Source && native.Hash == receipt.Hash && native.DepositId == receipt.Deposits[receipt.Cursor] && native.From == BoundaryZfDeposit(native.DepositId).Location &&
                (receipt.Stage == BoundaryZhouFeiStage.IssuingUse ? native.UseId == 0 && native.ActionId == 0 && _cardZones.GetLocation(native.CardId) == native.From : native.UseId > 0 && native.ActionId > 0 &&
                    ProgramEventHistory<BoundaryZhouFeiNativeEvent>().Count(entry => entry == new BoundaryZhouFeiNativeEvent(frame.Id, native.DepositId, native.UseId, native.ActionId, native.CardId)) == 1);
        }
        if (receipt.Op != SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin || frame.SkillId != BoundaryZfLiangyin) return false;
        if (context.Window == SkillProgramTriggerWindow.CardsMoved)
        {
            if (context.MovementBatch is not { } batch || batch.Id != receipt.WindowId || !BoundaryZfMovementGroups(batch).Any(group => group.Encoded == context.OccurrenceIndex && group.Direction == receipt.Direction && group.Index == context.MovementIndex) ||
                receipt.Stage is not (BoundaryZhouFeiStage.Target or BoundaryZhouFeiStage.Discard or BoundaryZhouFeiStage.DiscardChildren or BoundaryZhouFeiStage.Drawing or BoundaryZhouFeiStage.DrawChildren)) return false;
            if (receipt.Stage == BoundaryZhouFeiStage.Target) return receipt.Target == -1;
            if (!IsValidPlayerSeat(receipt.Target) || ProgramEventHistory<BoundaryZhouFeiTargetEvent>().Count(entry => entry == new BoundaryZhouFeiTargetEvent(frame.Id, frame.OwnerSeat, receipt.Round, receipt.Target, receipt.Direction == 0)) != 1) return false;
            if (receipt.Stage == BoundaryZhouFeiStage.DrawChildren) return receipt.Draw is { } draw && draw.RequestedCount == 1 && draw.RecipientSeat == receipt.Target && draw.SequenceBefore == receipt.Before && draw.SequenceAfter == receipt.After &&
                ProgramEventHistory<BoundaryZhouFeiDrawEvent>().Count(entry => entry == new BoundaryZhouFeiDrawEvent(frame.Id, receipt.Target, draw.AttemptFrameId, draw.ActualDrawIds.Count, receipt.Before, receipt.After)) == 1;
            if (receipt.Stage == BoundaryZhouFeiStage.DiscardChildren) return receipt.Payment is [var material] && material.From.OwnerSeat == receipt.Target &&
                ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(entry => entry.ParentFrameId == frame.Id && entry.InitiatorSeat == frame.OwnerSeat && entry.ExecutorSeat == receipt.Target && entry.SubjectSeat == receipt.Target && entry.ActualCount == 1 && entry.Reason.Value == BoundaryZfDiscardReason &&
                    ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(paid => paid.InvocationId == entry.InvocationId && paid.CardId == material.CardId && paid.From == material.From && paid.CompletionSequence > receipt.Before && paid.CompletionSequence <= receipt.After));
            return receipt.Draw is null && receipt.Payment.Count == 0;
        }
        return context.Window == SkillProgramTriggerWindow.RoundEnding && receipt.Stage is BoundaryZhouFeiStage.RoundChoice or BoundaryZhouFeiStage.EffectChildren &&
            (receipt.Stage == BoundaryZhouFeiStage.RoundChoice || ProgramEventHistory<BoundaryZhouFeiPaidEvent>().Count(entry => entry.FrameId == frame.Id && entry.Target == receipt.Target && entry.Branch == (receipt.Direction == 2 ? "recover" : "lose-hp") && entry.Count == 1) == 1);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 54700)]
    private bool BoundaryZfMovementProof(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation continuation) => effect is not null && ValidBoundaryZf(frame) &&
        frame.BoundaryZhouFei is { } receipt && effect.Op == receipt.Op && frame.PendingMovementContinuation == continuation && continuation.BeforeCount == 0 && continuation.CoverageResultBind is null &&
        continuation.SubjectSeat == (receipt.Op == SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin ? receipt.Target : receipt.Holder) && receipt.Stage is BoundaryZhouFeiStage.StoreChildren or BoundaryZhouFeiStage.DiscardChildren or BoundaryZhouFeiStage.DrawChildren or BoundaryZhouFeiStage.EffectChildren or BoundaryZhouFeiStage.ReturnChildren;
    private bool BoundaryZfChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (!ValidBoundaryZf(frame)) return false; var receipt = frame.BoundaryZhouFei!;
        if (child is CardUseFrame use) return receipt.Stage == BoundaryZhouFeiStage.NativeUse && BoundaryZfNativeRidesOn(use, frame);
        if (NativeDrawAttemptRidesOn(child, frame)) return receipt.Stage == BoundaryZhouFeiStage.Drawing && frame.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery) return ProgramRecoveryReplacementReceiptMatches(frame, recovery, CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp) return ProgramHpReturnMatches(frame, hp);
        if (child is CardsMovedTriggerWindowFrame moved) return frame.PendingMovementContinuation is not null && ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(move => _cardMovements.Contains(move) && move.Sequence > receipt.Before && move.Sequence <= receipt.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == frame.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram ||
            state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && ProgramEventHistory<CharacterStateChangedEvent>().Any(entry => entry.Change.Id == state.Id && entry.Change.ParentFrameId == frame.Id));
    }
    private bool BoundaryZfSuffix(int index) => HasObserverSuffix(index, static (engine, i) => engine.IsMovementObserverEdge(i) || engine.IsSameNameHandObserverEdge(i) || engine.IsYanPuDyingExitEdge(i) || engine.TypedDamagePostEventEdge(engine._resolutionStack[i - 1], engine._resolutionStack[i]) ||
        engine._resolutionStack[i - 1] is ProgramSkillFrame frame && engine.BoundaryZfChild(frame, engine._resolutionStack[i]) || NativeDrawAttemptRidesOn(engine._resolutionStack[i], engine._resolutionStack[i - 1]) || engine.RecoveryReplacementFrameRidesOn(engine._resolutionStack[i], engine._resolutionStack[i - 1]), static (engine, i, dying) => engine.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    private ProgramSkillFrame? BoundaryZfRoot() => FindProgramObserverRoot(static (engine, frame, child) => engine.BoundaryZfChild(frame, child), static (engine, index) => engine.BoundaryZfSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 54700)] [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 54700)]
    private bool BoundaryZfDamage(long id) => BoundaryZfRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == id && child is DamageTriggerWindowFrame) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 54700)]
    private bool BoundaryZfBeforeDamage(long id) => BoundaryZfRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == id && child is BeforeDamageProgramWindowFrame) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54700)] [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 54700)]
    private bool BoundaryZfDying() => ActiveDying is { } dying && BoundaryZfRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == dying.Id) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54700)] [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54700)]
    private bool BoundaryZfRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child)
    { if (!ValidBoundaryZf(frame)) return false; if (ProgramRecoveryReplacementReceiptMatches(frame, child, CompleteProgramEventHistory().ToArray())) return true; var a = _resolutionStack.FindIndex(candidate => candidate.Id == frame.Id); var b = _resolutionStack.FindIndex(candidate => candidate.Id == child.Id); return a >= 0 && b > a + 1 && BoundaryZfChild(frame, _resolutionStack[a + 1]) && BoundaryZfSuffix(a) && RecoveryReplacementFrameRidesOn(child, _resolutionStack[b - 1]); }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 54700)] [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 54700)]
    private bool KeepBoundaryZf(ProgramSkillFrame frame) => frame.BoundaryZhouFei is not null ? ValidBoundaryZf(frame) :
        frame.SkillId == BoundaryZfKongsheng && frame.TriggerId == "return-held" && frame.WindowContext is { } context &&
        IsIssuedBoundaryZf(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, 0), context);
    [ProgramContinuation(ProgramContinuationKind.Resume, 54700)]
    private bool ResumeBoundaryZf(long id) { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryZhouFei: not null } frame || frame.Id != id) return false; ContinueBoundaryZf(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 54700)]
    private bool ReturnBoundaryZfMovement(ProgramSkillFrame frame) => frame.PendingMovementContinuation is not null && ResumeBoundaryZf(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54700)]
    private bool AdvanceBoundaryZfChild() => _pendingDecision is null && BoundaryZfRoot() is not null && TryAdvanceObserverSubtree(true);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 54700)]
    private void AssertBoundaryZfFrame(ProgramSkillFrame frame) { if (frame.BoundaryZhouFei is not null && !ValidBoundaryZf(frame)) throw new InvalidOperationException("Mobile Zhou Fei lost its original holder, actual payment or typed child return."); }
}
