#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenLinMouRuntime
{
    internal MouChenLinDamageReceipt? MouChenLinDamageState(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        CardUseFrame f => f.MouChenLinDamage,
        ProgramSkillFrame f => f.MouChenLinDamage,
        JudgmentFrame f => f.MouChenLinDamage,
        _ => null
    };
    internal void SetMouChenLinDamageState(long id, MouChenLinDamageReceipt? receipt)
    {
        var f = _resolutionStackOperations._resolutionStack.Single(x => x.Id == id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f switch
        {
            CardUseFrame c => c with { MouChenLinDamage = receipt },
            ProgramSkillFrame p => p with { MouChenLinDamage = receipt },
            JudgmentFrame j => j with { MouChenLinDamage = receipt },
            _ => throw new InvalidOperationException("Yaozuo damage needs its native owning attack frame.")});
    }

    internal int MouChenLinDamageCursor(GameEngine.IDamageAttempt a) => !a.IsChainPropagation ? -1 : a is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(a.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(a.ResolutionId).ChainedTargetIndex;
    internal MouChenLinDamageTicketEvent[] AvailableMouChenLinTickets(GameEngine.IDamageAttempt a) => a.IsSourceLess ? [] : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenLinDamageTicketEvent>().Where(t => t.Source.OwnerSeat == a.SourceSeat && t.Target == a.TargetSeat && t.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber && _contentRegistryOperations._contentRegistry.GetSkill(t.Source.SkillId).Program?.GameplayHash == t.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenLinInvitationClosedEvent>().Count(e => e.FrameId == t.FrameId) == 1 && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenLinDamageTicketConsumedEvent>().Any(e => e.FrameId == t.FrameId && e.Target == t.Target)).ToArray();
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 37000)]
    internal int ApplyMouChenLinNextDamage(GameEngine.IDamageAttempt a)
    {
        if (!HasMouChenLin || a.IsSourceLess || a.DamageAmount <= 0)
            return 0;
        var cursor = MouChenLinDamageCursor(a);
        var tickets = AvailableMouChenLinTickets(a);
        if (tickets.Length == 0)
            return 0;
        if (MouChenLinDamageState(a.ResolutionId)is { } prior && prior.Target == a.TargetSeat && prior.Source == a.SourceSeat && prior.ChainCursor == cursor && prior.Tickets.SequenceEqual(tickets.Select(t => t.FrameId)))
            return prior.Tickets.Count;
        var r = new MouChenLinDamageReceipt(a.ResolutionId, cursor, a.TargetSeat, a.SourceSeat, a.DamageAmount)
        {
            Tickets = tickets.Select(t => t.FrameId).ToArray()
        };
        SetMouChenLinDamageState(a.ResolutionId, r);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenLinDamageIssuedEvent(a.ResolutionId, a.TargetSeat, cursor, r.Inherited, tickets.Length));
        return tickets.Length;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 37000)]
    internal void ConsumeMouChenLinDamage(IGameEvent payload)
    {
        if (!HasMouChenLin)
            return;
        if (payload is ExtraCardEffectStartedEvent extra && _coreCardUseLifecycleOperations.LifecycleCardUse(extra.CardUseFrameId)is { MouChenLinDamage: not null } restarted && _coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(restarted.Id))
            SetMouChenLinDamageState(restarted.Id, null);
        if (payload is ProgramDamageTransferredEvent moved && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } a && MouChenLinDamageState(a.ResolutionId)is { TransferredTarget: null } original && a.DamageRedirected && moved.ResolutionId == a.ResolutionId && moved.SourceSeat == a.SourceSeat && moved.TargetSeat == a.TargetSeat && original.ChainCursor == MouChenLinDamageCursor(a))
            SetMouChenLinDamageState(a.ResolutionId, original with { TransferredTarget = moved.TargetSeat });
        if (payload is not DamageAppliedEvent { Amount: > 0 } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.IsSourceLess || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.ParentFrameId != attack.ResolutionId || damage.TargetSeat != applied.TargetSeat || damage.SourceSeat != applied.SourceSeat || damage.Amount != applied.Amount || MouChenLinDamageState(attack.ResolutionId)is not { } r || r.Source != applied.SourceSeat || (r.TransferredTarget ?? r.Target) != applied.TargetSeat || r.ChainCursor != MouChenLinDamageCursor(attack))
            return;
        foreach (var ticket in r.Tickets)
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouChenLinDamageTicketConsumedEvent>().Any(e => e.FrameId == ticket && e.Target == r.Target))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouChenLinDamageTicketConsumedEvent(ticket, r.Target, attack.ResolutionId, r.ChainCursor));
    }

    internal void SetMouChenLinChainAmount(GameEngine.IDamageAttempt a, int amount)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(a, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !a.IsChainPropagation || amount < 0)
            throw new InvalidOperationException("Yaozuo can rebase only its exact current native chained recipient.");
        if (a is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(a.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(a.ResolutionId, s => s!with { DamageAmount = amount });
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 37000)]
    internal void PrepareMouChenLinChain(GameEngine.IDamageAttempt a)
    {
        if (!HasMouChenLin || MouChenLinDamageState(a.ResolutionId)is not { } r)
            return;
        // Preserve the first recipient's native propagated amount. An extra
        // ticket added only to a later recipient does not leak to the next.
        if (a.IsChainPropagation && r.ChainCursor == MouChenLinDamageCursor(a) && (r.TransferredTarget ?? r.Target) == a.TargetSeat)
            SetMouChenLinChainAmount(a, r.Inherited);
        SetMouChenLinDamageState(a.ResolutionId, null);
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 37000)]
    internal void ApplyMouChenLinChain(GameEngine.IDamageAttempt a)
    {
        if (HasMouChenLin && a.IsChainPropagation && a.DamageAmountFinalized)
            SetMouChenLinChainAmount(a, checked(a.DamageAmount + ApplyMouChenLinNextDamage(a)));
    }
}
