#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineColorFireAttacksEntries
{
    internal IReadOnlyList<Card> ColorFireAttackCosts(CardUseFrame pending) => _resolve().ColorFireAttackCosts(pending);
    internal void CompleteColorFireAttackWithoutDamage(CardUseFrame pending, bool publishFailure = true) => _resolve().CompleteColorFireAttackWithoutDamage(pending, publishFailure);
    internal void ContinueColorFireAttackPayment(long useId) => _resolve().ContinueColorFireAttackPayment(useId);
    internal ColorFireAttackReceipt? FreezeColorFireAttackPolicy(int sourceSeat, CardActionContext? action) => _resolve().FreezeColorFireAttackPolicy(sourceSeat, action);
    internal bool TryBeginColorFireAttackDiscard(CardUseFrame pending, Card revealed) => _resolve().TryBeginColorFireAttackDiscard(pending, revealed);
    internal bool TryBeginColorFireAttackReveal(CardUseFrame pending) => _resolve().TryBeginColorFireAttackReveal(pending);
    internal bool TryResolveAiColorFireAttack(CardUseFrame pending, PendingDecision decision) => _resolve().TryResolveAiColorFireAttack(pending, decision);
    internal bool TryResolveColorFireAttackDiscard(CardUseFrame pending, int? selectedCardId) => _resolve().TryResolveColorFireAttackDiscard(pending, selectedCardId);
}
