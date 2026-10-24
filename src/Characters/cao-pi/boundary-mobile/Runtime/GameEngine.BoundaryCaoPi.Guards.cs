namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 54500)]
    private void AssertBoundaryCaoPi(ProgramSkillFrame f)
    {
        if (f.BoundaryXingshang is { } corpse)
        {
            if (f.SkillId != BoundaryCaoPiComposition.Xingshang || f.InstructionIndex != 1 || f.WindowContext is not { } c || corpse.Source != BoundaryCaoPiSource(f) || corpse.Hash != f.GameplayHash ||
                c.ParentFrameId != corpse.DeathWindow || BoundaryCaoPiDeath(c) is not { } death || death.Id != corpse.DeathId || death.VictimSeat != corpse.Victim || corpse.ResultBind != "xingshang-result")
                throw new InvalidOperationException("Xingshang lost its original native cleanup frame, corpse or source/hash.");
        }
        if (f.BoundaryFangzhu is not { } r) return;
        if (f.SkillId != BoundaryCaoPiComposition.Fangzhu || f.TriggerId != "injury-recipient-choice" || f.InstructionIndex is < 2 or > 4 ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || r.Source != BoundaryCaoPiSource(f) || r.Hash != f.GameplayHash ||
            context.ParentFrameId != r.DamageWindow || context.DamageFrameId != r.DamageId || !f.SelectedTargetSeats.SequenceEqual([r.Target]) || r.Target == f.OwnerSeat || r.X < 0 ||
            r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Count > r.X || !Enum.IsDefined(r.Stage) || r.ResultBind != "fangzhu-result" ||
            ProgramEventHistory<BoundaryFangzhuStartedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var started] ||
            started != new BoundaryFangzhuStartedEvent(f.Id, r.Source, r.Hash, r.DamageWindow, r.DamageId, r.Target, r.X, r.ResultBind))
            throw new InvalidOperationException("Fangzhu lost its original damage window, chosen participant or frozen X.");
        if (r.Stage == BoundaryFangzhuStage.Choice && (f.InstructionIndex != 2 || r.Outcome is not null || r.Before != 0 || r.After != 0 || r.Actual != 0 || r.DiscardInvocation is not null ||
            r.Selected.Any(id => !BoundaryFangzhuMaterials(r.Target).Any(c => c.Id == id))))
            throw new InvalidOperationException("Fangzhu cannot prepay or substitute the recipient's staged owned HE entities.");
        if (r.Stage != BoundaryFangzhuStage.Choice && (r.Outcome is not ("discarded" or "drawn") || r.Before < 0 || r.After < r.Before || r.After > _movementSequence || r.Actual < 0 || r.Actual > r.X))
            throw new InvalidOperationException("Fangzhu lost its original chosen consequence or native movement interval.");
        if (r.Stage == BoundaryFangzhuStage.DrawRequest && (f.NativeDraw is not { RequestedCount: var requested, Operation: SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu } drawRequest ||
            requested != r.X || !drawRequest.Recipients.SequenceEqual([r.Target]) || f.PendingMovementContinuation is not null))
            throw new InvalidOperationException("Fangzhu requires the exact frozen-X native draw request on its original instruction.");
        if (r.Outcome == "discarded")
        {
            if (r.Selected.Count != r.X || ProgramEventHistory<BoundaryFangzhuPaidEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var paid] ||
                paid != new BoundaryFangzhuPaidEvent(f.Id, r.Target, r.X, r.DiscardInvocation, r.Actual, r.Before, r.After))
                throw new InvalidOperationException("Fangzhu requires one exact complete discard batch before authorizing HP loss.");
            if (r.DiscardInvocation is { } invocation)
            {
                if (ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.InvocationId == invocation).ToArray() is not [var native] || native.ParentFrameId != f.Id ||
                    native.SubjectSeat != r.Target || native.ExecutorSeat != r.Target || native.InitiatorSeat != f.OwnerSeat || native.ActualCount != r.Actual || native.SequenceBefore != r.Before ||
                    native.SequenceAfter > r.After || native.Reason.Value != BoundaryFangzhuDiscard) throw new InvalidOperationException("Fangzhu cannot borrow another frame's logical discard invoice.");
                var entities = ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == invocation).ToArray();
                if (entities.Length != r.Actual || entities.Select(e => e.CardId).Distinct().Count() != entities.Length || entities.Any(e => !r.Selected.Contains(e.CardId) || e.From.OwnerSeat != r.Target ||
                    e.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || e.CompletionSequence <= r.Before || e.CompletionSequence > r.After ||
                    !_cardMovements.Any(m => m.Sequence == e.CompletionSequence && m.CardId == e.CardId && m.To == e.To && (m.From == e.From || GetProgramDiscardSource(m) == e.From))))
                    throw new InvalidOperationException("Fangzhu's real paid entities differ from its recipient's frozen HE set.");
            }
            else if (r.Actual != 0) throw new InvalidOperationException("Fangzhu cannot manufacture an actual discarded entity.");
        }
        if (r.Outcome == "drawn" && r.Stage != BoundaryFangzhuStage.DrawRequest &&
            (ProgramEventHistory<BoundaryFangzhuDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var drawn] ||
             drawn != new BoundaryFangzhuDrawReturnedEvent(f.Id, r.Target, r.X, r.Actual, r.DrawAttempt, r.DrawReplaced, r.Before, r.After)))
            throw new InvalidOperationException("Fangzhu must retain the exact actual-draw/replacement typed result before flipping.");
        if (r.Stage is BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren && (f.InstructionIndex != 2 ||
            f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != r.Target))
            throw new InvalidOperationException("Fangzhu lost its original recipient's pending movement return.");
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 54501)]
    private void AssertBoundaryCaoPiChoice(ProgramSkillFrame f)
    {
        var chooser = f.BoundaryXingshang is not null ? f.OwnerSeat : f.BoundaryFangzhu is { Stage: BoundaryFangzhuStage.Choice } r ? r.Target : (int?)null;
        if (chooser is { } seat && _resolutionStack.LastOrDefault()?.Id == f.Id && (_pendingDecision is not { IsPrivate: true } p || p.PlayerSeat != seat ||
            !AssistedChoicesEqual(p.Choices.Select(c => c with { Parameters = c.Parameters.Where(x => !x.Key.StartsWith("owned-selection-", StringComparison.Ordinal)).ToDictionary(x => x.Key, x => x.Value) }).ToArray(), BoundaryCaoPiChoices(f))))
            throw new InvalidOperationException("Cao Pi lost the original participant's current private choice.");
    }
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 54500)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 54500)]
    private bool BoundaryFangzhuMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.BoundaryFangzhu is { Stage: BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren } r &&
        e?.Op == SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu && p == f.PendingMovementContinuation && p.SubjectSeat == r.Target && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54500)]
    private bool BoundaryFangzhuRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.BoundaryFangzhu is { Stage: BoundaryFangzhuStage.DiscardChildren } r &&
        RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion &&
        r.Selected.Any(id => GetAdvancedCard(id).Kind == CardKind.SilverLion && _cardMovements.Any(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(r.Target)));
    private bool BoundaryFangzhuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryFangzhu is not { } r) return false;
        if (r.Stage == BoundaryFangzhuStage.DrawRequest) return NativeDrawAttemptRidesOn(child, f);
        if (r.Stage is not (BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren)) return false;
        AssertBoundaryCaoPi(f);
        if (child is RecoveryReplacementFrame recovery) return BoundaryFangzhuRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp) return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame state) return state.ResumeProgramFrameId == f.Id &&
            (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange);
        return child is CardsMovedTriggerWindowFrame moved && ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId &&
            moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m =>
                _cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }
    private ProgramSkillFrame? BoundaryFangzhuObserverRoot() => FindProgramObserverRoot(static (e, f, c) => e.BoundaryFangzhuFirstChild(f, c),
        static (e, i) => e.HasObserverSuffix(i, static (g, n) => g.IsMovementObserverEdge(n) || g.IsSameNameHandObserverEdge(n) || g.IsYanPuDyingExitEdge(n) ||
            g.TypedDamagePostEventEdge(g._resolutionStack[n - 1], g._resolutionStack[n]) || g.RecoveryReplacementFrameRidesOn(g._resolutionStack[n], g._resolutionStack[n - 1]),
            static (g, n, d) => g.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 54500)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 54500)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 54500)]
    private bool BoundaryFangzhuDamageObserver(long id) => BoundaryFangzhuObserverRoot() is { } root && _resolutionStack.FindIndex(f => f.Id == id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 54500)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54500)]
    private bool BoundaryFangzhuDyingObserver() => ActiveDying is { } dying && BoundaryFangzhuObserverRoot() is { } root && _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54500)]
    private bool AdvanceBoundaryFangzhuObservers() => _pendingDecision is null && BoundaryFangzhuObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
