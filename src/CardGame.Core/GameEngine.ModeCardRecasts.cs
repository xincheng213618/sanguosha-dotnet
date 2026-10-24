namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool CanModeHandEquipmentRecast(Card card) => _modeRules.Cards.HasHandEquipmentRecast &&
        EquipmentRules.IsEquipment(card.Kind) &&
        _modeRules.Cards.AllowsHandEquipmentRecast(EquipmentRules.Get(card.Kind).Slot);
}
