#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHaoOlRuntime
{
    internal const string SunHaoCanshiDiscardReason = "skill-program.sun-hao.canshi.discard";
    internal static CardConversionSource SunHaoSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_sun_hao_ol_SunHaoCanshi.SunHaoSource(f);
    SkillProgramStepOutcome ISunHaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginSunHao(f, op);
    internal bool SunHaoCanshiUseKind(CardActionContext a, string skillId) => skillId == SunHaoComposition.MobileCanshi ? a.Type == CardActionType.Use ? CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(a.EffectiveKind)is SkillProgramCardCategory.Basic or SkillProgramCardCategory.Trick : a.Type == CardActionType.Response && a.EffectiveKind is CardKind.Dodge or CardKind.Nullification : skillId == "ol:sun-hao-canshi" && (a.Type == CardActionType.Use ? CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(a.EffectiveKind) : a.Type == CardActionType.Response && a.EffectiveKind == CardKind.Nullification);
    internal IReadOnlyList<SunHaoCanshiGrantedEvent> CurrentSunHaoGrants(int owner)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber <= 0)
            return[];
        var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoCanshiGrantedEvent>().Where(g => g.Source.OwnerSeat == owner && g.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && g.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat).OrderBy(g => g.FrameId).ToArray();
        return grants.Length == 0 || !SunHaoActualTurnOpen ? [] : Array.AsReadOnly(grants);
    }

    internal bool HasSunHaoCanshiResponseObligation(CardActionContext action, ProgramCardContinuation continuation) => HasSunHaoCanshiCapability && _coreResponseUseCompletionOperations.IsCompletedResponseUse(action, continuation) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) && _coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive && CurrentSunHaoGrants(action.ActorSeat).Any(g => SunHaoCanshiUseKind(action, g.Source.SkillId) && _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program?.GameplayHash == g.GameplayHash);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 18)]
    internal void CollectIssuedSunHaoCanshiCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasSunHaoCanshiCapability || window != SkillProgramTriggerWindow.CardUseCommitted || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive)
            return;
        foreach (var g in CurrentSunHaoGrants(action.ActorSeat).DistinctBy(g => (g.Source.SkillId, g.Source.SkillInstanceId)))
        {
            var p = _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program;
            if (p is null || p.GameplayHash != g.GameplayHash || !SunHaoCanshiUseKind(action, g.Source.SkillId))
                continue;
            var t = p.Triggers.Single(t => t.Id == "issued-turn-use-discard");
            if (result.Any(c => c.OwnerSeat == action.ActorSeat && c.SkillId == g.Source.SkillId && c.SkillInstanceId == g.Source.SkillInstanceId && c.TriggerId == t.Id))
                continue;
            var c = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, action.ActorSeat, action.ActorSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], action));
            result.Add(new(action.ActorSeat, action.ActorSeat, g.Source.SkillId, t.Id, g.GameplayHash, g.Source.SkillInstanceId, t.Priority, c));
        }
    }

    [ProgramIssuedTrigger(540)]
    internal bool IsIssuedSunHaoCanshiCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (candidate.SkillId is not ("ol:sun-hao-canshi" or SunHaoComposition.MobileCanshi) || candidate.BindingId != "issued-turn-use-discard" || c is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } card } || c.OwnerSeat != candidate.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } w || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !CharacterMaMiDiOlRuntime.MaMiDiCandidateMatches(candidate, CoreGameEngineCardActionsRuntime.ToSharedCandidate(w.Candidates[w.CandidateIndex])) || _coreCardActionsOperations.CreateCardActionProgramContext2(w, w.Candidates[w.CandidateIndex]) != c || w.Action.ActionId != card.CardActionId || w.Action.ActorSeat != candidate.OwnerSeat || !SunHaoCanshiUseKind(w.Action, candidate.SkillId) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return false;
        if (w.Action.Type == CardActionType.Use)
        {
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { } use || use.Action?.ActionId != w.Action.ActionId || use.SourceSeat != candidate.OwnerSeat || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id))
                return false;
        }
        else if (w.CompletedResponseReturn is not { IsCommitted: true } typed || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(w.Action, typed.Kind))
            return false;
        return CurrentSunHaoGrants(candidate.OwnerSeat).Any(g => g.Source.SkillId == candidate.SkillId && g.Source.SkillInstanceId == candidate.SkillInstanceId && g.GameplayHash == candidate.GameplayHash) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoCanshiStartedEvent>().Any(e => e.WindowId == w.Id && e.Source.SkillInstanceId == candidate.SkillInstanceId);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27330)]
    internal bool CanOfferSunHao(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c)
    {
        if (trigger.Effects is not [var e] || !SunHaoComposition.Handles(e.Op))
            return true;
        if (e.Op == SkillProgramEffectOp.ResolveSunHaoCanshiUseDiscard)
            return IsIssuedSunHaoCanshiCandidate(candidate, c);
        var mobile = candidate.SkillId == SunHaoComposition.MobileCanshi;
        return (mobile || candidate.SkillId == "ol:sun-hao-canshi") && candidate.BindingId == (mobile ? "normal-draw-replace-wounded" : "normal-draw-add-wounded") && c.Window == SkillProgramTriggerWindow.DrawPhaseStarting && c.OwnerSeat == candidate.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { NormalDrawReplaced: false, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase } && SunHaoActualTurnOpen && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && (mobile || _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(p)));
    }

    internal SkillProgramStepOutcome BeginSunHao(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SunHaoCanshi is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } c || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Sun Hao requires his exact native issuing instruction.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (!CanOfferSunHao(candidate, _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Sun Hao lost his current normal-draw or already-issued actual-turn entitlement.");
        if (op == SkillProgramEffectOp.GrantSunHaoCanshiDrawObligation)
        {
            if (f.SkillId == SunHaoComposition.MobileCanshi)
                return _characterSunHaoMobileOperations.Value.BeginMobileSunHaoDraw(f);
            var x = _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && _coreTreatedWoundedOperations.IsRuleWounded(p));
            if (x <= 0)
                throw new InvalidOperationException("Zero wounded characters cannot issue Canshi.");
            _coreProgramLifecycleOperations.AdjustProgramNormalDraw(f, x);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoCanshiGrantedEvent(f.Id, SunHaoSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, c.ParentFrameId, x));
            return SkillProgramStepOutcome.Continue;
        }

        var w = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == c.ParentFrameId);
        var grants = CurrentSunHaoGrants(f.OwnerSeat).Where(g => g.Source.SkillId == f.SkillId && g.Source.SkillInstanceId == f.SkillInstanceId && g.GameplayHash == f.GameplayHash).ToArray();
        var grant = grants[0];
        var use = w.Action.Type == CardActionType.Use ? _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId) : null;
        var r = new SunHaoCanshiReceipt(SunHaoSource(f), f.GameplayHash, grant.FrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, w.Id, w.Action.ActionId, use?.Id, SunHaoCanshiStage.SelectDiscard, use?.TargetSeats ?? [])
        {
            GrantIds = grants.Select(g => g.FrameId).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunHaoCanshi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoCanshiStartedEvent(f.Id, r.Source, r.GameplayHash, r.GrantId, r.ActualTurn, r.TurnOwner, r.WindowId, r.ActionId, r.UseId, r.OriginalTargets.Count, r.GrantIds.Count));
        for (var i = 0; i < r.GrantIds.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoCanshiFrozenGrantEvent(f.Id, i, r.GrantIds[i]));
        for (var i = 0; i < r.OriginalTargets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoCanshiOriginalTargetEvent(f.Id, i, r.OriginalTargets[i]));
        ContinueSunHao(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> SunHaoChoices(ProgramSkillFrame f) => f.SunHaoCanshi is not { Stage: SunHaoCanshiStage.SelectDiscard } || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None ? [] : Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(c => PromptChoiceProjection.Freeze(c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "sun-hao-canshi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) } })).ToArray());
    internal void PublishSunHao(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "残蚀：弃置一张手牌或装备牌，然后继续使用此牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("sun-hao-canshi")]
    internal void AnswerSunHao(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SunHaoCanshi: { Stage: SunHaoCanshiStage.SelectDiscard } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !SunHaoChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Canshi requires the exact current owner's legal private HE payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var id = choice.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var grain = from == CardLocation.Equipment(f.OwnerSeat) && card.Kind == CardKind.WoodenOx ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => c.Id).ToArray() : [];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunHaoCanshi = r with { Stage = SunHaoCanshiStage.DiscardChildren, WoodenOxGrain = grain }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new(SunHaoCanshiDiscardReason));
        var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == id && m.Reason.Value == SunHaoCanshiDiscardReason);
        var paid = new SunHaoCanshiDiscardedEvent(f.Id, id, card.Kind, from, m.To, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, r.Cursor);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunHaoCanshi = f.SunHaoCanshi!with { Payment = paid } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(paid);
        ContinueSunHao(f.Id);
    }

    internal bool DrainSunHao(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueSunHao(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { SunHaoCanshi: { } r } f && f.Id == id)
        {
            if (r.Stage == SunHaoCanshiStage.DiscardChildren)
            {
                if (DrainSunHao(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.SunHaoCanshi!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunHaoCanshi = r with { Stage = SunHaoCanshiStage.SelectDiscard, Cursor = r.Cursor + 1, Payment = null, WoodenOxGrain = [] }, PendingMovementContinuation = null });
                r = f.SunHaoCanshi!;
            }

            if (r.Cursor >= r.GrantIds.Count)
            {
                FinishSunHao(f);
                return;
            }

            var choices = SunHaoChoices(f);
            if (choices.Count > 0)
            {
                PublishSunHao(f, choices);
                return;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoCanshiSkippedEvent(f.Id, r.Cursor, r.GrantIds[r.Cursor]));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunHaoCanshi = r with { Cursor = r.Cursor + 1 } });
        }
    }

    internal void FinishSunHao(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoCanshiCompletedEvent(f.Id, _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoCanshiDiscardedEvent>().Any(e => e.FrameId == f.Id)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunHaoCanshi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27330)]
    internal bool ResumeSunHao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SunHaoCanshi: not null } f || f.Id != id)
            return false;
        ContinueSunHao(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27330)]
    internal bool ReturnSunHaoMovement(ProgramSkillFrame f)
    {
        if (f.SunHaoCanshi is null)
            return false;
        ContinueSunHao(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27330)]
    internal bool ContinueIssuedSunHao(ProgramSkillFrame f)
    {
        if (f.MobileSunHaoDraw is not null)
        {
            return true;
        }

        if (f.SunHaoCanshi is not null)
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId == "issued-turn-use-discard" && f.WindowContext is { } c && IsIssuedSunHaoCanshiCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27330)]
    internal bool ContinueDeadSunHao(ProgramSkillFrame f)
    {
        if (f.MobileSunHaoDraw is not null)
        {
            return true;
        }

        if (f.SunHaoCanshi is null)
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveSunHaoCanshiUseDiscard)]
    internal PromptChoice AiSunHao(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderBy(c => c.Cards is [var id] ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    internal bool HasSunHaoCanshiIssuedDistance(long? useId, int actorSeat, int targetSeat)
    {
        if (!HasSunHaoCanshiCapability || useId is not { } id || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { } action } use || action.ActorSeat != actorSeat || use.SourceSeat != actorSeat)
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoCanshiStartedEvent>().Any(s => s.UseId == id && s.ActionId == action.ActionId && s.Source.OwnerSeat == actorSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoCanshiOriginalTargetEvent>().Any(t => t.FrameId == s.FrameId && t.TargetSeat == targetSeat) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoCanshiDiscardedEvent>().Any(p => p.FrameId == s.FrameId && p.From.OwnerSeat == actorSeat && p.Before < p.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > p.Before && m.Sequence <= p.After && m.CardId == p.CardId && m.From == p.From && m.To == p.To && m.Reason.Value == SunHaoCanshiDiscardReason)));
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 70)]
    internal bool DistanceCardUseExemption70HasSunHaoCanshiIssuedDistance(ProgramCardDistanceQuery q) => HasSunHaoCanshiIssuedDistance(q.CardUseFrameId, q.Actor.Seat, q.Target.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 20)]
    internal bool DistancePhysicalSlash20HasSunHaoCanshiIssuedDistance(ProgramSlashDistanceQuery q) => HasSunHaoCanshiIssuedDistance(q.ExistingUseFrameId, q.Actor.Seat, q.Target.Seat);
}
