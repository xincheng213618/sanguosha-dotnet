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

    internal void AssertOriginalHandEntityState()
    {
        if (!_coreOriginalHandEntitiesOperations.TracksOriginalHandEntities && _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandEntityStates.Count == 0 && _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandPermanentBonuses.Count == 0)
            return;
        if (_coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandEntityStates.Count == 0 && _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandPermanentBonuses.Count == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<OriginalHandEntitiesInitializedEvent>().Count == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<OriginalHandEntityConsumedEvent>().Count == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<OriginalHandPermanentBonusGrantedEvent>().Count == 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<OriginalHandBonusTransferredEvent>().Count == 0)
            return;
        // Grants and transfers must still be replayed in their original mixed order.
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Where(fact => fact is OriginalHandEntitiesInitializedEvent or OriginalHandEntityConsumedEvent or OriginalHandPermanentBonusGrantedEvent or OriginalHandBonusTransferredEvent).ToArray();
        var initialized = history.OfType<OriginalHandEntitiesInitializedEvent>().ToArray();
        if (initialized.Length != _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandEntityStates.Count)
            throw new InvalidOperationException("Original-hand durable states lost their unique native initialization facts.");
        foreach (var pair in _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandEntityStates)
        {
            var key = pair.Key;
            var state = pair.Value;
            var dealt = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Reason == CardMoveReasons.InitialDeal && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(key.OwnerSeat)).OrderBy(m => m.Sequence).ToArray();
            var facts = initialized.Where(e => e.Source.OwnerSeat == key.OwnerSeat && e.Source.SkillId == key.SkillId && e.StateId == key.StateId).ToArray();
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(key.OwnerSeat) || state.Source.OwnerSeat != key.OwnerSeat || state.Source.SkillId != key.SkillId || state.InitializationFrameId <= 0 || facts is not [var init] || init.FrameId != state.InitializationFrameId || init.Source != state.Source || init.GameplayHash != state.GameplayHash || init.OriginalCount != dealt.Length || init.RemainingCount < 0 || init.RemainingCount > init.OriginalCount || state.Entities.Count != dealt.Length || state.Entities.Select(e => e.CardId).Distinct().Count() != state.Entities.Count)
                throw new InvalidOperationException("Original-hand durable state changed its actual initial-deal invoice.");
            for (var index = 0; index < dealt.Length; index++)
            {
                var entity = state.Entities[index];
                var deal = dealt[index];
                var first = _coreCardMovementJournalCardMovementsAccess._cardMovements.FirstOrDefault(m => m.Sequence > deal.Sequence && m.CardId == deal.CardId && m.From == CardLocation.Hand(key.OwnerSeat) && m.To != m.From)?.Sequence;
                if (entity.CardId != deal.CardId || entity.DealSequence != deal.Sequence || entity.FirstLossSequence != first)
                    throw new InvalidOperationException("An original physical entity was restored or consumed without its first actual hand loss.");
            }

            var losses = history.OfType<OriginalHandEntityConsumedEvent>().Where(e => e.OwnerSeat == key.OwnerSeat && e.SkillId == key.SkillId && e.StateId == key.StateId).ToArray();
            if (losses.Select(e => e.MovementSequence).Distinct().Count() != losses.Length || losses.Any(e => !state.Entities.Any(entity => entity.FirstLossSequence == e.MovementSequence)) || !losses.Select(e => e.RemainingCount).SequenceEqual(Enumerable.Range(0, losses.Length).Select(i => init.RemainingCount - i - 1)) || init.RemainingCount - losses.Length != state.Entities.Count(e => e.FirstLossSequence is null))
                throw new InvalidOperationException("Original-hand depletion lost its once-only sequence or remaining count.");
        }

        var expected = new Dictionary<long, OriginalHandPermanentBonus>();
        foreach (var fact in history)
        {
            if (fact is OriginalHandPermanentBonusGrantedEvent grant)
            {
                if (grant.Kind is not (OriginalHandBenefitKind.HandLimit or OriginalHandBenefitKind.AttackRange) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(grant.RecipientSeat) || !_coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandEntityStates.ContainsKey(new(grant.SourceOwnerSeat, grant.SourceSkillId, grant.StateId)) || !expected.TryAdd(grant.FrameId, new(grant.FrameId, grant.SourceOwnerSeat, grant.SourceSkillId, grant.StateId, grant.RecipientSeat, grant.Kind)))
                    throw new InvalidOperationException("Original-hand permanent bonuses lost their unique source attribution.");
            }

            if (fact is OriginalHandBonusTransferredEvent transfer)
            {
                if (!expected.TryGetValue(transfer.BonusId, out var previous) || previous.SourceOwnerSeat != transfer.SourceOwnerSeat || previous.SourceSkillId != transfer.SourceSkillId || previous.StateId != transfer.StateId || previous.Kind != transfer.Kind || previous.RecipientSeat != transfer.PreviousRecipientSeat || transfer.RecipientSeat == previous.RecipientSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(transfer.RecipientSeat))
                    throw new InvalidOperationException("Original-hand inheritance changed the identity or previous recipient of a received bonus.");
                expected[transfer.BonusId] = previous with
                {
                    RecipientSeat = transfer.RecipientSeat
                };
            }
        }

        if (expected.Count != _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandPermanentBonuses.Count || expected.Any(p => _coreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess._originalHandPermanentBonuses.GetValueOrDefault(p.Key) != p.Value))
            throw new InvalidOperationException("Original-hand permanent bonuses diverged from their actual grants and transfers.");
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

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 450)]
    internal void AssertOriginalHandEntityProgram(ProgramSkillFrame f)
    {
        var receipts = (f.OriginalHandBenefit is null ? 0 : 1) + (f.OriginalHandInheritance is null ? 0 : 1) + (f.OriginalHandAwakening is null ? 0 : 1);
        if (receipts == 0)
        {
            if (!_coreOriginalHandEntitiesOperations.TracksOriginalHandEntities || f.TriggerId is null || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects.All(effect => effect.Op is not (SkillProgramEffectOp.OfferOriginalHandLossBenefit or SkillProgramEffectOp.InheritOriginalHandBonuses or SkillProgramEffectOp.AwakenWhenOriginalHandEmpty)))
                return;
            var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
            if (history.OfType<OriginalHandBenefitStartedEvent>().Any(e => e.FrameId == f.Id) || history.OfType<OriginalHandInheritanceStartedEvent>().Any(e => e.FrameId == f.Id) || history.OfType<OriginalHandAwakeningMaximumPaidEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("An issued original-hand operation lost its owning receipt.");
            return;
        }

        if (receipts != 1 || f.OriginalHandBenefit is not null && !ValidOriginalHandBenefitReceipt(f) || f.OriginalHandInheritance is not null && !ValidOriginalHandInheritanceReceipt(f) || f.OriginalHandAwakening is not null && !_coreOriginalHandAwakeningOperations.ValidOriginalHandAwakeningReceipt(f))
            throw new InvalidOperationException("Original-hand operation lost its exact native parent, physical first loss, choice or issued payment.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        if (index + 1 < _resolutionStackOperations._resolutionStack.Count && !OriginalHandEntityFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]))
            throw new InvalidOperationException("Original-hand operation retained an unrelated first native child.");
        if (index == _resolutionStackOperations._resolutionStack.Count - 1 && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } prompt && (f.OriginalHandInheritance is { Stage: OriginalHandInheritanceStage.ChoosingTarget } || f.OriginalHandBenefit is { Stage: OriginalHandBenefitStage.ChoosingTarget or OriginalHandBenefitStage.ChoosingOption }) && !IsOriginalHandEntityChoice(f, prompt))
            throw new InvalidOperationException("Original-hand operation changed its published target or option choices.");
    }

    internal bool IsOriginalHandEntityChoice(ProgramSkillFrame f, PendingDecision decision)
    {
        if (decision.Kind != DecisionKind.ProgramTrigger || !decision.IsPrivate || decision.PlayerSeat != f.OwnerSeat || decision.SourceSeat != f.OwnerSeat || decision.ValidCardIds.Count != 0 || decision.ValidContentIds.Count != 0 || decision.RequiredCardCount != 0 || decision.SkillPrompt?.SkillId != f.SkillId)
            return false;
        var expected = _coreOriginalHandEntitiesOperations.OriginalHandEntityChoices(f);
        return decision.ValidTargetSeats.SequenceEqual(expected.SelectMany(c => c.Targets).Distinct()) && decision.Choices.Count == expected.Count && decision.Choices.Zip(expected).All(pair => pair.First.Id == pair.Second.Id && pair.First.Cards.Count == 0 && pair.First.Targets.SequenceEqual(pair.Second.Targets) && pair.First.Parameters.OrderBy(p => p.Key).SequenceEqual(pair.Second.Parameters.OrderBy(p => p.Key)));
    }

    internal ProgramSkillFrame? OriginalHandEntityObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreOriginalHandEntityObserversOperations.OriginalHandEntityFirstChild(root, child), (index) => _coreObserverSubtreesOperations2.HasObserverSuffix(index, (child) => _coreOriginalHandEntityObserversOperations.OriginalHandEntityObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    internal bool OriginalHandEntityObserverEdge(int child)
    {
        var parent = _resolutionStackOperations._resolutionStack[child - 1];
        var current = _resolutionStackOperations._resolutionStack[child];
        return global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime.IsObserverSkillsChangedEdge(parent, current) || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandStructuralEdge(parent, current) || _coreCompletedUndamagedTargetRevealObserversOperations.CompletedUndamagedTargetRevealStructuralEdge(parent, current) || _coreRecipientCategoryMarkObserversOperations.RecipientCategoryMarkStructuralEdge(parent, current) || _coreOffTurnUsedCardObserversOperations.OffTurnUsedCardGiftStructuralEdge(parent, current) || _coreCardSupplyCompletionOperations.CardSupplyCompletionStructuralEdge(parent, current) || _coreResponseCompletionOperations.ResponseCompletionStructuralEdge(parent, current) || OriginalHandEntityStructuralEdge(parent, current) || _coreOutsidePhaseDrawDiscardObserversOperations.OutsidePhaseDrawDiscardStructuralEdge(parent, current) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, current) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child);
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 10, group: 60)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 70)]
    internal bool IsOriginalHandEntityProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && OriginalHandEntityObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1850)]
    internal bool HasOriginalHandEntityDamageObserver(long windowId) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == windowId && f is DamageTriggerWindowFrame) && OriginalHandEntityObserverRoot()is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 100)]
    internal bool TryAdvanceOriginalHandEntitySubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || OriginalHandEntityObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
