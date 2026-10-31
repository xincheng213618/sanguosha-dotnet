namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_ActualTurnViewAs
{
    internal static string ActualTurnViewAsUsageId(SkillProgramViewAs rule) =>
        $"view-as-actual-turn:{rule.UsageGroup ?? rule.Id}";
}
