#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuKangWumiaoRuntime
{
    internal CardActionContext PrepareWuLuKangKegouSlashAction(CardActionContext action)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuKangKegou: { Stage: WuLuKangKegouStage.SlashIssuing } r } f)
            return action;
        if (action.Type != CardActionType.Use || action.EffectiveKind != CardKind.Slash || action.ActorSeat != r.OpponentSeat!.Value || action.ProviderSeat != r.OpponentSeat!.Value || action.RequesterSeat is not null || action.PhysicalCards.Count != 0 || action.ConversionChain is not [var source] || source != r.Source || !action.TargetSeats.SequenceEqual(r.NativeTargets) || r.NativeUseId != 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangKegouPindianReturnedEvent>().Count(e => e.FrameId == f.Id && e.Iteration == r.Iteration) != 1)
            throw new InvalidOperationException("Kegou cannot prepare a different actor, target, source or material group before native use recording.");
        return new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, Suit.None, 0, null, action.FactionOrigin);
    }

    internal void IssueWuLuKangKegouSlash(ProgramSkillFrame f)
    {
        var r = f.WuLuKangKegou!;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.OpponentSeat!.Value];
        var target = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!actor.IsAlive || !target.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !CanWuLuKangKegouSlash(f))
        {
            ContinueWuLuKangKegouAfterSkippedSlash(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangKegou = r with { Stage = WuLuKangKegouStage.SlashIssuing, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, CardKind.Slash, Suit.None, 0), actor.Seat, r.NativeTargets, CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), physicalCardIds: [], conversionSource: r.Source);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.WuLuKangKegouSlash is null)
            throw new InvalidOperationException("Kegou's native Slash was not bound before its declaration.");
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, use.TargetSeats[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[0]], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && use.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, r.Source))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, use.TargetSeats[0], IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[0]]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void BindWuLuKangKegouSlashUse(CardUseFrame supplied)
    {
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == supplied.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame { WuLuKangKegou: { Stage: WuLuKangKegouStage.SlashIssuing } r } parent)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(supplied.Id)!;
        if (r.NativeUseId != 0 || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || a.ActorSeat != r.OpponentSeat!.Value || a.ProviderSeat != r.OpponentSeat!.Value || a.RequesterSeat is not null || a.ConversionChain is not [var source] || source != r.Source || !use.TargetSeats.SequenceEqual(r.NativeTargets) || !a.TargetSeats.SequenceEqual(use.TargetSeats) || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || a.EffectiveIsRed is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangKegouPindianReturnedEvent>().Count(e => e.FrameId == parent.Id && e.Iteration == r.Iteration) != 1)
            throw new InvalidOperationException("Kegou cannot change the real loser, winner, zero output or original Pindian payment.");
        var origin = new WuLuKangKegouSlashOrigin(parent.Id, r.Source, r.GameplayHash, r.Iteration, r.PindianId!.Value, r.OpponentSeat!.Value, parent.OwnerSeat, use.Id, a.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WuLuKangKegou = r with { Stage = WuLuKangKegouStage.NativeSlash, NativeUseId = use.Id, ActionId = a.ActionId, NativeReturned = false } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { WuLuKangKegouSlash = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangKegouSlashIssuedEvent(origin));
    }

    internal bool IsIssuedWuLuKangKegouUse(CardUseFrame use)
    {
        if (use.WuLuKangKegouSlash is not { } o || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } current || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || o.UseId != use.Id || o.ActionId != current.ActionId || current.ProviderSeat != o.ActorSeat || current.RequesterSeat is not null || _contentRegistryOperations._contentRegistry.GetSkill(o.Source.SkillId).Program?.GameplayHash != o.GameplayHash || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangKegouSlashIssuedEvent>().Count(e => e.Origin == o) != 1 || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == o.ProgramFrameId)is not ProgramSkillFrame { WuLuKangKegou: { Stage: WuLuKangKegouStage.NativeSlash, NativeReturned: false } r } parent || r.NativeUseId != use.Id || r.ActionId != o.ActionId || r.Source != o.Source || r.GameplayHash != o.GameplayHash || r.Iteration != o.Iteration || r.PindianId != o.PindianId || r.OpponentSeat != o.ActorSeat || parent.OwnerSeat != o.TargetSeat)
            return false;
        var original = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            original = fire.OriginalAction;
        }
        else if (use.CardKind != CardKind.Slash)
            return false;
        if (original.EffectiveKind != CardKind.Slash || original.EffectiveSuit != Suit.None || original.EffectiveRank != 0 || original.EffectiveIsRed is not null || original.ConversionChain is not [var source] || source != o.Source)
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.ActorSeat, o.ActorSeat) : _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && use.SourceSeat == o.ActorSeat && current.ActorSeat == o.ActorSeat;
    }

    internal bool IsWuLuKangKegouUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedWuLuKangKegouUse(use);
    internal Card WuLuKangKegouRepresentation(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedWuLuKangKegouUse(use) ? new(0, use.CardKind, Suit.None, 0) : throw new InvalidOperationException("Kegou requires its issued native zero-entity representation.");
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 60)]
    internal bool FinishWuLuKangKegouAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsWuLuKangKegouUse(attack.ResolutionId))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    [NativeUseValidation(29700)]
    internal void ValidateWuLuKangKegouNative(CardUseFrame use)
    {
        if (use.WuLuKangKegouSlash is not null && (!IsIssuedWuLuKangKegouUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Kegou cannot retire before its exact whole native Slash completes.");
    }

    [NativeUseReturn(29700)]
    internal void ReturnWuLuKangKegouNative(CardUseFrame use)
    {
        if (use.WuLuKangKegouSlash is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        var r = f.WuLuKangKegou!;
        if (r.Stage != WuLuKangKegouStage.NativeSlash || r.NativeReturned || r.NativeUseId != use.Id || r.ActionId != o.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Kegou lost its unique typed native return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuLuKangKegou = r with { NativeReturned = true } });
        if (use.CardAttack is not { Active: true })
            SettleWuLuKangKegouNative(o);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 29700)]
    internal bool ReturnWuLuKangKegouAttack(GameEngine.AttackCompletionReceipt c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuKangKegou: { NativeReturned: true, Stage: WuLuKangKegouStage.NativeSlash } r } f || c.ResolutionId != r.NativeUseId)
            return false;
        if (c.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Kegou's native attack returned to a different program.");
        var o = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangKegouSlashIssuedEvent>().Single(e => e.Origin.ProgramFrameId == f.Id && e.Origin.Iteration == r.Iteration).Origin;
        SettleWuLuKangKegouNative(o);
        return true;
    }

    internal void ContinueWuLuKangKegouAfterSkippedSlash(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangKegou = f.WuLuKangKegou!with { Stage = WuLuKangKegouStage.RepeatChoice } });
        ContinueWuLuKangKegou(f.Id);
    }

    internal void SettleWuLuKangKegouNative(WuLuKangKegouSlashOrigin o)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        var r = f.WuLuKangKegou!;
        if (!r.NativeReturned || r.NativeUseId != o.UseId || r.ActionId != o.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WuLuKangKegouSlashReturnedEvent>().Any(e => e.FrameId == f.Id && e.Iteration == r.Iteration))
            throw new InvalidOperationException("Kegou cannot settle its native Use twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuLuKangKegou = r with { Stage = WuLuKangKegouStage.NativeSlashChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangKegouSlashReturnedEvent(f.Id, r.Iteration, o.UseId, o.ActionId));
        ContinueWuLuKangKegou(f.Id);
    }
}
