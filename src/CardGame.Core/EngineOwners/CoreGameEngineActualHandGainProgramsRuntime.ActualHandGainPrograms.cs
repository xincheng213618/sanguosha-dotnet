#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualHandGainProgramsRuntime
{
    internal const string ActualHandGainDrawReason = "program.actual-hand-gain.draw";
    internal const string ForeignHandCleanupReason = "program.foreign-turn-hand-gain.cleanup";
    internal const string SameCategoryDeckGiftReason = "program.same-category-deck.gift";
    internal bool HasActualHandGainCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawAfterActualOwnHandGain) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DiscardForeignTurnHandGains);

    internal bool ActualHandGainTurnOpen(int owner) => _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == owner && _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._phase is not (TurnPhase.NotStarted or TurnPhase.Finished) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == owner);
    internal CardLocation ActualHandGainOriginalSource(CardMovementRecord first, CardMovementBatchContext? batch = null)
    {
        var from = first.From;
        var sequence = first.Sequence;
        while (from == CardLocation.Processing)
        {
            var prior = batch?.Movements.Where(m => m.CardId == first.CardId && m.Sequence < sequence).MaxBy(m => m.Sequence);
            var ledger = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == first.CardId && m.Sequence < sequence);
            if (ledger is not null && (prior is null || ledger.Sequence > prior.Sequence))
                prior = ledger;
            if (prior?.To != CardLocation.Processing)
                break;
            from = prior.From;
            sequence = prior.Sequence;
        }

        return from;
    }

    internal int[] MatchingActualHandGainIndexes(CardMovementBatchContext batch, int owner, CardLocation location) => batch.Movements.Select((m, i) => (m, i)).GroupBy(x => x.m.CardId).Where(g => g.Any(x => x.m.To == CardLocation.Hand(owner) && ActualHandGainOriginalSource(x.m, batch) != CardLocation.Hand(owner))).Select(g => g.OrderBy(x => x.m.Sequence).Last()).Where(x => x.m.To == CardLocation.Hand(owner) && x.m.To == location).Select(x => x.i).Order().ToArray();
    internal ActualHandGainEntity[] ActualHandGainEntities(CardMovementBatchContext batch, int owner) => MatchingActualHandGainIndexes(batch, owner, CardLocation.Hand(owner)).Select(i =>
    {
        var acquisition = batch.Movements.Where(m => m.CardId == batch.Movements[i].CardId && m.To == CardLocation.Hand(owner) && ActualHandGainOriginalSource(m, batch) != CardLocation.Hand(owner)).OrderByDescending(m => m.Sequence).First();
        return new ActualHandGainEntity(acquisition.CardId, acquisition.Sequence);
    }).ToArray();
    internal void CaptureForeignTurnActualHandGains(CardMovementBatchContext batch)
    {
        if (!HasActualHandGainCapability || batch.MovementTiming is not { } timing || !ActualHandGainTurnOpen(timing.ActualTurnOwnerSeat))
            return;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
        {
            var gains = ActualHandGainEntities(batch, owner.Seat);
            if (gains.Length == 0)
                continue;
            if (owner.Seat == timing.ActualTurnOwnerSeat)
            {
                foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsGained).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects.Any(e => e.Op == SkillProgramEffectOp.DrawAfterActualOwnHandGain)))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualOwnTurnHandGainsRecordedEvent(new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, batch.TurnNumber, timing.ActualTurnOwnerSeat, batch.Id, gains));
                continue;
            }

            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.AfterTurnEnded).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardForeignTurnHandGains)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ForeignTurnHandGainsRecordedEvent(new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, batch.TurnNumber, timing.ActualTurnOwnerSeat, batch.Id, gains));
        }
    }

    internal bool HasRecordedActualOwnHandGain(ProgramTriggerCandidate c, CardMovementBatchContext batch) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualOwnTurnHandGainsRecordedEvent>().Any(e => e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && e.GameplayHash == c.GameplayHash && e.BatchId == batch.Id && e.ActualTurn == batch.TurnNumber && e.ActualTurnOwner == c.OwnerSeat && e.Gains.SequenceEqual(ActualHandGainEntities(batch, c.OwnerSeat)));
    internal ActualHandGainEntity[] ForeignHandGainRoster(int owner, string skill, string hash, int turn, int actualOwner) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ForeignTurnHandGainsRecordedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.GameplayHash == hash && e.ActualTurn == turn && e.ActualTurnOwner == actualOwner).SelectMany(e => e.Gains).GroupBy(e => e.CardId).Select(g => g.OrderByDescending(e => e.GainSequence).First()).OrderBy(e => e.GainSequence).ToArray();
    internal int[] CurrentForeignHandGainIds(int owner, IReadOnlyList<ActualHandGainEntity> roster) => roster.Where(g => _cardZonesOperations._cardZones.GetLocation(g.CardId) == CardLocation.Hand(owner) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == g.CardId && m.To == CardLocation.Hand(owner) && ActualHandGainOriginalSource(m) != CardLocation.Hand(owner))?.Sequence == g.GainSequence).Select(g => g.CardId).ToArray();
    internal static CardConversionSource ActualHandGainSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ActualHandGainPrograms.ActualHandGainSource(f);
    internal bool IsActualHandGainOwnDrawBatch(CardMovementBatchContext batch, int owner, string skill, string hash)
    {
        if (batch.OriginOwnerSeat != owner || batch.OriginSkillId != skill || batch.OriginSkillInstanceId is not { } instance || batch.ParentFrameId is not { } parentId || batch.AwaitingProgramFrameId != parentId || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.Id == parentId)is not { } producer || producer.ActualHandGain is not { Stage: ActualHandGainProgramStage.MovementChildren, Issued: true, Operation: SkillProgramEffectOp.DrawAfterActualOwnHandGain } paid || producer.PendingMovementContinuation is not { } pending || !IsActualHandGainMovement(producer, ActualHandGainPausedEffect(producer), pending) || paid.Source != new CardConversionSource(skill, producer.TriggerId!, owner, instance) || paid.Source != ActualHandGainSource(producer) || paid.GameplayHash != hash || producer.GameplayHash != hash || paid.InstructionIndex != producer.InstructionIndex || producer.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained, MovementBatch: { } original } context || context.ParentFrameId != paid.OriginalParentId || original.Id != paid.OriginalBatchId || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault(w => w.Id == paid.OriginalParentId)is not { } originalParent || originalParent.Batch.Id != original.Id || !originalParent.Batch.Movements.SequenceEqual(original.Movements) || originalParent.CandidateIndex < 0 || originalParent.CandidateIndex >= originalParent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(producer, originalParent.Candidates[originalParent.CandidateIndex]) || !HasRecordedActualOwnHandGain(originalParent.Candidates[originalParent.CandidateIndex], originalParent.Batch) || !paid.Gains.SequenceEqual(ActualHandGainEntities(originalParent.Batch, owner)))
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualHandGainDrawPaidEvent>().Where(e => e.FrameId == producer.Id).ToArray();
        if (facts is not [var fact] || fact.Source != paid.Source || fact.GameplayHash != hash || fact.OriginalBatchId != original.Id || fact.ActualTurn != paid.ActualTurn || fact.ActualTurnOwner != owner || fact.Before != paid.Before || fact.After != paid.After || fact.ActualCount != paid.PaidIds.Count || fact.ActualCount != 1 || batch.TurnNumber != paid.ActualTurn || batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= paid.Before || m.Sequence > paid.After))
            return false;
        var gains = MatchingActualHandGainIndexes(batch, owner, CardLocation.Hand(owner)).Select(i => batch.Movements[i]).ToArray();
        return gains.Length > 0 && gains.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(owner) && m.Reason.Value == ActualHandGainDrawReason && paid.PaidIds.Contains(m.CardId)) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.Reason.Value == ActualHandGainDrawReason && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(owner)).Select(m => m.CardId).SequenceEqual(paid.PaidIds);
    }

    internal static SkillProgramCardCategory ActualHandGainCategory(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ActualHandGainPrograms.ActualHandGainCategory(kind);
    internal bool SameCategoryGiftUsed(int owner, string skill, int turn, SkillProgramCardCategory category) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SameCategoryDeckGiftIssuedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.ActualTurn == turn && e.Category == category);
    internal bool ExactActualHandGainUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context, out ProgramCardTriggerWindowFrame parent, out CardUseFrame use)
    {
        parent = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)!;
        use = parent is null ? null ! : _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)!;
        var useId = use?.Id;
        var effectiveKind = parent?.Action.EffectiveKind;
        return context.Window == SkillProgramTriggerWindow.CardUseCompleted && parent is not null && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]) == c && parent.Action.Type == CardActionType.Use && parent.Action.ActorSeat == c.OwnerSeat && use is { Step: ResolutionFrameStep.Completed } && use.Action?.ActionId == parent.Action.ActionId && use.SourceSeat == c.OwnerSeat && context.CardUse is { } card && card.ParentCardUseFrameId == use.Id && card.CardActionId == parent.Action.ActionId && card.ActorSeat == c.OwnerSeat && card.EffectiveKind == parent.Action.EffectiveKind && _coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, parent.Action) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId && e.CardKind == effectiveKind) == 1;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 120)]
    internal bool CanRunActualHandGainTrigger(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        var op = t.Effects.FirstOrDefault()?.Op;
        if (op is null || !ActualHandGainAndCategoryGiftComposition.IsOperation(op.Value))
            return true;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
            return false;
        if (op == SkillProgramEffectOp.DrawAfterActualOwnHandGain)
            return _coreGainGiftProgramsOperations.ExactGainGiftMovementParent(c, context, out var moved) && moved.Batch.TurnNumber == _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber && moved.Batch.MovementTiming is { } timing && timing.ActualTurnOwnerSeat == c.OwnerSeat && timing.Phase is not (TurnPhase.NotStarted or TurnPhase.Finished) && HasRecordedActualOwnHandGain(c, moved.Batch) && !IsActualHandGainOwnDrawBatch(moved.Batch, c.OwnerSeat, c.SkillId, c.GameplayHash) && MatchingActualHandGainIndexes(moved.Batch, c.OwnerSeat, CardLocation.Hand(c.OwnerSeat)).Length > 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualHandGainDrawPaidEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.OriginalBatchId == moved.Batch.Id);
        if (op == SkillProgramEffectOp.DiscardForeignTurnHandGains)
            return _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is { } ended && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended) && _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ended) == c && ended.OwnerSeat != c.OwnerSeat && CurrentForeignHandGainIds(c.OwnerSeat, ForeignHandGainRoster(c.OwnerSeat, c.SkillId, c.GameplayHash, ended.TurnNumber, ended.OwnerSeat)).Length > 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ForeignTurnHandGainsCleanupPaidEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.ActualTurn == ended.TurnNumber);
        return ActualHandGainTurnOpen(c.OwnerSeat) && ExactActualHandGainUse(c, context, out var parent, out _) && !SameCategoryGiftUsed(c.OwnerSeat, c.SkillId, _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber, ActualHandGainCategory(parent.Action.EffectiveKind)) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat);
    }

    internal SkillProgramStepOutcome ExecuteActualHandGain(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ActualHandGain is not null)
            throw new InvalidOperationException("An actual hand gain cannot be issued twice.");
        var context = f.WindowContext ?? throw new InvalidOperationException("Actual hand gains require their original native window.");
        ProgramTriggerCandidate c;
        int actualOwner;
        int actualTurn;
        long batchId = 0, useId = 0, actionId = 0;
        ActualHandGainEntity[] gains = [];
        SkillProgramCardCategory? category = null;
        if (effect.Op == SkillProgramEffectOp.DrawAfterActualOwnHandGain)
        {
            var moved = _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(w => w.Id == context.ParentFrameId);
            c = moved.Candidates[moved.CandidateIndex];
            actualOwner = moved.Batch.MovementTiming!.ActualTurnOwnerSeat;
            actualTurn = moved.Batch.TurnNumber;
            batchId = moved.Batch.Id;
            gains = ActualHandGainEntities(moved.Batch, f.OwnerSeat);
        }
        else if (effect.Op == SkillProgramEffectOp.DiscardForeignTurnHandGains)
        {
            var ended = _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().Single(w => w.Id == context.ParentFrameId);
            c = _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(ended);
            actualOwner = ended.OwnerSeat;
            actualTurn = ended.TurnNumber;
            gains = ForeignHandGainRoster(f.OwnerSeat, f.SkillId, f.GameplayHash, actualTurn, actualOwner);
        }
        else
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == context.ParentFrameId);
            c = global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]);
            actualOwner = _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;
            actualTurn = _coreActualTurnProgressPhaseTurnNumberTurnProgressionAccess._turnNumber;
            useId = parent.ParentFrameId;
            actionId = parent.Action.ActionId;
            category = ActualHandGainCategory(parent.Action.EffectiveKind);
        }

        if (!global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) || !CanRunActualHandGainTrigger(c, _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Actual hand gains lost the exact original candidate, source or qualification.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualHandGain = new() { InstructionIndex = f.InstructionIndex, Operation = effect.Op, Source = ActualHandGainSource(f), GameplayHash = f.GameplayHash, OriginalParentId = context.ParentFrameId, ActualTurn = actualTurn, ActualTurnOwner = actualOwner, OriginalBatchId = batchId, OriginalCardUseFrameId = useId, OriginalActionId = actionId, Category = category, Gains = gains, Stage = ActualHandGainProgramStage.Choosing } });
        if (effect.Op == SkillProgramEffectOp.GiveSameCategoryFromDeck)
        {
            PublishActualHandGainChoice(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        PayActualHandGain(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ActualHandGainChoices(ProgramSkillFrame f)
    {
        // Availability and identity of matching draw-pile cards never determine a player's choices.
        var choices = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).OrderBy(p => p.Seat).Select(p => new PromptChoice(new($"same-category-deck.{(f.Id)}.{(p.Seat)}"), $"令 {(p.Name)} 获得牌堆中一张类型相同的牌。", [], [p.Seat], ActualHandGainParameters(f, "give"))).ToList();
        choices.Add(new(new($"same-category-deck.{(f.Id)}.decline"), "不发动应援。", [], [], ActualHandGainParameters(f, "decline")));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal static Dictionary<string, string> ActualHandGainParameters(ProgramSkillFrame f, string branch) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ActualHandGainPrograms.ActualHandGainParameters(f, branch);
    internal void PublishActualHandGainChoice(ProgramSkillFrame f)
    {
        var choices = ActualHandGainChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "应援：可令一名其他角色获得牌堆中一张类型相同的牌。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("actual-hand-gain-category")]
    internal void ResolveActualHandGainChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Category gift has no owning frame.");
        _coreActualHandGainObserversOperations.AssertActualHandGain(f);
        var r = f.ActualHandGain!;
        if (r.Operation != SkillProgramEffectOp.GiveSameCategoryFromDeck || r.Stage != ActualHandGainProgramStage.Choosing || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ActualHandGainChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Category gift choice lost its exact private owner and current living targets.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["branch"] == "decline" || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishActualHandGain(f);
            return;
        }

        if (!ActualHandGainTurnOpen(f.OwnerSeat) || SameCategoryGiftUsed(f.OwnerSeat, f.SkillId, r.ActualTurn, r.Category!.Value))
            throw new InvalidOperationException("Category gift lost its unpaid actual-turn quota.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualHandGain = r with { RecipientSeat = choice.Targets.Single() } });
        PayActualHandGain(f);
    }

    internal void PayActualHandGain(ProgramSkillFrame f)
    {
        var r = f.ActualHandGain!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var ids = r.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains ? CurrentForeignHandGainIds(f.OwnerSeat, r.Gains) : r.Operation == SkillProgramEffectOp.GiveSameCategoryFromDeck ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesSkillProgramCardCategory(c.Kind, r.Category!.Value)).Take(1).Select(c => c.Id).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualHandGain = r = r with { Stage = ActualHandGainProgramStage.MovementChildren, PaidIds = ids, Before = before, After = before, Issued = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (r.Operation == SkillProgramEffectOp.DrawAfterActualOwnHandGain)
        {
            var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(ActualHandGainDrawReason));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualHandGain = r = f.ActualHandGain!with { PaidIds = drawn.ToArray(), After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualHandGainDrawPaidEvent(f.Id, r.Source, r.GameplayHash, r.OriginalBatchId, r.ActualTurn, r.ActualTurnOwner, before, r.After, drawn.Count));
        }
        else
        {
            void Paid()
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var paid = current.ActualHandGain!with
                {
                    After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { ActualHandGain = paid });
                if (paid.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ForeignTurnHandGainsCleanupPaidEvent(f.Id, paid.Source, paid.GameplayHash, paid.ActualTurn, paid.ActualTurnOwner, paid.PaidIds, before, paid.After));
                else
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SameCategoryDeckGiftIssuedEvent(f.Id, paid.Source, paid.GameplayHash, paid.ActualTurn, paid.ActualTurnOwner, paid.OriginalCardUseFrameId, paid.OriginalActionId, paid.Category!.Value, paid.RecipientSeat, paid.PaidIds.Count, before, paid.After));
            }

            var logicalDiscard = r.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, ids, new(ForeignHandCleanupReason), f.Id) : null;
            if (ids.Length == 0)
                Paid();
            else
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, r.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains ? CardLocation.DiscardPile : CardLocation.Hand(r.RecipientSeat), new(r.Operation == SkillProgramEffectOp.DiscardForeignTurnHandGains ? ForeignHandCleanupReason : SameCategoryDeckGiftReason), (r.Operation == SkillProgramEffectOp.GiveSameCategoryFromDeck ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, r.RecipientSeat, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, _) => Paid()) : (_, _) => Paid()));
            _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        }

        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            ReturnActualHandGainMovement(f);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 320)]
    internal bool ResumeActualHandGain(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ActualHandGain: not null } f || f.Id != id)
            return false;
        _coreActualHandGainObserversOperations.AssertActualHandGain(f);
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd())
            return true;
        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnActualHandGainMovement(f);
            return true;
        }

        if (f.ActualHandGain.Stage != ActualHandGainProgramStage.Choosing)
            throw new InvalidOperationException("An issued actual gain lost its typed return.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishActualHandGain(f);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishActualHandGainChoice(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 310)]
    internal bool ReturnActualHandGainMovement(ProgramSkillFrame f)
    {
        if (f.ActualHandGain is null)
            return false;
        if (f.PendingMovementContinuation is not { } p || !IsActualHandGainMovement(f, ActualHandGainPausedEffect(f), p))
            throw new InvalidOperationException("Actual gains cannot consume another instruction's return.");
        _coreActualHandGainObserversOperations.AssertActualHandGain(f);
        if (_coreGainGiftProgramsOperations.GainGiftHasFinalGameEnd() || _coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        FinishActualHandGain(f);
        return true;
    }

    internal void FinishActualHandGain(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualHandGain = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal SkillProgramEffect? ActualHandGainPausedEffect(ProgramSkillFrame f) => f.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 250)]
    internal bool IsActualHandGainMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ActualHandGain is { Stage: ActualHandGainProgramStage.MovementChildren, Issued: true } r && r.InstructionIndex == f.InstructionIndex && e?.Op == r.Operation && e == ActualHandGainPausedEffect(f) && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramAiChoice(SkillProgramEffectOp.GiveSameCategoryFromDeck)]
    internal PromptChoice SelectAiActualHandGain(PendingDecision decision, ProgramSkillFrame f)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
        var hint = new SkillProgramAiHint(0, 0, 0, 1, 0, 0, true, false);
        return decision.Choices.Where(c => c.Targets.Count == 1).Select(c => (Choice: c, Score: _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, c.Targets[0], hint))).Where(x => x.Score > 0).OrderByDescending(x => x.Score).ThenBy(x => x.Choice.Targets[0]).Select(x => x.Choice).FirstOrDefault() ?? decision.Choices.Single(c => c.Targets.Count == 0);
    }

    internal CardUseFrame NormalizeCompletedCategoryVirtualUse(CardUseFrame use, long? programParentFrameId)
    {
        if (use.Action is not null || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GiveSameCategoryFromDeck))
            return use;
        if (use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || programParentFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.Id == parentId)is not { } parent || parent.InstructionIndex < 1 || ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect.Op != SkillProgramEffectOp.UseVirtualCard || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CompletedCategoryVirtualUseNormalizedEvent>().Any(e => e.CardUseFrameId == use.Id))
            throw new InvalidOperationException("Legacy virtual completion lost its exact Action-null zero-entity producer.");
        var producerSource = new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId);
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Id != use.Id)?.Action?.ActionId, CardActionType.Use, use.SourceSeat, use.SourceSeat, null, null, null, use.CardKind, use.TargetSeats, [], [producerSource], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use = use with { Action = action });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedCategoryVirtualUseNormalizedEvent(use.Id, use.SourceSeat, use.CardKind, parent.Id, producerSource, parent.GameplayHash, parent.InstructionIndex, action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        return use;
    }

    internal bool IsNormalizedLegacyCompletedCategoryUse(CardUseFrame use, int actor, CardKind kind, IReadOnlyList<int> completedTargets)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CompletedCategoryVirtualUseNormalizedEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        if (facts is not [var fact] || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.SourceSeat != actor || use.CardKind != kind || fact.ActorSeat != actor || fact.EffectiveKind != kind || use.Action is not { Type: CardActionType.Use } action || action.ActionId != fact.Action.ActionId || action.ActorSeat != actor || action.ProviderSeat != actor || action.EffectiveKind != kind || action.PhysicalCards.Count != 0 || action.ConversionChain is not [var source] || source != fact.ProducerSource || !action.TargetSeats.SequenceEqual(use.TargetSeats) || !fact.Action.TargetSeats.SequenceEqual(use.TargetSeats) || completedTargets.Count == 0 || !use.TargetSeats.SequenceEqual(completedTargets) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.Id == fact.ProgramParentFrameId)is not { } parent || parent.GameplayHash != fact.ProducerGameplayHash || parent.InstructionIndex != fact.ProducerInstructionIndex || ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!).GetPausedInstruction(parent.InstructionIndex).Effect.Op != SkillProgramEffectOp.UseVirtualCard || source != new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.Type == CardActionType.Use && e.Action.ActorSeat == actor && e.Action.ProviderSeat == actor && e.Action.EffectiveKind == kind && e.Action.PhysicalCards.Count == 0 && e.Action.TargetSeats.SequenceEqual(use.TargetSeats));
    }

    SkillProgramStepOutcome global::CardGame.Core.IActualHandGainAndCategoryGiftHost.ExecuteActualHandGain(ProgramSkillFrame f, SkillProgramEffect e) => ExecuteActualHandGain(f, e);
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyMovementIndexes, 500)]
    internal int[]? SelectWindowIndexes_MatchingActualHandGainIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardLocation location)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.DrawAfterActualOwnHandGain))
            return MatchingActualHandGainIndexes(batch, candidate.OwnerSeat, location);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 800)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureForeignTurnActualHandGains(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureForeignTurnActualHandGains(completed);
        return completed;
    }
}
