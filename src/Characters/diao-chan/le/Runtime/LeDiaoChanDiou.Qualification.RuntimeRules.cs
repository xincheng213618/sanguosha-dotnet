namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_diao_chan_le_LeDiaoChanDiou_Qualification
{
    internal static CardConversionSource DiouSource(ProgramSkillFrame f) =>
        new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
}
