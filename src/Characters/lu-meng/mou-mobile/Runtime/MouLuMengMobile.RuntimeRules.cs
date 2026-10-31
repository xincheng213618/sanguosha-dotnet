namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_lu_meng_mou_mobile_MouLuMengMobile
{
    internal static CardConversionSource MouLuMengSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
}
