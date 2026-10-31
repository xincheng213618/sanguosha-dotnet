
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_RecipientContests
{
    internal static string RecipientContestGiftReason(ProgramSkillFrame f) => $"skill-program.{f.SkillId}.{SkillProgramEffectOp.GiveAllHandAndStartRecipientPindian}";
}
