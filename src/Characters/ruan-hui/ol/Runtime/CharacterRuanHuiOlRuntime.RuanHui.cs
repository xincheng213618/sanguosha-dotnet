#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanHuiOlRuntime
{
    internal const string MingchaGain = "skill-program.ruan-hui.mingcha.gain", MingchaCleanup = "skill-program.ruan-hui.mingcha.cleanup", MingchaRandom = "skill-program.ruan-hui.mingcha.random", JingzhongGain = "skill-program.ruan-hui.jingzhong.gain", MingchaBind = "mingcha-public-three";
    SkillProgramStepOutcome global::CardGame.Core.IRuanHuiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginRuanHui(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27080)]
    internal bool QualifyRuanHui(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [var e] || !RuanHuiComposition.Handles(e.Op))
            return true;
        if (e.Op == SkillProgramEffectOp.ObtainJingzhongUsedEntities)
            return CanRunIssuedRuanHuiCandidate(c, context);
        if (e.Op == SkillProgramEffectOp.ResolveMingchaRevealDrawChoice)
            return context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && context.OwnerSeat == c.OwnerSeat;
        var phase = _coreProgramEventHistoryOperations.ProgramEventHistory<ActualDiscardRecoveryPhaseStartedEvent>().Select(e => e.Phase).LastOrDefault(p => p.Kind == ActualDiscardRecoveryPhaseKind.Discard && p.ActorSeat == c.OwnerSeat && p.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
        return context.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && phase is not null && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat) && _coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongDiscardEvent>().Count(e => e.PhaseToken == phase.Token && e.OwnerSeat == c.OwnerSeat) >= 2 && !_coreProgramEventHistoryOperations.ProgramEventHistory<JingzhongLeaseEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.AfterPhaseToken == phase.Token);
    }

    internal SkillProgramStepOutcome BeginRuanHui(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var context = f.WindowContext;
        if (f.RuanHui is not null || f.InstructionIndex != 1 || context is null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !QualifyRuanHui(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Ruan Hui requires her exact unissued native boundary.");
        var r = new RuanHuiReceipt(RuanHuiSource(f), f.GameplayHash, op, f.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber, op == SkillProgramEffectOp.ResolveMingchaRevealDrawChoice ? RuanHuiStage.RevealChildren : RuanHuiStage.TargetChoice, [], []);
        if (op == SkillProgramEffectOp.ObtainJingzhongUsedEntities)
        {
            var facts = RuanHuiClaimFacts(context.MovementBatch!, new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex));
            var first = facts.First(e => JingzhongUsedCount(e.LeaseId) < 3 && _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == e.CardId).Sequence == e.MovementSequence);
            var entities = facts.Where(e => e.LeaseId == first.LeaseId && e.ActionId == first.ActionId && _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == e.CardId).Sequence == e.MovementSequence).Select(e => new RuanHuiEntity(e.CardId, _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Kind, _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Rank, CardLocation.DiscardPile, e.MovementSequence)).ToArray();
            r = r with
            {
                LeaseId = first.LeaseId,
                ActionId = first.ActionId,
                Stage = RuanHuiStage.ClaimChildren,
                Entities = entities
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JingzhongClaimIssuedEvent(f.Id, first.LeaseId, first.PhaseToken, first.ActionId, context.MovementBatch!.Id, entities.Length));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanHui = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanHuiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.InstructionIndex, r.ParentId, r.ActualTurn, r.LeaseId, r.ActionId));
        if (op == SkillProgramEffectOp.ResolveMingchaRevealDrawChoice)
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, 3, null, MingchaBind, SkillProgramCardSetVisibility.Public);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var entities = _coreProgramLifecycleOperations.GetProgramCardSet(f, MingchaBind).CardIds.Select(id => new RuanHuiEntity(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank, CardLocation.Processing, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == id).Sequence)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { RuanHui = f.RuanHui!with { Entities = entities } });
            for (var i = 0; i < entities.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanHuiRevealedEntityEvent(f.Id, i, entities[i]));
            AddRuanHuiStep(f.Id, RuanHuiStage.RevealChildren, before, entities.Length);
            ContinueRuanHui(f.Id);
        }
        else if (op == SkillProgramEffectOp.ObtainJingzhongUsedEntities)
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            foreach (var entity in r.Entities)
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(entity.CardId), CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new(JingzhongGain));
            AddRuanHuiStep(f.Id, RuanHuiStage.ClaimChildren, before, r.Entities.Count);
            ContinueRuanHui(f.Id);
        }
        else
            PublishRuanHui(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> RuanHuiChoices(ProgramSkillFrame f)
    {
        var r = f.RuanHui!;
        var result = new List<PromptChoice>();
        void Add(string branch, string label, int? seat = null) => result.Add(new(new($"ruan-hui.{(f.Id)}.{(branch)}"), label, [], seat is { } s ? [s] : [], new Dictionary<string, string> { { "program-action", "ruan-hui" }, { "branch", branch } }));
        if (r.Stage == RuanHuiStage.DrawChoice)
        {
            Add("replace", "改为获得亮出牌中点数不大于8的牌");
            Add("normal", "仍进行正常摸牌");
        }
        else if (r.Stage == RuanHuiStage.TargetChoice)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && (r.Op == SkillProgramEffectOp.ArmJingzhongNextActualPlay || MingchaRandomCards(p.Seat).Count > 0)))
                Add("target." + p.Seat, p.Name, p.Seat);
            if (r.Op == SkillProgramEffectOp.ResolveMingchaRevealDrawChoice)
                Add("skip", "不获得其他角色的牌");
        }
        else
            throw new InvalidOperationException("Ruan Hui cannot offer a paid child stage as a new choice.");
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishRuanHui(ProgramSkillFrame f)
    {
        AssertRuanHui(f);
        var choices = RuanHuiChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
        {
            FinishRuanHui(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.RuanHui!.Stage == RuanHuiStage.DrawChoice ? "明察：已公开亮出三张牌，选择是否替代正常摸牌。" : "选择另一名角色。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("ruan-hui")]
    internal void AnswerRuanHui(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RuanHui: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !RuanHuiChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Ruan Hui requires her actual published native choice.");
        AssertRuanHui(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == RuanHuiStage.DrawChoice)
        {
            var replace = branch == "replace";
            if (replace)
            {
                if (_resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == r.ParentId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DrawPhaseStarting } parent || parent.OwnerSeat != f.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
                    throw new InvalidOperationException("Mingcha lost its exact normal-draw plan parent.");
                _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { NormalDrawReplaced = true });
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanHui = r with { ReplaceDraw = replace, Stage = RuanHuiStage.SettleChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), CardSetBindings = Array.AsReadOnly(f.CardSetBindings.Where(b => b.Name != MingchaBind).ToArray()) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanHuiDecisionEvent(f.Id, replace));
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var count = 0;
            foreach (var entity in r.Entities.Where(e => _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.Processing && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == e.CardId).Sequence == e.Sequence))
            {
                var gain = replace && entity.Rank <= 8;
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(entity.CardId), CardLocation.Processing, gain ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile, new(gain ? MingchaGain : MingchaCleanup));
                count++;
            }

            AddRuanHuiStep(f.Id, RuanHuiStage.SettleChildren, before, count);
            ContinueRuanHui(f.Id);
            return;
        }

        if (branch == "skip")
        {
            FinishRuanHui(f);
            return;
        }

        var target = choice.Targets.Single();
        if (r.Op == SkillProgramEffectOp.ArmJingzhongNextActualPlay)
        {
            var phase = _coreProgramEventHistoryOperations.ProgramEventHistory<ActualDiscardRecoveryPhaseStartedEvent>().Select(e => e.Phase).Last(p => p.ActorSeat == f.OwnerSeat && p.Kind == ActualDiscardRecoveryPhaseKind.Discard && p.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JingzhongLeaseEvent(f.Id, r.Source, r.GameplayHash, target, phase.Token, _coreActualTurnProgressTurnNumberAccess._turnNumber));
            FinishRuanHui(f);
            return;
        }

        var cards = MingchaRandomCards(target);
        var card = cards[_randomOperations._random.Next(cards.Count)];
        var entityTaken = new RuanHuiEntity(card.Id, card.Kind, card.Rank, _cardZonesOperations._cardZones.GetLocation(card.Id));
        var first = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanHui = r with { Stage = RuanHuiStage.RandomChildren, TargetSeat = target, RandomEntity = entityTaken }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanHuiRandomEntityEvent(f.Id, target, entityTaken));
        _corePhysicalMovementFacadesOperations.MoveCard(card, entityTaken.From, CardLocation.Hand(f.OwnerSeat), new(MingchaRandom));
        AddRuanHuiStep(f.Id, RuanHuiStage.RandomChildren, first, 1);
        ContinueRuanHui(f.Id);
    }

    internal void AddRuanHuiStep(long id, RuanHuiStage stage, long before, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.RuanHui!;
        var step = new RuanHuiStep(r.Steps.Count, stage, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { RuanHui = r with { Steps = r.Steps.Append(step).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanHuiStepEvent(id, step));
    }

    internal bool DrainRuanHui(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueRuanHui(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { RuanHui: { } r } f && f.Id == id)
        {
            AssertRuanHui(f);
            if (r.Stage is RuanHuiStage.DrawChoice or RuanHuiStage.TargetChoice)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishRuanHui(f);
                return;
            }

            if (DrainRuanHui(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            r = f.RuanHui!;
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                CleanupRuanHuiReveal(f);
                FinishRuanHui(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            }

            if (r.Stage == RuanHuiStage.RevealChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanHui = r with { Stage = RuanHuiStage.DrawChoice } });
                PublishRuanHui(f);
                return;
            }

            if (r.Stage == RuanHuiStage.SettleChildren && r.ReplaceDraw == true)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanHui = r with { Stage = RuanHuiStage.TargetChoice } });
                PublishRuanHui(f);
                return;
            }

            FinishRuanHui(f);
            return;
        }
    }

    internal void CleanupRuanHuiReveal(ProgramSkillFrame f)
    {
        foreach (var e in f.RuanHui!.Entities.Where(e => e.From == CardLocation.Processing && _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.Processing))
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId), CardLocation.Processing, CardLocation.DiscardPile, new(MingchaCleanup));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CardSetBindings = Array.AsReadOnly(f.CardSetBindings.Where(b => b.Name != MingchaBind).ToArray()) });
    }

    internal void FinishRuanHui(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanHuiCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanHui = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27080)]
    internal bool ResumeRuanHui(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RuanHui: not null } f || f.Id != id)
            return false;
        ContinueRuanHui(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27080)]
    internal bool ReturnRuanHuiMovement(ProgramSkillFrame f)
    {
        if (f.RuanHui is null)
            return false;
        ContinueRuanHui(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMingchaRevealDrawChoice, SkillProgramEffectOp.ArmJingzhongNextActualPlay, SkillProgramEffectOp.ObtainJingzhongUsedEntities)]
    internal PromptChoice AiRuanHui(PendingDecision p, ProgramSkillFrame f) => f.RuanHui!.Stage == RuanHuiStage.DrawChoice ? p.Choices.Single(c => c.Parameters["branch"] == (f.RuanHui.Entities.Count(e => e.Rank <= 8) >= 2 ? "replace" : "normal")) : p.Choices.OrderBy(c => c.Targets.Count == 0 ? 1 : 0).ThenBy(c => c.Targets is [var s] ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count : 0).First();
}
