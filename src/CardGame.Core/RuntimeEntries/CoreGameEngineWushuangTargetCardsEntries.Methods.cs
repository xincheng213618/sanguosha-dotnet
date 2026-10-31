#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangTargetCardsEntries
{
    internal void BindWushuangTargetCards(NullificationWindowFrame pending, int targetSeat) => _resolve().BindWushuangTargetCards(pending, targetSeat);
    internal void FinishWushuangTargetCards(CardUseFrame use) => _resolve().FinishWushuangTargetCards(use);
    internal bool IsWushuangExtraForeignSelection(long useId) => _resolve().IsWushuangExtraForeignSelection(useId);
    internal bool TryContinueWushuangTargetCards(long id, Card effectCard, CardKind effectiveKind, int source, int target, LegalActionKind kind) => _resolve().TryContinueWushuangTargetCards(id, effectCard, effectiveKind, source, target, kind);
    internal void WushuangClaimNullifiedTrick(NullificationWindowFrame window, IReadOnlyList<Card> finishedPhysicalCards, int beforeFinishMovement) => _resolve().WushuangClaimNullifiedTrick(window, finishedPhysicalCards, beforeFinishMovement);
}
