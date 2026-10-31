namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_cai_mao_ol_CaiMao
{
    internal static CardConversionSource CaiMaoSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);
}
