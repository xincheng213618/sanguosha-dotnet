#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundCardsRecastCapability(Func<RuntimeEntries.CoreGameEngineBoundCardsRecastsEntries> getCoreGameEngineBoundCardsRecastsRuntime) : IBoundCardsRecastHost
{
    public SkillProgramStepOutcome RecastBoundCards(ProgramSkillFrame frame, string sourceBind) => getCoreGameEngineBoundCardsRecastsRuntime().BeginBoundCardsRecast(frame, sourceBind);
}
