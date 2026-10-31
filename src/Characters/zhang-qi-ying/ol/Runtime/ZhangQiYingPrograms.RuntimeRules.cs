namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_zhang_qi_ying_ol_ZhangQiYingPrograms
{
    internal static CardConversionSource ZhangQiYingSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);
}
