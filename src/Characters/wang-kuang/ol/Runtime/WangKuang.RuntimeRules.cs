
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_wang_kuang_ol_WangKuang
{
    internal static CardConversionSource WangKuangSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static bool IsWangKuangDamageCardKind(CardKind kind) => RuntimeRules_Core_MouLuMeng.IsDamageCardKind(kind) || kind == CardKind.UnexpectedAssault;
}
