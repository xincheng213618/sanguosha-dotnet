namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_GuanduEquipment
{
    internal static string? GuanduEquipmentSkillGrantId(Card card) =>
        EquipmentCatalog.GetSkillBinding(card.Kind) is { } binding && binding.SkillId.StartsWith("guandu:", StringComparison.Ordinal)
            ? binding.CreateGrantId(card.Id) : null;

    internal static bool IsGuanduThunderChariotDeparture(Card card, CardLocation from, CardLocation to) =>
        card.Kind == CardKind.ThunderChariot && from.Zone == CardZoneKind.Equipment && from != to;
}
