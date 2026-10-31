namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_dong_bai_ol_DongBaiPrograms
{
    internal static CardConversionSource LianZhuSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static string LianZhuReason(LianZhuStage stage) => "skill-program.lianzhu." + stage;
}
