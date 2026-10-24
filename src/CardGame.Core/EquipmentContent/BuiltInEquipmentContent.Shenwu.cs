namespace CardGame.Core;

public static partial class BuiltInEquipmentContent
{
    public static EquipmentContentPackage Shenwu { get; } = new("equipment:shenwu",
    [
        new EquipmentDefinition(CardKind.NationalStyleRobe, "国风玉袍", EquipmentSlot.Armor,
                "不能成为其他角色使用普通锦囊牌的目标。"),

        new EquipmentDefinition(CardKind.CrimsonSoulZither, "赤焰镇魂琴", EquipmentSlot.Weapon,
                "造成的伤害均视为火属性。", WeaponAttackRange: 4),

        new EquipmentDefinition(CardKind.QimenEightTrigrams, "奇门八卦", EquipmentSlot.Armor,
                "其他角色使用的杀对你无效。"),

        new EquipmentDefinition(CardKind.DustlessGoldenSpear, "绝尘金戈", EquipmentSlot.DefensiveHorse,
                "敌方角色计算与己方其他角色的距离+1。", IncomingDistanceModifier: 1),

        new EquipmentDefinition(CardKind.ShuraInfernoHalberd, "修罗炼狱戟", EquipmentSlot.Weapon,
                "杀可额外指定任意名范围内其他角色；杀伤害+1，然后令受伤角色回复1。", WeaponAttackRange: 4),

        new EquipmentDefinition(CardKind.DelusionCrown, "虚妄之冕", EquipmentSlot.Treasure,
                "摸牌阶段额外摸两张牌；手牌上限-1。"),

        new EquipmentDefinition(
                CardKind.GhostDragonCrescentBlade,
                "鬼龙斩月刀",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 3，你使用的红色杀不能被闪响应。",
                WeaponAttackRange: 3),

        new EquipmentDefinition(
                CardKind.ScarletBloodSword,
                "赤血青锋",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 2，你的杀无视目标防具，目标不能使用或打出手牌响应。",
                WeaponAttackRange: 2,
                IgnoresArmor: true)
    ],
    [
        new(CardKind.ShuraInfernoHalberd, "shenwu:shura-inferno-halberd", "equipment:shenwu"),
        new(CardKind.DelusionCrown, "shenwu:delusion-crown", "equipment:shenwu")
    ]);
}
