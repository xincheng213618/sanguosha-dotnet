#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOpeningCustomizationRuntime
{
    internal CommandResult SubmitGeneralRoster(RequestGeneralRosterCommand command)
    {
        var validation = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.SelectGeneral, command.PromptId, CommandErrorCode.InvalidGeneral);
        if (validation is not null)
            return _kernelCoreMainOperations.Reject(validation.Code, validation.Message);
        return _kernelCoreMainOperations.Accept(() =>
        {
            var player = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
            var offers = _coreMatchParticipantSetupOperations.GetAvailableGeneralOffers(player, command.FullRoster);
            var candidates = offers.Select(offer => offer.General).ToArray();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GeneralSelectionRequestedEvent(player.Seat, candidates.Select(general => general.Id).ToArray()));
            _coreMatchParticipantSetupOperations.RequestHumanGeneralSelection(player, candidates, offers.ToDictionary(offer => offer.General.Id, offer => offer.CandidateId), command.FullRoster);
        });
    }

    internal void RequestOpeningHandConfirmation()
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[_coreMatchDefinitionOptionsAccess._options.HumanSeat]);
        // Publish kinds and the human's own payment card only. The source entity
        // and its hidden owner remain trusted setup details.
        var kinds = OpeningReplacementCards().Select(card => card.Kind).Distinct().Order().ToArray();
        var choices = new[]
        {
            "reroll",
            "confirm"
        }.Select(action => new PromptChoice(new ChoiceId($"setup.opening.{(action)}"), action == "reroll" ? "随机" : "确定", [], [], new Dictionary<string, string> { ["action"] = action })).ToList();
        foreach (var card in hand.Where(card => !IsFixedShanheOpeningCard(card)))
            foreach (var kind in kinds.Where(kind => kind != card.Kind))
                choices.Add(new(new($"setup.opening.replace.{(card.Id)}.{(kind)}"), CardCatalog.Get(kind).DisplayName, [card.Id], [], new Dictionary<string, string> { ["action"] = "replace", ["kind"] = kind.ToString() }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.OpeningHand, _coreMatchDefinitionOptionsAccess._options.HumanSeat, "在手牌区查看起手牌，可随机或选中一张牌换牌，点击确定进入牌局。", [], [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices.ToArray()
        };
        _coreMatchOutcomeStatusAccess._status = EngineStatus.AwaitingHumanOpeningHand;
    }

    internal IEnumerable<Card> OpeningReplacementCards() => _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Concat(_coreMatchParticipantsPlayersAccess._players.Where(player => player.Seat != _coreMatchDefinitionOptionsAccess._options.HumanSeat).SelectMany(_corePhysicalOwnedCardQueriesOperations.GetHand)).Where(card => !IsFixedShanheOpeningCard(card));
    internal bool IsFixedShanheOpeningCard(Card card) => _coreMatchDefinitionOptionsAccess._options.ShanheBattle is not null && _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(card.Id) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(move => move.CardId == card.Id && move.From == CardLocation.OutsideGame && move.To.Zone == CardZoneKind.Hand && move.Reason == CardMoveReasons.InitialDeal);
    internal void ReplaceOpeningCard(PromptChoice selected)
    {
        var hand = CardLocation.Hand(_coreMatchDefinitionOptionsAccess._options.HumanSeat);
        var outgoing = _cardZonesOperations._cardZones.CardsAt(hand).Single(card => card.Id == selected.Cards.Single());
        var kind = Enum.Parse<CardKind>(selected.Parameters["kind"]);
        var incoming = OpeningReplacementCards().First(card => card.Kind == kind);
        var source = _cardZonesOperations._cardZones.GetLocation(incoming.Id);
        var reason = new CardMoveReason("setup.opening-replace");
        var batch = _coreCardMovementProgramsOperations.BeginCardMovementBatch([hand, source], [source, hand]);
        var movements = new List<CardMovementRecord>(2);
        var committed = false;
        try
        {
            _cardZonesOperations._cardZones.MoveBatch([new(outgoing.Id, hand, source), new(incoming.Id, source, hand)]);
            movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(outgoing, hand, source, reason));
            movements.Add(_corePhysicalMovementFacadesOperations.RecordMovement(incoming, source, hand, reason));
            committed = true;
        }
        finally
        {
            _coreCardMovementProgramsOperations.CompleteCardMovementBatch(batch, movements, committed);
        }
    }

    internal void ResolveOpeningHandChoice(PromptChoice selected)
    {
        if (_coreMatchSetupSetupCompleteAccess._setupComplete || _coreActualTurnProgressTurnNumberAccess._turnNumber != 0 || !_coreMatchDefinitionOptionsAccess._options.ConfirmOpeningHand || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind != DecisionKind.OpeningHand || _resolutionStackOperations._resolutionStack.Count != 0)
            throw new InvalidOperationException("Opening hand changes require their actual pre-game prompt.");
        if (selected.Parameters["action"] == "replace")
        {
            ReplaceOpeningCard(selected);
            RequestOpeningHandConfirmation();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (selected.Parameters["action"] == "reroll")
        {
            var player = _coreMatchParticipantsPlayersAccess._players[_coreMatchDefinitionOptionsAccess._options.HumanSeat];
            var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(player).Where(card => !IsFixedShanheOpeningCard(card)).ToArray();
            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Hand(player.Seat), CardLocation.DrawPile, new("setup.opening-reroll"));
            _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
            _corePhysicalDrawFacadesOperations.DrawCards(player, cards.Length, log: false, CardMoveReasons.InitialDeal);
            RequestOpeningHandConfirmation();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchParticipantSetupOperations.FinalizeSetup();
        if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }
}
