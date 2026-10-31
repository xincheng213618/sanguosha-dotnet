namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_da_qiao_le_LeDaQiaoZixiEffects
{
    internal static CardConversionSource ZixiEffectsSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static bool ZixiEffectsKind(CardKind kind) => RuntimeRules_Core_ProgramCardPayments.GetProgramCardCategory(kind) == SkillProgramCardCategory.Basic || RuntimeRules_Core_CommonRules.IsOrdinaryTrick(kind);
}
