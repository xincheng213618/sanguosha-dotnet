
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_da_qiao_xiao_qiao_ol_DaQiaoXiaoQiao
{
    internal static CardConversionSource DaQiaoXiaoQiaoSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);
}
