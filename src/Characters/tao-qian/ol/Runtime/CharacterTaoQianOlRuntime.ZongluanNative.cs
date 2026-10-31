#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTaoQianOlRuntime
{
    internal CardActionContext PrepareZongluanSlashAction(CardActionContext action)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TaoQian: { Paid: true, Stage: TaoQianStage.SlashIssuing } r })
            return action;
        if (r.Op != SkillProgramEffectOp.ResolveZongluanPreparationSlashAndDiscard || r.UseId != 0 || action.Type != CardActionType.Use || action.EffectiveKind != CardKind.Slash || action.ActorSeat != r.ActorSeat || action.ProviderSeat != r.ActorSeat || action.RequesterSeat is not null || action.PhysicalCards.Count != 0 || action.ConversionChain is not [var source] || source != r.Source || !action.TargetSeats.SequenceEqual(r.Targets))
            throw new InvalidOperationException("Zongluan cannot change its issued actor, source, zero cost or original ordered targets.");
        return new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, Suit.None, 0, null, action.FactionOrigin);
    }

    internal void IssueZongluanSlash(ProgramSkillFrame f)
    {
        var r = f.TaoQian!;
        if (!r.Paid || r.Stage != TaoQianStage.SlashTargets || r.Targets.Count == 0 || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(s => !ZongluanTargets(r.ActorSeat).Contains(s)))
            throw new InvalidOperationException("Zongluan requires at least one real native legal target and cannot cancel its issued request.");
        var actor = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { TaoQian = r with { Stage = TaoQianStage.SlashIssuing } });
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, CardKind.Slash, Suit.None, 0), actor.Seat, r.Targets, CardKind.Slash, ignoresArmor: _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(actor), physicalCardIds: [], conversionSource: r.Source);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.ZongluanSlash is null)
            throw new InvalidOperationException("Zongluan was not bound before its real native declaration.");
        var target = use.TargetSeats[0];
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, target, IgnoresArmor: attack.IgnoresArmor));
        foreach (var seat in use.TargetSeats)
            _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[seat]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void BindZongluanSlashUse(long id)
    {
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame { TaoQian: { Paid: true, Stage: TaoQianStage.SlashIssuing } r } f)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (r.Op != SkillProgramEffectOp.ResolveZongluanPreparationSlashAndDiscard || r.UseId != 0 || use.CardId != 0 || use.CardKind != CardKind.Slash || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || a.ActorSeat != r.ActorSeat || a.ProviderSeat != r.ActorSeat || a.RequesterSeat is not null || a.ConversionChain is not [var source] || source != r.Source || !use.TargetSeats.SequenceEqual(r.Targets) || !a.TargetSeats.SequenceEqual(r.Targets) || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || a.EffectiveIsRed is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ZongluanCommittedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.ActorSeat == r.ActorSeat) != 1)
            throw new InvalidOperationException("Zongluan lost the exact source, actual actor, materials or paid multi-target request.");
        var origin = new ZongluanSlashOrigin(f.Id, r.Source, r.GameplayHash, r.ActorSeat, id, a.ActionId, r.ActualTurn, r.TurnOwner);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { TaoQian = r with { Stage = TaoQianStage.NativeSlash, UseId = id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { ZongluanSlash = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZongluanSlashIssuedEvent(origin, r.Targets.Count));
        for (var i = 0; i < r.Targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZongluanInitialTargetEvent(f.Id, id, i, r.Targets[i]));
    }

    internal bool IsIssuedZongluanUse(CardUseFrame use)
    {
        if (use.ZongluanSlash is not { } o || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } a || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || o.UseId != use.Id || o.ActionId != a.ActionId || a.ProviderSeat != o.ActorSeat || a.RequesterSeat is not null || _contentRegistryOperations._contentRegistry.GetSkill(o.Source.SkillId).Program?.GameplayHash != o.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<ZongluanSlashIssuedEvent>().Count(e => e.Origin == o) != 1 || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == o.ProgramFrameId)is not ProgramSkillFrame { TaoQian: { Paid: true, Stage: TaoQianStage.NativeSlash, NativeReturned: false } r } || r.UseId != use.Id || r.ActionId != a.ActionId || r.Source != o.Source || r.GameplayHash != o.GameplayHash || r.ActorSeat != o.ActorSeat || r.ActualTurn != o.ActualTurn || r.TurnOwner != o.TurnOwner)
            return false;
        var original = a;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            original = fire.OriginalAction;
        }
        else if (use.CardKind != CardKind.Slash)
            return false;
        if (original.EffectiveKind != CardKind.Slash || original.EffectiveSuit != Suit.None || original.EffectiveRank != 0 || original.EffectiveIsRed is not null || original.ConversionChain is not [var source] || source != o.Source || !_coreProgramEventHistoryOperations.ProgramEventHistory<ZongluanInitialTargetEvent>().Where(e => e.FrameId == o.ProgramFrameId && e.UseId == use.Id).OrderBy(e => e.Index).Select(e => e.TargetSeat).SequenceEqual(r.Targets))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.ActorSeat, o.ActorSeat) : _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == use.Id && use.SourceSeat == o.ActorSeat && a.ActorSeat == o.ActorSeat;
    }

    internal bool IsZongluanUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedZongluanUse(use);
    internal Card ZongluanRepresentation(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedZongluanUse(use) ? new(0, use.CardKind, Suit.None, 0) : throw new InvalidOperationException("Zongluan requires its exact issued native zero-entity Slash.");
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 80)]
    internal bool FinishZongluanAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsZongluanUse(attack.ResolutionId))
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

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28120)]
    internal void CaptureZongluanDirectDamage(IGameEvent payload)
    {
        if (payload is not DamageAppliedEvent { Amount: > 0 } e || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame d || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not NativeOwners.CardAttackHandle a || a.ResolutionId != d.ParentFrameId || a.IsChainPropagation || a.DamageRedirected || a.IsProgramSkillDamage || a.IsProgramJudgmentDamage || d.SourceSeat != e.SourceSeat || d.TargetSeat != e.TargetSeat || d.Amount != e.Amount || _coreCardUseLifecycleOperations.LifecycleCardUse(a.ResolutionId)is not { ZongluanSlash: { } o } use || !IsIssuedZongluanUse(use))
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZongluanDirectDamageEvent>().Any(x => x.DamageFrameId == d.Id))
            throw new InvalidOperationException("Zongluan cannot account for one actual Slash damage occurrence twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZongluanDirectDamageEvent(o.ProgramFrameId, use.Id, d.Id, d.SourceSeat, d.TargetSeat, e.Amount));
    }

    [NativeUseValidation(28120)]
    internal void ValidateZongluanNative(CardUseFrame use)
    {
        if (use.ZongluanSlash is not null && (!IsIssuedZongluanUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Zongluan cannot retire before its whole native Slash and real children finish.");
    }

    [NativeUseReturn(28120)]
    internal void ReturnZongluanNative(CardUseFrame use)
    {
        if (use.ZongluanSlash is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        var r = f.TaoQian!;
        if (r.Stage != TaoQianStage.NativeSlash || r.NativeReturned || r.UseId != use.Id || r.ActionId != o.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Zongluan lost its once-completed whole native Use return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TaoQian = r with { NativeReturned = true } });
        if (use.CardAttack is not { Active: true })
            SettleZongluanNative(o);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 28120)]
    internal bool ReturnZongluanAttack(GameEngine.AttackCompletionReceipt c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TaoQian: { Stage: TaoQianStage.NativeSlash, NativeReturned: true } r } f || c.ResolutionId != r.UseId)
            return false;
        if (c.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Zongluan's native Slash returned to another program.");
        SettleZongluanNative(_coreProgramEventHistoryOperations.ProgramEventHistory<ZongluanSlashIssuedEvent>().Single(e => e.Origin.ProgramFrameId == f.Id).Origin);
        return true;
    }

    internal void SettleZongluanNative(ZongluanSlashOrigin o)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        var r = f.TaoQian!;
        if (!r.NativeReturned || r.UseId != o.UseId || _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ZongluanSlashReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Zongluan cannot settle its whole Slash twice.");
        var count = _coreProgramEventHistoryOperations.ProgramEventHistory<ZongluanDirectDamageEvent>().Where(e => e.FrameId == f.Id && e.UseId == o.UseId && e.Amount > 0).Select(e => e.TargetSeat).Distinct().Count();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TaoQian = r with { Stage = TaoQianStage.NativeChildren, DamageRecipientCount = count }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZongluanSlashReturnedEvent(f.Id, o.UseId, o.ActionId, count));
    }
}
