namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool ValidBoundaryXiahouDun(ProgramSkillFrame f)
    {
        if (f.BoundaryXiahouDun is not { } r || f.SkillId != BoundaryXiahouDunQingjian || r.Source != BoundaryXiahouDunSource(f) || r.Hash != f.GameplayHash ||
            _contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId ||
            f.WindowContext is not { } context || context.ParentFrameId != r.WindowId || context.OwnerSeat != f.OwnerSeat || !_resolutionStack.Any(p => p.Id == r.WindowId) ||
            !Enum.IsDefined(r.Stage) || r.Given < 0 || r.Before < 0 || r.After < r.Before || r.After > _movementSequence ||
            r.Selected.Distinct().Count() != r.Selected.Count || r.Deposits.Distinct().Count() != r.Deposits.Count || r.Payment.Select(p => p.CardId).Distinct().Count() != r.Payment.Count ||
            ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId &&
                e.SkillInstanceId == f.SkillInstanceId && e.Window == context.Window) != 1) return false;
        if (r.Op == SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian)
        {
            if (f.TriggerId != "store-after-gain" || context.Window != SkillProgramTriggerWindow.CardsGained || r.Given != 0 || r.Deposits.Count != 0 ||
                r.Location != new CardLocation(CardZoneKind.PrivatePersistentPile, f.OwnerSeat, $"boundary-xiahou-dun:{f.Id}") || r.Stage is not (BoundaryXiahouDunStage.Selecting or BoundaryXiahouDunStage.StoreChildren)) return false;
            if (r.Stage == BoundaryXiahouDunStage.Selecting) return r.Payment.Count == 0 && r.Selected.All(id => _cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat));
            return r.Payment.Count == r.Selected.Count && r.Payment.All(p => r.Selected.Contains(p.CardId) && p.From == CardLocation.Hand(f.OwnerSeat) &&
                _cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == p.CardId && m.CardKind == p.Kind && m.From == p.From &&
                    m.To == NormalizeEphemeralCardDestination(GetAdvancedCard(p.CardId), p.From, r.Location!.Value, new(BoundaryXiahouDunStoreReason)) && m.Reason.Value == BoundaryXiahouDunStoreReason) == 1);
        }
        if (r.Op != SkillProgramEffectOp.DistributeBoundaryXiahouDunQingjian || f.TriggerId != "distribute-at-ending" || context.Window != SkillProgramTriggerWindow.TurnEnding ||
            r.Selected.Count != 0 || r.Location is not null || r.Stage is BoundaryXiahouDunStage.Selecting or BoundaryXiahouDunStage.StoreChildren ||
            r.Deposits.Any(id => { var d = BoundaryXiahouDunDeposit(id); return d.FrameId >= r.WindowId || d.Source.OwnerSeat != f.OwnerSeat || d.Source.SkillInstanceId != f.SkillInstanceId || d.Hash != f.GameplayHash; })) return false;
        var gifts = ProgramEventHistory<BoundaryXiahouDunGivenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (gifts.Sum(e => e.Count) != r.Given || gifts.Any(e => e.Owner != f.OwnerSeat || !IsValidPlayerSeat(e.Target) || e.Target == f.OwnerSeat || e.Count is < 0 or > 1 ||
            _cardMovements.Count(m => m.Sequence > e.Before && m.Sequence <= e.After && m.To == CardLocation.Hand(e.Target) && m.Reason.Value == BoundaryXiahouDunGiveReason &&
                r.Deposits.Any(id => BoundaryXiahouDunDeposit(id).Location == m.From)) != e.Count)) return false;
        if (r.Stage == BoundaryXiahouDunStage.GiveChildren) return IsValidPlayerSeat(r.Target) && r.Target != f.OwnerSeat && r.Payment is [var payment] &&
            r.Deposits.Any(id => BoundaryXiahouDunDeposit(id).Location == payment.From) && _cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After &&
                m.CardId == payment.CardId && m.CardKind == payment.Kind && m.From == payment.From && m.To == NormalizeEphemeralCardDestination(GetAdvancedCard(payment.CardId), payment.From, CardLocation.Hand(r.Target), new(BoundaryXiahouDunGiveReason)) &&
                m.Reason.Value == BoundaryXiahouDunGiveReason) == 1;
        if (r.Stage is BoundaryXiahouDunStage.Drawing or BoundaryXiahouDunStage.DrawChildren)
        {
            if (r.Given <= 1 || BoundaryXiahouDunHeld(f).Any() || r.Payment.Count != 0) return false;
            if (r.Stage == BoundaryXiahouDunStage.Drawing) return r.Draw is null;
            return r.Draw is { RequestedCount: 1 } draw && draw.RecipientSeat == f.OwnerSeat && draw.SequenceBefore == r.Before && draw.SequenceAfter == r.After &&
                ProgramEventHistory<BoundaryXiahouDunRewardEvent>().Count(e => e == new BoundaryXiahouDunRewardEvent(f.Id, f.OwnerSeat, r.Given, draw.AttemptFrameId, draw.ActualDrawIds.Count, r.Before, r.After)) == 1;
        }
        return r.Stage == BoundaryXiahouDunStage.Distributing && r.Payment.Count == 0 && r.Draw is null;
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 60010)]
    private bool BoundaryXiahouDunMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) =>
        f.BoundaryXiahouDun is { } r && e?.Op == r.Op && f.PendingMovementContinuation == pending && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 &&
        pending.CoverageResultBind is null && r.Stage is BoundaryXiahouDunStage.StoreChildren or BoundaryXiahouDunStage.GiveChildren or BoundaryXiahouDunStage.DrawChildren && ValidBoundaryXiahouDun(f);
    private bool BoundaryXiahouDunFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidBoundaryXiahouDun(f)) return false; var r = f.BoundaryXiahouDun!;
        if (NativeDrawAttemptRidesOn(child, f)) return r.Stage == BoundaryXiahouDunStage.Drawing && f.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery) return ProgramRecoveryReplacementReceiptMatches(f, recovery, CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp) return ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved) return f.PendingMovementContinuation is not null && ProgramMovementReturnMatches(f, moved) &&
            moved.Batch.Movements.All(m => _cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id &&
            (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram ||
             state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }
    private bool BoundaryXiahouDunSuffix(int index) => HasObserverSuffix(index, static (g, n) => g.IsMovementObserverEdge(n) || g.IsSameNameHandObserverEdge(n) || g.IsYanPuDyingExitEdge(n) ||
        g.TypedDamagePostEventEdge(g._resolutionStack[n - 1], g._resolutionStack[n]) || g._resolutionStack[n - 1] is ProgramSkillFrame f && g.BoundaryXiahouDunFirstChild(f, g._resolutionStack[n]) ||
        NativeDrawAttemptRidesOn(g._resolutionStack[n], g._resolutionStack[n - 1]) || g.RecoveryReplacementFrameRidesOn(g._resolutionStack[n], g._resolutionStack[n - 1]),
        static (g, n, d) => g.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    private ProgramSkillFrame? BoundaryXiahouDunObserverRoot() => FindProgramObserverRoot(static (g, f, c) => g.BoundaryXiahouDunFirstChild(f, c), static (g, n) => g.BoundaryXiahouDunSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 60010)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 60010)]
    private bool BoundaryXiahouDunDamageObserver(long id) => BoundaryXiahouDunObserverRoot() is { } f && _resolutionStack.FindIndex(c => c.Id == id && c is DamageTriggerWindowFrame) > _resolutionStack.FindIndex(c => c.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 60010)]
    private bool BoundaryXiahouDunBeforeDamageObserver(long id) => BoundaryXiahouDunObserverRoot() is { } f && _resolutionStack.FindIndex(c => c.Id == id && c is BeforeDamageProgramWindowFrame) > _resolutionStack.FindIndex(c => c.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 60010)]
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 60010)]
    private bool BoundaryXiahouDunDyingObserver() => ActiveDying is { } d && BoundaryXiahouDunObserverRoot() is { } f && _resolutionStack.FindIndex(c => c.Id == d.Id) > _resolutionStack.FindIndex(c => c.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 60010)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 60010)]
    private bool BoundaryXiahouDunRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child)
    {
        if (!ValidBoundaryXiahouDun(f)) return false;
        if (ProgramRecoveryReplacementReceiptMatches(f, child, CompleteProgramEventHistory().ToArray())) return true;
        var a = _resolutionStack.FindIndex(c => c.Id == f.Id); var b = _resolutionStack.FindIndex(c => c.Id == child.Id);
        return a >= 0 && b > a + 1 && BoundaryXiahouDunFirstChild(f, _resolutionStack[a + 1]) && BoundaryXiahouDunSuffix(a) && RecoveryReplacementFrameRidesOn(child, _resolutionStack[b - 1]);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 60010)]
    private bool ResumeBoundaryXiahouDun(long id) { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryXiahouDun: not null } f || f.Id != id) return false; ContinueBoundaryXiahouDun(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 60010)]
    private bool ReturnBoundaryXiahouDunMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeBoundaryXiahouDun(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 60010)]
    private bool AdvanceBoundaryXiahouDunChildren() => _pendingDecision is null && BoundaryXiahouDunObserverRoot() is not null && TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 60010)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 60010)]
    private bool KeepBoundaryXiahouDun(ProgramSkillFrame f) => f.BoundaryXiahouDun is { } r ? r.Stage != BoundaryXiahouDunStage.Selecting && ValidBoundaryXiahouDun(f) :
        f.SkillId == BoundaryXiahouDunQingjian && f.TriggerId == "distribute-at-ending" && f.WindowContext is { } context &&
        IssuedBoundaryXiahouDun(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), context);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 60010)]
    private void AssertBoundaryXiahouDun(ProgramSkillFrame f) { if (f.BoundaryXiahouDun is not null && !ValidBoundaryXiahouDun(f)) throw new InvalidOperationException("Mobile Qingjian lost its original private payment or typed return."); }
}
