namespace CardGame.Core.RuntimeSkills;

[global::CardGame.Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_ShenwuEquipment
{
    internal static string? ShenwuEquipmentSkillId(CardKind kind) =>
        EquipmentCatalog.GetSkillBinding(kind) is { } binding && binding.SkillId.StartsWith("shenwu:", StringComparison.Ordinal)
            ? binding.SkillId : null;

    internal static string? ShenwuEquipmentSkillGrantId(Card card) =>
        ShenwuEquipmentSkillId(card.Kind) is null ? null : EquipmentCatalog.GetSkillBinding(card.Kind)?.CreateGrantId(card.Id);
}
