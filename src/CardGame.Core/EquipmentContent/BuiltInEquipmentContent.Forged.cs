namespace CardGame.Core;

public static partial class BuiltInEquipmentContent
{
    public const string ForgedSource = "equipment:forged";
    public static EquipmentContentPackage Forged { get; } = new("equipment:forged",
    [
        new(CardKind.BintieDoubleHalberd, "镔铁双戟", EquipmentSlot.Weapon,
            "你的杀被抵消后，你可以失去1点体力，获得此杀并摸一张牌，本回合使用杀的次数+1。", WeaponAttackRange: 3),
        new(CardKind.BlackIronChain, "乌铁锁链", EquipmentSlot.Weapon,
            "你使用杀指定目标后，若其未横置，则横置之。", WeaponAttackRange: 3),
        new(CardKind.FiveElementsFan, "五行鹤翎扇", EquipmentSlot.Weapon,
            "你可以将属性杀当任意其他属性杀使用。", WeaponAttackRange: 4),
        new(CardKind.Sanlue, "三略", EquipmentSlot.Treasure,
            "锁定技，你的攻击范围、手牌上限、出牌阶段使用杀的次数各+1。", AttackRangeBonus: 1, SlashLimitBonus: 1),
        new(CardKind.TianjiDiagram, "天机图", EquipmentSlot.Treasure,
            "锁定技，此牌进入你的装备区时，弃置一张其他牌；离开你的装备区时，将手牌摸至五张。"),
        new(CardKind.TaigongYinfu, "太公阴符", EquipmentSlot.Treasure,
            "出牌阶段开始时，你可以横置或重置一名角色；出牌阶段结束时，你可以重铸一张手牌。")
    ],
    [
        new(CardKind.BintieDoubleHalberd, "equipment:bintie-double-halberd", "equipment:bintie"),
        new(CardKind.BlackIronChain, "equipment:black-iron-chain", "equipment:black-iron-chain"),
        new(CardKind.FiveElementsFan, "equipment:five-elements-fan", "equipment:five-elements-fan"),
        new(CardKind.Sanlue, "equipment:sanlue", "equipment:sanlue"),
        new(CardKind.TianjiDiagram, "equipment:tianji-diagram", "equipment:tianji-diagram"),
        new(CardKind.TaigongYinfu, "equipment:taigong-yinfu", "equipment:taigong-yinfu")
    ], destinationPolicies: [new(ForgedSource, DestroyOnDiscardPileEntry: true)]);
}
