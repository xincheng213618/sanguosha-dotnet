
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_NamedCardAcquisitions
{
    internal static string NamedAcquisitionReason(ProgramSkillFrame f) =>
        $"skill-program.{f.SkillId}.{SkillProgramEffectOp.LoseSkillsAndObtainNamedCard}";
}
