#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduCardsEntries
{
    internal void AddGuanduCardActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> cards) => _resolve().AddGuanduCardActions(actions, actor, cards);
    internal void AddGuanduPhysicalOrConvertedActions(List<LegalAction> actions, CharacterState actor, Card card, CardKind kind, CardConversionSource? conversion) => _resolve().AddGuanduPhysicalOrConvertedActions(actions, actor, card, kind, conversion);
    internal void ApplyGuanduRecovery(long id) => _resolve().ApplyGuanduRecovery(id);
    internal bool CanBeGuanduAdditionalTarget(CardUseFrame use, CharacterState target) => _resolve().CanBeGuanduAdditionalTarget(use, target);
    internal void ContinueGuanduCardEffect(long id) => _resolve().ContinueGuanduCardEffect(id);
    internal bool ContinueGuanduIntoxicationUse(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueGuanduIntoxicationUse(id, continuation);
    internal IReadOnlyList<int> GuanduEligibleTargets(CharacterState actor, CardKind kind, Suit? suit, bool? color, bool excludeOwner = false) => _resolve().GuanduEligibleTargets(actor, kind, suit, color, excludeOwner);
    internal IReadOnlyList<int>? GuanduExpectedNativeTargets(CharacterState actor, Card card, LegalAction action) => _resolve().GuanduExpectedNativeTargets(actor, card, action);
    internal int GuanduIntoxicationDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().GuanduIntoxicationDamageBonus(attack);
    internal bool GuanduIntoxicationTargetSpent(int target) => _resolve().GuanduIntoxicationTargetSpent(target);
    internal IEnumerable<IReadOnlyList<int>> GuanduTargetChoices(CharacterState actor, CardKind kind, Suit? suit, bool? color, bool excludeOwner = false) => _resolve().GuanduTargetChoices(actor, kind, suit, color, excludeOwner);
    internal bool IsGuanduCardEffectHpReturn(CardUseFrame use, HpChangedTriggerWindowFrame frame) => _resolve().IsGuanduCardEffectHpReturn(use, frame);
    internal bool IsGuanduCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsGuanduCardEffectMovement(use, batch);
    internal bool IsGuanduCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _resolve().IsGuanduCardEffectMovementReturn(use, frame);
    internal bool IsGuanduCardEffectRecoveryReturn(CardUseFrame use, RecoveryReplacementFrame frame) => _resolve().IsGuanduCardEffectRecoveryReturn(use, frame);
    internal bool IsGuanduCardRecoveryAttempt(CardUseFrame use, RecoveryAttempt attempt) => _resolve().IsGuanduCardRecoveryAttempt(use, attempt);
    internal void ResolveGuanduCard(CharacterState actor, Card card, LegalAction action) => _resolve().ResolveGuanduCard(actor, card, action);
    internal void ResolveGuanduTrickEffect(NullificationWindowFrame pending) => _resolve().ResolveGuanduTrickEffect(pending);
}
