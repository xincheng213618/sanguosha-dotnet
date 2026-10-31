#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWenyiDamageHealthRuntime
{
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 10)]
    internal bool TryBeginWenyiDamageHealth(GameEngine.IDamageAttempt attack, long damageFrameId)
    {
        if (!_characterHuanShuJunOlOperations.HasWenyiHealthSubject(attack.TargetSeat) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.Id != damageFrameId || damage.ParentFrameId != attack.ResolutionId || damage.WenyiHealth is not null)
            return false;
        var change = _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.FirstOrDefault(c => c.ParentFrameId == damage.Id && c.TargetSeat == damage.TargetSeat && c.Kind == HpChangeKind.Damage && c.HpBefore != 1 && c.HpAfter == 1 && _characterHuanShuJunOlOperations.CollectWenyiHpCandidates(c).Any());
        if (change is null)
            return false;
        var receipt = new WenyiDamageHealthReceipt(damage.Id, attack.ResolutionId, change.Id, damage.SourceSeat, damage.TargetSeat, damage.Amount, damage.Nature, attack.IsSourceLess);
        _coreRuntimeOperations.ReplaceRuntimeTop(damage with { WenyiHealth = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenyiDamageHealthStartedEvent(receipt));
        if (!_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(damage.Id, PostEventContinuation.WenyiDamageHealth))
            throw new InvalidOperationException("Wenyi lost its committed damage health-change window before native damage observers.");
        return true;
    }

    internal bool IsWenyiDamageHealthReturn(DamageFrame damage, HpChangedTriggerWindowFrame health) => damage.WenyiHealth is { } r && r.DamageFrameId == damage.Id && r.AttackFrameId == damage.ParentFrameId && r.SourceSeat == damage.SourceSeat && r.TargetSeat == damage.TargetSeat && r.Amount == damage.Amount && r.Nature == damage.Nature && health.Continuation == PostEventContinuation.WenyiDamageHealth && health.ResumeFrameId == damage.Id && health.Change.ParentFrameId == damage.Id && health.Id == health.Change.Id && health.Id == r.HealthChangeId && health.Change.TargetSeat == r.TargetSeat && health.Change.Kind == HpChangeKind.Damage && health.Change.Amount == (damage.ArmorPayment?.ActualHpLost ?? r.Amount) && health.Change.HpBefore == (damage.ArmorPayment?.HpBefore ?? r.Amount + 1) && health.Change.HpAfter == 1 && (damage.ArmorPayment is null || damage.ArmorPayment.HpAfter == 1 && _coreDamageArmorPaymentsOperations.Value.IsIssuedDamageArmorPayment(damage)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenyiDamageHealthStartedEvent>().Count(e => e.Receipt == r) == 1 && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenyiDamageHealthReturnedEvent>().Any(e => e.Receipt == r);

    internal void ReturnWenyiDamageHealth(HpChangedTriggerWindowFrame health)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || !IsWenyiDamageHealthReturn(damage, health) || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.ResolutionId != damage.ParentFrameId || attack.SourceSeat != damage.SourceSeat || attack.TargetSeat != damage.TargetSeat || attack.IsSourceLess != damage.WenyiHealth!.SourceLess)
            throw new InvalidOperationException("Wenyi damage health children returned to a different actual damage attempt.");
        var receipt = damage.WenyiHealth!;
        _coreRuntimeOperations.ReplaceRuntimeTop(damage with { WenyiHealth = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenyiDamageHealthReturnedEvent(receipt));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreNativeDamageWindowsOperations.TryBeginDamageTriggerWindow(attack, damage.Id, SkillProgramTriggerWindow.DamageAppliedBeforeDying))
            return;
        var target = _coreMatchParticipantsPlayersAccess._players[damage.TargetSeat];
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && target.IsAlive && target.Hp <= 0)
        {
            _coreNativeDyingOperations.BeginDying(attack, damage.Id, target, _coreMatchParticipantsPlayersAccess._players[damage.SourceSeat]);
            return;
        }

        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreNativeDamageWindowsOperations.TryBeginDamageTriggerWindow(attack, damage.Id, SkillProgramTriggerWindow.AfterDamageApplied))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AfterDamageEvent(damage.Id, damage.SourceSeat, damage.TargetSeat, damage.Amount, target.Hp, damage.Nature) { SourceLess = receipt.SourceLess });
        _coreRuntimeOperations.PopResolutionFrame(damage.Id, ResolutionFrameKind.Damage);
        _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
    }
}
