#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangShopCardsEntries
{
    internal CardMovementRecord? DestroyedEquipmentResponsePayment(CardActionContext action, long windowId, CardActionCost material, ProgramCardContinuation continuation) => _resolve().DestroyedEquipmentResponsePayment(action, windowId, material, continuation);
    internal bool IsWushuangShopDestroyedMaterial(CardMovementRecord movement) => _resolve().IsWushuangShopDestroyedMaterial(movement);
    internal bool IsWushuangShopDestroyedNullificationPayment(CardActionContext action, long windowId, CardActionCost material) => _resolve().IsWushuangShopDestroyedNullificationPayment(action, windowId, material);
    internal bool IsWushuangShopDestroyedNullificationPayment2(long windowId, int provider, CardActionCost material, int afterSequence = -1) => _resolve().IsWushuangShopDestroyedNullificationPayment(windowId, provider, material, afterSequence);
    internal bool IsWushuangShopEquipment(int cardId) => _resolve().IsWushuangShopEquipment(cardId);
    internal CardLocation NormalizeWushuangShopEquipmentDestination(Card card, CardLocation from, CardLocation to) => _resolve().NormalizeWushuangShopEquipmentDestination(card, from, to);
    internal void PlaceWushuangShopCard(int seat, string offerId, WushuangCard definition) => _resolve().PlaceWushuangShopCard(seat, offerId, definition);
    internal void RecordWushuangShopEquipmentDestruction(CardMovementRecord movement) => _resolve().RecordWushuangShopEquipmentDestruction(movement);
}
