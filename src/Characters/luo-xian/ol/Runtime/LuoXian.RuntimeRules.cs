namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_luo_xian_ol_LuoXian
{
    internal static CardConversionSource DailiSource(ProgramSkillFrame frame) => new(frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
}
