namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IModeSetupEquipmentHost CreateModeSetupEquipmentHost() => new ModeSetupEquipmentHost(this);

    private sealed class ModeSetupEquipmentHost(GameEngine engine) : IModeSetupEquipmentHost
    {
        public EquipmentDefinition Definition(CardKind kind) => engine.EquipmentRules.Get(kind);
        public EquipmentSkillBinding? SkillBinding(CardKind kind) => engine.EquipmentRules.GetSkillBinding(kind);
    }

    private void PrepareModeSelectedEquipmentAndDeck()
    {
        var expectedFacts = ModeSetup.InitialEquipmentCompletionFacts;
        if (expectedFacts.Count == 0) return;
        if (expectedFacts.Any(type => type is null || !type.IsClass || type.IsAbstract || !typeof(IGameEvent).IsAssignableFrom(type)) ||
            expectedFacts.Distinct().Count() != expectedFacts.Count)
            throw new InvalidOperationException("Initial-equipment completion facts must be unique concrete event types.");
        var history = CompleteProgramEventHistory().ToArray();
        var completed = expectedFacts.Count(type => history.Any(type.IsInstanceOfType));
        if (completed == expectedFacts.Count) return;
        if (completed != 0)
            throw new InvalidOperationException("An initial-equipment recipe cannot resume from a partial completion ledger.");
        if (_setupComplete || _turnNumber != 0 || _players.Any(player => !player.GeneralSelected) || _cardMovements.Count != 0 ||
            _cardZones.Count(CardLocation.DrawPile) != _cardZones.TotalCards)
            throw new InvalidOperationException("Mode equipment must be prepared after all drafts and before the first physical movement.");
        var plan = ModeSetup.PrepareSelectedEquipment(Array.AsReadOnly(_cardZones.CardsAt(CardLocation.DrawPile).ToArray()), CaptureModeSetupSeats()) ??
            throw new InvalidOperationException("Mode setup did not produce its selected-equipment plan.");
        if (!plan.CompletionFacts.Select(fact => fact.GetType()).SequenceEqual(expectedFacts) ||
            plan.Placements.Any(placement => !IsValidPlayerSeat(placement.Seat) || !plan.Cards.Any(card => card.Id == placement.CardId)) ||
            plan.Placements.Select(placement => placement.CardId).Distinct().Count() != plan.Placements.Count)
            throw new InvalidOperationException("The initial-equipment plan does not match its completion ledger and physical entities.");
        _cardZones.RebuildPristineDrawPile(plan.Cards);
        _initialCardCount = _cardZones.TotalCards;
        foreach (var placement in plan.Placements)
        {
            var card = _cardZones.CardsAt(CardLocation.DrawPile).First(card => card.Id == placement.CardId);
            PlaceAdvancedEquipment(card, placement.Seat);
        }
        foreach (var fact in plan.CompletionFacts) AdvanceEventRulesAndQueueFact(fact);
    }
}
