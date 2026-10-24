namespace CardGame.Core;

public static partial class BuiltInEquipmentContent
{
    public static EquipmentContentPackage Guandu { get; } = new("equipment:guandu",
    [
        new EquipmentDefinition(CardKind.QiBaoDao, "七宝刀", EquipmentSlot.Weapon, "锁定技，你使用【杀】无视目标防具；你使用【杀】对未受伤目标造成伤害+1。", WeaponAttackRange: 2, IgnoresArmor: true),

        new EquipmentDefinition(CardKind.BaiBiDao, "百辟刀", EquipmentSlot.Weapon, "当你使用杀造成伤害后，可以获得目标一张手牌。", WeaponAttackRange: 2),

        new EquipmentDefinition(CardKind.ThunderChariot, "霹雳车", EquipmentSlot.Weapon,
                "造成伤害后可弃置目标防具及防御坐骑；离开装备区销毁。", WeaponAttackRange: 9)
    ],
    [
        new(CardKind.ThunderChariot, "guandu:thunder-chariot", "equipment:guandu")
    ]);
}
