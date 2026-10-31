#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedDeathBenefitReturnsEntries
{
    internal bool ExactOwnedDeathBenefitReturn(ProgramSkillFrame f) => _resolve().ExactOwnedDeathBenefitReturn(f);
    internal void FinishOwnedDeathBenefitReturn(ProgramSkillFrame f, bool completed) => _resolve().FinishOwnedDeathBenefitReturn(f, completed);
    internal bool IsOriginalDyingSuspendedByOwnedDeathBenefit(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByOwnedDeathBenefit(dying);
    internal SkillProgramStepOutcome SelectIssuedFixedRecipientWithDeathReturn(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().SelectIssuedFixedRecipientWithDeathReturn(supplied, effect);
}
