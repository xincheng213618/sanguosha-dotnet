namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_zhao_zhong_ol_Yintian
{
    internal static bool IsYintianRecoveryTrigger(SkillProgramTrigger trigger) =>
        trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveYintianFirstRecovery }];
}
