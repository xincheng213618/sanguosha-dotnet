#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEphemeralCardMaterialsEntries
{
    internal bool IsEphemeralCardDestroyedMaterial(CardMovementRecord movement) => _resolve().IsEphemeralCardDestroyedMaterial(movement);
    internal bool IsEphemeralCardDestroyedMaterial2(int cardId, CardLocation from, CardLocation to) => _resolve().IsEphemeralCardDestroyedMaterial(cardId, from, to);
    internal CardLocation NormalizeEphemeralCardDestination(Card card, CardLocation from, CardLocation to, CardMoveReason? reason = default) => _resolve().NormalizeEphemeralCardDestination(card, from, to, reason);
}
