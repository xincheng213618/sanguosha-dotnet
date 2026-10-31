#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PopulationAndArrowBarrageProgramCapability(Func<RuntimeEntries.CoreProgramPopulationAndArrowBarrageOperationsEntries> getCoreProgramPopulationAndArrowBarrageOperationsRuntime) : IPopulationAndArrowBarrageProgramHost
{
    public void GrantFactionPopulationMarker(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreProgramPopulationAndArrowBarrageOperationsRuntime().GrantProgramFactionPopulationMarker(frame, effect);
    public void RemoveSelectedCurrentArrowBarrageTarget(ProgramSkillFrame frame) => getCoreProgramPopulationAndArrowBarrageOperationsRuntime().RemoveProgramSelectedCurrentArrowBarrageTarget(frame);
}
