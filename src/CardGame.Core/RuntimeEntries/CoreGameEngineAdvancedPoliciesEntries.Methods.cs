#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedPoliciesEntries
{
    internal bool DistanceCardUseExemption160HasNextUnlimitedCard(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption160HasNextUnlimitedCard(q);
    internal bool HasFriendQihuiAllowance(CharacterState owner) => _resolve().HasFriendQihuiAllowance(owner);
    internal bool HasNextUnlimitedCard(CharacterState owner) => _resolve().HasNextUnlimitedCard(owner);
    internal void ObserveAdvancedLifecycleEvent(IGameEvent payload) => _resolve().ObserveAdvancedLifecycleEvent(payload);
    internal int RedBladeActualDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().RedBladeActualDamageBonus(attack);
    internal int RedBladeDamageBonus(NativeOwners.CardAttackHandle attack) => _resolve().RedBladeDamageBonus(attack);
}
