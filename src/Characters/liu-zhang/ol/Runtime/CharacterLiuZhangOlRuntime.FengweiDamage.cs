#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{
    internal bool IsFengweiCardDamage(GameEngine.IDamageAttempt attack) => attack.Card is not null || attack.PhysicalCards.Count > 0 || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { } use && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) && use.CardKind == attack.EffectiveCardKind;
    internal FengweiDamageReceipt? FengweiDamageState(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        CardUseFrame f => f.FengweiDamage,
        ProgramSkillFrame f => f.FengweiDamage,
        JudgmentFrame f => f.FengweiDamage,
        _ => null
    };
    internal void SetFengweiDamageState(long id, FengweiDamageReceipt? r)
    {
        var f = _resolutionStackOperations._resolutionStack.Single(x => x.Id == id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f switch
        {
            CardUseFrame c => c with { FengweiDamage = r },
            ProgramSkillFrame p => p with { FengweiDamage = r },
            JudgmentFrame j => j with { FengweiDamage = r },
            _ => throw new InvalidOperationException("Fengwei requires the actual damage attempt's owning frame.")});
    }

    internal int FengweiDamageCursor(GameEngine.IDamageAttempt a) => !a.IsChainPropagation ? -1 : a is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(a.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(a.ResolutionId).ChainedTargetIndex;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 190)]
    internal int FengweiDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (!HasFengweiCapability)
            return 0;
        var cursor = FengweiDamageCursor(attack);
        if (FengweiDamageState(attack.ResolutionId)is { } old && old.TargetSeat == attack.TargetSeat && old.Cursor == cursor)
            return old.Bonus;
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var bonus = target.IsAlive && FengweiLive(target) && FengweiMarkedHand(target.Seat)is not null && IsFengweiCardDamage(attack) ? 1 : 0;
        if (bonus == 0)
        {
            if (FengweiDamageState(attack.ResolutionId)is not null)
                SetFengweiDamageState(attack.ResolutionId, null);
            return 0;
        }

        var r = new FengweiDamageReceipt(attack.ResolutionId, attack.TargetSeat, cursor, attack.DamageAmount, bonus, InvoiceId: ++_coreResolutionIdentityBoundaryOperations._resolutionSequence);
        SetFengweiDamageState(attack.ResolutionId, r);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FengweiDamageFrozenEvent(r.AttackId, r.TargetSeat, r.Cursor, r.InheritedAmount, r.Bonus, r.InvoiceId));
        return bonus;
    }

    internal void SetFengweiChainAmount(GameEngine.IDamageAttempt attack, int amount)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !attack.IsChainPropagation || amount < 0)
            throw new InvalidOperationException("Fengwei can only restore its exact native chained recipient.");
        if (attack is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, s => s!with { DamageAmount = amount });
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 20)]
    internal void PrepareFengweiChainAdvance(GameEngine.IDamageAttempt attack)
    {
        if (!HasFengweiCapability || FengweiDamageState(attack.ResolutionId)is not { } r)
            return;
        var seats = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats;
        var next = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex;
        if (seats is null || !seats.Skip(next).Any(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive))
            return;
        if (attack.IsChainPropagation && r.Cursor == FengweiDamageCursor(attack) && (r.TransferredTarget ?? r.TargetSeat) == attack.TargetSeat)
        {
            SetFengweiChainAmount(attack, r.InheritedAmount);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FengweiDamageRebasedEvent(r.AttackId, r.TargetSeat, r.Cursor, r.InheritedAmount));
        }

        SetFengweiDamageState(attack.ResolutionId, null);
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 50)]
    internal void ApplyFengweiChainedDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (HasFengweiCapability && attack.IsChainPropagation && attack.DamageAmountFinalized)
            SetFengweiChainAmount(attack, checked(attack.DamageAmount + FengweiDamageBonus(attack)));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 27510)]
    internal void AssertFengweiDamageReceipts()
    {
        if (!HasFengweiCapability)
            return;
        foreach (var frame in _resolutionStackOperations._resolutionStack)
        {
            var r = FengweiDamageState(frame.Id);
            if (r is null)
                continue;
            if (!HasFengweiCapability || r.AttackId != frame.Id || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.Cursor < -1 || r.InheritedAmount < 0 || r.Bonus != 1 || r.InvoiceId <= 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<FengweiDamageFrozenEvent>().Count(e => e == new FengweiDamageFrozenEvent(r.AttackId, r.TargetSeat, r.Cursor, r.InheritedAmount, r.Bonus, r.InvoiceId)) != 1 || (r.TransferredTarget is { } target ? !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || r.TransferWindowId is not { } window || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<FengweiDamageTransferredEvent>().Count(e => e.AttackId == r.AttackId && e.OriginalSeat == r.TargetSeat && e.TargetSeat == target && e.Cursor == r.Cursor && e.WindowId == window) != 1 : r.TransferWindowId is not null))
                throw new InvalidOperationException("Fengwei lost its actual native recipient's once-issued modifier invoice.");
        }
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27510)]
    internal void ObserveFengweiDamage(IGameEvent payload)
    {
        if (!HasFengweiCapability)
            return;
        if (payload is ExtraCardEffectStartedEvent extra && _coreCardUseLifecycleOperations.LifecycleCardUse(extra.CardUseFrameId)is { FengweiDamage: not null, Action: { } repeatedAction } repeated && repeatedAction.ActionId == extra.ActionId)
            SetFengweiDamageState(repeated.Id, null);
        if (payload is ProgramDamageTransferredEvent e && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && FengweiDamageState(attack.ResolutionId)is { TransferredTarget: null } r && _resolutionStackOperations._resolutionStack.Count >= 2 && _resolutionStackOperations._resolutionStack[^1] is ProgramSkillFrame f && _resolutionStackOperations._resolutionStack[^2] is BeforeDamageProgramWindowFrame w && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.RedirectCurrentDamage && f.WindowContext?.ParentFrameId == w.Id && e.ResolutionId == attack.ResolutionId && (w.ContinuationAttackResolutionId ?? w.ParentFrameId) == attack.ResolutionId && r.TargetSeat == w.TargetSeat && w.RedirectedTargetSeat == attack.TargetSeat && e.TargetSeat == attack.TargetSeat && e.SourceSeat == attack.SourceSeat && e.DamageAmount == w.Amount && e.SkillId == f.SkillId && e.OwnerSeat == f.OwnerSeat && attack.DamageRedirected)
        {
            SetFengweiDamageState(attack.ResolutionId, r with { TransferredTarget = attack.TargetSeat, TransferWindowId = w.Id });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FengweiDamageTransferredEvent(r.AttackId, r.TargetSeat, attack.TargetSeat, r.Cursor, w.Id));
        }
    }
}
