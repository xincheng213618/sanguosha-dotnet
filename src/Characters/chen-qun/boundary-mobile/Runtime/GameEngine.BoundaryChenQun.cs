namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static readonly CardMoveReason BoundaryDingpinDrawReason = new("skill-program.boundary-chen-qun.dingpin.draw");
    private static CardConversionSource BoundaryDingpinSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private bool BoundaryDingpinActiveTurn(int turn, int owner) => turn == _turnNumber && owner == _turnProgression.OwnerSeat;
    private bool BoundaryDingpinCanTarget(int owner, int target) => IsValidPlayerSeat(target) && _players[target].IsAlive &&
        !ProgramEventHistory<BoundaryDingpinTargetBannedEvent>().Any(e => e.Owner == owner && e.Target == target && BoundaryDingpinActiveTurn(e.Turn, e.TurnOwner));
    private bool BoundaryDingpinCanDiscard(int owner, Card card)
    {
        var category = GetProgramCardCategory(card.Kind); var mask = 1 << (int)category;
        if (ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.ActorSeat == owner && BoundaryDingpinActiveTurn(e.TurnNumber, e.TurnOwnerSeat) && (e.CardCategoryMask & mask) != 0)) return false;
        foreach (var discard in ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => BoundaryDingpinActiveTurn(e.TurnNumber, e.ActualTurnOwnerSeat)))
        foreach (var material in ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == discard.InvocationId && e.From.OwnerSeat == owner))
        {
            if (ProgramEventHistory<BoundaryDingpinHeartExceptionEvent>().Any(e => e.Owner == owner && e.InvocationId == discard.InvocationId && e.CardId == material.CardId && e.CostSequence == material.CompletionSequence && BoundaryDingpinActiveTurn(e.Turn, e.TurnOwner))) continue;
            if (GetProgramCardCategory(GetAdvancedCard(material.CardId).Kind) == category) return false;
        }
        return true;
    }
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 53600, SkillProgramEffectOp.PrepareBoundaryChenQunDingpin)]
    private bool CanActivateBoundaryDingpin(ProgramActivationLegalityContext c) => c.Owner.IsAlive &&
        CurrentActualDiscardRecoveryPhase() is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == c.OwnerSeat &&
        _phase == TurnPhase.Play && _currentSeat == c.OwnerSeat;

    [ProgramCapability]
    private sealed class BoundaryChenQunHost(GameEngine engine) : IBoundaryChenQunHost, IBoundaryChenQunSelection
    {
        public bool CanDiscard(int owner, Card card) => engine.BoundaryDingpinCanDiscard(owner, card);
        public bool CanTarget(int owner, int target) => engine.BoundaryDingpinCanTarget(owner, target);
        public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.PrepareBoundaryChenQunDingpin
            ? engine.PrepareBoundaryDingpin(f) : engine.ResolveBoundaryDingpin(f);
    }
    private SkillProgramStepOutcome PrepareBoundaryDingpin(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.BoundaryChenQunDingpin is not null || f.TriggerId is not null || f.ActivationId != BoundaryChenQunComposition.Activation || f.InstructionIndex != 1 ||
            f.SelectedCardIds is not [var id] || f.SelectedTargetSeats is not [var target] || !BoundaryDingpinCanTarget(f.OwnerSeat, target) ||
            CurrentActualDiscardRecoveryPhase() is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat ||
            _cardZones.GetLocation(id) is not { Zone: CardZoneKind.Hand or CardZoneKind.Equipment, OwnerSeat: { } owner } from || owner != f.OwnerSeat ||
            !BoundaryDingpinCanDiscard(f.OwnerSeat, GetAdvancedCard(id)))
            throw new InvalidOperationException("Dingpin requires one presently unused/undiscarded category, its own physical HE cost and a legal actual-Play target.");
        var r = new BoundaryDingpinReceipt(BoundaryDingpinSource(f), f.GameplayHash, phase, target, id, from, GetProgramCardCategory(GetAdvancedCard(id).Kind), _movementSequence);
        ReplaceRuntimeTop(f with { BoundaryChenQunDingpin = r });
        AdvanceEventRulesAndQueueFact(new BoundaryDingpinPreparedEvent(f.Id, r.Source, r.Hash, phase, target, id, from, r.Category, r.Before));
        return SkillProgramStepOutcome.Continue;
    }
    private (NativeLogicalDiscardInvocationRecordedEvent Invocation, NativeLogicalDiscardMaterialCompletedEvent Material)? BoundaryDingpinCost(ProgramSkillFrame f)
    {
        var r = f.BoundaryChenQunDingpin!;
        var candidates = ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ParentFrameId == f.Id &&
            e.Reason.Value == $"skill-program.{f.SkillId}.{SkillProgramEffectOp.MoveBoundCards}" && e.TurnNumber == r.Phase.TurnNumber &&
            e.ActualTurnOwnerSeat == r.Phase.ActualTurnOwnerSeat && e.InitiatorSeat == f.OwnerSeat && e.ExecutorSeat == f.OwnerSeat &&
            e.SequenceBefore >= r.Before && e.ActualCount == 1).ToArray();
        if (candidates is not [var invocation]) return null;
        var materials = ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == invocation.InvocationId).ToArray();
        if (materials is not [var m] || m.CardId != r.CostId || m.From != r.From || m.CompletionSequence <= r.Before ||
            !_cardMovements.Any(move => move.CardId == m.CardId && move.Sequence == m.CompletionSequence && move.To == m.To &&
                (move.From == m.From || GetProgramDiscardSource(move) == m.From))) return null;
        return (invocation, m);
    }
    private SkillProgramStepOutcome ResolveBoundaryDingpin(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id); var r = f.BoundaryChenQunDingpin ?? throw new InvalidOperationException("Dingpin lost its paid original frame.");
        if (f.InstructionIndex != 5 || r.Stage != BoundaryDingpinStage.Prepared || BoundaryDingpinCost(f) is not { } paid ||
            f.JudgmentOutcomeBindings?.SingleOrDefault(o => o.Name == BoundaryChenQunComposition.Outcome) is not { InstructionIndex: 4 } judgment ||
            judgment.TargetSeat != r.Target || judgment.Reason != BoundaryChenQunComposition.Reason || !IsValidProgramJudgmentOutcome(f, judgment))
            throw new InvalidOperationException("Dingpin requires the exact completed native discard and finalized judgment outcome, even if its physical judgment card was claimed.");
        var black = judgment.Suit is Suit.Spade or Suit.Club;
        var count = black && _players[r.Target].IsAlive && _players[r.Target].HasHpValue ? Math.Clamp(_players[r.Target].Hp, 0, 3) : 0;
        ReplaceRuntimeTop(f = f with { BoundaryChenQunDingpin = r = r with { CostInvocationId = paid.Invocation.InvocationId,
            CostSequence = paid.Material.CompletionSequence, Judgment = judgment, Requested = count, Stage = black ? BoundaryDingpinStage.Drawing : BoundaryDingpinStage.Resolved } });
        AdvanceEventRulesAndQueueFact(new BoundaryDingpinResultEvent(f.Id, r.Target, judgment.JudgmentFrameId, judgment.Suit, r.CostInvocationId.Value, r.CostSequence.Value, count));
        if (judgment.Suit == Suit.Heart)
            AdvanceEventRulesAndQueueFact(new BoundaryDingpinHeartExceptionEvent(f.Id, f.OwnerSeat, r.Phase.TurnNumber, r.Phase.ActualTurnOwnerSeat,
                paid.Invocation.InvocationId, r.CostId, paid.Material.CompletionSequence));
        if (!black) return SkillProgramStepOutcome.Continue;
        return BeginOperationProgramDraw(f, r.Target, count, BoundaryDingpinDrawReason);
    }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBoundaryChenQunDingpin)]
    private void ReturnBoundaryDingpinDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = GetActiveProgramFrame(supplied.Id); var r = f.BoundaryChenQunDingpin;
        if (r is not { Stage: BoundaryDingpinStage.Drawing, Draw: null, Judgment.Suit: Suit.Spade or Suit.Club } || f.InstructionIndex != 5 ||
            f.NativeDraw is not null || outcome.RecipientSeat != r.Target || outcome.RequestedCount != r.Requested ||
            outcome.SequenceBefore > outcome.SequenceAfter || outcome.SequenceAfter > _movementSequence ||
            outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.ActualDrawIds.Count > r.Requested ||
            outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 ||
            !_cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter &&
                    m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Target) && m.Reason == BoundaryDingpinDrawReason)
                .Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("Dingpin received a different or repeated native bounded-HP draw request.");
        ReplaceRuntimeTop(f = f with { BoundaryChenQunDingpin = r with { Stage = BoundaryDingpinStage.DrawChildren, Draw = outcome }, PendingMovementContinuation = new(r.Target, 0, null) });
        AdvanceEventRulesAndQueueFact(new BoundaryDingpinDrawReturnedEvent(f.Id, r.Target, r.Requested, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryDingpinDraw(f.Id);
    }
    private void ContinueBoundaryDingpinDraw(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryChenQunDingpin: { Stage: BoundaryDingpinStage.DrawChildren } r } f || f.Id != id) return;
        if (TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) ||
            TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || TryBeginAdvancedSkillsChanged(f.Id) ||
            TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id)) return;
        f = GetActiveProgramFrame(id);
        if (ProgramEventHistory<BoundaryDingpinTargetBannedEvent>().Any(e => e.FrameId == f.Id)) throw new InvalidOperationException("Dingpin cannot repeat the same paid black-result target restriction.");
        AdvanceEventRulesAndQueueFact(new BoundaryDingpinTargetBannedEvent(f.Id, f.OwnerSeat, r.Target, r.Phase.TurnNumber, r.Phase.ActualTurnOwnerSeat));
        ReplaceRuntimeTop(f with { BoundaryChenQunDingpin = r with { Stage = BoundaryDingpinStage.Resolved }, PendingMovementContinuation = null });
        AdvanceRuntimeProgram(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 53600)]
    private bool ResumeBoundaryDingpin(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryChenQunDingpin.Stage: BoundaryDingpinStage.DrawChildren } f || f.Id != id) return false;
        ContinueBoundaryDingpinDraw(id); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 53600)]
    private bool ReturnBoundaryDingpinMovement(ProgramSkillFrame f)
    { if (f.BoundaryChenQunDingpin?.Stage != BoundaryDingpinStage.DrawChildren) return false; ContinueBoundaryDingpinDraw(f.Id); return true; }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 53600)]
    private bool KeepBoundaryDingpinPaidFrame(ProgramSkillFrame f) => f.BoundaryChenQunDingpin is not null && BoundaryDingpinCost(f) is not null;
}
