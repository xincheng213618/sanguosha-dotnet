#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangNiBoundaryMobileRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 54200)]
    internal void AssertBoundaryWurong(ProgramSkillFrame f)
    {
        if (f.BoundaryWurong is not { } r)
            return;
        if (f.SkillId != BoundaryZhangNiComposition.Skill || f.ActivationId != BoundaryZhangNiComposition.Activation || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex is < 1 or > 7 || f.SelectedCardIds.Count != 0 || !f.SelectedTargetSeats.SequenceEqual([r.Target]) || r.Source != BoundaryWurongSource(f) || r.Hash != f.GameplayHash || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || r.Phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new BoundaryWurongStartedEvent(f.Id, r.Source, r.Hash, r.Phase, r.Target))
            throw new InvalidOperationException("Wurong lost its exact original activation, actual Play phase, selected counterpart or source/hash.");
        var commits = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var revealed = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongRevealedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == BoundaryWurongStage.OwnerChoice ? r.Suppress is not null || r.Resist is not null || r.Outcome is not null || commits.Length != 0 || revealed.Length != 0 || f.InstructionIndex != 1 : r.Stage == BoundaryWurongStage.TargetChoice ? r.Suppress is null || r.Resist is not null || r.Outcome is not null || commits is not [{ First: true }] || commits[0].Participant != f.OwnerSeat || revealed.Length != 0 || f.InstructionIndex != 1 : r.Suppress is null || r.Resist is null || r.Outcome != (r.Suppress.Value ? "suppress" : "appease") + (r.Resist.Value ? "-resist" : "-submit") || commits is not [{ First: true }, { First: false }] || commits[0].Participant != f.OwnerSeat || commits[1].Participant != r.Target || revealed is not [var result] || result != new BoundaryWurongRevealedEvent(f.Id, r.Suppress.Value, r.Resist.Value, r.Outcome))
            throw new InvalidOperationException("Wurong cannot publish or replace either strategy before both original participants commit.");
        if (r.Stage is BoundaryWurongStage.OwnerChoice or BoundaryWurongStage.TargetChoice or BoundaryWurongStage.GiftChoice)
        {
            var chooser = r.Stage == BoundaryWurongStage.OwnerChoice ? f.OwnerSeat : r.Stage == BoundaryWurongStage.TargetChoice ? r.Target : r.Giver;
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } p || p.PlayerSeat != chooser || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices.Select(c => c with { Parameters = c.Parameters.Where(x => !x.Key.StartsWith("owned-selection-", StringComparison.Ordinal)).ToDictionary(x => x.Key, x => x.Value) }).ToArray(), BoundaryWurongChoices(f))))
                throw new InvalidOperationException("Wurong's current private chooser or original staged-card prompt changed.");
        }

        if (r.Stage is BoundaryWurongStage.GiftChoice or BoundaryWurongStage.GiftChildren)
        {
            if (f.InstructionIndex != 5 || r.Resist != false || r.Giver != (r.Suppress == true ? f.OwnerSeat : r.Target) || r.Recipient != (r.Suppress == true ? r.Target : f.OwnerSeat) || r.Required is < 1 or > 2 || r.Selected.Count > r.Required || r.Selected.Distinct().Count() != r.Selected.Count || r.Stage == BoundaryWurongStage.GiftChoice && r.Selected.Any(id => _cardZonesOperations._cardZones.GetLocation(id)is not { OwnerSeat: { } holder, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } || holder != r.Giver))
                throw new InvalidOperationException("Wurong's gift must retain its exact distinct owned HE entities and direction.");
        }

        if (r.Stage == BoundaryWurongStage.GiftChildren || r.Stage == BoundaryWurongStage.Complete && r.Required > 0)
        {
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongGiftPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
            var entities = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongGiftEntityEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (r.Selected.Count != r.Required || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || paid is not [var invoice] || invoice != new BoundaryWurongGiftPaidEvent(f.Id, r.Giver, r.Recipient, r.Required, r.Before, r.After) || entities.Length != r.Required || entities.Select(e => e.CardId).Distinct().Count() != r.Required || entities.Any(e => !r.Selected.Contains(e.CardId) || e.From.OwnerSeat != r.Giver || e.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || e.To != CardLocation.Hand(r.Recipient) || e.Sequence <= r.Before || e.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason.Value == BoundaryWurongGift)) || r.Stage == BoundaryWurongStage.GiftChildren && f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } || r.Stage == BoundaryWurongStage.Complete && f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Wurong lost its one complete native gift batch or original movement return.");
        }
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 54200)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 54200)]
    internal bool BoundaryWurongGiftMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.BoundaryWurong is { Stage: BoundaryWurongStage.GiftChildren } r && e is { Op: SkillProgramEffectOp.ResolveBoundaryZhangNiWurong, StateId: "gifts" } && f.PendingMovementContinuation == p && p.SubjectSeat == r.Giver && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 54200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 54200)]
    internal bool BoundaryWurongGiftRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.BoundaryWurong is { Stage: BoundaryWurongStage.GiftChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Selected.Any(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(r.Giver)));
    internal bool BoundaryWurongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryWurong is not { Stage: BoundaryWurongStage.GiftChildren } r)
            return false;
        AssertBoundaryWurong(f);
        if (child is RecoveryReplacementFrame recovery)
            return BoundaryWurongGiftRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal ProgramSkillFrame? BoundaryWurongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhangNiBoundaryMobileOperations.BoundaryWurongFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 54200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 54200)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 54200)]
    internal bool BoundaryWurongDamageObserver(long id) => BoundaryWurongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 54200)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 54200)]
    internal bool BoundaryWurongDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && BoundaryWurongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 54200)]
    internal bool AdvanceBoundaryWurongObservers() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryWurongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
