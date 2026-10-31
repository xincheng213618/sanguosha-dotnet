#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenyiDamageHealthEntries
{
    internal bool IsWenyiDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health) => _resolve().IsWenyiDamageHealthReturn(damage, health);
    internal void ReturnWenyiDamageHealth(HpChangedTriggerWindowFrame health) => _resolve().ReturnWenyiDamageHealth(health);
    internal bool TryBeginWenyiDamageHealth(GameEngine.IDamageAttempt attack, long damageFrameId) => _resolve().TryBeginWenyiDamageHealth(attack, damageFrameId);
}
