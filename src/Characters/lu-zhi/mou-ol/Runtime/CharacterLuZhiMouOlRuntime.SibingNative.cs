#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiMouOlRuntime
{
    internal void IssueMouLzCounter(ProgramSkillFrame f)
    {
        var r = f.MouLuZhiOl!;
        if (!r.Paid || !MouLzSlashTargets(f.OwnerSeat).Contains(r.CounterTarget))
        {
            FinishMouLz(f);
            return;
        }

        var outer = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.OriginalId);
        var wasActive = outer is CardUseFrame { CardAttack.Active: true };
        if (wasActive)
            _coreCardAttackStateOperations.UpdateCardAttackState(r.OriginalId, a => a!with { Active = false });
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhiOl = r with { Stage = MouLuZhiOlStage.Issuing, OuterWasActive = wasActive } });
        _coreProgramVirtualSlashOffersOperations.BeginOfferedProgramVirtualSlash(f, f.OwnerSeat, r.CounterTarget);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 32901)]
    internal void BindMouLzCounter(IGameEvent fact)
    {
        if (fact is not CardUseDeclaredEvent d || _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId)is not { } use)
            return;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1] is not ProgramSkillFrame { MouLuZhiOl: { Stage: MouLuZhiOlStage.Issuing, Paid: true } r } f)
            return;
        if (!ValidMouLz(f) || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || !a.TargetSeats.SequenceEqual([r.CounterTarget]))
            throw new InvalidOperationException("Sibing cannot bind another native zero-material actor or target.");
        var o = new MouLuZhiOlCounterOrigin(f.Id, r.Source, r.GameplayHash, r.OriginalId, r.OriginalActionId, use.Id, a.ActionId, f.OwnerSeat, r.CounterTarget);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouLuZhiOl = r with { Stage = MouLuZhiOlStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouLuZhiOlCounter = o });
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && a.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && use.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(_coreMatchParticipantsPlayersAccess._players[a.ActorSeat], null))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(use.Id, a.ActorSeat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(a.ActorSeat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlCounterIssuedEvent(o));
    }

    internal bool IsMouLzCounterUse(CardUseFrame u) => u.MouLuZhiOlCounter is { } o && u.Id == o.UseId && u.CardId == 0 && u.CardKind == CardKind.Slash && u.PhysicalCardIds is { Count: 0 } && u.Action is { Type: CardActionType.Use, PhysicalCards.Count: 0 } a && a.ActionId == o.ActionId && a.ProviderSeat == o.ActorSeat && a.RequesterSeat is null && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is ProgramSkillFrame { MouLuZhiOl: { Stage: MouLuZhiOlStage.NativeUse, Paid: true } r } f && ValidMouLz(f) && r.Source == o.Source && r.GameplayHash == o.GameplayHash && r.UseId == u.Id && r.ActionId == a.ActionId && r.CounterTarget == o.TargetSeat && o.ActorSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiOlCounterIssuedEvent>().Count(e => e.Origin == o) == 1 && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(u, o.ActorSeat, o.ActorSeat);

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 32901)]
    internal bool FinishMouLzCounterAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouLuZhiOlCounter: not null } u || !IsMouLzCounterUse(u))
            return false;
        _coreNativeCardUseFramesOperations.SetCardUseStep(u.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(u.Id, 0, CardKind.Slash));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, u.Action!, SkillProgramTriggerWindow.CardUseCompleted, u.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(u.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(32901)]
    internal void ValidateMouLzCounter(CardUseFrame u)
    {
        if (u.MouLuZhiOlCounter is not null && (!IsMouLzCounterUse(u) || u.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Sibing must complete its real native counter Use.");
    }

    [NativeUseReturn(32901)]
    internal void ReturnMouLzCounter(CardUseFrame u)
    {
        if (u.MouLuZhiOlCounter is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MouLuZhiOl!;
        if (r.UseId != u.Id || r.ActionId != o.ActionId || r.NativeReturned || _coreCardUseLifecycleOperations.LifecycleCardUse(u.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == u.Id) != 1)
            throw new InvalidOperationException("Sibing lost its exact counter Use return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuZhiOl = r with { NativeReturned = true } });
        if (u.CardAttack is not { Active: true })
            CompleteMouLzCounter(o.FrameId);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 32901)]
    internal bool ReturnMouLzCounterAttack(GameEngine.AttackCompletionReceipt c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuZhiOl: { Stage: MouLuZhiOlStage.NativeUse, NativeReturned: true } r } f || r.UseId != c.ResolutionId)
            return false;
        if (c.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Sibing counter returned to another frame.");
        CompleteMouLzCounter(f.Id);
        return true;
    }

    internal void CompleteMouLzCounter(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouLuZhiOl!;
        if (!r.NativeReturned || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiOlCounterReturnedEvent>().Any(e => e.FrameId == id))
            throw new InvalidOperationException("Sibing counter cannot return twice.");
        if (r.OuterWasActive && _coreCardUseLifecycleOperations.LifecycleCardUse(r.OriginalId)?.CardAttack is { Active: false })
            _coreCardAttackStateOperations.UpdateCardAttackState(r.OriginalId, a => a!with { Active = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlCounterReturnedEvent(id, r.UseId, r.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuZhiOl = r with { Stage = MouLuZhiOlStage.NativeReturned } });
        ContinueMouLz(id);
    }

    internal bool MouLzCannotRespond(int source, int responder, CardKind kind, IReadOnlyList<int> targets)
    {
        // Innermost incoming native effect only: an ancestor ban cannot affect a nested different Use.
        var incoming = _resolutionStackOperations._resolutionStack.LastOrDefault(x => x is CardUseFrame || x is ProgramSkillFrame { MouLuZhiOlDirectDuel: not null, CardAttack.Active: true });
        long id;
        long action;
        var instruction = 0;
        if (incoming is CardUseFrame { Action: { } a } u)
        {
            if (a.ActorSeat != source || a.EffectiveKind != kind)
                return false;
            id = u.Id;
            action = a.ActionId;
        }
        else if (incoming is ProgramSkillFrame { MouLuZhiOlDirectDuel: { } d })
        {
            if (d.Actor != source || kind != CardKind.Duel)
                return false;
            id = d.FrameId;
            action = 0;
            instruction = d.Instruction;
        }
        else
            return false;
        return targets.Contains(responder) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiOlResponseBannedEvent>().Any(e => e.Ban.UseId == id && e.Ban.ActionId == action && e.Ban.TargetSeat == responder && e.Ban.Instruction == instruction);
    }

    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 32900)]
    internal bool RecordMouLzDamage(GameEngine.IDamageAttempt attack, long damageId)
    {
        if (attack is not NativeOwners.CardAttackHandle a || !a.DamageWasApplied || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damageId)is not DamageFrame { Amount: > 0 } d)
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        var direct = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == attack.ResolutionId && f.MouLuZhiOlDirectDuel is not null);
        if (use is null && direct is null || use is not null && !IsMouLzDamageCard(use.CardKind))
            return false;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuZhiOlUseDamageEvent>().Any(e => e.DamageId == damageId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlUseDamageEvent(attack.ResolutionId, direct?.MouLuZhiOlDirectDuel?.Instruction ?? 0, damageId, d.TargetSeat, d.Amount));
        return false;
    }
}
