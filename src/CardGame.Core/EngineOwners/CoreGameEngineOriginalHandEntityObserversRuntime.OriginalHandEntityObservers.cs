#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOriginalHandEntityObserversRuntime
{
    internal bool OriginalHandBindingStarted(ProgramSkillFrame f, SkillProgramTriggerWindow window) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == window) == 1;
    internal bool ExactOriginalHandLifecycleParent(ProgramSkillFrame f, SkillProgramTriggerWindow window)
    {
        if (f.TriggerId is null || f.WindowContext is not { } context || context.Window != window || context.OwnerSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.Window != window || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || context.SourceSeat != parent.OwnerSeat || context.TargetSeat != parent.OwnerSeat)
            return false;
        if (window == SkillProgramTriggerWindow.GameStarting && (parent.Continuation != ProgramLifecycleContinuation.CompleteGameStarting || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.NotStarted))
            return false;
        if (window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && (parent.Continuation != ProgramLifecycleContinuation.NormalTurnStart || parent.OwnerSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != f.OwnerSeat))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == parent.Id;
    }

    internal bool ExactOriginalHandBenefitParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch, MovementIndex: { } occurrence } context || context.OwnerSeat != f.OwnerSeat || context.SourceSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || occurrence < 0 || occurrence >= batch.Movements.Count || context.OccurrenceIndex != occurrence || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.Batch.Id != batch.Id || !parent.Batch.Movements.SequenceEqual(batch.Movements) || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || parent.Candidates[parent.CandidateIndex].OccurrenceIndex != occurrence)
            return false;
        var movement = batch.Movements[occurrence];
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == parent.Id && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement) && movement.From == CardLocation.Hand(f.OwnerSeat) && movement.To != movement.From;
    }

    internal bool ExactOriginalHandInheritanceParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } context || context.OwnerSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.OwnerSeat != f.OwnerSeat || parent.KillerSeat != context.SourceSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 1 && _resolutionStackOperations._resolutionStack[index - 1].Id == parent.Id && _resolutionStackOperations._resolutionStack[index - 2] is DeathFrame death && death.Id == parent.DeathFrameId && death.VictimSeat == f.OwnerSeat;
    }

    internal bool ValidOriginalHandBenefitReceipt(ProgramSkillFrame f)
    {
        if (f.OriginalHandBenefit is not { } r || f.InstructionIndex != 1 || r.InstructionIndex != 1 || !ExactOriginalHandBenefitParent(f) || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.OfferOriginalHandLossBenefit } effect] || effect.StateId != r.StateId || r.MovementWindowId != f.WindowContext!.ParentFrameId || r.BatchId != f.WindowContext.MovementBatch!.Id || r.MovementSequence != f.WindowContext.MovementBatch.Movements[f.WindowContext.MovementIndex!.Value].Sequence || !_coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandEntityStates.TryGetValue(new(f.OwnerSeat, f.SkillId, r.StateId), out var state) || !state.Entities.Any(e => e.CardId == f.WindowContext.MovementBatch.Movements[f.WindowContext.MovementIndex.Value].CardId && e.FirstLossSequence == r.MovementSequence) || _coreOriginalHandEntitiesOperations.OriginalHandLossEligibility(f.OwnerSeat, f.SkillId, r.StateId, f.TriggerId!, r.MovementSequence)is not { } eligibility || eligibility.SkillInstanceId != f.SkillInstanceId || eligibility.GameplayHash != f.GameplayHash || r.CandidateSeats.Count == 0 || r.CandidateSeats.Distinct().Count() != r.CandidateSeats.Count || r.CandidateSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.TargetSeat is { } selected && !r.CandidateSeats.Contains(selected))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (!OriginalHandBindingStarted(f, SkillProgramTriggerWindow.CardsMoved) || history.OfType<OriginalHandBenefitStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new OriginalHandBenefitStartedEvent(f.Id, r.Source, r.GameplayHash, r.StateId, r.MovementWindowId, r.BatchId, r.MovementSequence) || history.OfType<OriginalHandBenefitStartedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.StateId == r.StateId && e.MovementSequence == r.MovementSequence) != 1)
            return false;
        var chosen = history.OfType<OriginalHandBenefitChosenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var bonuses = history.OfType<OriginalHandPermanentBonusGrantedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var draws = history.OfType<OriginalHandBenefitDrawIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var completed = history.OfType<OriginalHandBenefitCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Option is { } kind ? !Enum.IsDefined(kind) || chosen is not [var choice] || choice.TargetSeat != r.TargetSeat || choice.Kind != kind : chosen.Length != 0)
            return false;
        if (r.BonusApplied ? r.Option is not (OriginalHandBenefitKind.HandLimit or OriginalHandBenefitKind.AttackRange) || bonuses is not [var granted] || granted.SourceOwnerSeat != f.OwnerSeat || granted.SourceSkillId != f.SkillId || granted.StateId != r.StateId || granted.RecipientSeat != r.TargetSeat || granted.Kind != r.Option : bonuses.Length != 0)
            return false;
        if (r.DrawIssued)
        {
            if (r.Option != OriginalHandBenefitKind.Draw || r.TargetSeat is not { } recipient || r.BonusApplied || r.ActualDrawCount is < 0 or > 1 || r.DrawBefore < 0 || r.DrawAfter < r.DrawBefore || r.DrawAfter > _coreOriginalHandEntitiesOperations.OriginalHandMovementSequence || draws is not [var draw] || draw != new OriginalHandBenefitDrawIssuedEvent(f.Id, recipient, r.ActualDrawCount, r.DrawBefore, r.DrawAfter))
                return false;
            var movements = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.DrawBefore && m.Sequence <= r.DrawAfter).ToArray();
            var reason = new CardMoveReason($"skill-program.{(f.SkillId)}.original-hand-benefit.draw");
            if (movements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) && m.Reason == reason) != r.ActualDrawCount || movements.Any(m => !(m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) && m.Reason == reason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)))
                return false;
        }
        else if (r.ActualDrawCount != 0 || r.DrawBefore != 0 || r.DrawAfter != 0 || draws.Length != 0)
            return false;
        if (r.Stage == OriginalHandBenefitStage.Complete)
            return f.PendingMovementContinuation is null && completed is [var completion] && completion == new OriginalHandBenefitCompletedEvent(f.Id, r.TargetSeat, r.Option, r.BonusApplied, r.DrawIssued, r.ActualDrawCount);
        if (completed.Length != 0)
            return false;
        return r.Stage switch
        {
            OriginalHandBenefitStage.ChoosingTarget => r.TargetSeat is null && r.Option is null && !r.BonusApplied && !r.DrawIssued && f.PendingMovementContinuation is null,
            OriginalHandBenefitStage.ChoosingOption => r.TargetSeat is not null && r.Option is null && !r.BonusApplied && !r.DrawIssued && f.PendingMovementContinuation is null,
            OriginalHandBenefitStage.DrawChildren => r.DrawIssued && f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == r.TargetSeat,
            _ => false
        };
    }

    internal bool ValidOriginalHandInheritanceReceipt(ProgramSkillFrame f)
    {
        if (f.OriginalHandInheritance is not { } r || f.InstructionIndex != 1 || r.InstructionIndex != 1 || !ExactOriginalHandInheritanceParent(f) || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || r.DeathWindowId != f.WindowContext!.ParentFrameId || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.InheritOriginalHandBonuses } effect] || effect.StateId != r.StateId || _resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().Single(w => w.Id == r.DeathWindowId).DeathFrameId != r.DeathFrameId || f.PendingMovementContinuation is not null || r.CandidateSeats.Count == 0 || r.CandidateSeats.Distinct().Count() != r.CandidateSeats.Count || r.CandidateSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == f.OwnerSeat) || r.Bonuses.Count == 0 || r.Bonuses.Select(b => b.BonusId).Distinct().Count() != r.Bonuses.Count || r.Bonuses.Any(b => b.RecipientSeat != f.OwnerSeat || b.SourceSkillId != f.SkillId || b.StateId != r.StateId || b.Kind is not (OriginalHandBenefitKind.HandLimit or OriginalHandBenefitKind.AttackRange)))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (!OriginalHandBindingStarted(f, SkillProgramTriggerWindow.OwnerDied) || history.OfType<OriginalHandInheritanceStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new OriginalHandInheritanceStartedEvent(f.Id, r.Source, r.GameplayHash, r.StateId, r.DeathFrameId, r.DeathWindowId, r.Bonuses.Count))
            return false;
        var transfers = history.OfType<OriginalHandBonusTransferredEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var completed = history.OfType<OriginalHandInheritanceCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == OriginalHandInheritanceStage.ChoosingTarget)
            return !r.Applied && r.TargetSeat is null && transfers.Length == 0 && completed.Length == 0 && r.Bonuses.SequenceEqual(_coreOriginalHandEntitiesOperations.OriginalHandInheritableBonuses(f.OwnerSeat, f.SkillId, r.StateId));
        return r.Applied && r.TargetSeat is { } recipient && r.CandidateSeats.Contains(recipient) && transfers.SequenceEqual(r.Bonuses.Select(b => new OriginalHandBonusTransferredEvent(f.Id, b.BonusId, b.SourceOwnerSeat, b.SourceSkillId, b.StateId, f.OwnerSeat, recipient, b.Kind))) && r.Bonuses.All(b => _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandPermanentBonuses.GetValueOrDefault(b.BonusId) == (b with { RecipientSeat = recipient })) && completed is [var completion] && completion == new OriginalHandInheritanceCompletedEvent(f.Id, recipient, r.Bonuses.Count);
    }

    internal bool OriginalHandBenefitFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.OriginalHandBenefit is not { Stage: OriginalHandBenefitStage.DrawChildren, DrawIssued: true, TargetSeat: { } recipient } r || !ValidOriginalHandBenefitReceipt(f) || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        var reason = new CardMoveReason($"skill-program.{(f.SkillId)}.original-hand-benefit.draw");
        return moved.ResumeProgramFrameId is null && moved.Batch.Id == moved.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.DrawBefore && m.Sequence <= r.DrawAfter && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient) && m.Reason == reason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle));
    }

    internal bool OriginalHandEntityFirstChild(ProgramSkillFrame f, ResolutionFrame child) => OriginalHandBenefitFirstChild(f, child) || f.OriginalHandAwakening is not null && _coreOriginalHandAwakeningOperations.OriginalHandAwakeningFirstChild(f, child);
    internal bool OriginalHandEntityStructuralEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (parent is ProgramSkillFrame f && OriginalHandEntityFirstChild(f, child))
            return true;
        return child is ProgramSkillFrame observer && observer.WindowContext?.ParentFrameId == parent.Id && (observer.OriginalHandBenefit is not null && ValidOriginalHandBenefitReceipt(observer) || observer.OriginalHandInheritance is not null && ValidOriginalHandInheritanceReceipt(observer) || observer.OriginalHandAwakening is not null && _coreOriginalHandAwakeningOperations.ValidOriginalHandAwakeningReceipt(observer));
    }

    internal ProgramSkillFrame? OriginalHandEntityObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.OriginalHandEntityFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => this.OriginalHandEntityObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    internal bool OriginalHandEntityObserverEdge(int child)
    {
        var parent = _resolutionStackOperations._resolutionStack[child - 1];
        var current = _resolutionStackOperations._resolutionStack[child];
        return global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime.IsObserverSkillsChangedEdge(parent, current) || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandStructuralEdge(parent, current) || _coreCompletedUndamagedTargetRevealObserversOperations.CompletedUndamagedTargetRevealStructuralEdge(parent, current) || _coreRecipientCategoryMarkObserversOperations.RecipientCategoryMarkStructuralEdge(parent, current) || _coreOffTurnUsedCardObserversOperations.OffTurnUsedCardGiftStructuralEdge(parent, current) || _coreCardSupplyCompletionOperations.CardSupplyCompletionStructuralEdge(parent, current) || _coreResponseCompletionOperations.ResponseCompletionStructuralEdge(parent, current) || OriginalHandEntityStructuralEdge(parent, current) || _coreOutsidePhaseDrawDiscardObserversOperations.OutsidePhaseDrawDiscardStructuralEdge(parent, current) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, current) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child);
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 70)]
    internal bool IsOriginalHandEntityProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && OriginalHandEntityObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 100)]
    internal bool TryAdvanceOriginalHandEntitySubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || OriginalHandEntityObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
