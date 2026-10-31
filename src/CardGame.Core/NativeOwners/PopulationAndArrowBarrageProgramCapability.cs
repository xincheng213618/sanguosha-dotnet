#nullable enable
using System.Collections.ObjectModel;

namespace CardGame.Core.NativeOwners;
internal sealed class PopulationAndArrowBarrageProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramPopulationAndArrowBarrageOperationsEntries> getCoreProgramPopulationAndArrowBarrageOperationsRuntime) : IPopulationAndArrowBarrageProgramHost
{
    public void GrantFactionPopulationMarker(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreProgramPopulationAndArrowBarrageOperationsRuntime().GrantProgramFactionPopulationMarker(frame, effect);
    public void RemoveSelectedCurrentArrowBarrageTarget(ProgramSkillFrame frame) => getCoreProgramPopulationAndArrowBarrageOperationsRuntime().RemoveProgramSelectedCurrentArrowBarrageTarget(frame);
}
