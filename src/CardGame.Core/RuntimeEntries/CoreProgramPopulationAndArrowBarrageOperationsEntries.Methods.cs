#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPopulationAndArrowBarrageOperationsEntries
{
    internal bool AllowsGrantFactionPopulationMarkerTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsGrantFactionPopulationMarkerTrigger(gate);
    internal bool AllowsRemoveSelectedCurrentArrowBarrageTargetTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsRemoveSelectedCurrentArrowBarrageTargetTrigger(gate);
    internal IReadOnlyList<int> GetCurrentArrowBarrageTargets(int ownerSeat, ProgramSkillWindowContext? context) => _resolve().GetCurrentArrowBarrageTargets(ownerSeat, context);
    internal void GrantProgramFactionPopulationMarker(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().GrantProgramFactionPopulationMarker(frame, effect);
    internal bool HasInitialPopulationMarkerGrant(int ownerSeat, string skillId, ProgramSkillWindowContext context) => _resolve().HasInitialPopulationMarkerGrant(ownerSeat, skillId, context);
    internal void RemoveProgramSelectedCurrentArrowBarrageTarget(ProgramSkillFrame frame) => _resolve().RemoveProgramSelectedCurrentArrowBarrageTarget(frame);
}
