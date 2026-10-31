#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheEmptySlotEquipmentEntries
{
    internal bool CanOfferShanheEmptySlotEquipment(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheEmptySlotEquipment(candidate, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome PlaceShanheEmptySlotEquipment(global::CardGame.Core.ProgramSkillFrame supplied, bool armorOnly) => _resolve().PlaceShanheEmptySlotEquipment(supplied, armorOnly);
    internal global::CardGame.Core.Card[] ShanheEmptySlotEquipmentCandidates(int ownerSeat, bool armorOnly = false) => _resolve().ShanheEmptySlotEquipmentCandidates(ownerSeat, armorOnly);
}
