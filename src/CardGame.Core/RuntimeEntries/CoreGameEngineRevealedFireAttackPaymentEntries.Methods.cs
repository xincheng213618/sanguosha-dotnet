#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRevealedFireAttackPaymentEntries
{
    internal void CompleteNativeFireAttackPayment(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.Card revealed, global::CardGame.Core.Card payment, bool revealInstead) => _resolve().CompleteNativeFireAttackPayment(pending, revealed, payment, revealInstead);
    internal void ContinueRevealedFireAttackPayment(global::CardGame.Core.CardUseFrame use) => _resolve().ContinueRevealedFireAttackPayment(use);
    internal bool FireAttackPaymentIsReveal(int seat) => _resolve().FireAttackPaymentIsReveal(seat);
    internal bool RefreshRevealedFireAttackPaymentPrompt() => _resolve().RefreshRevealedFireAttackPaymentPrompt();
}
