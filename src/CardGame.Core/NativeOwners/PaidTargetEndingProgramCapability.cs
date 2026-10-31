#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PaidTargetEndingProgramCapability(Func<RuntimeEntries.CoreGameEnginePaidOwnTargetObserversEntries> getCoreGameEnginePaidOwnTargetObserversRuntime, Func<RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries> getCoreGameEngineEarnedActualEndingBenefitsRuntime, Func<RuntimeEntries.CoreGameEngineLostHpOwnedGiftsEntries> getCoreGameEngineLostHpOwnedGiftsRuntime) : IPaidTargetEndingProgramHost
{
    public SkillProgramStepOutcome PayHpThenNullifyOwnActualUseTarget(ProgramSkillFrame f) => getCoreGameEnginePaidOwnTargetObserversRuntime().PayOwnActualUseTarget(f);
    public void ScheduleEarnedActualEndingBenefit(ProgramSkillFrame f, string skill, string binding) => getCoreGameEngineEarnedActualEndingBenefitsRuntime().IssueEarnedActualEnding(f, skill, binding);
    public SkillProgramStepOutcome DrawLostHpThenOfferOwnedCardsUpTo(ProgramSkillFrame f) => getCoreGameEngineLostHpOwnedGiftsRuntime().RunLostHpOwnedGift(f);
}
