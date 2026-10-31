#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreForeignEquipmentDiscardPoliciesEntries
{
    internal bool HasDiscardableHeBy(int actorSeat, int ownerSeat) => _resolve().HasDiscardableHeBy(actorSeat, ownerSeat);
    internal bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => _resolve().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
    internal bool HasForeignDiscardCapability { get => _resolve().HasForeignDiscardCapability; }
}
