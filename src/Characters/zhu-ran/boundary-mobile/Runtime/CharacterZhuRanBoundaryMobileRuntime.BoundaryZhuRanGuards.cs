#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRanBoundaryMobileRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 53900)]
    internal void AssertBoundaryDanshou(ProgramSkillFrame f)
    {
        if (f.BoundaryDanshou is not { } r)
            return;
        var c = f.WindowContext;
        if (f.SkillId != BoundaryZhuRanComposition.Skill || f.TriggerId is not (BoundaryZhuRanComposition.Draw or BoundaryZhuRanComposition.Pay) || f.ActivationId != f.TriggerId || f.InstructionIndex is < 1 or > 2 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != BoundaryDanshouSource(f) || r.Hash != f.GameplayHash || c is null || c.ParentFrameId != r.EndingId || c.SourceSeat != r.Actor || c.TargetSeat != r.Actor || BoundaryDanshouMother(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c)is not { } parent || parent.TurnNumber != r.Turn || r.Actor != parent.OwnerSeat || r.Actor == f.OwnerSeat || r.Cutoff < 0 || r.Cutoff > _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count() || r.X != BoundaryDanshouCount(f.OwnerSeat, r.Actor, r.Turn, r.TurnOwner, r.Cutoff) || (f.TriggerId == BoundaryZhuRanComposition.Draw ? r.X != 0 || r.Paid || r.Stage != BoundaryDanshouStage.Complete : r.X <= 0) || !Enum.IsDefined(r.Stage) || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Count > r.X || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryDanshouStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new BoundaryDanshouStartedEvent(f.Id, r.Source, r.Hash, r.EndingId, r.Turn, r.TurnOwner, r.Actor, r.Cutoff, r.X))
            throw new InvalidOperationException("Danshou lost its exact Ending mother, actual whole-use history cutoff or original source/hash.");
        if (r.Stage == BoundaryDanshouStage.Selecting)
        {
            var legal = BoundaryDanshouMaterials(f.OwnerSeat).Select(card => card.Id).ToHashSet();
            if (r.Paid || r.Selected.Any(id => !legal.Contains(id)) || f.InstructionIndex != 1 || f.PendingMovementContinuation is not null || r.DiscardInvocationId is not null || r.Actual != 0 || r.Before != 0 || r.After != 0)
                throw new InvalidOperationException("Danshou's unpaid draft must retain only its current distinct owned HE materials.");
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } p || p.PlayerSeat != f.OwnerSeat || !p.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices.Select(choice => choice with { Parameters = choice.Parameters.Where(pair => !pair.Key.StartsWith("owned-selection-", StringComparison.Ordinal)).ToDictionary(pair => pair.Key, pair => pair.Value) }).ToArray(), BoundaryDanshouChoices(f))))
                throw new InvalidOperationException("Danshou's original owner must retain its current whole-payment prompt.");
            return;
        }

        if (!r.Paid)
            return;
        if (r.Selected.Count != r.X || r.Before < 0 || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Actual < 0 || r.Actual > r.X || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryDanshouPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid != new BoundaryDanshouPaidEvent(f.Id, r.X, r.DiscardInvocationId, r.Actual, r.Before, r.After))
            throw new InvalidOperationException("Danshou lost its once-paid exact logical discard invoice.");
        if (r.DiscardInvocationId is { } invocation)
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.InvocationId == invocation).ToArray()is not [var native] || native.ParentFrameId != f.Id || native.InitiatorSeat != f.OwnerSeat || native.ExecutorSeat != f.OwnerSeat || native.SubjectSeat != f.OwnerSeat || native.TurnNumber != r.Turn || native.ActualTurnOwnerSeat != r.TurnOwner || native.Reason.Value != BoundaryDanshouDiscard || native.SequenceBefore != r.Before || native.SequenceAfter > r.After || native.ActualCount != r.Actual)
                throw new InvalidOperationException("Danshou cannot borrow another frame or actor's logical discard request.");
            var materials = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == invocation).ToArray();
            if (materials.Length != r.Actual || materials.Select(m => m.CardId).Distinct().Count() != materials.Length || materials.Any(m => !r.Selected.Contains(m.CardId) || m.From.OwnerSeat != f.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || m.CompletionSequence <= r.Before || m.CompletionSequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == m.CompletionSequence && move.CardId == m.CardId && move.To == m.To && (move.From == m.From || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(move) == m.From))))
                throw new InvalidOperationException("Danshou's paid materials differ from its native owner-scoped movement ledger.");
        }
        else if (r.Actual != 0)
            throw new InvalidOperationException("Danshou cannot manufacture a successful discard without its native invoice.");
        if (r.Stage == BoundaryDanshouStage.PaidChildren && (f.InstructionIndex != 1 || f.PendingMovementContinuation is { } pending && (pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)))
            throw new InvalidOperationException("Danshou lost its original paid movement continuation.");
        if (r.Stage == BoundaryDanshouStage.Complete && (f.PendingMovementContinuation is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryDanshouCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var complete] || complete.Actual != r.Actual || complete.DamageAuthorized && r.Actual != r.X))
            throw new InvalidOperationException("Danshou must finish its original complete payment before authorizing native damage.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 53900)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 53900)]
    internal bool BoundaryDanshouMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.BoundaryDanshou is { Stage: BoundaryDanshouStage.PaidChildren } && e?.Op == SkillProgramEffectOp.PrepareBoundaryZhuRanDanshou && f.PendingMovementContinuation == p && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 53900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 53900)]
    internal bool BoundaryDanshouRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.BoundaryDanshou is { Stage: BoundaryDanshouStage.PaidChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Selected.Any(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.Equipment(f.OwnerSeat)));
    internal bool BoundaryDanshouFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryDanshou is not { Stage: BoundaryDanshouStage.PaidChildren } r)
            return false;
        AssertBoundaryDanshou(f);
        if (child is RecoveryReplacementFrame recovery)
            return BoundaryDanshouRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal ProgramSkillFrame? BoundaryDanshouObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhuRanBoundaryMobileOperations.BoundaryDanshouFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 53900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 53900)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 53900)]
    internal bool BoundaryDanshouDamageObserver(long id) => BoundaryDanshouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 53900)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 53900)]
    internal bool BoundaryDanshouDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && BoundaryDanshouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 53900)]
    internal bool AdvanceBoundaryDanshouObservers() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryDanshouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
