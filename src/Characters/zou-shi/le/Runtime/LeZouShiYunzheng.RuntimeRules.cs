namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_zou_shi_le_LeZouShiYunzheng
{
    internal static CardConversionSource YunzhengSource(ProgramSkillFrame frame) =>
        new(frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
}
