
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_SameTypeActualUseAid
{
    // 2018 official-site community tutorial is auxiliary classification evidence.
    // This is an opt-in engineering interpretation, not a new global card category.
    internal static bool IsSameTypeAidEffectiveCard(CardKind kind) => RuntimeRules_Core_CommonRules.IsSlashCard(kind) ||
        kind is CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage;

    internal static bool IsSameTypeAidCard(CardKind paid, CardKind effective) =>
        CardCatalog.Get(paid).CategoryName == CardCatalog.Get(effective).CategoryName &&
        RuntimeRules_Core_AlternativeViewAsCosts.ProgramBasicCardName(paid) != RuntimeRules_Core_AlternativeViewAsCosts.ProgramBasicCardName(effective);
}
