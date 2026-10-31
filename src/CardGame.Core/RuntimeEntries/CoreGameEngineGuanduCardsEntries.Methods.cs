#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduCardsEntries
{
    internal void AddGuanduCardActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards) => _resolve().AddGuanduCardActions(actions, actor, cards);
    internal void AddGuanduPhysicalOrConvertedActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion) => _resolve().AddGuanduPhysicalOrConvertedActions(actions, actor, card, kind, conversion);
    internal void ApplyGuanduRecovery(long id) => _resolve().ApplyGuanduRecovery(id);
    internal bool CanBeGuanduAdditionalTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => _resolve().CanBeGuanduAdditionalTarget(use, target);
    internal void ContinueGuanduCardEffect(long id) => _resolve().ContinueGuanduCardEffect(id);
    internal bool ContinueGuanduIntoxicationUse(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueGuanduIntoxicationUse(id, continuation);
    internal global::System.Collections.Generic.IReadOnlyList<int> GuanduEligibleTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit, bool? color, bool excludeOwner = false) => _resolve().GuanduEligibleTargets(actor, kind, suit, color, excludeOwner);
    internal global::System.Collections.Generic.IReadOnlyList<int>? GuanduExpectedNativeTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => _resolve().GuanduExpectedNativeTargets(actor, card, action);
    internal int GuanduIntoxicationDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().GuanduIntoxicationDamageBonus(attack);
    internal bool GuanduIntoxicationTargetSpent(int target) => _resolve().GuanduIntoxicationTargetSpent(target);
    internal global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.IReadOnlyList<int>> GuanduTargetChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit, bool? color, bool excludeOwner = false) => _resolve().GuanduTargetChoices(actor, kind, suit, color, excludeOwner);
    internal bool IsGuanduCardEffectHpReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame frame) => _resolve().IsGuanduCardEffectHpReturn(use, frame);
    internal bool IsGuanduCardEffectMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsGuanduCardEffectMovement(use, batch);
    internal bool IsGuanduCardEffectMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => _resolve().IsGuanduCardEffectMovementReturn(use, frame);
    internal bool IsGuanduCardEffectRecoveryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryReplacementFrame frame) => _resolve().IsGuanduCardEffectRecoveryReturn(use, frame);
    internal bool IsGuanduCardRecoveryAttempt(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RecoveryAttempt attempt) => _resolve().IsGuanduCardRecoveryAttempt(use, attempt);
    internal void ResolveGuanduCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => _resolve().ResolveGuanduCard(actor, card, action);
    internal void ResolveGuanduTrickEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveGuanduTrickEffect(pending);
}
