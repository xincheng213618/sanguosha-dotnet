#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineStrategicProgramsEntries
{
    internal bool AllowsSuppressGeneralSkillTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsSuppressGeneralSkillTrigger(gate);
    internal IEnumerable<RuleQueryContribution> CollectStrategicRuleContributions(CharacterState player, SkillRuleQuery query) => _resolve().CollectStrategicRuleContributions(player, query);
    internal SkillProgramStepOutcome ExecuteStrategicProgramEffect(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat) => _resolve().ExecuteStrategicProgramEffect(effect, frame, targetSeat);
    internal void ExpireProgramSuppressions(int turnOwnerSeat) => _resolve().ExpireProgramSuppressions(turnOwnerSeat);
    internal IReadOnlyDictionary<string, int>? GetProgramPublicCounters(CharacterState player) => _resolve().GetProgramPublicCounters(player);
    internal IReadOnlyDictionary<string, bool>? GetPublicProgramBooleanStates(CharacterState player) => _resolve().GetPublicProgramBooleanStates(player);
    internal void PublishStrategicPrompt(ProgramSkillFrame frame) => _resolve().PublishStrategicPrompt(frame);
    internal void ResetPlayPhaseProgramStates(int ownerSeat) => _resolve().ResetPlayPhaseProgramStates(ownerSeat);
    internal void ResolveStrategicProgramChoice(PromptChoice choice) => _resolve().ResolveStrategicProgramChoice(choice);
    internal void SetStrategicMarker(int seat, PlayerMarkerKind marker, int sourceSeat, int count, ProgramSkillFrame frame) => _resolve().SetStrategicMarker(seat, marker, sourceSeat, count, frame);
    internal bool UsesStrategicTriggerValue(SkillProgramTriggerValueKind kind) => _resolve().UsesStrategicTriggerValue(kind);
}
