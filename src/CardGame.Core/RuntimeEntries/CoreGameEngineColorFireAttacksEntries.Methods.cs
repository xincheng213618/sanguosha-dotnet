#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineColorFireAttacksEntries
{
    internal void AssertColorFireAttackReceipts() => _resolve().AssertColorFireAttackReceipts();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> ColorFireAttackCosts(global::CardGame.Core.CardUseFrame pending) => _resolve().ColorFireAttackCosts(pending);
    internal void CompleteColorFireAttackWithoutDamage(global::CardGame.Core.CardUseFrame pending, bool publishFailure = true) => _resolve().CompleteColorFireAttackWithoutDamage(pending, publishFailure);
    internal void ContinueColorFireAttackPayment(long useId) => _resolve().ContinueColorFireAttackPayment(useId);
    internal global::CardGame.Core.ColorFireAttackReceipt? FreezeColorFireAttackPolicy(int sourceSeat, global::CardGame.Core.CardActionContext? action) => _resolve().FreezeColorFireAttackPolicy(sourceSeat, action);
    internal bool TryBeginColorFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.Card revealed) => _resolve().TryBeginColorFireAttackDiscard(pending, revealed);
    internal bool TryBeginColorFireAttackReveal(global::CardGame.Core.CardUseFrame pending) => _resolve().TryBeginColorFireAttackReveal(pending);
    internal bool TryResolveAiColorFireAttack(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.PendingDecision decision) => _resolve().TryResolveAiColorFireAttack(pending, decision);
    internal bool TryResolveColorFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId) => _resolve().TryResolveColorFireAttackDiscard(pending, selectedCardId);
}
