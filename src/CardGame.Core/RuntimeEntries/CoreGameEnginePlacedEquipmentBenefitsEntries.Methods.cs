#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitsEntries
{
    internal SkillProgramStepOutcome BeginPlacedEquipmentBenefit(ProgramSkillFrame supplied) => _resolve().BeginPlacedEquipmentBenefit(supplied);
    internal bool CanPlaceBenefitEquipment(int owner, Card card, CardLocation from) => _resolve().CanPlaceBenefitEquipment(owner, card, from);
    internal bool CanRunPlacedEquipmentBenefit(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanRunPlacedEquipmentBenefit(c, t, context);
    internal void ContinuePlacedEquipmentMovement(ProgramSkillFrame f) => _resolve().ContinuePlacedEquipmentMovement(f);
    internal void FinishPlacedEquipmentBenefit(ProgramSkillFrame f) => _resolve().FinishPlacedEquipmentBenefit(f);
    internal Dictionary<string, string> PlacedEquipmentArgs(ProgramSkillFrame f, string step) => _resolve().PlacedEquipmentArgs(f, step);
    internal IReadOnlyList<PromptChoice> PlacedEquipmentChoices(ProgramSkillFrame f) => _resolve().PlacedEquipmentChoices(f);
    internal bool PlacedEquipmentParentMatches(ProgramSkillFrame f) => _resolve().PlacedEquipmentParentMatches(f);
    internal bool PlacedEquipmentSourceCurrent(ProgramSkillFrame f) => _resolve().PlacedEquipmentSourceCurrent(f);
    internal void PublishPlacedEquipmentChoice(ProgramSkillFrame f) => _resolve().PublishPlacedEquipmentChoice(f);
    internal void ResolvePlacedEquipmentChoice(PromptChoice selected) => _resolve().ResolvePlacedEquipmentChoice(selected);
    internal bool ResumePlacedEquipmentBenefit(long id) => _resolve().ResumePlacedEquipmentBenefit(id);
    internal bool ReturnPlacedEquipmentMovement(ProgramSkillFrame f) => _resolve().ReturnPlacedEquipmentMovement(f);
    internal PromptChoice SelectAiPlacedEquipmentBenefit(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiPlacedEquipmentBenefit(decision, f);
}
