namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_zhou_fei_le_LeZhouFeiXianshu
{
    internal static CardConversionSource LeZhouFeiXianshuSource(ProgramSkillFrame frame) =>
        new(frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
}
