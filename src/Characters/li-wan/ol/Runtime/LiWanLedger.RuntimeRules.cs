namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_li_wan_ol_LiWanLedger
{
    internal static CardConversionSource LiWanSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
}
