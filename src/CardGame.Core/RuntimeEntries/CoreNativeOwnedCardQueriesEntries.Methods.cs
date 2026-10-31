#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeOwnedCardQueriesEntries
{
    internal CardLocation FindOwnedCardLocation(CharacterState actor, Card card) => _resolve().FindOwnedCardLocation(actor, card);
    internal Card? FindOwnedPlayableCard(CharacterState actor, int? cardId) => _resolve().FindOwnedPlayableCard(actor, cardId);
    internal void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, CardKind effectiveCardKind) => _resolve().MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, effectiveCardKind);
}
