namespace CardGame.Core;

public static partial class BuiltInEquipmentContent
{
    public static EquipmentContentPackage Standard { get; } = new("equipment:standard",
    [
        new EquipmentDefinition(
                CardKind.Crossbow,
                "诸葛连弩",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 1，出牌阶段可使用任意数量的杀。",
                WeaponAttackRange: 1,
                AttackRangeBonus: 1,
                SlashLimitBonus: int.MaxValue),

        new EquipmentDefinition(
                CardKind.QinggangSword,
                "青釭剑",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 2，你使用杀指定目标后无视其防具。",
                WeaponAttackRange: 2,
                IgnoresArmor: true),

        new EquipmentDefinition(
                CardKind.StoneAxe,
                "贯石斧",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 3，当你的杀被闪抵消后，你可以弃置两张牌，令此杀仍造成伤害。",
                WeaponAttackRange: 3),

        new EquipmentDefinition(
                CardKind.ZhangbaSerpentSpear,
                "丈八蛇矛",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 3，你可以将两张手牌当一张杀使用或打出。",
                WeaponAttackRange: 3),

        new EquipmentDefinition(
                CardKind.CixiongDoubleSwords,
                "雌雄双股剑",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 2，使用杀指定异性目标后，可令其弃一张手牌或令你摸一张牌。",
                WeaponAttackRange: 2),

        new EquipmentDefinition(
                CardKind.QinglongCrescentBlade,
                "青龙偃月刀",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 3，当你使用的杀被闪抵消后，你可以对同一目标再使用一张杀（无距离限制）。",
                WeaponAttackRange: 3),

        new EquipmentDefinition(
                CardKind.IceSword,
                "寒冰剑",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 2，当你使用杀即将造成伤害且目标有手牌或装备时，可防止此伤害并依次弃置其至多两张牌。",
                WeaponAttackRange: 2),

        new EquipmentDefinition(
                CardKind.QilinBow,
                "麒麟弓",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 5，当你使用杀对目标角色造成伤害时，可弃置其装备区的一张坐骑牌。",
                WeaponAttackRange: 5),

        new EquipmentDefinition(
                CardKind.FangtianHalberd,
                "方天画戟",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 4，当你使用最后的手牌杀时，可额外指定至多两个合法目标。",
                WeaponAttackRange: 4),

        new EquipmentDefinition(
                CardKind.GudingBlade,
                "古锭刀",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 2，锁定技，当你使用杀对没有手牌的目标造成伤害时，此伤害 +1。",
                WeaponAttackRange: 2),

        new EquipmentDefinition(
                CardKind.ZhuqueFan,
                "朱雀羽扇",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 4，你可以将使用的普通杀改为火杀。",
                WeaponAttackRange: 4),

        new EquipmentDefinition(
                CardKind.BaguaFormation,
                "八卦阵",
                EquipmentSlot.Armor,
                "装备至防具槽；每当需要使用或打出闪时，可进行一次判定，红色判定牌视为打出闪。"),

        new EquipmentDefinition(
                CardKind.RenwangShield,
                "仁王盾",
                EquipmentSlot.Armor,
                "装备至防具槽；黑色杀对你无效。",
                BlocksBlackSlash: true),

        new EquipmentDefinition(
                CardKind.Tengjia,
                "藤甲",
                EquipmentSlot.Armor,
                "锁定技，南蛮入侵、万箭齐发和普通杀对你无效；你受到的火焰伤害 +1。"),

        new EquipmentDefinition(
                CardKind.SilverLion,
                "白银狮子",
                EquipmentSlot.Armor,
                "锁定技，你受到大于1点的伤害时将伤害值改为1；失去装备区里的白银狮子后回复1点体力。"),

        new EquipmentDefinition(
                CardKind.OffensiveHorse,
                "赤兔",
                EquipmentSlot.OffensiveHorse,
                "装备至进攻坐骑槽；你到其他角色的战斗距离 -1。",
                OutgoingDistanceModifier: -1),

        new EquipmentDefinition(
                CardKind.DefensiveHorse,
                "绝影",
                EquipmentSlot.DefensiveHorse,
                "装备至防御坐骑槽；其他角色到你的战斗距离 +1。",
                IncomingDistanceModifier: 1),

        new EquipmentDefinition(
                CardKind.Dawan,
                "大宛",
                EquipmentSlot.OffensiveHorse,
                "装备至进攻坐骑槽；你到其他角色的战斗距离 -1。",
                OutgoingDistanceModifier: -1),

        new EquipmentDefinition(
                CardKind.Zixing,
                "紫骍",
                EquipmentSlot.OffensiveHorse,
                "装备至进攻坐骑槽；你到其他角色的战斗距离 -1。",
                OutgoingDistanceModifier: -1),

        new EquipmentDefinition(
                CardKind.Dilu,
                "的卢",
                EquipmentSlot.DefensiveHorse,
                "装备至防御坐骑槽；其他角色到你的战斗距离 +1。",
                IncomingDistanceModifier: 1),

        new EquipmentDefinition(
                CardKind.Zhaohuangfeidian,
                "爪黄飞电",
                EquipmentSlot.DefensiveHorse,
                "装备至防御坐骑槽；其他角色到你的战斗距离 +1。",
                IncomingDistanceModifier: 1),

        new EquipmentDefinition(
                CardKind.Hualiu,
                "骅骝",
                EquipmentSlot.DefensiveHorse,
                "装备至防御坐骑槽；其他角色到你的战斗距离 +1。",
                IncomingDistanceModifier: 1),

        new EquipmentDefinition(
                CardKind.JadeSeal,
                "玉玺",
                EquipmentSlot.Treasure,
                "装备至宝物槽；摸牌阶段额外摸一张牌。",
                DrawCountBonus: 1),

        new EquipmentDefinition(
                CardKind.WoodenOx,
                "木牛流马",
                EquipmentSlot.Treasure,
                "出牌阶段限一次，你可以将一张手牌扣置于此牌下；若如此做，你可以将此装备移动至一名其他角色的装备区。你可使用或打出此牌下的牌。")
    ]);
}
