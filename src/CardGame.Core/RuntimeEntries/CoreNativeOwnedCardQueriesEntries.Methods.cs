#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeOwnedCardQueriesEntries
{
    internal global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => _resolve().FindOwnedCardLocation(actor, card);
    internal global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId) => _resolve().FindOwnedPlayableCard(actor, cardId);
    internal void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind) => _resolve().MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, effectiveCardKind);
}
