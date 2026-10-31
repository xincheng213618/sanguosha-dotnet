#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentDonationObserversEntries
{
    internal bool AllowsDonateAllEquipmentAndOfferRecipientBenefitsActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsDonateAllEquipmentAndOfferRecipientBenefitsActivation(gate);
    internal bool EquipmentDonationInitialObserverEdge(int rootIndex, ProgramSkillFrame root) => _resolve().EquipmentDonationInitialObserverEdge(rootIndex, root);
    internal ProgramSkillFrame? EquipmentDonationObserverRoot(long rootId) => _resolve().EquipmentDonationObserverRoot(rootId);
    internal bool HasEquipmentDonationDamageObserver(long? damageWindowId = default) => _resolve().HasEquipmentDonationDamageObserver(damageWindowId);
    internal bool IsEquipmentDonationAttack(ProgramSkillFrame f) => _resolve().IsEquipmentDonationAttack(f);
    internal bool IsEquipmentDonationProgramDying() => _resolve().IsEquipmentDonationProgramDying();
    internal bool IsValidActualEndedEquipment(ProgramSkillFrame f) => _resolve().IsValidActualEndedEquipment(f);
    internal bool IsValidAllEquipmentDonation(ProgramSkillFrame f) => _resolve().IsValidAllEquipmentDonation(f);
}
