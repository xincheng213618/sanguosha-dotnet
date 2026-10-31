#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentDonationProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome AwaitEquipmentDonationMovements(long frameId) => _resolve().AwaitEquipmentDonationMovements(frameId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> EquipmentDonationChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().EquipmentDonationChoices(f);
    internal global::System.Collections.Generic.Dictionary<string, string> EquipmentDonationParameters(global::CardGame.Core.ProgramSkillFrame f, string action, string option) => _resolve().EquipmentDonationParameters(f, action, option);
    internal bool EquipmentDonationSourceValid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().EquipmentDonationSourceValid(f);
    internal bool HasPayableAllEquipmentDonation(int ownerSeat, string skillId, string usageId) => _resolve().HasPayableAllEquipmentDonation(ownerSeat, skillId, usageId);
    internal void PublishEquipmentDonationPrompt(global::CardGame.Core.ProgramSkillFrame f, int chooser, string text, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishEquipmentDonationPrompt(f, chooser, text, choices);
    internal void ResolveEquipmentDonationChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().ResolveEquipmentDonationChoice(f, choice);
    internal global::CardGame.Core.SkillProgramStepOutcome RunAllEquipmentDonation(global::CardGame.Core.ProgramSkillFrame frame, string usageId) => _resolve().RunAllEquipmentDonation(frame, usageId);
    internal global::CardGame.Core.PromptChoice SelectAiEquipmentDonation(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiEquipmentDonation(decision, f);
    internal long EquipmentDonationSequence { get => _resolve().EquipmentDonationSequence; }
}
