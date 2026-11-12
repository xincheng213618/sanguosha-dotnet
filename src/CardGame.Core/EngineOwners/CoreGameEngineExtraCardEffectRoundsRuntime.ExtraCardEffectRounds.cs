#nullable enable
namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExtraCardEffectRoundsRuntime
{
    internal void ApplyExtraCapableAlcoholEffect(long id, CharacterState target)
    {
        if (IsExtraCardEffectRound(id)) target.AlcoholDamageBonus = checked(target.AlcoholDamageBonus + 1);
        else target.HasAlcoholEffect = true;
    }
    internal bool RestoreExtraGroupDamageSource(NativeOwners.GroupCardHandle group)
    {
        if (_episodes.Ordinal(group.ResolutionId) == 0 || _episodes.Original(group.ResolutionId)?.GroupDamageSourceSeat is not { } source) return false;
        group.DamageSourceSeat = source;
        return true;
    }
    internal void CaptureExtraGroupDamageSource(NativeOwners.GroupCardHandle group) =>
        _episodes.CaptureGroupDamageSource(group.ResolutionId, group.DamageSourceSeat);
    internal bool IsExtraCardEffectRound(long id) => _episodes.Ordinal(id) > 0;
    internal bool HasUnstartedExtraCardEffect(long id) => _episodes.HasPending(id);
    internal void CaptureExtraCardEffectAttack(NativeOwners.CardAttackHandle attack)
    {
        if (_uses.LifecycleCardUse(attack.ResolutionId) is not { CardAttack: { } original } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind)) return;
        _episodes.CaptureAttack(use.Id, use.TargetIndex, original, use.Continuations.FangtianHalberd);
    }
    internal bool TryBeginExtraCardEffectRound(long id, bool afterAttack = false) => _episodes.TryBegin(id, afterAttack);
    internal static CardAttackState ResetExtraCardEffectAttack(CardAttackState original) => RuntimeSkills.RuntimeRules_Core_ExtraCardEffectRounds.ResetExtraCardEffectAttack(original);
    internal bool ContinueExtraCardEffectFangtian(NativeOwners.CardAttackHandle attack)
    {
        if (!_episodes.IsActive(attack.ResolutionId)) return false;
        var use = _uses.LifecycleCardUse(attack.ResolutionId)!;
        var seed = _episodes.Original(use.Id)?.Attacks.SingleOrDefault(seed => seed.TargetIndex == use.TargetIndex);
        if (seed is null) { _completion.CompleteAttack(attack); return true; }
        _attacks.UpdateCardAttackState(use.Id, _ => ResetExtraCardEffectAttack(seed.Attack));
        _slash.ContinueSlashDefense(attack);
        return true;
    }
    internal bool TryBeginExtraAttackRound(NativeOwners.CardAttackHandle attack) => TryBeginExtraCardEffectRound(attack.ResolutionId, afterAttack: true);
    internal bool TryContinueExtraSlashTargets(NativeOwners.CardAttackHandle attack)
    {
        if (_episodes.Current(attack.ResolutionId) is not { } cursor || _episodes.IsTargetScoped(attack.ResolutionId) ||
            _uses.LifecycleCardUse(attack.ResolutionId) is not { } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) ||
            _episodes.Original(use.Id) is not { } original) return false;
        return _execution.ContinueSlashTargets(use, original, cursor);
    }
    [ProgramDamageHook(ProgramDamagePhase.AttackCompletionPrelude, 20)]
    internal bool FinishExtraAttackRound(NativeOwners.CardAttackHandle attack, out bool paused)
    { paused = TryBeginExtraAttackRound(attack); return paused; }
}
