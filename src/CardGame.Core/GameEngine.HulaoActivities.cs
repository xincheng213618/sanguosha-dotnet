namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool CanHulaoWeaponRecast(Card card) => _modeDefinition.HulaoActivity is not null &&
        EquipmentRules.IsEquipment(card.Kind) && EquipmentRules.Get(card.Kind).Slot == EquipmentSlot.Weapon;

}
