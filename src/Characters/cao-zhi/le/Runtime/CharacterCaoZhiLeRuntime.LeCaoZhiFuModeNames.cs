#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhiLeRuntime
{
    internal CardKind[] LeCaoZhiModeNamePool()
    {
        var cards = global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime.CreateDeckFromRegistry(_contentRegistryOperations._contentRegistry, global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime.ResolveDeckDefinition(_contentRegistryOperations._contentRegistry, _coreMatchDefinitionModeDefinitionOptionsAccess._options.DeckId ?? _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.DeckId));
        cards = _coreModeSetupOperations.ModeSetup.ProjectDeckCardNames(cards);
        return cards.Select(card => card.Kind).Where(kind => !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind)).Distinct().Order().ToArray();
    }
}
