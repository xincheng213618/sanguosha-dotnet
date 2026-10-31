#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SuitPreventionAndJudgmentPlacementCapability(Func<RuntimeEntries.CoreGameEngineSuitDamagePreventionBenefitsEntries> getCoreGameEngineSuitDamagePreventionBenefitsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineMatchedJudgmentPlacementsRuntime>> getCoreGameEngineMatchedJudgmentPlacementsRuntime) : ISuitPreventionAndJudgmentPlacementHost
{
    public SkillProgramStepOutcome DiscardSuitPreventDamageAndBenefit(ProgramSkillFrame f) => getCoreGameEngineSuitDamagePreventionBenefitsRuntime().BeginSuitPreventionBenefit(f);
    public SkillProgramStepOutcome PlaceMatchedJudgmentCard(ProgramSkillFrame f, string bind, IReadOnlyList<Suit> suits) => getCoreGameEngineMatchedJudgmentPlacementsRuntime().Value.BeginMatchedJudgmentPlacement(f, bind, suits);
}
