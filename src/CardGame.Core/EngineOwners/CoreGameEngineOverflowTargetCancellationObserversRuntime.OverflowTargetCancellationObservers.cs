#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOverflowTargetCancellationObserversRuntime
{
    internal static bool SameOverflowAction(CardActionContext a, CardActionContext b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OverflowTargetCancellationObservers.SameOverflowAction(a, b);
    internal static bool SameOverflowIssuance(OverflowTargetCancellationReceipt a, OverflowTargetCancellationReceipt b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_OverflowTargetCancellationObservers.SameOverflowIssuance(a, b);
    internal bool HasOverflowAcceptedPrefix(CardUseFrame use, OverflowUseTargetsCanceledEvent fact, IReadOnlyList<IGameEvent> history, int boundary)
    {
        var r = fact.Receipt;
        var before = r.ActionBeforeCancellation;
        var prior = history.Take(boundary).ToArray();
        var previousUse = use with
        {
            SourceSeat = before.ActorSeat,
            CardKind = before.EffectiveKind,
            TargetSeats = r.BeforeTargetSeats,
            Action = before
        };
        if (prior.OfType<CardUseDeclaredEvent>().Where(e => e.ResolutionId == use.Id && e.CardId == use.CardId).ToArray()is not [var declared] || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(previousUse, declared.SourceSeat, before.ProviderSeat))
            return false;
        for (var i = prior.Length - 1; i >= 0; i--)
        {
            if (prior[i] is not CardActionAcceptedEvent { Action: var accepted } || accepted.ActionId != before.ActionId || accepted.Type != CardActionType.Use || accepted.ProviderSeat != before.ProviderSeat || accepted.ParentActionId != before.ParentActionId || accepted.RequesterSeat != before.RequesterSeat || accepted.ResponderSeat != before.ResponderSeat || accepted.OpponentSeat != before.OpponentSeat || accepted.EffectiveSuit != before.EffectiveSuit || accepted.EffectiveRank != before.EffectiveRank || !accepted.PhysicalCards.SequenceEqual(before.PhysicalCards))
                continue;
            if (_coreCompletedCategoryUseProofOperations.RebuildAcceptedActualHandGainUse(previousUse, accepted, before, prior, i))
                return true;
        }

        return false;
    }

    internal bool IsOverflowTargetCancellationFact(CardUseFrame use, OverflowUseTargetsCanceledEvent fact, IReadOnlyList<int> beforeTargets)
    {
        var r = fact.Receipt;
        var before = r.ActionBeforeCancellation;
        if (use.Action is not { Type: CardActionType.Use } current || r.CardUseFrameId != use.Id || r.ActionId != current.ActionId || r.InstructionIndex != 1 || r.WindowFrameId <= use.Id || fact.ProgramFrameId <= r.WindowFrameId || before.Type != CardActionType.Use || before.ActionId != r.ActionId || before.ActorSeat != r.Source.OwnerSeat || before.ProviderSeat != r.ProviderSeat || before.EffectiveKind != r.EffectiveKind || before.ResponderSeat is not null || before.OpponentSeat is not null || current.ProviderSeat != r.ProviderSeat || !current.PhysicalCards.SequenceEqual(before.PhysicalCards) || !r.BeforeTargetSeats.SequenceEqual(beforeTargets) || !before.TargetSeats.SequenceEqual(r.BeforeTargetSeats) || r.HandCount <= r.HandLimit || r.HandLimit < 0 || r.MaterialCardIds.Distinct().Count() != r.MaterialCardIds.Count || r.MaterialCardIds.Any(id => !before.PhysicalCards.Any(c => c.CardId == id)) || r.BeforeTargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.OriginalTargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.EffectiveKind == CardKind.BorrowedSword && (r.BeforeTargetSeats.Count % 2 != 0 || r.OriginalTargetSeats.Count % 2 != 0) || !r.CanceledPrimaryTargetSeats.SequenceEqual(global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime.OverflowPrimaryTargets(r.EffectiveKind, r.BeforeTargetSeats).Where(s => s != r.Source.OwnerSeat)) || r.CanceledPrimaryTargetSeats.Count == 0 || !r.ResultTargetSeats.SequenceEqual(global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime.OverflowRetainedTargets(r.EffectiveKind, r.Source.OwnerSeat, r.BeforeTargetSeats)) || r.SequenceBefore < 0 || r.SequenceAfter != r.SequenceBefore || r.MovementBatchId is not null || r.MovementIssued || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program is not { } program || program.GameplayHash != r.GameplayHash || program.Triggers.SingleOrDefault(t => t.Id == r.Source.BindingId)is not { } trigger)
            return false;
        OverflowTargetCancellationContract.ValidateTrigger(r.Source.SkillId, trigger);
        var originalPrimary = global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime.OverflowPrimaryTargets(r.EffectiveKind, r.OriginalTargetSeats);
        int? expectedRecipient = originalPrimary is [var unique] && unique != r.Source.OwnerSeat ? unique : null;
        if (r.RecipientSeat != expectedRecipient || r.RecipientSeat is null && r.MaterialCardIds.Count != 0)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var facts = history.OfType<OverflowUseTargetsCanceledEvent>().Where(e => e.ProgramFrameId == fact.ProgramFrameId).ToArray();
        var boundary = Array.FindIndex(history, e => e is OverflowUseTargetsCanceledEvent canceled && canceled.ProgramFrameId == fact.ProgramFrameId);
        return facts is [var issued] && SameOverflowIssuance(issued.Receipt, r) && boundary >= 0 && history.Take(boundary).OfType<TargetsConfirmedEvent>().Where(e => e.ResolutionId == use.Id).ToArray()is [var declaredTargets] && declaredTargets.TargetSeats.SequenceEqual(r.OriginalTargetSeats) && history.Take(boundary).OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.SkillId == r.Source.SkillId && e.OwnerSeat == r.Source.OwnerSeat && e.BindingId == r.Source.BindingId && e.SkillInstanceId == r.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1 && HasOverflowAcceptedPrefix(use, fact, history, boundary);
    }

    internal bool ValidOverflowTargetCancellationReceipt(ProgramSkillFrame frame)
    {
        if (frame.OverflowTargetCancellation is not { } r || !_coreOverflowTargetCancellationOperations.ExactOverflowTargetParent(frame, out var use, out _) || r.Source != new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || r.GameplayHash != frame.GameplayHash || r.WindowFrameId != frame.WindowContext!.ParentFrameId || r.CardUseFrameId != use.Id || r.ActionId != use.Action!.ActionId || r.ProviderSeat != use.Action.ProviderSeat || r.EffectiveKind != use.CardKind || r.InstructionIndex != frame.InstructionIndex || r.SequenceAfter < r.SequenceBefore || !use.TargetSeats.SequenceEqual(r.ResultTargetSeats) || !SameOverflowAction(use.Action, _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(r.ActionBeforeCancellation, r.ResultTargetSeats)) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OverflowUseTargetsCanceledEvent>().Where(e => e.ProgramFrameId == frame.Id).ToArray()is not [var fact] || !SameOverflowIssuance(fact.Receipt, r) || !IsOverflowTargetCancellationFact(use, fact, r.BeforeTargetSeats) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OverflowTargetCancellationCompletedEvent>().Any(e => e.ProgramFrameId == frame.Id))
            return false;
        var movements = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OverflowUseMaterialTransferIssuedEvent>().Where(e => e.ProgramFrameId == frame.Id).ToArray();
        if (r.MaterialCardIds.Count == 0)
            return !r.MovementIssued && r.MovementBatchId is null && r.SequenceBefore == r.SequenceAfter && movements.Length == 0 && frame.PendingMovementContinuation is null;
        if (!r.MovementIssued || r.MovementBatchId is null || r.RecipientSeat is not { } recipient || movements is not [var movement] || movement.CardUseFrameId != r.CardUseFrameId || movement.ActionId != r.ActionId || movement.RecipientSeat != recipient || movement.ActualCount != r.MaterialCardIds.Count || movement.SequenceBefore != r.SequenceBefore || movement.SequenceAfter != r.SequenceAfter || movement.MovementBatchId != r.MovementBatchId || frame.PendingMovementContinuation is not { } pending || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        var paid = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        return paid.Select(m => m.CardId).SequenceEqual(r.MaterialCardIds) && paid.All(m => m.From == CardLocation.Processing && m.To == CardLocation.Hand(recipient) && m.Reason.Value == "skill-program.overflow-target-cancellation.give" && r.ActionBeforeCancellation.PhysicalCards.Any(c => c.CardId == m.CardId && c.CardKind == m.CardKind));
    }

    internal bool OverflowTargetCancellationFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.OverflowTargetCancellation is not { } r || !ValidOverflowTargetCancellationReceipt(frame))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.MovementIssued && moved.ResumeProgramFrameId is null && moved.Batch.Id == r.MovementBatchId && moved.Batch.ParentFrameId == frame.Id && moved.Batch.AwaitingProgramFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.Count == r.MaterialCardIds.Count && moved.Batch.Movements.Select(m => m.CardId).SequenceEqual(r.MaterialCardIds) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.From == CardLocation.Processing && m.To == CardLocation.Hand(r.RecipientSeat!.Value) && m.Reason.Value == "skill-program.overflow-target-cancellation.give");
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == frame.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == frame.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal bool OverflowTargetCancellationStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => parent is ProgramSkillFrame root && OverflowTargetCancellationFirstChild(root, child) || parent is ProgramCardTriggerWindowFrame window && child is ProgramSkillFrame { OverflowTargetCancellation: { } r } program && r.WindowFrameId == window.Id && ValidOverflowTargetCancellationReceipt(program);
    internal ProgramSkillFrame? OverflowTargetCancellationObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.OverflowTargetCancellationFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => this.OverflowTargetCancellationObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(child, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(child, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(child, dying))));
    internal bool OverflowTargetCancellationObserverEdge(int child)
    {
        var parent = _resolutionStackOperations._resolutionStack[child - 1];
        var current = _resolutionStackOperations._resolutionStack[child];
        return global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime.IsObserverSkillsChangedEdge(parent, current) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, current) || _coreOriginalHandEntityObserversOperations.OriginalHandEntityStructuralEdge(parent, current) || _coreOutsidePhaseDrawDiscardObserversOperations.OutsidePhaseDrawDiscardStructuralEdge(parent, current) || _coreCompletedUndamagedTargetRevealObserversOperations.CompletedUndamagedTargetRevealStructuralEdge(parent, current) || _coreRecipientCategoryMarkObserversOperations.RecipientCategoryMarkStructuralEdge(parent, current) || _coreOffTurnUsedCardObserversOperations.OffTurnUsedCardGiftStructuralEdge(parent, current) || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandStructuralEdge(parent, current) || _coreResponseCompletionOperations.ResponseCompletionStructuralEdge(parent, current) || _coreCardSupplyCompletionOperations.CardSupplyCompletionStructuralEdge(parent, current) || _coreMatchingRecastObserversOperations.MatchingRecastStructuralEdge(parent, current) || _coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossStructuralEdge(parent, current) || OverflowTargetCancellationStructuralEdge(parent, current) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child);
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 600)]
    internal bool IsOverflowTargetCancellationDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && OverflowTargetCancellationObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 30)]
    internal bool TryAdvanceOverflowTargetCancellationSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || OverflowTargetCancellationObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
