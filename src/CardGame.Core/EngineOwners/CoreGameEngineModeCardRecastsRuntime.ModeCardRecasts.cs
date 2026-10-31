#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineModeCardRecastsRuntime
{
    internal bool CanModeHandEquipmentRecast(Card card) => _modeRulesOperations._modeRules.Cards.HasHandEquipmentRecast && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && _modeRulesOperations._modeRules.Cards.AllowsHandEquipmentRecast(_coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot);
}
