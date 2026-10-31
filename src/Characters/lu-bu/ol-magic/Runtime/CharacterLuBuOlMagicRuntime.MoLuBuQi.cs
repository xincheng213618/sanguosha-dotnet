#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    internal bool HasMoLuBu => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeMoLuBuQiAndOpeningPlay);

    internal static CardConversionSource MoLuBuSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lu_bu_ol_magic_MoLuBuQi.MoLuBuSource(f);
    // Registration only; the query still belongs to legacy GameEngine.MoLuBuQi.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 60)]
    internal void ProjectMoLuBuQiCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasMoLuBuQi(cardId) ? "炁" : null);
    }

    internal bool HasMoLuBuQi(int id) => HasMoLuBu && id > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuQiMarkedEvent>().Any(e => e.CardId == id);
    internal ProgramLifecycleTriggerWindowFrame CaptureMoLuBuInitialQi(ProgramLifecycleTriggerWindowFrame f)
    {
        if (!HasMoLuBu || f.Window != SkillProgramTriggerWindow.GameStarting || f.Continuation != ProgramLifecycleContinuation.CompleteGameStarting)
            return f;
        return f with
        {
            MoLuBuInitialQi = _coreMatchParticipantsPlayersAccess._players.SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Select(card => new MoLuBuInitialQi(p.Seat, card.Id, card.Kind, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == card.Id && m.To == CardLocation.Hand(p.Seat)).Sequence))).ToArray()
        };
    }

    internal bool MoLuBuQiZone(CardLocation location) => location.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment or CardZoneKind.DrawPile or CardZoneKind.DiscardPile;
    internal IReadOnlyList<MoLuBuQiCandidate> MoLuBuQiCandidates(int original) => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuQiMarkedEvent>().Where(e => e.OriginalOwnerSeat == original).DistinctBy(e => e.CardId).Select(e => new MoLuBuQiCandidate(e.CardId, _cardZonesOperations._cardZones.GetLocation(e.CardId))).Where(e => MoLuBuQiZone(e.From)).ToArray());
    internal bool MoLuBuLastUseWasQi(int actor) => _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).LastOrDefault(e => e is MoLuBuActualUseQiEvent used && used.ActorSeat == actor || e is MoLuBuLegacyDuelUseEvent duel && duel.ActorSeat == actor)is MoLuBuActualUseQiEvent { IsQi: true };
    internal void CaptureMoLuBuLegacyDuel(ProgramSkillFrame frame, int actor)
    {
        if (!HasMoLuBu)
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuLegacyDuelUseEvent>().Any(e => e.ProducerFrameId == frame.Id && e.InstructionIndex == frame.InstructionIndex))
            throw new InvalidOperationException("A native direct Duel cannot update the Gangquan use prefix twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuLegacyDuelUseEvent(frame.Id, frame.InstructionIndex, actor));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 30800)]
    internal void ObserveMoLuBuUse(IGameEvent fact)
    {
        if (!HasMoLuBu)
            return;
        var action = fact is CardUseDeclaredEvent declaration ? _coreCardUseLifecycleOperations.LifecycleCardUse(declaration.ResolutionId)?.Action : fact is CardActionAcceptedEvent accepted ? accepted.Action : null;
        if (action is null || !IsMoLuBuTrueUse(action) || _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuActualUseQiEvent>().Any(e => e.ActionId == action.ActionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuActualUseQiEvent(action.ActionId, action.ActorSeat, action.PhysicalCards.Any(c => HasMoLuBuQi(c.CardId))));
    }

    internal bool IsMoLuBuTrueUse(CardActionContext action) => action.Type == CardActionType.Use || action.EffectiveKind == CardKind.Nullification && _coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.NullificationResponse) || _coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.Dodge) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ParentActionId)is { CardAttack: { EffectiveCardKind: { } incoming } attack } && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(incoming) && attack.TargetSeat == action.ActorSeat;
    internal SkillProgramStepOutcome InitializeMoLuBu(ProgramSkillFrame f)
    {
        if (f.SkillId != MoLuBuComposition.Duoqi || f.TriggerId != "initial-qi-opening-play" || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } c || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { MoLuBuInitialQi: { } initial } || _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuInitializedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Duoqi needs its one actual opening-Hand snapshot.");
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuQiMarkedEvent>().Any())
            foreach (var card in initial)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuQiMarkedEvent(f.Id, card.OriginalOwnerSeat, card.CardId, card.PrintedKind, card.EntrySequence));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuInitializedEvent(f.Id, MoLuBuSource(f), f.GameplayHash, c.ParentFrameId));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30800, SkillProgramEffectOp.ResolveMoLuBuDuoqi)]
    internal bool CanOfferMoDuoqi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => c.Window == SkillProgramTriggerWindow.AfterDamageApplied && c.SourceSeat == candidate.OwnerSeat && c.TargetSeat is { } victim && _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Count(e => !e.SourceLess && e.SourceSeat == candidate.OwnerSeat && e.TargetSeat == victim) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuStartedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Operation == SkillProgramEffectOp.ResolveMoLuBuDuoqi && e.OriginalOwnerSeat == victim && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber);
    internal SkillProgramStepOutcome BeginMoDuoqi(ProgramSkillFrame f)
    {
        var c = f.WindowContext ?? throw new InvalidOperationException("Duoqi needs native damage.");
        var r = new MoLuBuReceipt(MoLuBuSource(f), f.GameplayHash, SkillProgramEffectOp.ResolveMoLuBuDuoqi, c.ParentFrameId, c.TargetSeat!.Value, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, MoLuBuStage.QiChoice)
        {
            Candidates = MoLuBuQiCandidates(c.TargetSeat.Value)
        };
        StartMoLuBu(f, r);
        PublishMoLuBu(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void StartMoLuBu(ProgramSkillFrame f, MoLuBuReceipt r)
    {
        if (f.MoLuBu is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Lu Bu cannot issue its obligation twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MoLuBu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.WindowId, r.OriginalOwnerSeat, r.ActualTurn, r.Reassign));
    }

    internal IReadOnlyList<PromptChoice> MoLuBuChoices(ProgramSkillFrame f)
    {
        var r = f.MoLuBu!;
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Params(string verb, int index) => new()
        {
            ["program-action"] = "mo-lu-bu",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["verb"] = verb,
            ["index"] = index.ToString(CultureInfo.InvariantCulture)
        };
        if (r.Stage == MoLuBuStage.QiChoice)
            for (var n = 0; n < r.Candidates.Count; n++)
            {
                var item = r.Candidates[n];
                if (_cardZonesOperations._cardZones.GetLocation(item.CardId) != item.From || !MoLuBuQiZone(item.From) || item.From == CardLocation.Hand(f.OwnerSeat))
                    continue;
                var hidden = item.From.Zone == CardZoneKind.DrawPile || item.From.Zone == CardZoneKind.Hand && item.From.OwnerSeat != f.OwnerSeat;
                var card = _cardZonesOperations._cardZones.CardsAt(item.From).Single(c => c.Id == item.CardId);
                choices.Add(new(new($"mo-lu-bu.{(f.Id)}.qi.{(n)}"), hidden ? $"取回第{(n + 1)}张初始炁" : $"取回【{(card.DisplayName)}】", hidden ? [] : [card.Id], [], Params("qi", n)));
            }

        if (r.Stage == MoLuBuStage.PairChoice)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                choices.Add(new(new($"mo-lu-bu.{(f.Id)}.pair.{(p.Seat)}"), $"令{(p.Name)}成为狂角色", [], [p.Seat], Params("pair", p.Seat)));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMoLuBu(ProgramSkillFrame f)
    {
        var choices = MoLuBuChoices(f);
        if (choices.Count == 0)
        {
            CompleteMoLuBu(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：请选择本次真实效果。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(skill.Id, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mo-lu-bu")]
    internal void AnswerMoLuBu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoLuBu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MoLuBuChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Lu Bu lost its current canonical private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MoLuBuStage.QiChoice)
        {
            var index = int.Parse(choice.Parameters["index"], CultureInfo.InvariantCulture);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoLuBu = r with { Candidates = [r.Candidates[index]], Index = 0, Stage = MoLuBuStage.Claiming } });
            ContinueMoLuBu(f.Id);
            return;
        }

        if (r.Stage != MoLuBuStage.PairChoice)
            throw new InvalidOperationException("Unknown Lu Bu choice stage.");
        SetMoLuBuPair(f, choice.Targets.Single(), false);
        CompleteMoLuBu(f);
    }

    internal void MoveMoLuBuQi(ProgramSkillFrame f, MoLuBuQiCandidate item)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var r = f.MoLuBu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoLuBu = r with { Stage = MoLuBuStage.MovementChildren, Before = before, After = before, Index = r.Index + 1 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var card = _cardZonesOperations._cardZones.CardsAt(item.From).Single(c => c.Id == item.CardId);
        _corePhysicalMovementFacadesOperations.MoveCard(card, item.From, CardLocation.Hand(f.OwnerSeat), new("skill-program.ol-mo-lu-bu.qi"), moved => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuQiMovedEvent(f.Id, moved.CardId, moved.From, moved.To, moved.Sequence)), disclosure: item.From.Zone == CardZoneKind.DrawPile || item.From.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoLuBu = f.MoLuBu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuMovementSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
    }

    internal bool DrainMoLuBu(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMoLuBu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoLuBu: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return;
        if (r.Stage == MoLuBuStage.EntryChildren)
        {
            if (DrainMoLuBu(f))
                return;
            CompleteMoLuBu(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return;
        }

        while (true)
        {
            if (f.MoLuBu!.Stage == MoLuBuStage.MovementChildren)
            {
                if (DrainMoLuBu(f))
                    return;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null, MoLuBu = _coreProgramLifecycleOperations.GetActiveProgramFrame(id).MoLuBu!with { Stage = MoLuBuStage.Claiming } });
            }

            r = f.MoLuBu!;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CompleteMoLuBu(f);
                return;
            }

            if (r.Stage != MoLuBuStage.Claiming)
            {
                PublishMoLuBu(f);
                return;
            }

            if (r.Index < r.Candidates.Count)
            {
                var item = r.Candidates[r.Index];
                var current = _cardZonesOperations._cardZones.GetLocation(item.CardId);
                if (!MoLuBuQiZone(current) || current == CardLocation.Hand(f.OwnerSeat))
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoLuBu = r with { Index = r.Index + 1 } });
                    continue;
                }

                MoveMoLuBuQi(f, item with { From = current });
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                continue;
            }

            if (r.Reassign)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoLuBu = r with { Stage = MoLuBuStage.PairChoice } });
                PublishMoLuBu(f);
                return;
            }

            CompleteMoLuBu(f);
            return;
        }
    }

    internal void CompleteMoLuBu(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoLuBu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 30800)]
    internal bool ResumeMoLuBu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoLuBu: not null } f || f.Id != id)
            return false;
        ContinueMoLuBu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30800)]
    internal bool ReturnMoLuBuMovement(ProgramSkillFrame f) => f.MoLuBu is not null && ResumeMoLuBu(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMoLuBuDuoqi, SkillProgramEffectOp.ResolveMoLuBuKuangmoDeath)]
    internal PromptChoice ChooseMoLuBuAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Targets is [var target] && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target])) ?? p.Choices[0];
    SkillProgramStepOutcome global::CardGame.Core.IMoLuBuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.InitializeMoLuBuQiAndOpeningPlay => InitializeMoLuBu(f),
        SkillProgramEffectOp.ResolveMoLuBuDuoqi => BeginMoDuoqi(f),
        SkillProgramEffectOp.EnterMoLuBuKuangmo => BeginMoKuangmo(f),
        _ => BeginMoKuangmoDeath(f)};
}
