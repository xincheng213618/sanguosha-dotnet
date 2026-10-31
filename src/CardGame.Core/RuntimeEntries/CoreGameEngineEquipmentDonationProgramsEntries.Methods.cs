#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentDonationProgramsEntries
{
    internal SkillProgramStepOutcome AwaitEquipmentDonationMovements(long frameId) => _resolve().AwaitEquipmentDonationMovements(frameId);
    internal IReadOnlyList<PromptChoice> EquipmentDonationChoices(ProgramSkillFrame f) => _resolve().EquipmentDonationChoices(f);
    internal Dictionary<string, string> EquipmentDonationParameters(ProgramSkillFrame f, string action, string option) => _resolve().EquipmentDonationParameters(f, action, option);
    internal bool EquipmentDonationSourceValid(ProgramSkillFrame f) => _resolve().EquipmentDonationSourceValid(f);
    internal bool HasPayableAllEquipmentDonation(int ownerSeat, string skillId, string usageId) => _resolve().HasPayableAllEquipmentDonation(ownerSeat, skillId, usageId);
    internal void PublishEquipmentDonationPrompt(ProgramSkillFrame f, int chooser, string text, IReadOnlyList<PromptChoice> choices) => _resolve().PublishEquipmentDonationPrompt(f, chooser, text, choices);
    internal void ResolveEquipmentDonationChoice(ProgramSkillFrame f, PromptChoice choice) => _resolve().ResolveEquipmentDonationChoice(f, choice);
    internal SkillProgramStepOutcome RunAllEquipmentDonation(ProgramSkillFrame frame, string usageId) => _resolve().RunAllEquipmentDonation(frame, usageId);
    internal PromptChoice SelectAiEquipmentDonation(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiEquipmentDonation(decision, f);
    internal long EquipmentDonationSequence { get => _resolve().EquipmentDonationSequence; }
}
