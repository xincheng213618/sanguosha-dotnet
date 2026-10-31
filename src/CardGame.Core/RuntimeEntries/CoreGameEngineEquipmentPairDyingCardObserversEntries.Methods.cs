#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentPairDyingCardObserversEntries
{
    internal bool DyingOwnedCardCostIsPaid(global::CardGame.Core.ProgramSkillFrame root) => _resolve().DyingOwnedCardCostIsPaid(root);
    internal global::CardGame.Core.ProgramSkillFrame? DyingOwnedCardObserverRoot(long? originalDyingId = default) => _resolve().DyingOwnedCardObserverRoot(originalDyingId);
    internal bool EquipmentPairDyingCardChildEdge(int index) => _resolve().EquipmentPairDyingCardChildEdge(index);
    internal bool EquipmentPairDyingCardSuffix(int rootIndex) => _resolve().EquipmentPairDyingCardSuffix(rootIndex);
    internal global::CardGame.Core.ProgramSkillFrame? EquipmentPairPaymentObserverRoot() => _resolve().EquipmentPairPaymentObserverRoot();
    internal bool IsEquipmentPairOrDyingCardProgramDying() => _resolve().IsEquipmentPairOrDyingCardProgramDying();
    internal bool IsExactDyingOwnedCardRide(long dyingId) => _resolve().IsExactDyingOwnedCardRide(dyingId);
    internal bool PaymentRemovedSilverLion(global::CardGame.Core.ProgramSkillFrame root, string reason, long before, int? ownerSeat = default) => _resolve().PaymentRemovedSilverLion(root, reason, before, ownerSeat);
}
