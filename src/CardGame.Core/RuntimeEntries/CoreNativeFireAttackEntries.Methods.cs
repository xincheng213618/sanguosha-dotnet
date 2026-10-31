#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeFireAttackEntries
{
    internal void BeginFireAttackDiscard(CardUseFrame pending, Card revealed) => _resolve().BeginFireAttackDiscard(pending, revealed);
    internal void BeginFireAttackReveal(CardUseFrame pending) => _resolve().BeginFireAttackReveal(pending);
    internal Card GetFireAttackEffectCard(CardUseFrame pending) => _resolve().GetFireAttackEffectCard(pending);
    internal Card GetFireAttackRevealedCard(CardUseFrame pending) => _resolve().GetFireAttackRevealedCard(pending);
    internal void ResolveFireAttackDiscard(CardUseFrame pending, int? selectedCardId) => _resolve().ResolveFireAttackDiscard(pending, selectedCardId);
    internal void ResolveFireAttackReveal(CardUseFrame pending, int cardId) => _resolve().ResolveFireAttackReveal(pending, cardId);
    internal CardUseFrame SetFireAttackSelection(long frameId, FireAttackSelectionState? selection) => _resolve().SetFireAttackSelection(frameId, selection);
}
