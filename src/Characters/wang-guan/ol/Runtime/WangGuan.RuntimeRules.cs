namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_wang_guan_ol_WangGuan
{
    internal static CardConversionSource WangGuanSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);
}
