#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHealthReplacementProgramsRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 160)]
    internal void ObserveProgramHealthChange(IGameEvent payload)
    {
        if (payload is MaximumHpChangedEvent qiaoMaximum && _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(qiaoMaximum.PlayerSeat))
            return;
        if (payload is DamageAppliedEvent paidDamage)
            _characterDongBaiOlOperations.ObserveDongBaiHpDecreased(paidDamage.TargetSeat, paidDamage.ArmorPayment?.HpBefore ?? _coreMatchParticipantsPlayersAccess._players[paidDamage.TargetSeat].Hp + paidDamage.Amount, paidDamage.ArmorPayment?.HpAfter ?? _coreMatchParticipantsPlayersAccess._players[paidDamage.TargetSeat].Hp);
        else if (payload is MaximumHpChangedEvent { HpBeforeClamp: { } clampBefore } clamp)
            _characterDongBaiOlOperations.ObserveDongBaiHpDecreased(clamp.PlayerSeat, clampBefore, _coreMatchParticipantsPlayersAccess._players[clamp.PlayerSeat].Hp);
        var changed = payload switch
        {
            DamageAppliedEvent damage => (damage.TargetSeat, HpChangeKind.Damage, damage.ArmorPayment?.ActualHpLost ?? damage.Amount),
            MaximumHpChangedEvent maximum => (maximum.PlayerSeat, HpChangeKind.MaximumHp, Math.Abs(maximum.Delta)),
            _ => (-1, HpChangeKind.Damage, 0)};
        if (!_coreMatchSetupSetupCompleteAccess._setupComplete || changed.Item1 < 0 || changed.Item3 <= 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || (_coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[changed.Item1], SkillProgramTriggerWindow.AfterHealthChanged).Count == 0 && !_coreQimeiProgramsOperations.HasQimeiHealthSubject(changed.Item1) && !_characterHuanShuJunOlOperations.HasWenyiHealthSubject(changed.Item1) && !_programPipelineHooksOperations._programPipelineHooks.HealthChangeSubject(changed.Item1)))
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[changed.Item1];
        var healthOwner = _coreGrantedEntityPhasesOperations.GrantedEntityTrailerHealthOwner(payload) ?? _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id;
        var before = payload is DamageAppliedEvent damagePaid ? damagePaid.ArmorPayment?.HpBefore ?? owner.Hp + changed.Item3 : payload is MaximumHpChangedEvent { HpBeforeClamp: { } beforeClamp } ? beforeClamp : owner.Hp;
        var after = payload is DamageAppliedEvent { ArmorPayment: { } payment } ? payment.HpAfter : owner.Hp;
        var change = new HpChangeContext(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, healthOwner, null, owner.Seat, changed.Item2, changed.Item3, before, after)
        {
            HappyQiaoPool = _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(owner.Seat, healthOwner)
        };
        _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Add(change);
        _coreQimeiProgramsOperations.CaptureQimeiHpQualification(change);
        _characterSunJianMouOlOperations.CaptureMouSunJianOlHp(change);
        _programPipelineHooksOperations._programPipelineHooks.CaptureHealthChange(change);
    }

    internal bool ApplyAttackAsHpLoss(GameEngine.IDamageAttempt attack)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var amount = _coreCardDamageMaximumHpReplacementOperations.CalculateCardDamageLayer(attack);
        if (_coreIdentityConcealmentOperations.TryBeginIdentityAppearanceForDamage(attack, amount))
            return true;
        if (_coreHealthQueriesOperations.TryBeginHappyQiaoAttackHpLossChoice(attack))
            return true;
        var qiaoOwner = _characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(target.Seat) ? _resolutionStackOperations._resolutionStack.Last().Id : (long? )null;
        var before = target.Hp;
        target.Hp -= amount;
        if (qiaoOwner is { } ownerId)
            _characterDaQiaoXiaoQiaoHappyGodOperations.CompleteHappyQiaoHealthPayment(ownerId, target.Seat, HpChangeKind.Loss, before, target.Hp);
        _coreHpChangeProgramsOperations.RecordHpChange(attack.ResolutionId, null, target.Seat, before, target.Hp, HpChangeKind.Loss);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DamageReplacedWithHpLossEvent(attack.ResolutionId, attack.SourceSeat, target.Seat, amount, target.Hp));
        _coreMatchLogOperations.AddLog("HpLost", $"{(target.Name)} 失去 {(amount)} 点体力，剩余 {(target.Hp)} 点体力。", target.Seat);
        if (target.Hp > 0)
            return false;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null)
            throw new InvalidOperationException("An HP-loss attack cannot nest another dying victim.");
        var responders = Array.AsReadOnly(_coreNativeDyingOperations.BuildDyingResponderSeats(target.Seat).ToArray());
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.PushRuntimeFrame(new DyingFrame(id, attack.ResolutionId, target.Seat, null, responders, 0, DyingContinuationKind.AttackHpLoss) { HappyQiaoPool = _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(target.Seat) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerDyingEvent(id, target.Seat, null));
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        if (!_coreProgramLifecycleOperations.TryBeginMandatorySelfDyingProgram(_coreDyingCursorRuntimeOperations.ActiveDying!) && !_coreDyingEntryProgramsOperations.TryBeginDyingEntryProgramWindow(_coreDyingCursorRuntimeOperations.ActiveDying!))
            _coreNativeDyingOperations.ExposeHumanDyingPrompt();
        return true;
    }
}
