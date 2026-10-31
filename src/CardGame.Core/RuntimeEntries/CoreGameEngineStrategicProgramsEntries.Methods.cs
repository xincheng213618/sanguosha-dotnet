#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineStrategicProgramsEntries
{
    internal bool AllowsSuppressGeneralSkillTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsSuppressGeneralSkillTrigger(gate);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> CollectStrategicRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().CollectStrategicRuleContributions(player, query);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteStrategicProgramEffect(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => _resolve().ExecuteStrategicProgramEffect(effect, frame, targetSeat);
    internal void ExpireProgramSuppressions(int turnOwnerSeat) => _resolve().ExpireProgramSuppressions(turnOwnerSeat);
    internal global::System.Collections.Generic.IReadOnlyDictionary<string, int>? GetProgramPublicCounters(global::CardGame.Core.CharacterState player) => _resolve().GetProgramPublicCounters(player);
    internal global::System.Collections.Generic.IReadOnlyDictionary<string, bool>? GetPublicProgramBooleanStates(global::CardGame.Core.CharacterState player) => _resolve().GetPublicProgramBooleanStates(player);
    internal void PublishStrategicPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishStrategicPrompt(frame);
    internal void ResetPlayPhaseProgramStates(int ownerSeat) => _resolve().ResetPlayPhaseProgramStates(ownerSeat);
    internal void ResolveStrategicProgramChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveStrategicProgramChoice(choice);
    internal void SetStrategicMarker(int seat, global::CardGame.Core.PlayerMarkerKind marker, int sourceSeat, int count, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SetStrategicMarker(seat, marker, sourceSeat, count, frame);
    internal bool UsesStrategicTriggerValue(global::CardGame.Core.SkillProgramTriggerValueKind kind) => _resolve().UsesStrategicTriggerValue(kind);
}
