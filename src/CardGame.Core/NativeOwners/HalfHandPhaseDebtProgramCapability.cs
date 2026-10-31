#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HalfHandPhaseDebtProgramCapability(Func<RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries> getCoreGameEngineHalfHandPhaseDebtProgramsRuntime, Func<RuntimeEntries.CoreGameEngineHalfHandTargetSupportEntries> getCoreGameEngineHalfHandTargetSupportRuntime) : IHalfHandPhaseDebtProgramHost
{
    public SkillProgramStepOutcome DrawExtraAndArmHalfHandSupport(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().DrawExtraHalfHandSupport(f, e);
    public SkillProgramStepOutcome GiveHalfHandAndIssueTargetSupport(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().GiveHalfHandSupport(f, e);
    public SkillProgramStepOutcome ExchangeHandsAndArmPhaseDebt(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().ExchangeAndArmPhaseHandDebt(f, e);
    public SkillProgramStepOutcome SelectFrozenHandExchangeDebtPayment(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().SelectPhaseHandDebtPayment(f, e);
    public SkillProgramStepOutcome OfferHalfHandRecipientSupport(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineHalfHandTargetSupportRuntime().OfferHalfHandSupport(f, e);
}
