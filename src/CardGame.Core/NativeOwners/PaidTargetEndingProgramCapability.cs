#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PaidTargetEndingProgramCapability(Func<RuntimeEntries.CoreGameEnginePaidOwnTargetObserversEntries> getCoreGameEnginePaidOwnTargetObserversRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime>> getCoreGameEngineEarnedActualEndingBenefitsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineLostHpOwnedGiftsRuntime>> getCoreGameEngineLostHpOwnedGiftsRuntime) : IPaidTargetEndingProgramHost
{
    public SkillProgramStepOutcome PayHpThenNullifyOwnActualUseTarget(ProgramSkillFrame f) => getCoreGameEnginePaidOwnTargetObserversRuntime().PayOwnActualUseTarget(f);
    public void ScheduleEarnedActualEndingBenefit(ProgramSkillFrame f, string skill, string binding) => getCoreGameEngineEarnedActualEndingBenefitsRuntime().Value.IssueEarnedActualEnding(f, skill, binding);
    public SkillProgramStepOutcome DrawLostHpThenOfferOwnedCardsUpTo(ProgramSkillFrame f) => getCoreGameEngineLostHpOwnedGiftsRuntime().Value.RunLostHpOwnedGift(f);
}
