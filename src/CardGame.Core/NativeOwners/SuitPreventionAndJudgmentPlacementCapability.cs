#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SuitPreventionAndJudgmentPlacementCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSuitDamagePreventionBenefitsEntries> getCoreGameEngineSuitDamagePreventionBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineMatchedJudgmentPlacementsEntries> getCoreGameEngineMatchedJudgmentPlacementsRuntime) : ISuitPreventionAndJudgmentPlacementHost
{
    public SkillProgramStepOutcome DiscardSuitPreventDamageAndBenefit(ProgramSkillFrame f) => getCoreGameEngineSuitDamagePreventionBenefitsRuntime().BeginSuitPreventionBenefit(f);
    public SkillProgramStepOutcome PlaceMatchedJudgmentCard(ProgramSkillFrame f, string bind, IReadOnlyList<Suit> suits) => getCoreGameEngineMatchedJudgmentPlacementsRuntime().BeginMatchedJudgmentPlacement(f, bind, suits);
}
