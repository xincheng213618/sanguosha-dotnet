#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenheCardsEntries
{
    internal void AddWenheCardActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> cards) => _resolve().AddWenheCardActions(actions, actor, cards);
    internal void AssertWenheCardReceipt(CardUseFrame use) => _resolve().AssertWenheCardReceipt(use);
    internal bool CanBeWenheAdditionalTarget(CardUseFrame use, CharacterState target) => _resolve().CanBeWenheAdditionalTarget(use, target);
    internal void CompleteWenheCardChaos(long useId) => _resolve().CompleteWenheCardChaos(useId);
    internal void ContinueWenheCardEffect(long id) => _resolve().ContinueWenheCardEffect(id);
    internal bool IsWenheCardEffectHpReturn(CardUseFrame use, HpChangedTriggerWindowFrame hp) => _resolve().IsWenheCardEffectHpReturn(use, hp);
    internal bool IsWenheCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsWenheCardEffectMovement(use, batch);
    internal bool IsWenheCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => _resolve().IsWenheCardEffectMovementReturn(use, movement);
    internal WenheCardEffectReceipt PlanWenheBeamSwap(CardUseFrame use, WenheCardEffectReceipt receipt) => _resolve().PlanWenheBeamSwap(use, receipt);
    internal WenheCardEffectReceipt PlanWenheHandRedistribution(CardUseFrame use, WenheCardEffectReceipt receipt) => _resolve().PlanWenheHandRedistribution(use, receipt);
    internal WenheCardEffectReceipt PlanWenheStarShift(CardUseFrame use, WenheCardEffectReceipt receipt) => _resolve().PlanWenheStarShift(use, receipt);
    internal void ResolveWenheCard(CharacterState actor, Card card, CardKind kind, int? targetSeat = default) => _resolve().ResolveWenheCard(actor, card, kind, targetSeat);
    internal void ResolveWenheCardEffect(NullificationWindowFrame pending) => _resolve().ResolveWenheCardEffect(pending);
    internal IReadOnlyList<CardKind> WenheOrdinaryTrickKinds { get => _resolve().WenheOrdinaryTrickKinds; }
}
