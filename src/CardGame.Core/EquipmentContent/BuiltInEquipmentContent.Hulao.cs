namespace CardGame.Core;

public static partial class BuiltInEquipmentContent
{
    public static EquipmentContentPackage Hulao { get; } = new("equipment:hulao",
    [
        new EquipmentDefinition(CardKind.WushuangHalberd, "无双方天戟", EquipmentSlot.Weapon,
                "杀造成伤害后，可摸1张牌或弃置目标1张牌。", WeaponAttackRange: 4),

        new EquipmentDefinition(CardKind.PurpleGoldCrown, "束发紫金冠", EquipmentSlot.Treasure,
                "准备阶段，可对一名其他角色造成1点伤害。"),

        new EquipmentDefinition(CardKind.RedCottonRobe, "红棉百花袍", EquipmentSlot.Armor,
                "防止受到的属性伤害。"),

        new EquipmentDefinition(CardKind.LionBelt, "玲珑狮蛮带", EquipmentSlot.Armor,
                "成为其他角色用牌的唯一目标后，可判定：红桃则此牌对你无效。")
    ],
    [
        new(CardKind.WushuangHalberd, "hulao:wushuang-halberd", "equipment:hulao"),
        new(CardKind.PurpleGoldCrown, "hulao:purple-gold-crown", "equipment:hulao"),
        new(CardKind.RedCottonRobe, "hulao:red-cotton-robe", "equipment:hulao"),
        new(CardKind.LionBelt, "hulao:lion-belt", "equipment:hulao")
    ]);
}
