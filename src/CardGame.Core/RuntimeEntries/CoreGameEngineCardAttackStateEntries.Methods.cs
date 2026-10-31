#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardAttackStateEntries
{
    internal global::CardGame.Core.GameEngine.AttackCompletionReceipt CaptureAttackCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CaptureAttackCompletion(attack);
    internal long? FindCardContinuationOwner(global::System.Func<global::CardGame.Core.CardResolutionContinuations, bool> matches) => _resolve().FindCardContinuationOwner(matches);
    internal global::CardGame.Core.Card GetAttackCard(int id) => _resolve().GetAttackCard(id);
    internal global::CardGame.Core.CardAttackState GetCardAttackState(long id) => _resolve().GetCardAttackState(id);
    internal global::CardGame.Core.CardResolutionContinuations GetCardContinuations(long id) => _resolve().GetCardContinuations(id);
    internal bool GetCardUseCausedDamage(long id) => _resolve().GetCardUseCausedDamage(id);
    internal int GetCardUseTargetIndex(long id) => _resolve().GetCardUseTargetIndex(id);
    internal void InitializeCardAttackState(long id, global::CardGame.Core.CardAttackState state) => _resolve().InitializeCardAttackState(id, state);
    internal global::CardGame.Core.Card ReadCardAppearance(global::CardGame.Core.CardAppearanceReference appearance) => _resolve().ReadCardAppearance(appearance);
    internal void SetCardUseCausedDamage(long id, bool value) => _resolve().SetCardUseCausedDamage(id, value);
    internal void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update) => _resolve().UpdateCardAttackState(id, update);
    internal void UpdateCardContinuations(long id, global::System.Func<global::CardGame.Core.CardResolutionContinuations, global::CardGame.Core.CardResolutionContinuations> update) => _resolve().UpdateCardContinuations(id, update);
}
