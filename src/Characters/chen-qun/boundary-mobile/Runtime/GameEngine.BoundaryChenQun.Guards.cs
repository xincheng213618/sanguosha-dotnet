namespace CardGame.Core;

public sealed partial class GameEngine
{
    // The initial card filter deliberately rejects a category immediately after
    // this cost is discarded. Only this original once-paid activation may keep
    // that historical selection while its native children and judgment finish.
    private bool HasPaidBoundaryChenQunInitialSelection(ProgramSkillFrame f)
    {
        if (f.BoundaryChenQunDingpin is not { } r || f.SkillId != BoundaryChenQunComposition.Skill ||
            f.ActivationId != BoundaryChenQunComposition.Activation || f.TriggerId is not null || f.WindowContext is not null ||
            f.InstructionIndex is < 3 or > 7 || r.Source != BoundaryDingpinSource(f) || r.Hash != f.GameplayHash ||
            _contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash ||
            f.SelectedCardIds is not [var cost] || cost != r.CostId || f.SelectedTargetSeats is not [var target] || target != r.Target ||
            r.From.OwnerSeat != f.OwnerSeat || r.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) ||
            r.Category != GetProgramCardCategory(GetAdvancedCard(cost).Kind) || r.Before < 0 || r.Before > _movementSequence ||
            r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || r.Phase.ActorSeat != f.OwnerSeat || !IsRecordedActualDiscardRecoveryPhase(r.Phase) ||
            f.CardSetBindings.SingleOrDefault(b => b.Name == BoundaryChenQunComposition.Cost) is not { } binding ||
            !binding.CardIds.SequenceEqual([cost]) || !binding.SourceLocations.SequenceEqual([r.From]) ||
            ProgramEventHistory<BoundaryDingpinPreparedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var start] ||
            start != new BoundaryDingpinPreparedEvent(f.Id, r.Source, r.Hash, r.Phase, target, cost, r.From, r.Category, r.Before)) return false;
        return BoundaryDingpinCost(f) is not null;
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 53600)]
    private void AssertBoundaryDingpin(ProgramSkillFrame f)
    {
        if (f.BoundaryChenQunDingpin is not { } r) return;
        if (f.SkillId != BoundaryChenQunComposition.Skill || f.ActivationId != BoundaryChenQunComposition.Activation || f.TriggerId is not null ||
            f.InstructionIndex is < 1 or > 7 || r.Source != BoundaryDingpinSource(f) || r.Hash != f.GameplayHash ||
            f.SelectedCardIds is not [var cost] || cost != r.CostId || f.SelectedTargetSeats is not [var target] || target != r.Target ||
            r.From.OwnerSeat != f.OwnerSeat || r.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) ||
            r.Category != GetProgramCardCategory(GetAdvancedCard(cost).Kind) || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.Before > _movementSequence ||
            r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || r.Phase.ActorSeat != f.OwnerSeat || !IsRecordedActualDiscardRecoveryPhase(r.Phase) ||
            ProgramEventHistory<BoundaryDingpinPreparedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var start] ||
            start != new BoundaryDingpinPreparedEvent(f.Id, r.Source, r.Hash, r.Phase, target, cost, r.From, r.Category, r.Before))
            throw new InvalidOperationException("Dingpin lost its original actual Play, source/hash, selected owned entity or recipient.");
        var paid = BoundaryDingpinCost(f);
        if (f.InstructionIndex >= 4 && paid is null)
            throw new InvalidOperationException("Dingpin reached its native judgment without its actual original logical discard invoice.");
        if (r.Stage == BoundaryDingpinStage.Prepared)
        {
            if (r.CostInvocationId is not null || r.CostSequence is not null || r.Judgment is not null || r.Requested != 0 || r.Draw is not null ||
                ProgramEventHistory<BoundaryDingpinResultEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Dingpin cannot manufacture a result before the original native judgment returns.");
            return;
        }
        if (paid is not { } exact || r.CostInvocationId != exact.Invocation.InvocationId || r.CostSequence != exact.Material.CompletionSequence ||
            r.Judgment is not { } outcome || outcome.Name != BoundaryChenQunComposition.Outcome || outcome.TargetSeat != target ||
            outcome.InstructionIndex != 4 || !IsValidProgramJudgmentOutcome(f, outcome) || r.Requested is < 0 or > 3 ||
            ProgramEventHistory<BoundaryDingpinResultEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var result] ||
            result != new BoundaryDingpinResultEvent(f.Id, target, outcome.JudgmentFrameId, outcome.Suit, exact.Invocation.InvocationId, exact.Material.CompletionSequence, r.Requested))
            throw new InvalidOperationException("Dingpin lost its exact finalized judgment value, original cost or bounded current-HP amount.");
        var heart = ProgramEventHistory<BoundaryDingpinHeartExceptionEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (outcome.Suit == Suit.Heart ? heart is not [var h] ||
                h != new BoundaryDingpinHeartExceptionEvent(f.Id, f.OwnerSeat, r.Phase.TurnNumber, r.Phase.ActualTurnOwnerSeat, exact.Invocation.InvocationId, cost, exact.Material.CompletionSequence)
            : heart.Length != 0)
            throw new InvalidOperationException("Dingpin's heart exception must name only this invocation's one actual discard material.");
        if (r.Stage == BoundaryDingpinStage.Drawing && (f.InstructionIndex != 5 || r.Draw is not null || outcome.Suit is not (Suit.Spade or Suit.Club)))
            throw new InvalidOperationException("Dingpin cannot enter a second or unrelated black-result draw.");
        if (r.Stage == BoundaryDingpinStage.DrawChildren)
        {
            if (f.InstructionIndex != 5 || r.Draw is not { } draw || draw.RecipientSeat != target || draw.RequestedCount != r.Requested ||
                draw.SequenceBefore < 0 || draw.SequenceBefore > draw.SequenceAfter || draw.SequenceAfter > _movementSequence ||
                f.PendingMovementContinuation is { } p && (p.SubjectSeat != target || p.BeforeCount != 0 || p.CoverageResultBind is not null) ||
                ProgramEventHistory<BoundaryDingpinDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var returned] ||
                returned != new BoundaryDingpinDrawReturnedEvent(f.Id, target, r.Requested, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count, draw.SequenceBefore, draw.SequenceAfter) ||
                ProgramEventHistory<BoundaryDingpinTargetBannedEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Dingpin's paid native draw must return through the same original movement child before banning its target.");
        }
        if (r.Stage == BoundaryDingpinStage.Resolved && outcome.Suit is Suit.Spade or Suit.Club)
        {
            if (ProgramEventHistory<BoundaryDingpinTargetBannedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var ban] ||
                ban != new BoundaryDingpinTargetBannedEvent(f.Id, f.OwnerSeat, target, r.Phase.TurnNumber, r.Phase.ActualTurnOwnerSeat))
                throw new InvalidOperationException("Dingpin lost the once-only target restriction of its original black result.");
        }
    }
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 53600)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 53600)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 53600)]
    private bool BoundaryDingpinMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) =>
        f.BoundaryChenQunDingpin is { Stage: BoundaryDingpinStage.DrawChildren } r &&
        e?.Op == SkillProgramEffectOp.ResolveBoundaryChenQunDingpin && p == f.PendingMovementContinuation &&
        p.SubjectSeat == r.Target && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 53600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 53600)]
    private bool BoundaryDingpinRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) =>
        f.BoundaryChenQunDingpin is { Stage: BoundaryDingpinStage.DrawChildren, Draw.Replacement: not null } && RecoveryReplacementFrameRidesOn(recovery, f) &&
        recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
    private bool BoundaryDingpinFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryChenQunDingpin is not { } r) return false;
        if (r.Stage == BoundaryDingpinStage.Drawing && f.NativeDraw is not null) return NativeDrawAttemptRidesOn(child, f);
        if (r.Stage != BoundaryDingpinStage.DrawChildren || r.Draw is not { } draw) return false;
        AssertBoundaryDingpin(f);
        if (child is RecoveryReplacementFrame recovery) return BoundaryDingpinRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp) return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame state) return state.ResumeProgramFrameId == f.Id &&
            (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram ||
             state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
        return child is CardsMovedTriggerWindowFrame moved && ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id &&
            moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat &&
            moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _cardMovements.Contains(m) && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter);
    }
    private ProgramSkillFrame? BoundaryDingpinObserverRoot() => FindProgramObserverRoot(static (e, f, c) => e.BoundaryDingpinFirstChild(f, c),
        static (e, i) => e.HasObserverSuffix(i, static (g, n) => g.IsMovementObserverEdge(n) || g.IsSameNameHandObserverEdge(n) || g.IsYanPuDyingExitEdge(n) ||
            g.TypedDamagePostEventEdge(g._resolutionStack[n - 1], g._resolutionStack[n]) || NativeDrawAttemptRidesOn(g._resolutionStack[n], g._resolutionStack[n - 1]) ||
            g.RecoveryReplacementFrameRidesOn(g._resolutionStack[n], g._resolutionStack[n - 1]), static (g, n, d) => g.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 53600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 53600)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 53600)]
    private bool BoundaryDingpinDamageObserver(long id) => BoundaryDingpinObserverRoot() is { } root &&
        _resolutionStack.FindIndex(f => f.Id == id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 53600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 53600)]
    private bool BoundaryDingpinDyingObserver() => ActiveDying is { } dying && BoundaryDingpinObserverRoot() is { } root &&
        _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 53600)]
    private bool AdvanceBoundaryDingpinObservers() => _pendingDecision is null && BoundaryDingpinObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
