
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_DrawDiscardCategoryRefund
{
    internal static string DrawDiscardCategoryReason(ProgramSkillFrame f, string part) => $"skill-program.{f.SkillId}.draw-discard-category.{part}";
}
