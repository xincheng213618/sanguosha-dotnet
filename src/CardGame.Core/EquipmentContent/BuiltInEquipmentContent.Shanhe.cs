namespace CardGame.Core;

public static partial class BuiltInEquipmentContent
{
    public static EquipmentContentPackage Shanhe { get; } = new("equipment:shanhe",
    [
        new EquipmentDefinition(CardKind.ShanheSanshou, "三首", EquipmentSlot.Armor,
            "当你受到伤害时，你可以亮出牌堆顶三张牌，若包含本回合未使用过的类型，你防止此伤害。"),
        new EquipmentDefinition(CardKind.ShanRangZhaoShu, "禅让诏书", EquipmentSlot.Treasure,
            "每回合每名角色限一次，当其他角色于其回合外首次获得牌时，你可以交给其一张牌或令其交给你一张牌。"),
        new EquipmentDefinition(
                CardKind.ZhanhunDao,
                "斩魂刀",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 3。对已受伤角色造成伤害时，可防止此伤害并令其体力值上限-1。",
                WeaponAttackRange: 3),

        new EquipmentDefinition(
                CardKind.AnqiFeiren, "暗器飞刃", EquipmentSlot.Weapon,
                "锁定技，每回合每名角色限一次，你对其他角色造成伤害时，令其随机一个非锁定技失效直到其回合结束。",
                WeaponAttackRange: 5),

        new EquipmentDefinition(
                CardKind.YaodaoChitong, "妖刀赤瞳", EquipmentSlot.Weapon,
                "锁定技，你使用黑色杀造成的伤害+1；当你使用杀指定目标后判定，若为黑色，则随机转移为其他合法目标。",
                WeaponAttackRange: 3),

        new EquipmentDefinition(
                CardKind.ShanheXingtianAxe, "刑天破军斧", EquipmentSlot.Weapon,
                "出牌阶段，使用牌指定唯一目标后，可弃置两张牌，令其本回合不能使用或打出牌且防具失效。攻击范围4为本地约定。",
                WeaponAttackRange: 4),

        new EquipmentDefinition(
                CardKind.Leizhen, "雷震", EquipmentSlot.Weapon,
                "锁定技，每使用三张手牌后，下一张杀造成的伤害+1（无法叠加）。",
                WeaponAttackRange: 2)
    ],
    [
        new(CardKind.ShanheSanshou, "shanhe:sanshou-armor-3278", "equipment:shanhe-sanshou"),
        new(CardKind.ShanRangZhaoShu, "shanhe:chanrang-edict-1510", "equipment:shanhe-chanrang"),
        new(CardKind.ZhanhunDao, "shanhe:zhanhun-dao", "equipment:shanhe-zhanhun"),
        new(CardKind.AnqiFeiren, "shanhe:anqi-feiren", "equipment:shanhe-anqi"),
        new(CardKind.YaodaoChitong, "shanhe:yaodao-chitong", "equipment:shanhe-yaodao"),
        new(CardKind.ShanheXingtianAxe, "shanhe:xingtian-axe-20766-effect", "equipment:shanhe-xingtian"),
        new(CardKind.Leizhen, "shanhe:leizhen", "equipment:shanhe-leizhen")
    ]);
}
