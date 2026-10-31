#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DamageJudgmentSuitPaymentCapability(Func<RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentsEntries> getCoreGameEngineDamageJudgmentSuitPaymentsRuntime) : IDamageJudgmentSuitPaymentHost
{
    public SkillProgramStepOutcome JudgeDamageTargetThenOfferSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineDamageJudgmentSuitPaymentsRuntime().BeginDamageJudgmentSuitPayment(frame, effect);
}
