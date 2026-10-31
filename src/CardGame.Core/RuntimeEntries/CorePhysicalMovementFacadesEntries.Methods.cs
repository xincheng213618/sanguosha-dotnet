#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePhysicalMovementFacadesEntries
{
    internal void ClearJudgmentEffectiveKindAfterMove(Card card, CardLocation from, CardLocation to) => _resolve().ClearJudgmentEffectiveKindAfterMove(card, from, to);
    internal void MoveAllCards(CardLocation from, CardLocation to, CardMoveReason reason, CardMovementDisclosure disclosure = (CardMovementDisclosure)0) => _resolve().MoveAllCards(from, to, reason, disclosure);
    internal void MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, CardMovementDisclosure disclosure = (CardMovementDisclosure)0) => _resolve().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
    internal void MoveCards(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, CardMovementDisclosure disclosure = (CardMovementDisclosure)0) => _resolve().MoveCards(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
    internal CardMovementRecord RecordMovement(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = default, CardKind? adjacentDiscardIdentity = default, CardMovementDisclosure disclosure = (CardMovementDisclosure)0, HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = default, HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = default) => _resolve().RecordMovement(card, from, to, reason, beforeFact, adjacentDiscardIdentity, disclosure, happyWeiYanGainDestination, happyZuoCiPhantomDestination);
    internal void ResolveSilverLionRemoval(Card card, CardLocation from, CardMoveReason reason) => _resolve().ResolveSilverLionRemoval(card, from, reason);
    internal void ResolveWoodenOxMove(Card card, CardLocation from, CardLocation to) => _resolve().ResolveWoodenOxMove(card, from, to);
}
