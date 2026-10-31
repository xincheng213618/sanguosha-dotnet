
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_liu_cheng_mou_mobile_MouLiuChengCompletion
{
    internal static CardConversionSource MouLiuChengSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static bool IsMouLiuChengNonDamageOrdinary(CardKind kind) => RuntimeRules_Core_CommonRules.IsOrdinaryTrick(kind) && kind is not
        (CardKind.Duel or CardKind.FireAttack or CardKind.BarbarianAssault or CardKind.ArrowBarrage or CardKind.UnexpectedAssault or CardKind.AgainstCurrent or CardKind.WumiaoFlood);
}
