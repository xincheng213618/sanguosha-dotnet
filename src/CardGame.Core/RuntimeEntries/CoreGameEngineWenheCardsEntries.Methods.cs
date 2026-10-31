#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenheCardsEntries
{
    internal void AddWenheCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards) => _resolve().AddWenheCardActions(actions, actor, cards);
    internal void AssertWenheCardReceipt(global::CardGame.Core.CardUseFrame use) => _resolve().AssertWenheCardReceipt(use);
    internal bool CanBeWenheAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => _resolve().CanBeWenheAdditionalTarget(use, target);
    internal void CompleteWenheCardChaos(long useId) => _resolve().CompleteWenheCardChaos(useId);
    internal void ContinueWenheCardEffect(long id) => _resolve().ContinueWenheCardEffect(id);
    internal bool IsWenheCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => _resolve().IsWenheCardEffectHpReturn(use, hp);
    internal bool IsWenheCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsWenheCardEffectMovement(use, batch);
    internal bool IsWenheCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => _resolve().IsWenheCardEffectMovementReturn(use, movement);
    internal global::CardGame.Core.WenheCardEffectReceipt PlanWenheBeamSwap(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.WenheCardEffectReceipt receipt) => _resolve().PlanWenheBeamSwap(use, receipt);
    internal global::CardGame.Core.WenheCardEffectReceipt PlanWenheHandRedistribution(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.WenheCardEffectReceipt receipt) => _resolve().PlanWenheHandRedistribution(use, receipt);
    internal global::CardGame.Core.WenheCardEffectReceipt PlanWenheStarShift(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.WenheCardEffectReceipt receipt) => _resolve().PlanWenheStarShift(use, receipt);
    internal void ResolveWenheCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int? targetSeat = default) => _resolve().ResolveWenheCard(actor, card, kind, targetSeat);
    internal void ResolveWenheCardEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveWenheCardEffect(pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> WenheOrdinaryTrickKinds { get => _resolve().WenheOrdinaryTrickKinds; }
}
