#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlacedEquipmentBenefitsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPlacedEquipmentBenefit(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginPlacedEquipmentBenefit(supplied);
    internal bool CanPlaceBenefitEquipment(int owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().CanPlaceBenefitEquipment(owner, card, from);
    internal bool CanRunPlacedEquipmentBenefit(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunPlacedEquipmentBenefit(c, t, context);
    internal void ContinuePlacedEquipmentMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePlacedEquipmentMovement(f);
    internal void FinishPlacedEquipmentBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishPlacedEquipmentBenefit(f);
    internal global::System.Collections.Generic.Dictionary<string, string> PlacedEquipmentArgs(global::CardGame.Core.ProgramSkillFrame f, string step) => _resolve().PlacedEquipmentArgs(f, step);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PlacedEquipmentChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PlacedEquipmentChoices(f);
    internal bool PlacedEquipmentParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PlacedEquipmentParentMatches(f);
    internal bool PlacedEquipmentSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PlacedEquipmentSourceCurrent(f);
    internal void PublishPlacedEquipmentChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPlacedEquipmentChoice(f);
    internal void ResolvePlacedEquipmentChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolvePlacedEquipmentChoice(selected);
    internal bool ResumePlacedEquipmentBenefit(long id) => _resolve().ResumePlacedEquipmentBenefit(id);
    internal bool ReturnPlacedEquipmentMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPlacedEquipmentMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiPlacedEquipmentBenefit(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiPlacedEquipmentBenefit(decision, f);
}
