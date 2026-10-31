#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangShopCardsEntries
{
    internal global::CardGame.Core.CardMovementRecord? DestroyedEquipmentResponsePayment(global::CardGame.Core.CardActionContext action, long windowId, global::CardGame.Core.CardActionCost material, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().DestroyedEquipmentResponsePayment(action, windowId, material, continuation);
    internal bool IsWushuangShopDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement) => _resolve().IsWushuangShopDestroyedMaterial(movement);
    internal bool IsWushuangShopDestroyedNullificationPayment(global::CardGame.Core.CardActionContext action, long windowId, global::CardGame.Core.CardActionCost material) => _resolve().IsWushuangShopDestroyedNullificationPayment(action, windowId, material);
    internal bool IsWushuangShopDestroyedNullificationPayment2(long windowId, int provider, global::CardGame.Core.CardActionCost material, int afterSequence = -1) => _resolve().IsWushuangShopDestroyedNullificationPayment(windowId, provider, material, afterSequence);
    internal bool IsWushuangShopEquipment(int cardId) => _resolve().IsWushuangShopEquipment(cardId);
    internal global::CardGame.Core.CardLocation NormalizeWushuangShopEquipmentDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().NormalizeWushuangShopEquipmentDestination(card, from, to);
    internal void PlaceWushuangShopCard(int seat, string offerId, global::CardGame.Core.WushuangCard definition) => _resolve().PlaceWushuangShopCard(seat, offerId, definition);
    internal void RecordWushuangShopEquipmentDestruction(global::CardGame.Core.CardMovementRecord movement) => _resolve().RecordWushuangShopEquipmentDestruction(movement);
}
