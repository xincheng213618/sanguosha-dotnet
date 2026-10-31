#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHaoOlRuntime
{
    internal bool HasSunHaoChouhaiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.EmptyHandDamageTakenBonus);

    internal SunHaoChouhaiReceipt? SunHaoDamageState(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        CardUseFrame f => f.SunHaoChouhai,
        ProgramSkillFrame f => f.SunHaoChouhai,
        JudgmentFrame f => f.SunHaoChouhai,
        _ => null
    };
    internal void SetSunHaoDamageState(long id, SunHaoChouhaiReceipt? r)
    {
        var f = _resolutionStackOperations._resolutionStack.Single(x => x.Id == id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f switch
        {
            CardUseFrame c => c with { SunHaoChouhai = r },
            ProgramSkillFrame p => p with { SunHaoChouhai = r },
            JudgmentFrame j => j with { SunHaoChouhai = r },
            _ => throw new InvalidOperationException("Chouhai requires the actual damage attempt's owning frame.")});
    }

    internal int SunHaoDamageCursor(GameEngine.IDamageAttempt a) => !a.IsChainPropagation ? -1 : a is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(a.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(a.ResolutionId).ChainedTargetIndex;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 180)]
    internal int SunHaoChouhaiDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (!HasSunHaoChouhaiCapability)
            return 0;
        var cursor = SunHaoDamageCursor(attack);
        if (SunHaoDamageState(attack.ResolutionId)is { } old && old.TargetSeat == attack.TargetSeat && old.Cursor == cursor)
            return old.Bonus;
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var bonus = target.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count == 0 && _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.EmptyHandDamageTakenBonus) ? 1 : 0;
        if (bonus == 0)
        {
            if (SunHaoDamageState(attack.ResolutionId)is not null)
                SetSunHaoDamageState(attack.ResolutionId, null);
            return 0;
        }

        var r = new SunHaoChouhaiReceipt(attack.ResolutionId, attack.TargetSeat, cursor, attack.DamageAmount, bonus, InvoiceId: ++_coreResolutionIdentityBoundaryOperations._resolutionSequence);
        SetSunHaoDamageState(attack.ResolutionId, r);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoChouhaiFrozenEvent(r.AttackId, r.TargetSeat, r.Cursor, r.InheritedAmount, r.Bonus, r.InvoiceId));
        return bonus;
    }

    internal void SetSunHaoChainAmount(GameEngine.IDamageAttempt attack, int amount)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !attack.IsChainPropagation || amount < 0)
            throw new InvalidOperationException("Chouhai can only restore its exact native chained recipient.");
        if (attack is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, s => s!with { DamageAmount = amount });
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 30)]
    internal void PrepareSunHaoChainAdvance(GameEngine.IDamageAttempt attack)
    {
        if (!HasSunHaoChouhaiCapability || SunHaoDamageState(attack.ResolutionId)is not { } r)
            return;
        var seats = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats;
        var next = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex;
        if (seats is null || !seats.Skip(next).Any(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive))
            return;
        if (attack.IsChainPropagation && r.Cursor == SunHaoDamageCursor(attack) && (r.TransferredTarget ?? r.TargetSeat) == attack.TargetSeat)
        {
            SetSunHaoChainAmount(attack, r.InheritedAmount);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoChouhaiRebasedEvent(r.AttackId, r.TargetSeat, r.Cursor, r.InheritedAmount));
        }

        SetSunHaoDamageState(attack.ResolutionId, null);
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 40)]
    internal void ApplySunHaoChainedDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (HasSunHaoChouhaiCapability && attack.IsChainPropagation && attack.DamageAmountFinalized)
            SetSunHaoChainAmount(attack, checked(attack.DamageAmount + SunHaoChouhaiDamageBonus(attack)));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 27332)]
    internal void AssertSunHaoDamageReceipts()
    {
        if (!HasSunHaoChouhaiCapability)
            return;
        foreach (var frame in _resolutionStackOperations._resolutionStack)
        {
            var r = SunHaoDamageState(frame.Id);
            if (r is null)
                continue;
            if (!HasSunHaoChouhaiCapability || r.AttackId != frame.Id || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.Cursor < -1 || r.InheritedAmount < 0 || r.Bonus != 1 || r.InvoiceId <= 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoChouhaiFrozenEvent>().Count(e => e == new SunHaoChouhaiFrozenEvent(r.AttackId, r.TargetSeat, r.Cursor, r.InheritedAmount, r.Bonus, r.InvoiceId)) != 1 || (r.TransferredTarget is { } target ? !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || r.TransferWindowId is not { } window || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoChouhaiTransferredEvent>().Count(e => e.AttackId == r.AttackId && e.OriginalSeat == r.TargetSeat && e.TargetSeat == target && e.Cursor == r.Cursor && e.WindowId == window) != 1 : r.TransferWindowId is not null))
                throw new InvalidOperationException("Chouhai lost its actual native recipient's once-issued modifier invoice.");
        }
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27332)]
    internal void ObserveSunHaoDamage(IGameEvent payload)
    {
        if (!HasSunHaoChouhaiCapability)
            return;
        if (payload is ExtraCardEffectStartedEvent extra && _coreCardUseLifecycleOperations.LifecycleCardUse(extra.CardUseFrameId)is { SunHaoChouhai: not null, Action: { } repeatedAction } repeated && repeatedAction.ActionId == extra.ActionId)
            SetSunHaoDamageState(repeated.Id, null);
        if (payload is ProgramDamageTransferredEvent e && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && SunHaoDamageState(attack.ResolutionId)is { TransferredTarget: null } r && _resolutionStackOperations._resolutionStack.Count >= 2 && _resolutionStackOperations._resolutionStack[^1] is ProgramSkillFrame f && _resolutionStackOperations._resolutionStack[^2] is BeforeDamageProgramWindowFrame w && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.RedirectCurrentDamage && f.WindowContext?.ParentFrameId == w.Id && e.ResolutionId == attack.ResolutionId && (w.ContinuationAttackResolutionId ?? w.ParentFrameId) == attack.ResolutionId && r.TargetSeat == w.TargetSeat && w.RedirectedTargetSeat == attack.TargetSeat && e.TargetSeat == attack.TargetSeat && e.SourceSeat == attack.SourceSeat && e.DamageAmount == w.Amount && e.SkillId == f.SkillId && e.OwnerSeat == f.OwnerSeat && attack.DamageRedirected)
        {
            SetSunHaoDamageState(attack.ResolutionId, r with { TransferredTarget = attack.TargetSeat, TransferWindowId = w.Id });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoChouhaiTransferredEvent(r.AttackId, r.TargetSeat, attack.TargetSeat, r.Cursor, w.Id));
        }
    }
}
