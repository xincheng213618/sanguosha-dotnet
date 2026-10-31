#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraCardEffectRoundsEntries
{
    internal void ApplyExtraCapableAlcoholEffect(long id, global::CardGame.Core.CharacterState target) => _resolve().ApplyExtraCapableAlcoholEffect(id, target);
    internal void CaptureExtraCardEffectAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().CaptureExtraCardEffectAttack(attack);
    internal void CaptureExtraGroupDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().CaptureExtraGroupDamageSource(group);
    internal bool ContinueExtraCardEffectFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ContinueExtraCardEffectFangtian(attack);
    internal bool FinishExtraAttackRound(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishExtraAttackRound(attack, out paused);
    internal bool HasUnstartedExtraCardEffect(long id) => _resolve().HasUnstartedExtraCardEffect(id);
    internal bool IsExtraCardEffectRound(long id) => _resolve().IsExtraCardEffectRound(id);
    internal bool RestoreExtraGroupDamageSource(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().RestoreExtraGroupDamageSource(group);
    internal bool TryBeginExtraAttackRound(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginExtraAttackRound(attack);
    internal bool TryBeginExtraCardEffectRound(long id, bool afterAttack = false) => _resolve().TryBeginExtraCardEffectRound(id, afterAttack);
}
