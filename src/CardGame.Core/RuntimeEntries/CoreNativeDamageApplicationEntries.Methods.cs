#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageApplicationEntries
{
    internal bool ApplyAttackDamage(GameEngine.IDamageAttempt attack) => _resolve().ApplyAttackDamage(attack);
}
