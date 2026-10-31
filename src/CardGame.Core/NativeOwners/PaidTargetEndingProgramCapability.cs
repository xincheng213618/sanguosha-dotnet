#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PaidTargetEndingProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePaidOwnTargetObserversEntries> getCoreGameEnginePaidOwnTargetObserversRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries> getCoreGameEngineEarnedActualEndingBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineLostHpOwnedGiftsEntries> getCoreGameEngineLostHpOwnedGiftsRuntime) : IPaidTargetEndingProgramHost
{
    public SkillProgramStepOutcome PayHpThenNullifyOwnActualUseTarget(ProgramSkillFrame f) => getCoreGameEnginePaidOwnTargetObserversRuntime().PayOwnActualUseTarget(f);
    public void ScheduleEarnedActualEndingBenefit(ProgramSkillFrame f, string skill, string binding) => getCoreGameEngineEarnedActualEndingBenefitsRuntime().IssueEarnedActualEnding(f, skill, binding);
    public SkillProgramStepOutcome DrawLostHpThenOfferOwnedCardsUpTo(ProgramSkillFrame f) => getCoreGameEngineLostHpOwnedGiftsRuntime().RunLostHpOwnedGift(f);
}
