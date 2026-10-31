#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraCardEffectRoundsEntries
{
    internal void ApplyExtraCapableAlcoholEffect(long id, CharacterState target) => _resolve().ApplyExtraCapableAlcoholEffect(id, target);
    internal void CaptureExtraCardEffectAttack(NativeOwners.CardAttackHandle attack) => _resolve().CaptureExtraCardEffectAttack(attack);
    internal void CaptureExtraGroupDamageSource(NativeOwners.GroupCardHandle group) => _resolve().CaptureExtraGroupDamageSource(group);
    internal bool ContinueExtraCardEffectFangtian(NativeOwners.CardAttackHandle attack) => _resolve().ContinueExtraCardEffectFangtian(attack);
    internal bool FinishExtraAttackRound(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishExtraAttackRound(attack, out paused);
    internal bool HasUnstartedExtraCardEffect(long id) => _resolve().HasUnstartedExtraCardEffect(id);
    internal bool IsExtraCardEffectRound(long id) => _resolve().IsExtraCardEffectRound(id);
    internal bool RestoreExtraGroupDamageSource(NativeOwners.GroupCardHandle group) => _resolve().RestoreExtraGroupDamageSource(group);
    internal bool TryBeginExtraAttackRound(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginExtraAttackRound(attack);
    internal bool TryBeginExtraCardEffectRound(long id, bool afterAttack = false) => _resolve().TryBeginExtraCardEffectRound(id, afterAttack);
}
