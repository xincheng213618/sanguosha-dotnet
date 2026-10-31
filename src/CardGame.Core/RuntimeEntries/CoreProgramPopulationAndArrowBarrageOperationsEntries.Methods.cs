#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPopulationAndArrowBarrageOperationsEntries
{
    internal bool AllowsGrantFactionPopulationMarkerTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsGrantFactionPopulationMarkerTrigger(gate);
    internal bool AllowsRemoveSelectedCurrentArrowBarrageTargetTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsRemoveSelectedCurrentArrowBarrageTargetTrigger(gate);
    internal global::System.Collections.Generic.IReadOnlyList<int> GetCurrentArrowBarrageTargets(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().GetCurrentArrowBarrageTargets(ownerSeat, context);
    internal void GrantProgramFactionPopulationMarker(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().GrantProgramFactionPopulationMarker(frame, effect);
    internal bool HasInitialPopulationMarkerGrant(int ownerSeat, string skillId, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasInitialPopulationMarkerGrant(ownerSeat, skillId, context);
    internal void RemoveProgramSelectedCurrentArrowBarrageTarget(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RemoveProgramSelectedCurrentArrowBarrageTarget(frame);
}
