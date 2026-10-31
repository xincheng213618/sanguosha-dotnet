namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_sun_quan_ol_magic_MoSunQuanTianen
{
    internal static CardConversionSource MoTianenSource(ProgramSkillFrame f) =>
        new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
}
