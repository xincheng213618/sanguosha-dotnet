#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualEndedTurnEquipmentEntries
{
    internal IReadOnlyList<PromptChoice> ActualEndedEquipmentChoices(ProgramSkillFrame f) => _resolve().ActualEndedEquipmentChoices(f);
    internal ActualEndedTurnEquipmentContext? CaptureActualEndedEquipmentContext(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CaptureActualEndedEquipmentContext(parent, candidate, context);
    internal bool IsActualEndedEquipmentTrigger(SkillProgramTrigger trigger) => _resolve().IsActualEndedEquipmentTrigger(trigger);
    internal bool MatchesActualEndedEquipmentContext(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, SkillProgramTrigger trigger, bool requireLivingTurnOwner = true) => _resolve().MatchesActualEndedEquipmentContext(parent, candidate, context, trigger, requireLivingTurnOwner);
    internal bool MatchesActualEndedEquipmentRoot(ProgramSkillFrame f) => _resolve().MatchesActualEndedEquipmentRoot(f);
    internal void ResolveActualEndedEquipmentChoice(ProgramSkillFrame f, PromptChoice choice) => _resolve().ResolveActualEndedEquipmentChoice(f, choice);
    internal SkillProgramStepOutcome RunActualEndedTurnEquipment(ProgramSkillFrame f) => _resolve().RunActualEndedTurnEquipment(f);
    internal PromptChoice SelectAiActualEndedEquipment(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiActualEndedEquipment(decision, f);
}
