namespace CardGame.Core;

public static partial class BuiltInEquipmentContent
{
    public static EquipmentContentPackage CharacterGenerated { get; } = new("equipment:charactergenerated",
    [
        new EquipmentDefinition(CardKind.DagongChe, "大攻车", EquipmentSlot.Treasure,
                "出牌阶段开始时，你可视为使用一张杀（不计入次数）；此杀造成伤害后，你弃置目标一张牌。升级选项：无视距离和防具；目标数+1；弃牌数+1，每个大攻车最多升级5次。未升级时不能被弃置，离开装备区时销毁。"),

        new EquipmentDefinition(CardKind.Xuanjian, "玄剑", EquipmentSlot.Weapon,
                "你可以将一种花色的所有手牌当普通杀使用。", WeaponAttackRange: 3),

        new EquipmentDefinition(CardKind.GeneralWeapon, "武将武器", EquipmentSlot.Weapon,
                "无花色点数，攻击范围为武将体力上限，具有牌上描述含杀的非觉醒、限定、转换、主公技能。离开装备区销毁。", WeaponAttackRange: 1),

        new EquipmentDefinition(CardKind.RedBloodBlade, "赤血刃", EquipmentSlot.Weapon,
                "攻击范围3（若你未拥有神御，改为0）。每回合首次你的杀对攻击范围内最远角色造成伤害时，此伤害+1。首次替换其他武器牌后，获得其技能。", WeaponAttackRange: 3),

        new EquipmentDefinition(
                CardKind.XingtianAxe,
                "刑天破军斧",
                EquipmentSlot.Weapon,
                "装备至武器槽；攻击范围 4，你对单一其他角色用牌后可弃两张牌，令其本回合不能使用或打出手牌且防具失效。",
                WeaponAttackRange: 4)
    ],
    [
        new(CardKind.XingtianAxe, "special:xingtian-axe-effect", "equipment:xingtian")
    ],
    [new(GeneralWeaponSource, DestroyOnEquipmentExit: true, RequiredEquipmentKind: CardKind.GeneralWeapon)],
    [
        new(SkillProgramEffectOp.EquipSampledGenerals, CardKind.GeneralWeapon),
        new(SkillProgramEffectOp.ResolveAnniversaryZhangFenWanglu, CardKind.DagongChe),
        new(SkillProgramEffectOp.ResolveAnniversaryDagongCheAttack, CardKind.DagongChe)
    ]);
}
