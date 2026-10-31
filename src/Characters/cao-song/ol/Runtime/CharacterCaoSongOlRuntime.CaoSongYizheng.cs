#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoSongOlRuntime
{
    internal CaoSongYizhengGrantedEvent[] ActiveCaoSongYizhengGrants(int beneficiary)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrantYizhengSupport))
            return[];
        var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoSongYizhengGrantedEvent>();
        if (grants.Count == 0)
            return[];
        var expired = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoSongYizhengExpiredEvent>().Select(e => e.GrantId).ToHashSet();
        return grants.Where(e => e.RecipientSeat == beneficiary && !expired.Contains(e.GrantId)).OrderBy(e => e.Source.OwnerSeat).ThenBy(e => e.GrantId).ToArray();
    }

    internal bool CanPayCaoSongYizheng(CaoSongYizhengGrantedEvent grant, int beneficiary) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(beneficiary) && _coreMatchParticipantsPlayersAccess._players[beneficiary] is { IsAlive: true, HasHpValue: true } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(grant.Source.OwnerSeat) && grant.Source.OwnerSeat != beneficiary && _coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat] is { IsAlive: true, HasHpValue: true } owner && owner.MaxHp > _coreMatchParticipantsPlayersAccess._players[beneficiary].MaxHp && ActiveCaoSongYizhengGrants(beneficiary).Any(e => e == grant);
    internal bool HasRecoveryAmountSupport(int targetSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && _coreMatchParticipantsPlayersAccess._players[targetSeat] is { IsAlive: true, HasHpValue: true } target && target.Hp < target.MaxHp && ActiveCaoSongYizhengGrants(targetSeat).Any(grant => CanPayCaoSongYizheng(grant, targetSeat));
    internal bool HasDamageAmountSupport(int sourceSeat, int targetSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && ActiveCaoSongYizhengGrants(sourceSeat).Any(grant => CanPayCaoSongYizheng(grant, sourceSeat));
    internal void IssueCaoSongYizheng(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding, OwnerSeat: var owner } context || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } ending || ending.OwnerSeat != frame.OwnerSeat || ending.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || owner != frame.OwnerSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frame.SelectedTargetSeats is not [var recipient] || recipient == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoSongYizhengGrantedEvent>().Any(e => e.GrantId == frame.Id))
            throw new InvalidOperationException("Yizheng requires one exact other living recipient at the owner's native ending.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoSongYizhengGrantedEvent(frame.Id, CaoSongSource(frame), recipient, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
    }

    // The granted duration is independent of retaining the skill. A dead donor
    // cannot make a new payment, but a cost already paid remains effective.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26031)]
    internal void ExpireCaoSongYizhengAtNextOwnTurn(TurnStartedEvent started)
    {
        var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoSongYizhengGrantedEvent>();
        if (grants.Count == 0)
            return;
        var expired = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoSongYizhengExpiredEvent>().Select(e => e.GrantId).ToHashSet();
        foreach (var grant in grants.Where(e => e.Source.OwnerSeat == started.ActorSeat && !expired.Contains(e.GrantId)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoSongYizhengExpiredEvent(grant.GrantId, started.ActorSeat, started.TurnNumber));
    }

    internal void PayCaoSongYizhengMaximum(long parentId, CaoSongYizhengGrantedEvent grant, int beneficiary, bool recovery)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parentId || !CanPayCaoSongYizheng(grant, beneficiary))
            throw new InvalidOperationException("Yizheng maximum-HP cost lost its current issuing grant and strict maximum comparison.");
        var owner = _coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat];
        var previous = owner.MaxHp;
        var hpBefore = owner.Hp;
        owner.MaxHp = checked(previous - 1);
        owner.Hp = Math.Min(owner.Hp, owner.MaxHp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(owner.Seat, -1, owner.MaxHp, grant.Source.SkillId) { HpBeforeClamp = _coreQimeiProgramsOperations.HasQimeiHealthSubject(owner.Seat) || owner.XiaHuiRestrictions is { Count: > 0 } || _characterSunJianMouOlOperations.HasMouSunJianOlHpSubject(owner.Seat) ? hpBefore : null });
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeRecoveryApply, 26030)]
    internal bool ApplyCaoSongRecoverySupport()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not RecoveryReplacementFrame { Stage: RecoveryReplacementStage.Choosing, PendingApplication: { } application } frame)
            return false;
        if (frame.CaoSongRecoverySupport is null)
        {
            if (application.RequestedAmount <= 0 || !HasRecoveryAmountSupport(application.TargetSeat))
                return false;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CaoSongRecoverySupport = new(application.TargetSeat, application.RequestedAmount) { Grants = ActiveCaoSongYizhengGrants(application.TargetSeat) } });
        }

        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is RecoveryReplacementFrame { CaoSongRecoverySupport: { Complete: false } receipt } current && current.Id == frame.Id)
        {
            if (receipt.NextGrantIndex >= receipt.Grants.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { CaoSongRecoverySupport = receipt with { Complete = true, HpBeforeApplication = _coreMatchParticipantsPlayersAccess._players[receipt.BeneficiarySeat].Hp } });
                return false;
            }

            var grant = receipt.Grants[receipt.NextGrantIndex];
            if (receipt.WaitingForMaximumChildren)
            {
                if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(current.Id, PostEventContinuation.AmountSupport))
                    return true;
                ValidateCaoSongSupportPaid(current.Id, receipt, true);
                var before = current.PendingApplication!.RequestedAmount;
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { PendingApplication = current.PendingApplication with { RequestedAmount = checked(before + 1) }, CaoSongRecoverySupport = receipt with { NextGrantIndex = receipt.NextGrantIndex + 1, WaitingForMaximumChildren = false } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoSongYizhengAmountIncreasedEvent(current.Id, grant.GrantId, receipt.BeneficiarySeat, true, before, before + 1));
                continue;
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !CanPayCaoSongYizheng(grant, receipt.BeneficiarySeat) || !_coreMatchParticipantsPlayersAccess._players[receipt.BeneficiarySeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[receipt.BeneficiarySeat].Hp >= _coreMatchParticipantsPlayersAccess._players[receipt.BeneficiarySeat].MaxHp)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { CaoSongRecoverySupport = receipt with { NextGrantIndex = receipt.NextGrantIndex + 1 } });
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(current with { CaoSongRecoverySupport = receipt with { WaitingForMaximumChildren = true } });
            PayCaoSongYizhengMaximum(current.Id, grant, receipt.BeneficiarySeat, true);
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(current.Id, PostEventContinuation.AmountSupport))
                return true;
        }

        return false;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDamageApply, 26030)]
    internal bool ApplyCaoSongDamageSupport()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not BeforeDamageProgramWindowFrame frame || frame.Prevented || frame.RedirectedTargetSeat is not null || frame.Amount <= 0 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } attack || attack.SourceSeat != frame.SourceSeat || attack.TargetSeat != frame.TargetSeat)
            return false;
        if (frame.CaoSongDamageSupport is null)
        {
            if (!HasDamageAmountSupport(frame.SourceSeat, frame.TargetSeat))
                return false;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CaoSongDamageSupport = new(frame.SourceSeat, frame.Amount) { Grants = ActiveCaoSongYizhengGrants(frame.SourceSeat) } });
        }

        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is BeforeDamageProgramWindowFrame { CaoSongDamageSupport: { Complete: false } receipt } current && current.Id == frame.Id)
        {
            if (receipt.NextGrantIndex >= receipt.Grants.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { CaoSongDamageSupport = receipt with { Complete = true } });
                return false;
            }

            var grant = receipt.Grants[receipt.NextGrantIndex];
            if (receipt.WaitingForMaximumChildren)
            {
                if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(current.Id, PostEventContinuation.AmountSupport))
                    return true;
                ValidateCaoSongSupportPaid(current.Id, receipt, false);
                if (_coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } actual || actual.SourceSeat != current.SourceSeat || actual.TargetSeat != current.TargetSeat)
                    throw new InvalidOperationException("Yizheng paid cost lost its original damage attempt.");
                actual.IncreaseFinalizedDamageAmount(1);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { Amount = checked(current.Amount + 1), CaoSongDamageSupport = receipt with { NextGrantIndex = receipt.NextGrantIndex + 1, WaitingForMaximumChildren = false } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoSongYizhengAmountIncreasedEvent(current.Id, grant.GrantId, receipt.BeneficiarySeat, false, current.Amount, current.Amount + 1));
                continue;
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !CanPayCaoSongYizheng(grant, receipt.BeneficiarySeat))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { CaoSongDamageSupport = receipt with { NextGrantIndex = receipt.NextGrantIndex + 1 } });
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(current with { CaoSongDamageSupport = receipt with { WaitingForMaximumChildren = true } });
            PayCaoSongYizhengMaximum(current.Id, grant, receipt.BeneficiarySeat, false);
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(current.Id, PostEventContinuation.AmountSupport))
                return true;
        }

        return false;
    }

    internal void ValidateCaoSongSupportPaid(long parentId, CaoSongAmountSupportReceipt receipt, bool recovery)
    {
        var grant = receipt.Grants[receipt.NextGrantIndex];
    }

    internal bool IsAmountSupportHealthChild(ResolutionFrame? parent, HpChangedTriggerWindowFrame hp)
    {
        var receipt = parent switch
        {
            RecoveryReplacementFrame r => r.CaoSongRecoverySupport,
            BeforeDamageProgramWindowFrame d => d.CaoSongDamageSupport,
            _ => null
        };
        if (parent is null || receipt is not { WaitingForMaximumChildren: true, Complete: false } || receipt.NextGrantIndex < 0 || receipt.NextGrantIndex >= receipt.Grants.Count || hp.Continuation != PostEventContinuation.AmountSupport || hp.ResumeFrameId != parent.Id || hp.Change.ParentFrameId != parent.Id || hp.Change.Kind != HpChangeKind.MaximumHp || hp.Change.Amount != 1 || hp.Change.TargetSeat != receipt.Grants[receipt.NextGrantIndex].Source.OwnerSeat || hp.Change.SourceSeat is not null || hp.CardId is not null || hp.CardKind is not null)
            return false;
        ValidateCaoSongSupportPaid(parent.Id, receipt, parent is RecoveryReplacementFrame);
        return true;
    }

    internal int SupportedCaoSongRecoveryAmount(RecoveryReplacementFrame recovery, int baseAmount)
    {
        if (recovery.CaoSongRecoverySupport is not { } receipt)
            return baseAmount;
        var increases = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoSongYizhengAmountIncreasedEvent>().Where(e => e.ParentFrameId == recovery.Id && e.Recovery).ToArray();
        var amount = receipt.OriginalAmount;
        foreach (var increase in increases)
        {
            amount++;
        }

        return Math.Max(baseAmount, amount);
    }

    internal int SupportedRecoveryHpBefore(RecoveryReplacementFrame recovery, int fallback) => recovery.AmountBonus is { } bonus ? bonus.HpBefore : recovery.CaoSongRecoverySupport is { Complete: true, HpBeforeApplication: { } hp } ? hp : fallback;
}
