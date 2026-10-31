
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_qian_zhao_ol_WeifuUses
{
    internal static bool WeifuCanAddTargets(CardUseFrame use) => RuntimeRules_Core_ProgramCardPayments.GetProgramCardCategory(use.CardKind) == SkillProgramCardCategory.Basic ||
        RuntimeRules_Core_CommonRules.IsOrdinaryTrick(use.CardKind);
}
