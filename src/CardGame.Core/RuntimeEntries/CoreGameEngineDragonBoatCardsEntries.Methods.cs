#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatCardsEntries
{
    internal void AddDragonBoatCardActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> cards) => _resolve().AddDragonBoatCardActions(actions, actor, cards);
    internal void AddDragonBoatPhysicalOrConvertedActions(List<LegalAction> actions, CharacterState actor, Card card, CardKind kind, CardConversionSource? conversion) => _resolve().AddDragonBoatPhysicalOrConvertedActions(actions, actor, card, kind, conversion);
    internal void BeginDragonBoatDiscard(CardUseFrame use) => _resolve().BeginDragonBoatDiscard(use);
    internal bool CanBeDragonBoatAdditionalTarget(CardUseFrame use, CharacterState target) => _resolve().CanBeDragonBoatAdditionalTarget(use, target);
    internal void ContinueDragonBoatCardEffect(long id) => _resolve().ContinueDragonBoatCardEffect(id);
    internal bool DragonBoatCardAllies(int first, int second) => _resolve().DragonBoatCardAllies(first, second);
    internal (bool Most, bool Least) DragonBoatCardRank(int seat) => _resolve().DragonBoatCardRank(seat);
    internal int DragonBoatCardTeamScore(int seat) => _resolve().DragonBoatCardTeamScore(seat);
    internal IReadOnlyList<Card> DragonBoatDiscardableCards(int seat) => _resolve().DragonBoatDiscardableCards(seat);
    internal IReadOnlyList<int>? DragonBoatExpectedNativeTargets(CharacterState actor, Card card, LegalAction action) => _resolve().DragonBoatExpectedNativeTargets(actor, card, action);
    internal IReadOnlyList<int> DragonBoatGlobalTargets(CharacterState actor, Card card) => _resolve().DragonBoatGlobalTargets(actor, card);
    internal bool HasDragonBoatDiscardPrompt() => _resolve().HasDragonBoatDiscardPrompt();
    internal bool IsDragonBoatCardEffectHpReturn(CardUseFrame use, HpChangedTriggerWindowFrame hp) => _resolve().IsDragonBoatCardEffectHpReturn(use, hp);
    internal bool IsDragonBoatCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsDragonBoatCardEffectMovement(use, batch);
    internal bool IsDragonBoatCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _resolve().IsDragonBoatCardEffectMovementReturn(use, frame);
    internal bool IsDragonBoatCardEffectRecoveryReturn(CardUseFrame use, RecoveryReplacementFrame recovery) => _resolve().IsDragonBoatCardEffectRecoveryReturn(use, recovery);
    internal bool IsDragonBoatCardRecoveryAttempt(CardUseFrame use, RecoveryAttempt attempt) => _resolve().IsDragonBoatCardRecoveryAttempt(use, attempt);
    internal bool IsDragonBoatRealgarWineQuotaUse(CardUseFrame use, CardActionContext action) => _resolve().IsDragonBoatRealgarWineQuotaUse(use, action);
    internal void ResolveDragonBoatCard(CharacterState actor, Card card, LegalAction action) => _resolve().ResolveDragonBoatCard(actor, card, action);
    internal void ResolveDragonBoatDiscard(PromptChoice selected) => _resolve().ResolveDragonBoatDiscard(selected);
    internal void ResolveDragonBoatTrickEffect(NullificationWindowFrame pending) => _resolve().ResolveDragonBoatTrickEffect(pending);
    internal bool TryAdvanceDragonBoatDiscardAi() => _resolve().TryAdvanceDragonBoatDiscardAi();
    internal bool TryCompleteDragonBoatAgainstCurrentAttack(NativeOwners.CardAttackHandle attack) => _resolve().TryCompleteDragonBoatAgainstCurrentAttack(attack);
}
