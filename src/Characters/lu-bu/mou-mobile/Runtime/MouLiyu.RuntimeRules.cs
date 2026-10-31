
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_lu_bu_mou_mobile_MouLiyu
{
    internal static int MouLbCategory(CardKind kind) => RuntimeRules_Core_ProgramCardPayments.GetProgramCardCategory(kind) switch { SkillProgramCardCategory.Basic => 1, SkillProgramCardCategory.Equipment => 4, _ => 2 };
}
