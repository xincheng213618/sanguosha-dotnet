namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_NamedUseConversions
{
    internal static string NamedUseLedgerId(SkillProgramViewAs rule) =>
        $"view-as-name:{rule.NameLedgerId}:{RuntimeRules_Core_AlternativeViewAsCosts.ProgramBasicCardName(rule.OutputKind)}";
}
