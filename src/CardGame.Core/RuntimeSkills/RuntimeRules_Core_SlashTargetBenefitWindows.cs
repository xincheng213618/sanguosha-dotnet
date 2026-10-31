namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_SlashTargetBenefitWindows
{
    internal static bool IsSlashTargetBenefitWindow(SkillProgramTriggerWindow w) =>
        w is SkillProgramTriggerWindow.ActualSlashTargetBenefit or SkillProgramTriggerWindow.SlashDodgeCancelledBenefit;
}
