#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramAtomicCardMovesEntries
{
    internal bool IsRedirectedPaidDiscardMovement(CardMovementRecord move, CardLocation expectedSource, CardMoveReason expectedReason) => _resolve().IsRedirectedPaidDiscardMovement(move, expectedSource, expectedReason);
    internal void MoveProgramCardsFromMultipleSources(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason, Action<long, IReadOnlyList<CardMovementRecord>>? afterPhysicalRecords = default, Func<Card, CardLocation, CardLocation>? destinationForCard = default, CardMovementDisclosure disclosure = (CardMovementDisclosure)0, Action<HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default) => _resolve().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, destinationForCard, disclosure, afterRequestSettled, producerFrameId);
}
