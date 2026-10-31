#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentDonationObserversEntries
{
    internal bool AllowsDonateAllEquipmentAndOfferRecipientBenefitsActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsDonateAllEquipmentAndOfferRecipientBenefitsActivation(gate);
    internal bool EquipmentDonationInitialObserverEdge(int rootIndex, global::CardGame.Core.ProgramSkillFrame root) => _resolve().EquipmentDonationInitialObserverEdge(rootIndex, root);
    internal global::CardGame.Core.ProgramSkillFrame? EquipmentDonationObserverRoot(long rootId) => _resolve().EquipmentDonationObserverRoot(rootId);
    internal bool HasEquipmentDonationDamageObserver(long? damageWindowId = default) => _resolve().HasEquipmentDonationDamageObserver(damageWindowId);
    internal bool IsEquipmentDonationAttack(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsEquipmentDonationAttack(f);
    internal bool IsEquipmentDonationProgramDying() => _resolve().IsEquipmentDonationProgramDying();
    internal bool IsValidActualEndedEquipment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsValidActualEndedEquipment(f);
    internal bool IsValidAllEquipmentDonation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsValidAllEquipmentDonation(f);
}
