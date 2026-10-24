namespace CardGame.Core;

/// <summary>
/// The five public equipment slots used by the K6 foundation. A player may
/// have at most one card in each slot; replacing a card moves the old physical
/// card to the discard pile before the new card enters the slot.
/// </summary>
public enum EquipmentSlot
{
    Weapon,
    Armor,
    OffensiveHorse,
    DefensiveHorse,
    Treasure
}

/// <summary>
/// Data-only modifiers for an equipment card. The engine consumes these values
/// through explicit rules queries; content does not mutate runtime state.
/// </summary>
public sealed record EquipmentDefinition(
    CardKind Kind,
    string DisplayName,
    EquipmentSlot Slot,
    string Description,
    int? WeaponAttackRange = null,
    int AttackRangeBonus = 0,
    int SlashLimitBonus = 0,
    int OutgoingDistanceModifier = 0,
    int IncomingDistanceModifier = 0,
    int DrawCountBonus = 0,
    bool IgnoresArmor = false,
    bool BlocksBlackSlash = false);

public static class EquipmentCatalog
{
    private static readonly EquipmentContentRegistry Definitions = EquipmentContentRegistry.Create(BuiltInEquipmentContent.All);

    public static IReadOnlyList<EquipmentDefinition> Implemented { get; } = Definitions.Implemented;

    public static bool IsEquipment(CardKind kind) => Definitions.IsEquipment(kind);

    public static EquipmentDefinition Get(CardKind kind) => Definitions.Get(kind);

    public static EquipmentSkillBinding? GetSkillBinding(CardKind kind) => Definitions.GetSkillBinding(kind);

    public static string GetSlotName(EquipmentSlot slot) => slot switch
    {
        EquipmentSlot.Weapon => "武器",
        EquipmentSlot.Armor => "防具",
        EquipmentSlot.OffensiveHorse => "进攻坐骑",
        EquipmentSlot.DefensiveHorse => "防御坐骑",
        EquipmentSlot.Treasure => "宝物",
        _ => slot.ToString()
    };
}
