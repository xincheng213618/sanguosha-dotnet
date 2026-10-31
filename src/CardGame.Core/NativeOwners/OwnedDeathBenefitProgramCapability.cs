#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnedDeathBenefitProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedDeathBenefitReturnsEntries> getCoreGameEngineOwnedDeathBenefitReturnsRuntime) : IOwnedDeathBenefitProgramHost
{
    public SkillProgramStepOutcome SelectIssuedFixedRecipientWithDeathReturn(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineOwnedDeathBenefitReturnsRuntime().SelectIssuedFixedRecipientWithDeathReturn(frame, effect);
}
