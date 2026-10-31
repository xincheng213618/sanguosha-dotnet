#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhiLeRuntime
{
    internal CardKind[] LeCaoZhiModeNamePool()
    {
        var cards = CoreMatchParticipantSetupRuntime.CreateDeckFromRegistry(_contentRegistryOperations._contentRegistry, CoreMatchParticipantSetupRuntime.ResolveDeckDefinition(_contentRegistryOperations._contentRegistry, _coreMatchDefinitionModeDefinitionOptionsAccess._options.DeckId ?? _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.DeckId));
        cards = _coreModeSetupOperations.ModeSetup.ProjectDeckCardNames(cards);
        return cards.Select(card => card.Kind).Where(kind => !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind)).Distinct().Order().ToArray();
    }
}
