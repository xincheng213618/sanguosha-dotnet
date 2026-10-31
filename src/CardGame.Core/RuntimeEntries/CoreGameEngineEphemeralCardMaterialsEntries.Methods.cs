#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEphemeralCardMaterialsEntries
{
    internal bool IsEphemeralCardDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement) => _resolve().IsEphemeralCardDestroyedMaterial(movement);
    internal bool IsEphemeralCardDestroyedMaterial2(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().IsEphemeralCardDestroyedMaterial(cardId, from, to);
    internal bool IsEphemeralCardDestroyedMaterial3(long useId, int cardId) => _resolve().IsEphemeralCardDestroyedMaterial(useId, cardId);
    internal global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default) => _resolve().NormalizeEphemeralCardDestination(card, from, to, reason);
}
