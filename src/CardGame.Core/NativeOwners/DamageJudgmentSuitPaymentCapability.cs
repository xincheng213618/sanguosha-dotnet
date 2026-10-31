#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DamageJudgmentSuitPaymentCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageJudgmentSuitPaymentsEntries> getCoreGameEngineDamageJudgmentSuitPaymentsRuntime) : IDamageJudgmentSuitPaymentHost
{
    public SkillProgramStepOutcome JudgeDamageTargetThenOfferSuitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineDamageJudgmentSuitPaymentsRuntime().BeginDamageJudgmentSuitPayment(frame, effect);
}
