namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 54200)]
    private void AssertBoundaryWurong(ProgramSkillFrame f)
    {
        if (f.BoundaryWurong is not { } r) return;
        if (f.SkillId != BoundaryZhangNiComposition.Skill || f.ActivationId != BoundaryZhangNiComposition.Activation || f.TriggerId is not null || f.WindowContext is not null ||
            f.InstructionIndex is < 1 or > 7 || f.SelectedCardIds.Count != 0 || !f.SelectedTargetSeats.SequenceEqual([r.Target]) ||
            r.Source != BoundaryWurongSource(f) || r.Hash != f.GameplayHash || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || r.Phase.ActorSeat != f.OwnerSeat ||
            !IsRecordedActualDiscardRecoveryPhase(r.Phase) || !Enum.IsDefined(r.Stage) ||
            ProgramEventHistory<BoundaryWurongStartedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var start] ||
            start != new BoundaryWurongStartedEvent(f.Id, r.Source, r.Hash, r.Phase, r.Target))
            throw new InvalidOperationException("Wurong lost its exact original activation, actual Play phase, selected counterpart or source/hash.");
        var commits = ProgramEventHistory<BoundaryWurongCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var revealed = ProgramEventHistory<BoundaryWurongRevealedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == BoundaryWurongStage.OwnerChoice ? r.Suppress is not null || r.Resist is not null || r.Outcome is not null || commits.Length != 0 || revealed.Length != 0 || f.InstructionIndex != 1 :
            r.Stage == BoundaryWurongStage.TargetChoice ? r.Suppress is null || r.Resist is not null || r.Outcome is not null || commits is not [{ First: true }] || commits[0].Participant != f.OwnerSeat || revealed.Length != 0 || f.InstructionIndex != 1 :
            r.Suppress is null || r.Resist is null || r.Outcome != (r.Suppress.Value ? "suppress" : "appease") + (r.Resist.Value ? "-resist" : "-submit") ||
                commits is not [{ First: true }, { First: false }] || commits[0].Participant != f.OwnerSeat || commits[1].Participant != r.Target ||
                revealed is not [var result] || result != new BoundaryWurongRevealedEvent(f.Id, r.Suppress.Value, r.Resist.Value, r.Outcome))
            throw new InvalidOperationException("Wurong cannot publish or replace either strategy before both original participants commit.");
        if (r.Stage is BoundaryWurongStage.OwnerChoice or BoundaryWurongStage.TargetChoice or BoundaryWurongStage.GiftChoice)
        {
            var chooser = r.Stage == BoundaryWurongStage.OwnerChoice ? f.OwnerSeat : r.Stage == BoundaryWurongStage.TargetChoice ? r.Target : r.Giver;
            if (_resolutionStack.LastOrDefault()?.Id == f.Id && (_pendingDecision is not { IsPrivate: true } p || p.PlayerSeat != chooser ||
                !AssistedChoicesEqual(p.Choices.Select(c => c with { Parameters = c.Parameters.Where(x => !x.Key.StartsWith("owned-selection-", StringComparison.Ordinal)).ToDictionary(x => x.Key, x => x.Value) }).ToArray(), BoundaryWurongChoices(f))))
                throw new InvalidOperationException("Wurong's current private chooser or original staged-card prompt changed.");
        }
        if (r.Stage is BoundaryWurongStage.GiftChoice or BoundaryWurongStage.GiftChildren)
        {
            if (f.InstructionIndex != 5 || r.Resist != false || r.Giver != (r.Suppress == true ? f.OwnerSeat : r.Target) || r.Recipient != (r.Suppress == true ? r.Target : f.OwnerSeat) ||
                r.Required is < 1 or > 2 || r.Selected.Count > r.Required || r.Selected.Distinct().Count() != r.Selected.Count ||
                r.Stage == BoundaryWurongStage.GiftChoice && r.Selected.Any(id => _cardZones.GetLocation(id) is not { OwnerSeat: { } holder, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } || holder != r.Giver))
                throw new InvalidOperationException("Wurong's gift must retain its exact distinct owned HE entities and direction.");
        }
        if (r.Stage == BoundaryWurongStage.GiftChildren || r.Stage == BoundaryWurongStage.Complete && r.Required > 0)
        {
            var paid = ProgramEventHistory<BoundaryWurongGiftPaidEvent>().Where(e => e.FrameId == f.Id).ToArray(); var entities = ProgramEventHistory<BoundaryWurongGiftEntityEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (r.Selected.Count != r.Required || r.Before < 0 || r.After < r.Before || r.After > _movementSequence || paid is not [var invoice] ||
                invoice != new BoundaryWurongGiftPaidEvent(f.Id, r.Giver, r.Recipient, r.Required, r.Before, r.After) || entities.Length != r.Required ||
                entities.Select(e => e.CardId).Distinct().Count() != r.Required || entities.Any(e => !r.Selected.Contains(e.CardId) || e.From.OwnerSeat != r.Giver ||
                    e.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || e.To != CardLocation.Hand(r.Recipient) || e.Sequence <= r.Before || e.Sequence > r.After ||
                    !_cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason.Value == BoundaryWurongGift)) ||
                r.Stage == BoundaryWurongStage.GiftChildren && f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } ||
                r.Stage == BoundaryWurongStage.Complete && f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Wurong lost its one complete native gift batch or original movement return.");
        }
    }
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 54200)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 54200)]
    private bool BoundaryWurongGiftMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) =>
        f.BoundaryWurong is { Stage: BoundaryWurongStage.GiftChildren } r && e is { Op: SkillProgramEffectOp.ResolveBoundaryZhangNiWurong, StateId: "gifts" } &&
        f.PendingMovementContinuation == p && p.SubjectSeat == r.Giver && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54200)]
    private bool BoundaryWurongGiftRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.BoundaryWurong is { Stage: BoundaryWurongStage.GiftChildren } r &&
        RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion &&
        r.Selected.Any(id => GetAdvancedCard(id).Kind == CardKind.SilverLion && _cardMovements.Any(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(r.Giver)));
    private bool BoundaryWurongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryWurong is not { Stage: BoundaryWurongStage.GiftChildren } r) return false;
        AssertBoundaryWurong(f);
        if (child is RecoveryReplacementFrame recovery) return BoundaryWurongGiftRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp) return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame state) return state.ResumeProgramFrameId == f.Id &&
            (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange);
        return child is CardsMovedTriggerWindowFrame moved && ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id &&
            moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat &&
            moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }
    private ProgramSkillFrame? BoundaryWurongObserverRoot() => FindProgramObserverRoot(static (e, f, c) => e.BoundaryWurongFirstChild(f, c),
        static (e, i) => e.HasObserverSuffix(i, static (g, n) => g.IsMovementObserverEdge(n) || g.IsSameNameHandObserverEdge(n) || g.IsYanPuDyingExitEdge(n) ||
            g.TypedDamagePostEventEdge(g._resolutionStack[n - 1], g._resolutionStack[n]) || g.RecoveryReplacementFrameRidesOn(g._resolutionStack[n], g._resolutionStack[n - 1]),
            static (g, n, d) => g.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 54200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 54200)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 54200)]
    private bool BoundaryWurongDamageObserver(long id) => BoundaryWurongObserverRoot() is { } root && _resolutionStack.FindIndex(f => f.Id == id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 54200)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54200)]
    private bool BoundaryWurongDyingObserver() => ActiveDying is { } dying && BoundaryWurongObserverRoot() is { } root && _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54200)]
    private bool AdvanceBoundaryWurongObservers() => _pendingDecision is null && BoundaryWurongObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
