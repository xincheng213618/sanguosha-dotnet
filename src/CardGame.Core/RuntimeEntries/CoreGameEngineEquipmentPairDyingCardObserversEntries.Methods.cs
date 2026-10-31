#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentPairDyingCardObserversEntries
{
    internal bool DyingOwnedCardCostIsPaid(ProgramSkillFrame root) => _resolve().DyingOwnedCardCostIsPaid(root);
    internal ProgramSkillFrame? DyingOwnedCardObserverRoot(long? originalDyingId = default) => _resolve().DyingOwnedCardObserverRoot(originalDyingId);
    internal bool EquipmentPairDyingCardChildEdge(int index) => _resolve().EquipmentPairDyingCardChildEdge(index);
    internal bool EquipmentPairDyingCardSuffix(int rootIndex) => _resolve().EquipmentPairDyingCardSuffix(rootIndex);
    internal ProgramSkillFrame? EquipmentPairPaymentObserverRoot() => _resolve().EquipmentPairPaymentObserverRoot();
    internal bool IsEquipmentPairOrDyingCardProgramDying() => _resolve().IsEquipmentPairOrDyingCardProgramDying();
    internal bool PaymentRemovedSilverLion(ProgramSkillFrame root, string reason, long before, int? ownerSeat = default) => _resolve().PaymentRemovedSilverLion(root, reason, before, ownerSeat);
}
