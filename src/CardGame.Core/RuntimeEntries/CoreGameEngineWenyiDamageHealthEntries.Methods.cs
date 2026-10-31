#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenyiDamageHealthEntries
{
    internal bool IsWenyiDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health) => _resolve().IsWenyiDamageHealthReturn(damage, health);
    internal void ReturnWenyiDamageHealth(global::CardGame.Core.HpChangedTriggerWindowFrame health) => _resolve().ReturnWenyiDamageHealth(health);
    internal bool TryBeginWenyiDamageHealth(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId) => _resolve().TryBeginWenyiDamageHealth(attack, damageFrameId);
}
