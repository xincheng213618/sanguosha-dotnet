
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_zhang_fei_wumiao_WuZhangFeiUse
{
    internal static CardConversionSource WuZhangFeiSource(ProgramSkillFrame f) =>
        new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
}
