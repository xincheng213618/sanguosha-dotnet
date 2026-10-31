#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineModeSetupEquipmentRuntime
{
    internal IModeSetupEquipmentHost CreateModeSetupEquipmentHost() => _kernelCoreGameEngineModeSetupEquipmentOperations.CreateModeSetupEquipmentHost();
    internal void PrepareModeSelectedEquipmentAndDeck()
    {
        var expectedFacts = _coreModeSetupOperations.ModeSetup.InitialEquipmentCompletionFacts;
        if (expectedFacts.Count == 0)
            return;
        if (expectedFacts.Any(type => type is null || !type.IsClass || type.IsAbstract || !typeof(IGameEvent).IsAssignableFrom(type)) || expectedFacts.Distinct().Count() != expectedFacts.Count)
            throw new InvalidOperationException("Initial-equipment completion facts must be unique concrete event types.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var completed = expectedFacts.Count(type => history.Any(type.IsInstanceOfType));
        if (completed == expectedFacts.Count)
            return;
        if (completed != 0)
            throw new InvalidOperationException("An initial-equipment recipe cannot resume from a partial completion ledger.");
        if (_coreMatchSetupSetupCompleteAccess._setupComplete || _coreActualTurnProgressTurnNumberAccess._turnNumber != 0 || _coreMatchParticipantsPlayersAccess._players.Any(player => !player.GeneralSelected) || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count != 0 || _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) != _cardZonesOperations._cardZones.TotalCards)
            throw new InvalidOperationException("Mode equipment must be prepared after all drafts and before the first physical movement.");
        var plan = _coreModeSetupOperations.ModeSetup.PrepareSelectedEquipment(Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).ToArray()), _coreModeSetupOperations.CaptureModeSetupSeats()) ?? throw new InvalidOperationException("Mode setup did not produce its selected-equipment plan.");
        if (!plan.CompletionFacts.Select(fact => fact.GetType()).SequenceEqual(expectedFacts) || plan.Placements.Any(placement => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(placement.Seat) || !plan.Cards.Any(card => card.Id == placement.CardId)) || plan.Placements.Select(placement => placement.CardId).Distinct().Count() != plan.Placements.Count)
            throw new InvalidOperationException("The initial-equipment plan does not match its completion ledger and physical entities.");
        _cardZonesOperations._cardZones.RebuildPristineDrawPile(plan.Cards);
        _corePhysicalCardPopulationInitialCardCountAccess._initialCardCount = _cardZonesOperations._cardZones.TotalCards;
        foreach (var placement in plan.Placements)
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).First(card => card.Id == placement.CardId);
            _coreAdvancedWeaponsOperations.PlaceAdvancedEquipment(card, placement.Seat);
        }

        foreach (var fact in plan.CompletionFacts)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
    }
}
