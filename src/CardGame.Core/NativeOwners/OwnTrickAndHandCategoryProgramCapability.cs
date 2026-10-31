#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OwnTrickAndHandCategoryProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnTrickDrawTargetsEntries> getCoreGameEngineOwnTrickDrawTargetsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime) : IOwnTrickAndHandCategoryProgramHost
{
    public SkillProgramStepOutcome DrawThenNullifyOwnMultiTargetTrick(ProgramSkillFrame f) => getCoreGameEngineOwnTrickDrawTargetsRuntime().BeginOwnTrickDraw(f);
    public void RestrictDamageSourceHandCategory(ProgramSkillFrame f, SkillProgramCardCategory category) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().GrantDamageSourceHandCategory(f, category);
}
