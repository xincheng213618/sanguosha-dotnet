#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuFengBoundaryRuntime
{
    internal void IssueBoundaryLiuFengSlash(ProgramSkillFrame f)
    {
        var r = f.BoundaryLiuFeng!;
        if (!ValidBoundaryLiuFeng(f) || r.Stage != BoundaryLiuFengStage.Issuing || r.Targets is not [var target])
            throw new InvalidOperationException("Qinling must issue from its exact once-paid cost frame.");
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var id = _coreNativeCardUseOperations.BeginCardUse(new(0, CardKind.Slash, Suit.None, 0), actor.Seat, [target], CardKind.Slash, physicalCardIds: [], conversionSource: r.Need is null ? null : r.Source);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.BoundaryLiuFeng is not { FrameId: var producer } || producer != f.Id)
            throw new InvalidOperationException("Qinling lost its original zero-material use origin.");
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, use.TargetSeats[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[0]], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        if (r.Need?.Intent == BoundaryLiuFengIntent.BorrowedSword)
            _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!.ActiveAttack = attack;
        _characterLvKuangLvXiangOlOperations.CompleteQigongNativePayment(id);
        _characterCaoChunOlOperations.CompleteCaoChunNativePayment(id);
        if (BoundaryLiuFengCountsSlash(f) && use.UnlimitedUse != true)
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, use.TargetSeats[0], IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[0]]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 50201)]
    internal void BindBoundaryLiuFengUse(CardUseDeclaredEvent e)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)is { } use)
            BindBoundaryLiuFengNativeUse(use);
    }

    internal void BindBoundaryLiuFengNativeUse(CardUseFrame use)
    {
        if (use.BoundaryLiuFeng is not null || use.Action is not { Type: CardActionType.Use } a)
            return;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1] is not ProgramSkillFrame { BoundaryLiuFeng: { Stage: BoundaryLiuFengStage.Issuing } r } f)
            return;
        if (!ValidBoundaryLiuFeng(f) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || a.PhysicalCards.Count != 0 || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.EffectiveKind != CardKind.Slash || !a.ConversionChain.SequenceEqual(r.Need is null ? [] : new[] { r.Source }) || !r.Targets.SequenceEqual(use.TargetSeats) || r.CostSequence <= 0)
            throw new InvalidOperationException("Qinling cannot turn its discarded cost into use materials or change its real actor.");
        var origin = new BoundaryLiuFengUseOrigin(f.Id, r.Source, r.Hash, use.Id, a.ActionId, r.Turn, r.TurnOwner, f.OwnerSeat, 1 + _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].AlcoholDamageBonus);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { BoundaryLiuFeng = r with { Stage = BoundaryLiuFengStage.NativeUse, UseId = use.Id, ActionId = a.ActionId, BaseDamage = origin.BaseDamage } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { BoundaryLiuFeng = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengUseIssuedEvent(origin));
        if (r.Need is { } n)
        {
            if (_resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == n.OwnerFrameId)is ProgramSkillFrame { BoundaryLiuFengCaller: { } caller } parent)
                _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { BoundaryLiuFengCaller = caller with { UseId = use.Id, ActionId = a.ActionId } });
            if (n.Intent == BoundaryLiuFengIntent.Qinglong)
            {
                var original = _coreCardUseLifecycleOperations.LifecycleCardUse(n.OwnerFrameId)!;
                var suspension = original.Continuations.QinglongFollowup ?? throw new InvalidOperationException("Qinling lost its suspended Qinglong request.");
                if (suspension.FollowupUseId is not null)
                    throw new InvalidOperationException("Qinling cannot issue two follow-ups for one Qinglong request.");
                _coreCardAttackStateOperations.UpdateCardContinuations(original.Id, s => s with { QinglongFollowup = suspension with { FollowupUseId = use.Id } });
            }
        }
    }

    internal bool IsBoundaryLiuFengUse(CardUseFrame use)
    {
        if (use.BoundaryLiuFeng is not { } o || o.UseId != use.Id || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } current || current.ActionId != o.ActionId || current.ProviderSeat != o.Actor || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || current.EffectiveKind != use.CardKind)
            return false;
        var a = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            a = fire.OriginalAction;
        }

        var actorMatches = _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Actor, o.Actor) || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) && use.SourceSeat == o.Actor && a.ActorSeat == o.Actor;
        return actorMatches && a.EffectiveKind == CardKind.Slash && (a.ConversionChain.Count == 0 || a.ConversionChain is [var s] && s == o.Source) && _contentRegistryOperations._contentRegistry.GetSkill(o.Source.SkillId).Program?.GameplayHash == o.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengUseIssuedEvent>().Count(e => e.Origin == o) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengCostPaidEvent>().Count(e => e.FrameId == o.FrameId && e.Source == o.Source && e.Hash == o.Hash && e.Turn == o.Turn && e.TurnOwner == o.TurnOwner) == 1;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 50201)]
    internal bool CompleteBoundaryLiuFengAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { BoundaryLiuFeng: { } o } use || !IsBoundaryLiuFengUse(use))
            return false;
        var next = use.TargetIndex + 1;
        while (next < use.TargetSeats.Count && !_coreMatchParticipantsPlayersAccess._players[use.TargetSeats[next]].IsAlive)
            next++;
        if (!_characterDaQiaoLeOperations.IsTargetScopedZixiEffectRound(use.Id) && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && next < use.TargetSeats.Count)
        {
            _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(use.Id, next);
            _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
            var continued = _kernelCoreMainOperations.CreateCardAttackHandle(use.Id, use.SourceSeat, use.TargetSeats[next], card: null, damageAmount: use.ProgramAdjustedSlashBaseDamage ?? o.BaseDamage, playedCardKind: use.CardKind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[next]], use.CardKind), programSkillCardUseFrameId: o.FrameId);
            continued.SetCardUseCausedDamage(attack.CardUseCausedDamage);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = continued;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(continued);
            paused = true;
            return true;
        }

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

    [NativeUseValidation(50201)]
    internal void ValidateBoundaryLiuFengUse(CardUseFrame use)
    {
        if (use.BoundaryLiuFeng is not null && (!IsBoundaryLiuFengUse(use) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Qinling must await the whole real Slash and its native children.");
    }

    [NativeUseReturn(1)]
    internal void ReturnBoundaryLiuFengUse(CardUseFrame use)
    {
        if (use.BoundaryLiuFeng is not { } o)
            return;
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == o.FrameId);
        var r = f.BoundaryLiuFeng!;
        if (!IsBoundaryLiuFengUse(use) || r.Returned || r.UseId != use.Id || r.ActionId != o.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null)
            throw new InvalidOperationException("Qinling native return changed its original once-paid frame.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { BoundaryLiuFeng = r with { Returned = true, Damaged = use.CausedDamage } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengUseReturnedEvent(f.Id, use.Id, o.ActionId, use.CausedDamage));
        if (r.Need is { } n && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == n.OwnerFrameId)is ProgramSkillFrame { BoundaryLiuFengCaller: { } caller } parent)
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { BoundaryLiuFengCaller = caller with { Returned = true } });
        if (use.CardAttack is not { Active: true })
            ReturnBoundaryLiuFengTail(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 50201)]
    internal bool ReturnBoundaryLiuFengAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryLiuFeng: { Stage: BoundaryLiuFengStage.NativeUse, Returned: true } } f)
            return false;
        ReturnBoundaryLiuFengTail(f);
        return true;
    }

    internal void ReturnBoundaryLiuFengTail(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = f.BoundaryLiuFeng!with { Stage = BoundaryLiuFengStage.NativeTail, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueBoundaryLiuFeng(f.Id);
    }

    internal void SettleBoundaryLiuFeng(ProgramSkillFrame f)
    {
        var r = f.BoundaryLiuFeng!;
        var move = !r.Damaged && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.Cards is [var card] && _cardZonesOperations._cardZones.GetLocation(card) == CardLocation.DiscardPile;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = r with { ConsequencePaid = true, Movement = "reward", Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengConsequenceEvent(f.Id, r.UseId, r.Damaged, move));
        if (move)
        {
            BeginBoundaryLiuFengMovement(f, [r.Cards[0]], CardLocation.Authority(f.OwnerSeat), "inverse");
            return;
        }

        if (r.Damaged && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = f.BoundaryLiuFeng!with { Stage = BoundaryLiuFengStage.RewardDraw }, PendingMovementContinuation = null });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill-program.boundary:qinling.reward"));
            return;
        }

        ContinueBoundaryLiuFeng(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBoundaryLiuFengQinling)]
    internal void ReturnBoundaryLiuFengRewardDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!ValidBoundaryLiuFeng(f) || f.BoundaryLiuFeng is not { Stage: BoundaryLiuFengStage.RewardDraw, ConsequencePaid: true, Returned: true, Damaged: true, RewardDraw: null } r || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengRewardDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Qinling reward lost its original once-issued native draw and typed return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryLiuFeng = r with { Stage = BoundaryLiuFengStage.NativeTail, RewardDraw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiuFengRewardDrawReturnedEvent(f.Id, r.UseId, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryLiuFeng(f.Id);
    }
}
