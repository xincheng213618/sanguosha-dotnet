#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedPoliciesEntries
{
    internal bool DistanceCardUseExemption160HasNextUnlimitedCard(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption160HasNextUnlimitedCard(q);
    internal bool HasFriendQihuiAllowance(global::CardGame.Core.CharacterState owner) => _resolve().HasFriendQihuiAllowance(owner);
    internal bool HasNextUnlimitedCard(global::CardGame.Core.CharacterState owner) => _resolve().HasNextUnlimitedCard(owner);
    internal void ObserveAdvancedLifecycleEvent(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveAdvancedLifecycleEvent(payload);
    internal int RedBladeActualDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().RedBladeActualDamageBonus(attack);
    internal int RedBladeDamageBonus(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().RedBladeDamageBonus(attack);
}
