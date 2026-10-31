#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedDeathBenefitReturnsEntries
{
    internal bool ExactOwnedDeathBenefitReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactOwnedDeathBenefitReturn(f);
    internal void FinishOwnedDeathBenefitReturn(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().FinishOwnedDeathBenefitReturn(f, completed);
    internal bool IsOriginalDyingSuspendedByOwnedDeathBenefit(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByOwnedDeathBenefit(dying);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectIssuedFixedRecipientWithDeathReturn(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectIssuedFixedRecipientWithDeathReturn(supplied, effect);
}
