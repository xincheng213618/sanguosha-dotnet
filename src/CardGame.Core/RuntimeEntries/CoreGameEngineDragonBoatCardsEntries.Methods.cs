#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatCardsEntries
{
    internal void AddDragonBoatCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards) => _resolve().AddDragonBoatCardActions(actions, actor, cards);
    internal void AddDragonBoatPhysicalOrConvertedActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion) => _resolve().AddDragonBoatPhysicalOrConvertedActions(actions, actor, card, kind, conversion);
    internal void BeginDragonBoatDiscard(global::CardGame.Core.CardUseFrame use) => _resolve().BeginDragonBoatDiscard(use);
    internal bool CanBeDragonBoatAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => _resolve().CanBeDragonBoatAdditionalTarget(use, target);
    internal void ContinueDragonBoatCardEffect(long id) => _resolve().ContinueDragonBoatCardEffect(id);
    internal bool DragonBoatCardAllies(int first, int second) => _resolve().DragonBoatCardAllies(first, second);
    internal (bool Most, bool Least) DragonBoatCardRank(int seat) => _resolve().DragonBoatCardRank(seat);
    internal int DragonBoatCardTeamScore(int seat) => _resolve().DragonBoatCardTeamScore(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> DragonBoatDiscardableCards(int seat) => _resolve().DragonBoatDiscardableCards(seat);
    internal global::System.Collections.Generic.IReadOnlyList<int>? DragonBoatExpectedNativeTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => _resolve().DragonBoatExpectedNativeTargets(actor, card, action);
    internal global::System.Collections.Generic.IReadOnlyList<int> DragonBoatGlobalTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => _resolve().DragonBoatGlobalTargets(actor, card);
    internal bool HasDragonBoatDiscardPrompt() => _resolve().HasDragonBoatDiscardPrompt();
    internal bool IsDragonBoatCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => _resolve().IsDragonBoatCardEffectHpReturn(use, hp);
    internal bool IsDragonBoatCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsDragonBoatCardEffectMovement(use, batch);
    internal bool IsDragonBoatCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => _resolve().IsDragonBoatCardEffectMovementReturn(use, frame);
    internal bool IsDragonBoatCardEffectRecoveryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsDragonBoatCardEffectRecoveryReturn(use, recovery);
    internal bool IsDragonBoatCardRecoveryAttempt(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryAttempt attempt) => _resolve().IsDragonBoatCardRecoveryAttempt(use, attempt);
    internal bool IsDragonBoatRealgarWineQuotaUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => _resolve().IsDragonBoatRealgarWineQuotaUse(use, action);
    internal void ResolveDragonBoatCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => _resolve().ResolveDragonBoatCard(actor, card, action);
    internal void ResolveDragonBoatDiscard(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDragonBoatDiscard(selected);
    internal void ResolveDragonBoatTrickEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveDragonBoatTrickEffect(pending);
    internal bool TryAdvanceDragonBoatDiscardAi() => _resolve().TryAdvanceDragonBoatDiscardAi();
    internal bool TryCompleteDragonBoatAgainstCurrentAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryCompleteDragonBoatAgainstCurrentAttack(attack);
}
