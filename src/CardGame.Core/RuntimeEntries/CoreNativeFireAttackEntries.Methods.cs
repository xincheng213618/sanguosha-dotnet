#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeFireAttackEntries
{
    internal void BeginFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.Card revealed) => _resolve().BeginFireAttackDiscard(pending, revealed);
    internal void BeginFireAttackReveal(global::CardGame.Core.CardUseFrame pending) => _resolve().BeginFireAttackReveal(pending);
    internal global::CardGame.Core.Card GetFireAttackEffectCard(global::CardGame.Core.CardUseFrame pending) => _resolve().GetFireAttackEffectCard(pending);
    internal global::CardGame.Core.Card GetFireAttackRevealedCard(global::CardGame.Core.CardUseFrame pending) => _resolve().GetFireAttackRevealedCard(pending);
    internal void ResolveFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId) => _resolve().ResolveFireAttackDiscard(pending, selectedCardId);
    internal void ResolveFireAttackReveal(global::CardGame.Core.CardUseFrame pending, int cardId) => _resolve().ResolveFireAttackReveal(pending, cardId);
    internal global::CardGame.Core.CardUseFrame SetFireAttackSelection(long frameId, global::CardGame.Core.FireAttackSelectionState? selection) => _resolve().SetFireAttackSelection(frameId, selection);
}
