namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Match options and content are frozen before the first equipment query.
    // The cache contains immutable definitions only, never card or frame state.
    private EquipmentContentRegistry? _equipmentRules;
    private EquipmentContentRegistry EquipmentRules => _equipmentRules ??=
        BuiltInEquipmentContent.CreateForMatch(_modeDefinition, _contentRegistry,
            MatchEquipmentKinds(), _contentRegistry.EquipmentPackages);

    private IEnumerable<CardKind> MatchEquipmentKinds()
    {
        var deckId = _options.DeckId ?? _modeDefinition.DeckId;
        if (deckId is not null && _contentRegistry.Decks.TryGetValue(deckId, out var deck))
            foreach (var id in deck.Cards.Select(card => card.CardDefinitionId)
                .Concat((deck.PhysicalCards ?? []).Select(card => card.CardDefinitionId)))
                if (_contentRegistry.Cards[id].LegacyKind is { } kind) yield return kind;

        if (_options.ShanheBattle is { } setup)
        {
            foreach (var card in setup.StartingCards.Concat(setup.StartingEquipment)) yield return card.Kind;
            foreach (var member in setup.AllyRoster.Concat(setup.EnemyRoster))
                foreach (var card in (member.StartingHand ?? []).Concat(member.StartingCards ?? []))
                    yield return card.Kind;
        }

        if (_modeDefinition.Id == WushuangMode.ModeId)
            foreach (var card in WushuangCatalogue.Cards.Where(card => card.IsAvailableInShop && card.IsEquipment))
                if (card.Kind is { } kind) yield return kind;
    }
}
