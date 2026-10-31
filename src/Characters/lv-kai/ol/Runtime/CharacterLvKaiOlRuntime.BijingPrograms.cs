#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKaiOlRuntime
{

    internal SkillProgramStepOutcome BijingProgramMarkHandCards(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var binding = active.CardSetBindings.SingleOrDefault(item => item.Name == effect.SourceBind!);
        if (binding is null)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "闭境的选牌绑定未生成，标记结束。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        foreach (var cardId in binding.CardIds)
            if (_cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(active.OwnerSeat))
                _characterLvKaiOlBijingMarkedCardIdsAccess._bijingMarkedCardIds[cardId] = active.OwnerSeat;
        if (binding.CardIds.Count > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBijingMarkedEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, binding.CardIds));
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome BijingProgramRecastMarkedCards(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive)
            return SkillProgramStepOutcome.Continue;
        var marked = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => _characterLvKaiOlBijingMarkedCardIdsAccess._bijingMarkedCardIds.TryGetValue(card.Id, out var seat) && seat == active.OwnerSeat).ToArray();
        foreach (var card in marked)
            _characterLvKaiOlBijingMarkedCardIdsAccess._bijingMarkedCardIds.Remove(card.Id);
        if (marked.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var reason = new CardMoveReason($"skill-program.{(active.SkillId)}.bijing-recast");
        _corePhysicalMovementFacadesOperations.MoveCards(marked, CardLocation.Hand(active.OwnerSeat), CardLocation.DiscardPile, reason);
        _corePhysicalDrawFacadesOperations.DrawCards(owner, marked.Length, true, new($"skill-program.{(active.SkillId)}.bijing-recast-draw"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBijingRecastEvent(active.Id, active.SkillId, active.OwnerSeat, Array.AsReadOnly(marked.Select(card => card.Id).ToArray()), marked.Length));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome BijingProgramPunishDiscardPhase(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var ownerSeat = active.OwnerSeat;
        if (frame.WindowContext is not { SourceSeat: { } turnOwner } || turnOwner == ownerSeat || !_coreMatchParticipantsPlayersAccess._players[turnOwner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[ownerSeat], active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var lost = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(movement => movement.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && movement.From == CardLocation.Hand(ownerSeat) && _characterLvKaiOlBijingMarkedCardIdsAccess._bijingMarkedCardIds.TryGetValue(movement.CardId, out var seat) && seat == ownerSeat).Select(movement => movement.CardId).Distinct().ToArray();
        if (lost.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var payable = BijingPunishChoices(active, turnOwner);
        if (payable.Count == 0)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBijingPunishEvent(active.Id, active.SkillId, ownerSeat, turnOwner, lost, []));
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { BijingPunish = new BijingPunishState(turnOwner, ownerSeat, lost, Math.Min(2, payable.Count), []) });
        PresentBijingPunishCardPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BijingPunishChoices(ProgramSkillFrame frame, int payer) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, payer, payer, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard);
    internal void PresentBijingPunishCardPrompt(ProgramSkillFrame frame)
    {
        var state = frame.BijingPunish ?? throw new InvalidOperationException("The Bijing punish lost its state.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var materialChoices = BijingPunishChoices(frame, state.TurnOwnerSeat).Where(choice => !state.SelectedCardIds.Contains(choice.Cards.Single())).ToArray();
        var required = state.Remaining - state.SelectedCardIds.Count;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = materialChoices.Select(choice => new PromptChoice(new ChoiceId($"bijing-punish.frame-{(frame.Id)}.card-{(choice.Cards.Single())}"), choice.Description, choice.Cards, [], new Dictionary<string, string> { ["program-action"] = "bijing-punish-discard", ["card-id"] = choice.Cards.Single().ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, state.TurnOwnerSeat, $"【{(presentation.Name)}】请选择另外 {(required)} 张手牌或装备牌，一并弃置。", choices.SelectMany(choice => choice.Cards).ToArray(), [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = state.TurnOwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 弃牌", $"{(_coreMatchParticipantsPlayersAccess._players[state.OwnerSeat].Name)} 本回合失去过“闭境”牌，你一并弃置 {(state.Remaining)} 张牌（已选 {(state.SelectedCardIds.Count)}）。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[state.TurnOwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("bijing-punish-discard")]
    internal void ResolveBijingPunishChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Bijing punish choice lost its program frame.");
        var state = frame.BijingPunish ?? throw new InvalidOperationException("The Bijing punish lost its state.");
        if (!int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.CultureInfo.InvariantCulture, out var cardId) || state.Paid || !BijingPunishChoices(frame, state.TurnOwnerSeat).Any(choice => choice.Cards.SequenceEqual([cardId])) || state.SelectedCardIds.Contains(cardId) || state.SelectedCardIds.Any(id => !BijingPunishChoices(frame, state.TurnOwnerSeat).Any(choice => choice.Cards.SequenceEqual([id]))))
            throw new InvalidOperationException("The Bijing punish card is no longer in hand.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var chosen = state.SelectedCardIds.Append(cardId).ToArray();
        if (chosen.Length < state.Remaining)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { BijingPunish = state with { SelectedCardIds = chosen } });
            PresentBijingPunishCardPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        // The official skill says discard two cards, without an ordered
        // per-card effect: https://www.sanguosha.com/hero/465 . Select the
        // whole unpaid group before moving it or publishing its loss batch.
        if (chosen.Length != state.Remaining)
            throw new InvalidOperationException("Bijing cannot overpay its original discard group.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { BijingPunish = state with { SelectedCardIds = chosen, SelectedLocations = chosen.Select(_cardZonesOperations._cardZones.GetLocation).ToArray(), Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(state.TurnOwnerSeat, 0, null) });
        active = _coreNativeLogicalDiscardsOperations.AppendFrameNativeLogicalDiscard(active, state.TurnOwnerSeat, chosen, new($"skill-program.{(active.SkillId)}.bijing-punish"));
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(chosen, CardLocation.DiscardPile, new($"skill-program.{(active.SkillId)}.bijing-punish"), disclosure: CardMovementDisclosure.FaceUp);
        var discarded = state.DiscardedCardIds.Concat(chosen).ToArray();
        active = _coreNativeLogicalDiscardsOperations.CompleteFrameNativeLogicalDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { BijingPunish = active.BijingPunish!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBijingPunishEvent(active.Id, active.SkillId, state.OwnerSeat, state.TurnOwnerSeat, state.LostCardIds, discarded));
        ContinueBijingPaidGroup(active.Id);
    }
}
