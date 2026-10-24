namespace CardGame.Core.RuntimeSkills;

[global::CardGame.Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_EquipmentSkills
{
    internal static string? EquipmentSkillGrantId(Card card) =>
        EquipmentCatalog.GetSkillBinding(card.Kind)?.CreateGrantId(card.Id);
}
