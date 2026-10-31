#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PopulationAndArrowBarrageProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreProgramPopulationAndArrowBarrageOperationsRuntime>> getCoreProgramPopulationAndArrowBarrageOperationsRuntime) : IPopulationAndArrowBarrageProgramHost
{
    public void GrantFactionPopulationMarker(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreProgramPopulationAndArrowBarrageOperationsRuntime().Value.GrantProgramFactionPopulationMarker(frame, effect);
    public void RemoveSelectedCurrentArrowBarrageTarget(ProgramSkillFrame frame) => getCoreProgramPopulationAndArrowBarrageOperationsRuntime().Value.RemoveProgramSelectedCurrentArrowBarrageTarget(frame);
}
