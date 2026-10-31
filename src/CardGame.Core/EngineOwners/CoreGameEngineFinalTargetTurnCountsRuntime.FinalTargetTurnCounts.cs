#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFinalTargetTurnCountsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IFinalTargetTurnCountHost.ExecuteFinalTargetTurnCount(SkillProgramEffect effect, ProgramSkillFrame frame) => ExecuteFinalTargetTurnCount(effect, frame);
    internal int GetNamedTurnSkillCount(int seat, string skill) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NamedTurnSkillChoiceCommittedEvent>().Count(e => e.OwnerSeat == seat && e.SkillId == skill && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
    internal bool NamedTurnActionAlreadyCommitted(int seat, string skill, long actionId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NamedTurnSkillChoiceCommittedEvent>().Any(e => e.OwnerSeat == seat && e.SkillId == skill && e.ActionId == actionId);
    internal CardUseFrame NonFinalTargetUse(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } use } || use.ActorSeat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != frame.OwnerSeat || _coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.EffectiveKind) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == use.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } parent || action.ActionId != use.CardActionId || action.ActorSeat != frame.OwnerSeat || action.EffectiveKind != use.EffectiveKind)
            throw new InvalidOperationException("Non-final-target payment lost its actual own-turn non-equipment use.");
        return parent;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AfterInitialPayments, 40, SkillProgramEffectOp.DiscardNonFinalTargetCardThenDraw)]
    internal bool CanOfferNonFinalTargetPayment(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.CardUse is { ActorSeat: var actor, EffectiveKind: var kind } use && actor == candidate.OwnerSeat && actor == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind) && !NamedTurnActionAlreadyCommitted(actor, candidate.SkillId, use.CardActionId) && _coreProgramLifecycleOperations.GetProgramTargetSeats(actor, SkillProgramTargetKind.CurrentCardUseTargets, context)is { } targets && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && !targets.Contains(p.Seat) && _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(actor, p.Seat));
    internal IReadOnlyList<PromptChoice> NonFinalTargetChoices(ProgramSkillFrame frame)
    {
        var targets = NonFinalTargetUse(frame).TargetSeats;
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !targets.Contains(p.Seat)).SelectMany(p => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, p.Seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(choice => choice with { Id = new($"outside-target.frame-{(frame.Id)}.owner-{(p.Seat)}.{(choice.Id.Value)}"), Description = p.Name + "：" + choice.Description, Targets = [p.Seat], Parameters = new Dictionary<string, string>(choice.Parameters) { ["program-action"] = "named-turn-flow" } })).ToArray();
    }

    internal SkillProgramStepOutcome ExecuteFinalTargetTurnCount(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        if (frame.NamedTurnCountFlow is not null)
            throw new InvalidOperationException("Turn-count flow cannot restart a paid cursor.");
        if (effect.Op == SkillProgramEffectOp.DiscardNonFinalTargetCardThenDraw)
        {
            if (NamedTurnActionAlreadyCommitted(frame.OwnerSeat, frame.SkillId, NonFinalTargetUse(frame).Action!.ActionId))
                return SkillProgramStepOutcome.Continue;
            var choices = NonFinalTargetChoices(frame);
            if (choices.Count == 0)
                return SkillProgramStepOutcome.Continue;
            PublishNamedTurnFlow(frame, choices, "弃置本次用牌全部最终目标之外一名角色的一张手牌或装备牌，然后令其摸一张牌。", true);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select(c => c.Id).ToArray();
        var required = Math.Max(0, cards.Length - GetNamedTurnSkillCount(frame.OwnerSeat, effect.SkillIds.Single()));
        if (required == 0)
            return SkillProgramStepOutcome.Continue;
        frame = frame with
        {
            NamedTurnCountFlow = new("hand", frame.OwnerSeat, required, cards, [])
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        PublishNamedTurnHandDiscard(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishNamedTurnFlow(ProgramSkillFrame frame, IReadOnlyList<PromptChoice> choices, string message, bool privateChoice)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, message, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = privateChoice,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void PublishNamedTurnHandDiscard(ProgramSkillFrame frame)
    {
        var draft = frame.NamedTurnCountFlow!;
        var choices = draft.CardIds.Except(draft.SelectedIds).Select(id =>
        {
            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Single(c => c.Id == id);
            return new PromptChoice(new($"named-turn-hand.frame-{(frame.Id)}.card-{(id)}"), "弃置【" + global::CardGame.Core.EngineOwners.CoreGameEnginePublicPersistentPilesRuntime.PublicPileCardLabel(card) + "】", [id], [], new Dictionary<string, string> { ["program-action"] = "named-turn-flow", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        }).ToArray();
        PublishNamedTurnFlow(frame, choices, $"请选择 {(draft.Required - draft.SelectedIds.Count)} 张手牌弃置。", true);
    }

    [ProgramAnswer("named-turn-flow")]
    internal void ResolveNamedTurnFlowChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Turn-count choice lost its frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { PlayerSeat: var chooser } || chooser != frame.OwnerSeat || !_corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices.Any(c => c.Id == selected.Id))
            throw new InvalidOperationException("Turn-count choice lost its published cursor.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "技能来源已失效。");
            return;
        }

        if (effect.Op == SkillProgramEffectOp.DiscardNonFinalTargetCardThenDraw)
        {
            var use = NonFinalTargetUse(frame);
            if (NamedTurnActionAlreadyCommitted(frame.OwnerSeat, frame.SkillId, use.Action!.ActionId))
                throw new InvalidOperationException("Turn action was already committed.");
            var valid = NonFinalTargetChoices(frame).SingleOrDefault(c => c.Id == selected.Id) ?? throw new InvalidOperationException("Non-target payment is no longer valid.");
            var seat = valid.Targets.Single();
            var from = new CardLocation(Enum.Parse<CardZoneKind>(valid.Parameters["source-zone"]), seat);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(valid.Parameters["slot-index"], System.Globalization.CultureInfo.InvariantCulture)];
            if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, from, OwnedCardMoveIntent.Discard))
                throw new InvalidOperationException("Non-target payment is no longer discardable.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            frame = frame with
            {
                NamedTurnCountFlow = new("target-movement", seat, 1, [], [], use.Action.ActionId),
                PendingMovementContinuation = new(frame.OwnerSeat, 0, null)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NamedTurnSkillChoiceCommittedEvent(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.TriggerId!, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, use.Action.ActionId, frame.WindowContext!.ParentFrameId, seat));
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, frame.OwnerSeat, card, from, new("skill-program.non-final-target.discard"), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, frame.OwnerSeat, frame.Id));
            if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
            return;
        }

        if (effect.Op != SkillProgramEffectOp.DiscardHandToNamedTurnCount || frame.NamedTurnCountFlow is not { Stage: "hand" } draft || selected.Cards is not [var id] || selected.Targets.Count != 0 || !draft.CardIds.Except(draft.SelectedIds).Contains(id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat))
            throw new InvalidOperationException("Named-count discard lost actual hand payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        draft = draft with
        {
            SelectedIds = draft.SelectedIds.Append(id).ToArray()
        };
        frame = frame with
        {
            NamedTurnCountFlow = draft
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        if (draft.SelectedIds.Count < draft.Required)
        {
            PublishNamedTurnHandDiscard(frame);
            return;
        }

        if (draft.SelectedIds.Any(cardId => _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(frame.OwnerSeat)))
            throw new InvalidOperationException("Named-count hand cost moved before commit.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NamedTurnCountFlow = draft with { Stage = "hand-movement" }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(frame.OwnerSeat, frame.OwnerSeat, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Where(c => draft.SelectedIds.Contains(c.Id)).ToArray(), CardLocation.Hand(frame.OwnerSeat), new("skill-program.named-turn-count.discard"));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 900)]
    internal bool ResumeNamedTurnFlow(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var draft = frame.NamedTurnCountFlow;
        if (draft is null)
            return false;
        if (draft.Stage == "target-movement")
        {
            if (!_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NamedTurnCountFlow = null });
                return false;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NamedTurnCountFlow = draft with { Stage = "target-draw" } });
            _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, draft.TargetSeat, 1, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.non-final-target.draw"));
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return true;
        }

        if (draft.Stage is "target-draw" or "hand-movement")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { NamedTurnCountFlow = null });
            return false;
        }

        return false;
    }

    internal void AssertNamedTurnFlow(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        if (frame.NamedTurnCountFlow is not { } draft)
            return;
        var payment = effect.Op == SkillProgramEffectOp.DiscardNonFinalTargetCardThenDraw;
        if ((!payment && effect.Op != SkillProgramEffectOp.DiscardHandToNamedTurnCount) || draft.Stage is not ("target-movement" or "target-draw" or "hand" or "hand-movement") || payment != (draft.Stage is "target-movement" or "target-draw") || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(draft.TargetSeat) || draft.Required < 0 || draft.CardIds.Distinct().Count() != draft.CardIds.Count || draft.SelectedIds.Distinct().Count() != draft.SelectedIds.Count || draft.SelectedIds.Any(id => !draft.CardIds.Contains(id)) || draft.SelectedIds.Count > draft.Required)
            throw new InvalidOperationException("Named-turn flow lost its typed stage and payment cursor.");
        if (payment && (draft.ActionId != NonFinalTargetUse(frame).Action!.ActionId || !NamedTurnActionAlreadyCommitted(frame.OwnerSeat, frame.SkillId, draft.ActionId!.Value)))
            throw new InvalidOperationException("Named-turn flow lost its committed action.");
        if (draft.Stage == "hand" && (draft.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat)) || ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || _corePendingDecisionPolicyPipelineOperations._pendingDecision.IsPrivate != true)))
            throw new InvalidOperationException("Named-turn hand draft lost its private physical costs.");
    }
}
