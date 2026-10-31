#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualEndedTurnEquipmentEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ActualEndedEquipmentChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ActualEndedEquipmentChoices(f);
    internal global::CardGame.Core.ActualEndedTurnEquipmentContext? CaptureActualEndedEquipmentContext(global::CardGame.Core.DeferredTurnEndFrame parent) => _resolve().CaptureActualEndedEquipmentContext(parent);
    internal bool IsActualEndedEquipmentTrigger(global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().IsActualEndedEquipmentTrigger(trigger);
    internal bool MatchesActualEndedEquipmentContext(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.SkillProgramTrigger trigger, bool requireLivingTurnOwner = true) => _resolve().MatchesActualEndedEquipmentContext(parent, candidate, context, trigger, requireLivingTurnOwner);
    internal bool MatchesActualEndedEquipmentRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MatchesActualEndedEquipmentRoot(f);
    internal void ObserveActualForeignUseTargets(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveActualForeignUseTargets(payload);
    internal void ResolveActualEndedEquipmentChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().ResolveActualEndedEquipmentChoice(f, choice);
    internal global::CardGame.Core.SkillProgramStepOutcome RunActualEndedTurnEquipment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RunActualEndedTurnEquipment(f);
    internal global::CardGame.Core.PromptChoice SelectAiActualEndedEquipment(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiActualEndedEquipment(decision, f);
    internal bool TracksActualForeignUseTargets { get => _resolve().TracksActualForeignUseTargets; }
}
