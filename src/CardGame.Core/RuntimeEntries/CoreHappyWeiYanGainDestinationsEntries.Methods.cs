#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHappyWeiYanGainDestinationsEntries
{
    internal bool IsHappyWeiYanGainDestinationPlan(HappyWeiYanGainDestinationPlan plan) => _resolve().IsHappyWeiYanGainDestinationPlan(plan);
    internal bool IsHappyWeiYanRedirectedGain(CardMovementRecord movement, CardLocation expectedTo) => _resolve().IsHappyWeiYanRedirectedGain(movement, expectedTo);
    internal bool IsHappyWeiYanRetainedGainSettlement(HappyWeiYanRetainedGainSettlement settlement, long producerFrameId, int cardId, CardLocation requestedTo, long before) => _resolve().IsHappyWeiYanRetainedGainSettlement(settlement, producerFrameId, cardId, requestedTo, before);
    internal void MoveNativeGivenCards(long producerFrameId, int giver, int recipient, IReadOnlyList<int> cardIds, CardMoveReason reason, Action<HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, CardMovementDisclosure disclosure = (CardMovementDisclosure)0) => _resolve().MoveNativeGivenCards(producerFrameId, giver, recipient, cardIds, reason, afterRequestSettled, disclosure);
    internal HappyWeiYanGainDestinationPlan? PlanHappyWeiYanGainDestination(Card card, CardLocation from, CardLocation requestedTo, CardMoveReason reason) => _resolve().PlanHappyWeiYanGainDestination(card, from, requestedTo, reason);
    internal HappyWeiYanRetainedGainSettlement SettleHappyWeiYanRetainedGain(HappyWeiYanGainDestinationPlan plan, long? producerFrameId = default) => _resolve().SettleHappyWeiYanRetainedGain(plan, producerFrameId);
    internal bool TrySettleHappyWeiYanRetainedGain(Card card, CardLocation from, CardLocation requestedTo, CardMoveReason reason, long? producerFrameId = default) => _resolve().TrySettleHappyWeiYanRetainedGain(card, from, requestedTo, reason, producerFrameId);
    internal bool TrySettleHappyWeiYanRetainedGain2(HappyWeiYanGainDestinationPlan plan, long? producerFrameId = default) => _resolve().TrySettleHappyWeiYanRetainedGain(plan, producerFrameId);
    internal void ValidateHappyWeiYanGainDestinationRecord(CardMovementRecord movement) => _resolve().ValidateHappyWeiYanGainDestinationRecord(movement);
    internal void ValidateHappyWeiYanGainRequestSettlement(HappyWeiYanGainRequestSettlement settlement, long producerFrameId, IReadOnlyList<int> intendedIds, CardLocation requestedTo, long before) => _resolve().ValidateHappyWeiYanGainRequestSettlement(settlement, producerFrameId, intendedIds, requestedTo, before);
}
