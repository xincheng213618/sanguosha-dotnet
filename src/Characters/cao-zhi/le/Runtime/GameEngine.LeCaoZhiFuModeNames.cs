namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Reconstruct definitions, never live zones. The helpers below only map
    // card faces and do not draw, publish facts or consume random numbers.
    private CardKind[] LeCaoZhiModeNamePool()
    {
        var cards = CreateDeckFromRegistry(_contentRegistry,
            ResolveDeckDefinition(_contentRegistry, _options.DeckId ?? _modeDefinition.DeckId));
        cards = ModeSetup.ProjectDeckCardNames(cards);
        return cards.Select(card => card.Kind).Where(kind => !EquipmentRules.IsEquipment(kind))
            .Distinct().Order().ToArray();
    }
}
