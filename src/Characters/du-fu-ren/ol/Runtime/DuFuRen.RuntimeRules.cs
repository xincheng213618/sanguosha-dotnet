
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_du_fu_ren_ol_DuFuRen
{
    internal static CardConversionSource DuFuRenSource(ProgramSkillFrame f) => new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);
}
