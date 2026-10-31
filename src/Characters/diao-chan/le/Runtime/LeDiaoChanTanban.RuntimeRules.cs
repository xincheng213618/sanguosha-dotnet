namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_diao_chan_le_LeDiaoChanTanban
{
    internal static CardConversionSource TanbanSource(ProgramSkillFrame frame) =>
        new(frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
}
