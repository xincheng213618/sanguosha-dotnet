#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardUseCompletionEntries
{
    internal void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CompleteFinishedAttackCardUse(attack);
    internal void FinishCardUse(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => _resolve().FinishCardUse(frameId, card, playedCardKind);
    internal void PopFinishedCardUse(long frameId) => _resolve().PopFinishedCardUse(frameId);
    internal void QueueCardUseFinishedWithoutPop(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind) => _resolve().QueueCardUseFinishedWithoutPop(frameId, card, playedCardKind);
}
