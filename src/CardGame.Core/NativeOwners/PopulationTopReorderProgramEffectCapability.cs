#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PopulationTopReorderProgramEffectCapability(Func<RuntimeEntries.CoreGameEngineProgramTopReorderEntries> getCoreGameEngineProgramTopReorderRuntime) : IPopulationTopReorderProgramEffectHost
{
    public SkillProgramStepOutcome ReorderTopCardsByPopulation(ProgramSkillFrame frame, int maximumCards, ProgramPopulationThresholdCount population, string? allBottomStateId) => getCoreGameEngineProgramTopReorderRuntime().BeginProgramTopReorder(frame, maximumCards, null, population: population, allBottomStateId: allBottomStateId);
}
