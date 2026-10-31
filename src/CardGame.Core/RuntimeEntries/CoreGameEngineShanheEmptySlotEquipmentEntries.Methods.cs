#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheEmptySlotEquipmentEntries
{
    internal bool CanOfferShanheEmptySlotEquipment(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheEmptySlotEquipment(candidate, trigger, context);
    internal SkillProgramStepOutcome PlaceShanheEmptySlotEquipment(ProgramSkillFrame supplied, bool armorOnly) => _resolve().PlaceShanheEmptySlotEquipment(supplied, armorOnly);
    internal Card[] ShanheEmptySlotEquipmentCandidates(int ownerSeat, bool armorOnly = false) => _resolve().ShanheEmptySlotEquipmentCandidates(ownerSeat, armorOnly);
}
