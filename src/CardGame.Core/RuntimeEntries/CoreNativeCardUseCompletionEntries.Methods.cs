#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardUseCompletionEntries
{
    internal void CompleteFinishedAttackCardUse(NativeOwners.CardAttackHandle attack) => _resolve().CompleteFinishedAttackCardUse(attack);
    internal NativeUseDiscardEntry? DiscardFinishedUseMaterial(Card card) => _resolve().DiscardFinishedUseMaterial(card);
    internal void FinishCardUse(long frameId, Card card, CardKind? playedCardKind = default) => _resolve().FinishCardUse(frameId, card, playedCardKind);
    internal void PopFinishedCardUse(long frameId) => _resolve().PopFinishedCardUse(frameId);
    internal void QueueCardUseFinishedWithoutPop(long frameId, Card card, CardKind? playedCardKind) => _resolve().QueueCardUseFinishedWithoutPop(frameId, card, playedCardKind);
    internal void StoreFinishedUseDiscards(long frameId, List<NativeUseDiscardEntry>? finished) => _resolve().StoreFinishedUseDiscards(frameId, finished);
}
