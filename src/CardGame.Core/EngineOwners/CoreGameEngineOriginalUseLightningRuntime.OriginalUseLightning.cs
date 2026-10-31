#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOriginalUseLightningRuntime
{
    internal void BeginOriginalUseLightning(long useId, CardConversionSource source, string gameplayHash)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("Lightning lost its native Use.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != useId || use.OriginalUseLightning is not null || use.CardId != 0 || use.CardKind != CardKind.Lightning || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } action || action.ActorSeat != use.SourceSeat || action.ProviderSeat != action.ActorSeat || action.RequesterSeat is not null || action.ResponderSeat is not null || action.EffectiveKind != CardKind.Lightning || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || action.EffectiveIsRed is not null || !action.ConversionChain.SequenceEqual([source]) || source.OwnerSeat != use.SourceSeat || !action.TargetSeats.SequenceEqual(use.TargetSeats) || use.TargetSeats.Count == 0 || use.TargetSeats.Distinct().Count() != use.TargetSeats.Count || use.TargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Program?.GameplayHash != gameplayHash)
            throw new InvalidOperationException("Held Lightning requires its exact accepted empty-material native Use and source.");
        var origin = new OriginalUseLightningOrigin(use.Id, action.ActionId, source, gameplayHash);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { OriginalUseLightning = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalUseLightningIssuedEvent(origin));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(use.Id, new(0, CardKind.Lightning, Suit.None, 0), use.SourceSeat, use.TargetSeats, LegalActionKind.Lightning, playedCardKind: CardKind.Lightning);
    }

    internal bool IsOriginalUseLightning(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { OriginalUseLightning: { } origin, CardId: 0, CardKind: CardKind.Lightning, PhysicalCardIds.Count: 0, Action: { Type: CardActionType.Use, PhysicalCards.Count: 0 } action } use && origin.UseId == id && origin.ActionId == action.ActionId && action.EffectiveKind == CardKind.Lightning && action.ActorSeat == use.SourceSeat && action.ProviderSeat == use.SourceSeat && action.EffectiveSuit == Suit.None && action.EffectiveRank == 0 && action.EffectiveIsRed is null && action.RequesterSeat is null && action.ResponderSeat is null && origin.Source.OwnerSeat == use.SourceSeat && action.ConversionChain.SequenceEqual([origin.Source]) && action.TargetSeats.SequenceEqual(use.TargetSeats) && _coreProgramEventHistoryOperations.ProgramEventHistory<OriginalUseLightningIssuedEvent>().Count(e => e.Origin == origin) == 1;
    [NativeUseValidation(37450)]
    internal void ValidateOriginalUseLightning(CardUseFrame use)
    {
        if (use.OriginalUseLightning is not null && !IsOriginalUseLightning(use.Id))
            throw new InvalidOperationException("Lightning lost its original empty-material native Use.");
    }

    internal bool MatchesOriginalUseLightningAction(long id, CardActionContext action, int cardId) => cardId == 0 && IsOriginalUseLightning(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action is { } issued && action.ActionId == issued.ActionId && action.Type == issued.Type && action.ActorSeat == issued.ActorSeat && action.ProviderSeat == issued.ProviderSeat && action.RequesterSeat == issued.RequesterSeat && action.ResponderSeat == issued.ResponderSeat && action.EffectiveKind == issued.EffectiveKind && action.EffectiveSuit == issued.EffectiveSuit && action.EffectiveRank == issued.EffectiveRank && action.EffectiveIsRed == issued.EffectiveIsRed && action.PhysicalCards.Count == 0 && action.ConversionChain.SequenceEqual(issued.ConversionChain) && action.TargetSeats.SequenceEqual(issued.TargetSeats);
    internal bool BeginOriginalUseLightningFinalized(long id, Card card, LegalActionKind actionKind)
    {
        if (!IsOriginalUseLightning(id) || card.Id != 0 || actionKind != LegalActionKind.Lightning)
            throw new InvalidOperationException("Lightning finalized targets lost their original zero-material Use.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (use.FinalizedTrickProgramsStarted)
            return false;
        _coreCardUseLifecycleOperations.TryMarkProgramUseAccepted(id);
        if (!_coreCardUseLifecycleOperations.TryMarkFinalizedTrickProgramsStarted(id))
            return false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(use.Action!));
        return _coreCardActionsOperations.TryBeginProgramCardWindow(null, use.Action!, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, ProgramCardContinuation.FinalizedDelayedPlacement, trickContinuation: new(0, LegalActionKind.Lightning));
    }

    internal bool IsOriginalUseLightningFinalizedWindow(ProgramCardTriggerWindowFrame window) => window.Continuation == ProgramCardContinuation.FinalizedDelayedPlacement && window.TrickContinuation is { EffectCardId: 0, ActionKind: LegalActionKind.Lightning, TargetCardId: null, RequiredCardKind: null } && window.SimpleContinuation is null && window.CompletedResponseReturn is null && window.AttackOwnerFrameId is null && IsOriginalUseLightning(window.ParentFrameId) && _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is { FinalizedTrickProgramsStarted: true, ProgramUseAccepted: true } && MatchesOriginalUseLightningAction(window.ParentFrameId, window.Action, 0);
    internal void ContinueOriginalUseLightningFinalized(ProgramCardTriggerWindowFrame window)
    {
        if (!IsOriginalUseLightningFinalizedWindow(window) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != window.ParentFrameId)
            throw new InvalidOperationException("Lightning finalized targets lost their typed native parent.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)!;
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(use.Id, new(0, CardKind.Lightning, Suit.None, 0), use.SourceSeat, use.TargetSeats, LegalActionKind.Lightning, playedCardKind: CardKind.Lightning);
    }

    internal IReadOnlyList<int> OriginalUseLightningPlacementTargets(NativeOwners.JizhiResolution pending) => IsOriginalUseLightning(pending.ResolutionId) ? _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)!.TargetSeats.Skip(_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)!.TargetIndex).Take(1).ToArray() : pending.TargetSeats;
    internal bool PlaceOriginalUseLightning(NullificationWindowFrame pending)
    {
        if (!IsOriginalUseLightning(pending.ParentFrameId))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId)!;
        if (pending.EffectCardId != 0 || pending.EffectCardKind != CardKind.Lightning || pending.ActionKind != LegalActionKind.Lightning || pending.TargetSeats is not [var seat] || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || seat != use.TargetSeats[use.TargetIndex])
            throw new InvalidOperationException("Lightning placement changed its original target cursor.");
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        if (!target.IsAlive || target.JudgmentAreaAbolished || _coreActualTurnsOperations.HasJudgmentEffectiveCard(target, CardKind.Lightning) || _coreProgramCardPoliciesOperations.HasCardPolicy(target, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) || use.IneffectiveTargetSeats?.Contains(seat) == true)
        {
            _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(use.Id, seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(use.Id, use.SourceSeat, seat, CardKind.Lightning, CardEffectSkipReason.PublicTargetMissing));
        }
        else
        {
            AttachOriginalUseLightning(use, use.OriginalUseLightning!.Source, use.OriginalUseLightning.GameplayHash, seat);
            if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(use.Id))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Lightning, use.SourceSeat, seat));
            _coreMatchLogOperations.AddLog("CardEffect", $"{(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].Name)} 对 {(target.Name)} 使用【闪电】，留下延时效果。", use.SourceSeat, seat);
        }

        _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, new(0, CardKind.Lightning, Suit.None, 0), CardKind.Lightning);
        return true;
    }

    internal OriginalUseLightningAttachment AttachOriginalUseLightning(CardUseFrame use, CardConversionSource source, string gameplayHash, int target)
    {
        if (!IsOriginalUseLightning(use.Id) || use.OriginalUseLightning is not { } origin || origin.Source != source || origin.GameplayHash != gameplayHash || !use.TargetSeats.Contains(target) || _coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Lightning) || _coreProgramEventHistoryOperations.ProgramEventHistory<OriginalUseLightningPlacedEvent>().Any(e => e.Attachment.Origin == origin && e.Attachment.OriginalTarget == target))
            throw new InvalidOperationException("Lightning cannot replace or duplicate its original Use placement.");
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var attachment = new OriginalUseLightningAttachment(id, origin, target, target, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
        _coreMatchParticipantsPlayersAccess._players[target].HeldOriginalUseLightning = _coreMatchParticipantsPlayersAccess._players[target].HeldOriginalUseLightning.Append(attachment).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalUseLightningPlacedEvent(attachment));
        return attachment;
    }

    internal bool TryContinueOriginalUseLightningPlacement(long id)
    {
        if (!IsOriginalUseLightning(id))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || use.TargetIndex + 1 >= use.TargetSeats.Count)
            return false;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, f => f with { TargetIndex = f.TargetIndex + 1 });
        _coreNativeNullificationOperations.ContinueJizhiOrNullificationAfterTargetTriggers(new(use.SourceSeat, id, new(0, CardKind.Lightning, Suit.None, 0), CardKind.Lightning, use.TargetSeats, LegalActionKind.Lightning, null, null));
        return true;
    }

    internal bool ValidOriginalUseLightning(OriginalUseLightningAttachment a) => a.Id > 0 && a.Origin.UseId > 0 && a.Origin.ActionId > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(a.Origin.Source.OwnerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(a.OriginalTarget) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(a.Holder) && a.AfterMovement >= 0 && a.Order > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<OriginalUseLightningIssuedEvent>().Count(e => e.Origin == a.Origin) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<OriginalUseLightningPlacedEvent>().Count(e => e.Attachment.Id == a.Id && e.Attachment.Origin == a.Origin && e.Attachment.OriginalTarget == a.OriginalTarget && e.Attachment.PlacedTurn == a.PlacedTurn) == 1;
    internal OriginalUseLightningAttachment? HeldOriginalUseLightning(long id) => _coreMatchParticipantsPlayersAccess._players.SelectMany(p => p.HeldOriginalUseLightning).SingleOrDefault(a => a.Id == id);
    internal bool ExactOriginalUseLightning(OriginalUseLightningAttachment a) => ValidOriginalUseLightning(a) && HeldOriginalUseLightning(a.Id) == a;
    internal bool HasHeldOriginalUseLightning(CharacterState player) => player.HeldOriginalUseLightning.Any(ValidOriginalUseLightning);
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendOriginalUseLightningMarker(int seat, IReadOnlyList<PlayerMarkerSnapshot>? markers) => HasHeldOriginalUseLightning(_coreMatchParticipantsPlayersAccess._players[seat]) ? Array.AsReadOnly((markers ?? []).Append(new PlayerMarkerSnapshot(PlayerMarkerKind.OriginalUseLightning, "闪电", _coreMatchParticipantsPlayersAccess._players[seat].HeldOriginalUseLightning.Count)).ToArray()) : markers;
    internal bool TryBeginHeldOriginalUseLightning(CharacterState current, int? excludedCard)
    {
        var attachment = current.HeldOriginalUseLightning.Where(a => a.LastJudgmentTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && ValidOriginalUseLightning(a)).OrderBy(a => a.AfterMovement).ThenBy(a => a.Order).FirstOrDefault();
        if (attachment is null)
            return false;
        var physical = _corePhysicalOwnedCardQueriesOperations.GetJudgment(current).FirstOrDefault(c => !_characterDaQiaoLeOperations.IsZixiQinJudgmentCard(c.Id) && CoreNativeCardClassificationsRuntime.IsDelayedCard(_coreActualTurnsOperations.GetJudgmentEffectiveCardKind(c)) && c.Id != excludedCard);
        if (physical is not null && (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == physical.Id)?.Sequence ?? 0) <= attachment.AfterMovement)
            return false;
        if (!current.IsAlive || current.JudgmentAreaAbolished || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber < 1 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Draw || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != current.Seat)
            throw new InvalidOperationException("Held Lightning requires its holder's actual judgment boundary.");
        attachment = attachment with
        {
            LastJudgmentTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber
        };
        ReplaceHeldOriginalUseLightning(attachment);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalUseLightningVisitedEvent(attachment.Id, current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber));
        var parent = _resolutionStackOperations._resolutionStack.LastOrDefault();
        var delayed = new DelayedEffectContinuation(0, CardKind.Lightning, current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase, parent?.Id ?? 0, parent?.Kind, parent?.Step)
        {
            OriginalUseLightning = attachment,
            OriginalUseLightningExcludedCardId = excludedCard
        };
        var window = new NullificationWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, delayed.ParentFrameId, current.Seat, [current.Seat], 0, CardKind.Lightning, LegalActionKind.Lightning, null, null, _coreNativeNullificationOperations.BuildNullificationCandidateSeats(current.Seat))
        {
            DelayedReturn = delayed
        };
        _coreRuntimeOperations.PushRuntimeFrame(window);
        _coreNativeNullificationOperations.ContinueNullificationWindow(window);
        return true;
    }

    internal bool ValidOriginalUseLightningDelayed(DelayedEffectContinuation d) => d.OriginalUseLightning is { } a && ExactOriginalUseLightning(a) && d.CardId == 0 && d.EffectiveKind == CardKind.Lightning && d.OwnerSeat == a.Holder && d.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && d.Phase == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == a.Holder;
    internal void CompleteOriginalUseLightningNullification(DelayedEffectContinuation delayed, bool nullified, long windowId)
    {
        if (!ValidOriginalUseLightningDelayed(delayed))
            throw new InvalidOperationException("Held Lightning lost its exact counterspell return.");
        var a = delayed.OriginalUseLightning!;
        if (SkipUnavailableOriginalUseLightning(a, delayed.OriginalUseLightningExcludedCardId))
            return;
        if (nullified)
        {
            TransferOriginalUseLightning(a, windowId, null, delayed.OriginalUseLightningExcludedCardId);
            return;
        }

        if (_coreCardEffectProgramsOperations.HasCardEffectBeforeApplyPrograms)
        {
            var action = new CardActionContext(0, null, CardActionType.Use, a.Holder, a.Holder, null, null, null, CardKind.Lightning, [a.Holder], [], [], [a.Holder]);
            if (_coreCardEffectProgramsOperations.BeginCardEffectPrograms(action, [a.Holder], null, delayed))
                return;
        }

        BeginOriginalUseLightningJudgment(delayed);
    }

    internal void BeginOriginalUseLightningJudgment(DelayedEffectContinuation delayed)
    {
        if (!ValidOriginalUseLightningDelayed(delayed))
            throw new InvalidOperationException("Held Lightning lost its exact effect return.");
        var a = delayed.OriginalUseLightning!;
        if (SkipUnavailableOriginalUseLightning(a, delayed.OriginalUseLightningExcludedCardId))
            return;
        var result = _coreNativeJudgmentOperations.BeginJudgment(null, a.Holder, JudgmentReasons.Lightning, 0, CardKind.Lightning, JudgmentContinuationKind.Lightning, sourceSeat: a.Holder, originalUseLightning: a, originalUseLightningExcludedCardId: delayed.OriginalUseLightningExcludedCardId);
        if (result is null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal bool IsOriginalUseLightningJudgment(JudgmentFrame j) => j.OriginalUseLightning is { } a && ValidOriginalUseLightning(a) && j.TargetSeat == a.Holder && j.SourceSeat == a.Holder && j.ParentFrameId == 0 && j.ParentAttackId is null && j.DelayedCardId is null && j.Reason == JudgmentReasons.Lightning && j.Continuation == JudgmentContinuationKind.Lightning && _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentRequestedEvent>().Count(e => e.ResolutionId == j.Id && e.ParentResolutionId == 0 && e.TargetSeat == a.Holder && e.Reason == JudgmentReasons.Lightning && e.SourceCard == CardKind.Lightning) == 1;
    internal void CompleteOriginalUseLightningJudgment(JudgmentFrame pending, bool hit)
    {
        var judgment = _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == pending.Id) ?? pending;
        if (!IsOriginalUseLightningJudgment(judgment) || judgment.OriginalUseLightning is not { } a || !ExactOriginalUseLightning(a))
            throw new InvalidOperationException("Lightning judgment lost its original held effect.");
        if (!hit)
        {
            TransferOriginalUseLightning(a, judgment.Id, judgment.CardId, judgment.OriginalUseLightningExcludedCardId);
            return;
        }

        if (judgment.Succeeded != true || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != judgment.Id)
            throw new InvalidOperationException("Only the actual finalized Lightning hit can begin native damage.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalUseLightningResultEvent(a.Id, judgment.Id, a.Holder, true, judgment.CardId, null));
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(judgment.Id, a.Holder, a.Holder, card: null, damageAmount: 3, playedCardKind: CardKind.Lightning, isDelayedJudgmentDamage: true, delayedJudgmentSeat: a.Holder);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }
    internal bool FinishOriginalUseLightningAttack(NativeOwners.CardAttackHandle attack)
    {
        if (_resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == attack.ResolutionId)is not { OriginalUseLightning: { } a } judgment)
            return false;
        if (!attack.IsDelayedJudgmentDamage || !attack.IsSourceLess || !IsOriginalUseLightningJudgment(judgment))
            throw new InvalidOperationException("Only the attachment's native delayed attack can consume it.");
        RemoveOriginalUseLightning(a, "resolved");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
            _kernelCoreMainOperations2.CompleteGame();
        return true;
    }

    internal void TransferOriginalUseLightning(OriginalUseLightningAttachment a, long judgmentId, int? cardId, int? excludedCard)
    {
        if (!ExactOriginalUseLightning(a))
            throw new InvalidOperationException("Lightning transfer lost its original held effect.");
        var holder = a.Holder;
        var nextHolder = Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(offset => _coreTableOrderOperations.TableStepSeat(holder, offset)).FirstOrDefault(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && !_coreMatchParticipantsPlayersAccess._players[seat].JudgmentAreaAbolished && !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) && !_coreDyingWindowSkillPoliciesOperations.IsBlackTrickTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.Lightning, null) && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.Lightning), holder);
        _coreMatchParticipantsPlayersAccess._players[holder].HeldOriginalUseLightning = _coreMatchParticipantsPlayersAccess._players[holder].HeldOriginalUseLightning.Where(x => x.Id != a.Id).ToArray();
        var next = a with
        {
            Holder = nextHolder,
            AfterMovement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Order = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence,
            LastJudgmentTurn = nextHolder == holder ? _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber : -1
        };
        _coreMatchParticipantsPlayersAccess._players[nextHolder].HeldOriginalUseLightning = _coreMatchParticipantsPlayersAccess._players[nextHolder].HeldOriginalUseLightning.Append(next).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalUseLightningTransferredEvent(a.Id, holder, nextHolder, next.AfterMovement, next.Order));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalUseLightningResultEvent(a.Id, judgmentId, holder, false, cardId, nextHolder));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            _coreNativeJudgmentOperations.ResumeAfterLightningDamage(holder, excludedCard);
    }

    internal void ReplaceHeldOriginalUseLightning(OriginalUseLightningAttachment a) => _coreMatchParticipantsPlayersAccess._players[a.Holder].HeldOriginalUseLightning = _coreMatchParticipantsPlayersAccess._players[a.Holder].HeldOriginalUseLightning.Select(x => x.Id == a.Id ? a : x).ToArray();
    internal void RemoveOriginalUseLightning(OriginalUseLightningAttachment a, string reason)
    {
        if (!ExactOriginalUseLightning(a))
            throw new InvalidOperationException("Lightning removal lost its exact held identity.");
        _coreMatchParticipantsPlayersAccess._players[a.Holder].HeldOriginalUseLightning = _coreMatchParticipantsPlayersAccess._players[a.Holder].HeldOriginalUseLightning.Where(x => x.Id != a.Id).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OriginalUseLightningRemovedEvent(a.Id, a.Holder, reason));
    }

    internal bool SkipUnavailableOriginalUseLightning(OriginalUseLightningAttachment a, int? excludedCard)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[a.Holder].IsAlive && !_coreMatchParticipantsPlayersAccess._players[a.Holder].JudgmentAreaAbolished)
            return false;
        RemoveOriginalUseLightning(a, "holder-unavailable");
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            _coreNativeJudgmentOperations.ResumeAfterLightningDamage(a.Holder, excludedCard);
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 37450)]
    internal void ClearUnavailableOriginalUseLightning()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => !p.IsAlive || p.JudgmentAreaAbolished))
            foreach (var a in player.HeldOriginalUseLightning.ToArray())
            {
                if (_resolutionStackOperations._resolutionStack.Any(f => f is JudgmentFrame j && j.OriginalUseLightning?.Id == a.Id || f is NullificationWindowFrame w && w.DelayedReturn?.OriginalUseLightning?.Id == a.Id || f is CardEffectBeforeApplyFrame e && e.DelayedReturn?.OriginalUseLightning?.Id == a.Id))
                    continue;
                RemoveOriginalUseLightning(a, "holder-unavailable");
            }
    }

    internal void PreserveNativeDelayedAttackCompletion(GameEngine.AttackCompletionReceipt completion)
    {
        if (!completion.DelayedTurn)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not JudgmentFrame judgment || judgment.Id != completion.ResolutionId || completion.ProgramFrameId is not null || completion.DelayedTurnSeat != judgment.TargetSeat)
            throw new InvalidOperationException("Delayed attack cleanup lost its native Judgment owner.");
        if (judgment.NativeDelayedAttackCompletion is { } prior && prior != completion)
            throw new InvalidOperationException("A delayed child cannot replace its native completion receipt.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(judgment.Id, judgment with { NativeDelayedAttackCompletion = completion });
    }

    internal ProgramSkillFrame BindNativeDelayedAttackChild(ProgramSkillFrame child, long judgmentId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not JudgmentFrame { NativeDelayedAttackCompletion: { } completion } judgment || judgment.Id != judgmentId || completion.ResolutionId != judgmentId || !completion.DelayedTurn || completion.ProgramFrameId is not null || completion.DelayedTurnSeat != judgment.TargetSeat || child.Id <= judgmentId || child.NativeDelayedAttackReturn is not null || _resolutionStackOperations._resolutionStack.Any(f => f.Id == child.Id))
            throw new InvalidOperationException("A delayed cleanup child requires its exact completed native Judgment parent.");
        return child with
        {
            NativeDelayedAttackReturn = new(judgmentId, child.Id)
        };
    }

    internal void ResumeNativeDelayedAttackCompletion(long judgmentId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not JudgmentFrame { NativeDelayedAttackCompletion: { } completion } judgment || judgment.Id != judgmentId || completion.ResolutionId != judgmentId || !completion.DelayedTurn || completion.ProgramFrameId is not null || completion.DelayedTurnSeat != judgment.TargetSeat || _coreDyingCursorRuntimeOperations.ActiveDying is not null)
            throw new InvalidOperationException("Delayed child completion lost its exact native cleanup return.");
        _coreNativeAttackCompletionOperations.CompleteAttackAfterCardResolution(completion);
    }
}
