
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_DamageTargetBudget
{
    internal static bool DamageBudgetCard(CardActionContext a) => RuntimeRules_Core_CommonRules.IsSlashCard(a.EffectiveKind) ||
        RuntimeRules_Core_CommonRules.IsOrdinaryTrick(a.EffectiveKind) && a.EffectiveIsRed == false;
}
