#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenyiDamageHealthEntries
{
    internal void AssertWenyiDamageHealth() => _resolve().AssertWenyiDamageHealth();
    internal bool HasWenyiDamageHealthContinuation(long attackFrameId) => _resolve().HasWenyiDamageHealthContinuation(attackFrameId);
    internal bool IsWenyiDamageHealthReturn(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.HpChangedTriggerWindowFrame health) => _resolve().IsWenyiDamageHealthReturn(damage, health);
    internal void ReturnWenyiDamageHealth(global::CardGame.Core.HpChangedTriggerWindowFrame health) => _resolve().ReturnWenyiDamageHealth(health);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SeparateWenyiDamageHealthChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().SeparateWenyiDamageHealthChildProcessing(attack, processing);
    internal bool TryBeginWenyiDamageHealth(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId) => _resolve().TryBeginWenyiDamageHealth(attack, damageFrameId);
}
