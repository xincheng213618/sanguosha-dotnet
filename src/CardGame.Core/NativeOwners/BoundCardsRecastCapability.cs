#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundCardsRecastCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundCardsRecastsEntries> getCoreGameEngineBoundCardsRecastsRuntime) : IBoundCardsRecastHost
{
    public SkillProgramStepOutcome RecastBoundCards(ProgramSkillFrame frame, string sourceBind) => getCoreGameEngineBoundCardsRecastsRuntime().BeginBoundCardsRecast(frame, sourceBind);
}
