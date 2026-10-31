#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardAttackStateEntries
{
    internal GameEngine.AttackCompletionReceipt CaptureAttackCompletion(NativeOwners.CardAttackHandle attack) => _resolve().CaptureAttackCompletion(attack);
    internal long? FindCardContinuationOwner(Func<CardResolutionContinuations, bool> matches) => _resolve().FindCardContinuationOwner(matches);
    internal Card GetAttackCard(int id) => _resolve().GetAttackCard(id);
    internal CardAttackState GetCardAttackState(long id) => _resolve().GetCardAttackState(id);
    internal CardResolutionContinuations GetCardContinuations(long id) => _resolve().GetCardContinuations(id);
    internal bool GetCardUseCausedDamage(long id) => _resolve().GetCardUseCausedDamage(id);
    internal int GetCardUseTargetIndex(long id) => _resolve().GetCardUseTargetIndex(id);
    internal void InitializeCardAttackState(long id, CardAttackState state) => _resolve().InitializeCardAttackState(id, state);
    internal Card ReadCardAppearance(CardAppearanceReference appearance) => _resolve().ReadCardAppearance(appearance);
    internal void SetCardUseCausedDamage(long id, bool value) => _resolve().SetCardUseCausedDamage(id, value);
    internal void UpdateCardAttackState(long id, Func<CardAttackState?, CardAttackState> update) => _resolve().UpdateCardAttackState(id, update);
    internal void UpdateCardContinuations(long id, Func<CardResolutionContinuations, CardResolutionContinuations> update) => _resolve().UpdateCardContinuations(id, update);
}
