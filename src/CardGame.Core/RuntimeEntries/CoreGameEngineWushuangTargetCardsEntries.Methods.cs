#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangTargetCardsEntries
{
    internal void BindWushuangTargetCards(global::CardGame.Core.NullificationWindowFrame pending, int targetSeat) => _resolve().BindWushuangTargetCards(pending, targetSeat);
    internal void FinishWushuangTargetCards(global::CardGame.Core.CardUseFrame use) => _resolve().FinishWushuangTargetCards(use);
    internal bool IsWushuangExtraForeignSelection(long useId) => _resolve().IsWushuangExtraForeignSelection(useId);
    internal bool TryContinueWushuangTargetCards(long id, global::CardGame.Core.Card effectCard, global::CardGame.Core.CardKind effectiveKind, int source, int target, global::CardGame.Core.LegalActionKind kind) => _resolve().TryContinueWushuangTargetCards(id, effectCard, effectiveKind, source, target, kind);
    internal void WushuangClaimNullifiedTrick(global::CardGame.Core.NullificationWindowFrame window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> finishedPhysicalCards, int beforeFinishMovement) => _resolve().WushuangClaimNullifiedTrick(window, finishedPhysicalCards, beforeFinishMovement);
}
