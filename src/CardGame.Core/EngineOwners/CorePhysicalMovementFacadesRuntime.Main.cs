#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePhysicalMovementFacadesRuntime
{
    internal void MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, bool emitCardMovedEvents = true, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified) => _coreAtomicCardMovementOperations.AtomicCardMovements.Move(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
    internal void MoveCards(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, bool emitCardMovedEvents = true, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified) => _coreAtomicCardMovementOperations.AtomicCardMovements.MoveMany(cards, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
    internal void MoveAllCards(CardLocation from, CardLocation to, CardMoveReason reason, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified) => _coreAtomicCardMovementOperations.AtomicCardMovements.MoveAll(from, to, reason, disclosure);
    internal void ResolveWoodenOxMove(Card card, CardLocation from, CardLocation to) => _movementEquipmentEffectsOperations.Get().ResolveStorageMove(card, from, to);
    internal void ResolveSilverLionRemoval(Card card, CardLocation from, CardMoveReason reason) => _movementEquipmentEffectsOperations.Get().ResolveArmorRemoval(card, from, reason);
    internal void ClearJudgmentEffectiveKindAfterMove(Card card, CardLocation from, CardLocation to) => _movementEquipmentEffectsOperations.Get().ClearJudgmentIdentity(card, from, to);
    internal CardMovementRecord RecordMovement(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, CardKind? adjacentDiscardIdentity = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified, HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = null, HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = null) => _movementRecordOperations.Get().Record(card, from, to, reason, beforeFact, adjacentDiscardIdentity, disclosure, happyWeiYanGainDestination, happyZuoCiPhantomDestination);
}
