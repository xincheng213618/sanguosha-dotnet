#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundCardsRecastCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineBoundCardsRecastsRuntime>> getCoreGameEngineBoundCardsRecastsRuntime) : IBoundCardsRecastHost
{
    public SkillProgramStepOutcome RecastBoundCards(ProgramSkillFrame frame, string sourceBind) => getCoreGameEngineBoundCardsRecastsRuntime().Value.BeginBoundCardsRecast(frame, sourceBind);
}
