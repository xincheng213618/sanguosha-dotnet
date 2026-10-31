namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_DamagePreventionPrograms
{
    internal static bool UsesDamageNatureCondition(SkillProgramTriggerCondition condition) =>
        condition.Kind == SkillProgramTriggerConditionKind.DamageNatureIs ||
        condition.Children.Any(RuntimeRules_Core_DamagePreventionPrograms.UsesDamageNatureCondition);
}
