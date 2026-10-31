namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_HulaoEquipment
{
    internal static string? HulaoEquipmentSkillId(CardKind kind) =>
        EquipmentCatalog.GetSkillBinding(kind) is { } binding && binding.SkillId.StartsWith("hulao:", StringComparison.Ordinal)
            ? binding.SkillId : null;

    internal static string? HulaoEquipmentSkillGrantId(Card card) =>
        HulaoEquipmentSkillId(card.Kind) is null ? null : EquipmentCatalog.GetSkillBinding(card.Kind)?.CreateGrantId(card.Id);
}
