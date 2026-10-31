#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangJiaoAnniversaryGodRuntime
{
    internal const string AnniversaryYizhao = "anniversary-god:yizhao", AnniversarySanshou = "anniversary-god:sanshou", AnniversarySijun = "anniversary-god:sijun", AnniversaryTianjie = "anniversary-god:tianjie";
    internal const string HappyZhangJiaoYizhao = "happy:yizhao", HappyZhangJiaoSanshou = "happy:sanshou", HappyZhangJiaoSijun = "happy:sijun", HappyZhangJiaoTianjie = "happy:tianjie";
    internal static CardConversionSource AnniversaryZjSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool AnniversaryZjTurnShuffled() => _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).Any(e => e is CardMovedEvent { Reason: var reason } && reason == CardMoveReasons.Reshuffle || e is AnniversaryZhangJiaoShuffledEvent s && s.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && s.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || e is MobileShiHuanJieShuffledEvent);
    internal bool HasAnniversaryTianjieDeadOwnerEnding(int actualOwner) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(actualOwner) && actualOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !_coreMatchParticipantsPlayersAccess._players[actualOwner].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && AnniversaryZjTurnShuffled() && _coreMatchParticipantsPlayersAccess._players.Any(p => p.Seat != actualOwner && p.IsAlive && (_coreNationalSkillsOperations.HasRuntimeSkill(p, AnniversaryTianjie) || _coreNationalSkillsOperations.HasRuntimeSkill(p, HappyZhangJiaoTianjie)));
    internal bool IsAnniversaryTianjieDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int actualOwner) => HasAnniversaryTianjieDeadOwnerEnding(actualOwner) && candidate.SkillId is (AnniversaryTianjie or HappyZhangJiaoTianjie) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) && candidate.OwnerSeat != actualOwner && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate)is { Window: SkillProgramTriggerWindow.TurnEnding, TurnOwnerScope: SkillProgramTurnOwnerScope.OtherLiving, Effects: [{ Op: SkillProgramEffectOp.ResolveAnniversaryZhangJiaoTianjie }] };
    internal int AnniversaryZjRank(int owner, ProgramSkillWindowContext? context)
    {
        if (context is { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index })
        {
            if (index < 0 || index >= batch.Movements.Count)
                return 0;
            var m = batch.Movements[index];
            return _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)?.OwnerSeat == owner && m.To == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) ? _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Rank : 0;
        }

        if (context?.Window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardResponseAccepted))
            return 0;
        var action = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()?.Action ?? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action?.ActionId == context.CardUse?.CardActionId)?.Action;
        if (action?.ActorSeat != owner || action.ActionId != context.CardUse?.CardActionId)
            return 0;
        return action.EffectiveRank ?? (action.PhysicalCards.Count == 1 ? _coreAdvancedLifecycleOperations.GetAdvancedCard(action.PhysicalCards[0].CardId).Rank : 0);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 39400)]
    internal bool CanTriggerAnniversaryZj(ProgramTriggerLegalityContext g) => g.SkillId switch
    {
        AnniversaryYizhao or HappyZhangJiaoYizhao => AnniversaryZjRank(g.OwnerSeat, g.WindowContext) > 0,
        AnniversarySijun or HappyZhangJiaoSijun => g.Owner.Markers.GetValueOrDefault(PlayerMarkerKind.Huang) > _cardZonesOperations._cardZones.Count(CardLocation.DrawPile),
        AnniversaryTianjie or HappyZhangJiaoTianjie => AnniversaryZjTurnShuffled() && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != g.OwnerSeat),
        _ => true
    };
    SkillProgramStepOutcome IAnniversaryZhangJiaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffect e) => BeginAnniversaryZj(f, e);
    internal SkillProgramStepOutcome BeginAnniversaryZj(ProgramSkillFrame supplied, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (e.Op == SkillProgramEffectOp.ResolveAnniversaryZhangJiaoSanshou)
            return ResolveAnniversarySanshou(f, e);
        if (f.AnniversaryZhangJiao is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Anniversary Zhang Jiao requires his original unpaid instruction.");
        var r = new AnniversaryZhangJiaoReceipt(AnniversaryZjSource(f), f.GameplayHash, e.Op, f.InstructionIndex, AnniversaryZhangJiaoStage.Obtained, Before: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After: _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, RandomBefore: _randomOperations._random.State, RandomAfter: _randomOperations._random.State);
        if (e.Op == SkillProgramEffectOp.ResolveAnniversaryZhangJiaoYizhao)
        {
            var rank = AnniversaryZjRank(f.OwnerSeat, f.WindowContext);
            if (rank <= 0)
                return SkillProgramStepOutcome.Continue;
            var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.Huang);
            var after = checked(before + rank);
            var tens = after / 10 % 10;
            var matches = before / 10 % 10 == tens ? [] : _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => c.Rank == tens).ToArray();
            var cards = matches.Length == 0 ? [] : new[]
            {
                matches[_randomOperations._random.Next(matches.Length)].Id
            };
            r = r with
            {
                Rank = rank,
                MarkerBefore = before,
                MarkerAfter = after,
                Cards = cards,
                RandomAfter = _randomOperations._random.State
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangJiao = r });
            _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.Huang, f.OwnerSeat, after, f);
            ObtainAnniversaryZjCards(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYizhaoAccumulatedEvent(f.Id, r.Source, rank, before, after, cards.Length));
        }
        else if (e.Op == SkillProgramEffectOp.ResolveAnniversaryZhangJiaoSijun)
        {
            var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.Huang);
            if (f.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || before <= _cardZonesOperations._cardZones.Count(CardLocation.DrawPile))
                throw new InvalidOperationException("Sijun requires its actual preparation and strictly more Huang than draw-pile cards.");
            var pool = f.SkillId == HappyZhangJiaoSijun ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).ToArray() : _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile)).ToArray();
            var cards = SampleAnniversarySijun(pool);
            r = r with
            {
                MarkerBefore = before,
                Cards = cards,
                Rank = cards.Sum(id => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Rank),
                RandomAfter = _randomOperations._random.State
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangJiao = r });
            _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.Huang, f.OwnerSeat, 0, f);
            ObtainAnniversaryZjCards(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySijunObtainedEvent(f.Id, r.Source, before, cards.Length, r.Rank));
        }
        else
        {
            if (e.Op != SkillProgramEffectOp.ResolveAnniversaryZhangJiaoTianjie || f.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || !AnniversaryZjTurnShuffled())
                throw new InvalidOperationException("Tianjie requires an actual shuffle in this ending turn.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryZhangJiao = r with { Stage = AnniversaryZhangJiaoStage.Choosing } });
        }

        ContinueAnniversaryZj(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome ResolveAnniversarySanshou(ProgramSkillFrame f, SkillProgramEffect e)
    {
        if (!CoreGameEngineShanheSanshouArmorRuntime.IsNativeSanshouSkill(f.SkillId) || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: var victim } || victim != f.OwnerSeat)
            throw new InvalidOperationException("Sanshou requires its original damage-target window.");
        if (e.SourceBind is null)
        {
            if (f.InstructionIndex != 1 || f.AnniversaryZhangJiao is not null)
                throw new InvalidOperationException("Sanshou already froze its actual-use categories.");
            var mask = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Where(x => x.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && x.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat).Aggregate(0, (m, x) => m | x.CardCategoryMask);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryZhangJiao = new(AnniversaryZjSource(f), f.GameplayHash, e.Op, 1, AnniversaryZhangJiaoStage.SanshouSnapshot, UsedCategories: mask) });
            return SkillProgramStepOutcome.Continue;
        }

        if (f.InstructionIndex != 3 || e.SourceBind != "sanshou-top" || f.AnniversaryZhangJiao is not { Stage: AnniversaryZhangJiaoStage.SanshouSnapshot } r)
            throw new InvalidOperationException("Sanshou lost its snapshot before the native reveal.");
        var shown = _coreProgramLifecycleOperations.GetProgramCardSet(f, e.SourceBind);
        if (shown.Visibility != SkillProgramCardSetVisibility.Public || shown.CardIds.Count > 3 || shown.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing))
            throw new InvalidOperationException("Sanshou requires the actual publicly revealed processing entities.");
        var prevented = shown.CardIds.Any(id => (r.UsedCategories & (1 << (int)CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind))) == 0);
        if (prevented)
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySanshouResolvedEvent(f.Id, r.Source, r.UsedCategories, prevented));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryZhangJiao = null });
        return SkillProgramStepOutcome.Continue;
    }

    internal int[] SampleAnniversarySijun(IReadOnlyList<Card> pool)
    {
        var remaining = 36;
        var available = pool.Where(c => c.Rank > 0 && c.Rank <= remaining).ToList();
        var selected = new List<int>();
        if (!RankSubsetSearch.CanComplete(available.Select(c => c.Rank), remaining))
            return[];
        while (remaining > 0)
        {
            var feasible = available.Where(c => c.Rank <= remaining && RankSubsetSearch.CanComplete(available.Where(o => o.Id != c.Id).Select(o => o.Rank), remaining - c.Rank)).ToArray();
            var card = feasible[_randomOperations._random.Next(feasible.Length)];
            selected.Add(card.Id);
            available.Remove(card);
            remaining -= card.Rank;
        }

        return selected.ToArray();
    }

    internal void ObtainAnniversaryZjCards(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryZhangJiao!;
        List<CardMovementRecord>? happyPayments = f.SkillId == HappyZhangJiaoSijun ? [] : null;
        if (r.Cards.Count > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            foreach (var id in r.Cards)
            {
                var from = _cardZonesOperations._cardZones.GetLocation(id);
                if (from != CardLocation.DrawPile && (r.Op != SkillProgramEffectOp.ResolveAnniversaryZhangJiaoSijun || f.SkillId != AnniversarySijun || from != CardLocation.DiscardPile))
                    throw new InvalidOperationException("Zhang Jiao lost a randomly selected physical pile entity.");
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), from, CardLocation.Hand(f.OwnerSeat), new("skill.anniversary-zhang-jiao.obtain"), movement => happyPayments?.Add(movement), disclosure: CardMovementDisclosure.FaceDown);
            }
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { AnniversaryZhangJiao = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, HappySijunPayment = happyPayments is null ? null : new(r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) { Movements = happyPayments } } });
    }

    internal bool DrainAnniversaryZj(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueAnniversaryZj(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryZhangJiao: { } r } f && f.Id == id)
        {
            if (r.Stage is AnniversaryZhangJiaoStage.Obtained or AnniversaryZhangJiaoStage.Shuffled)
            {
                if (DrainAnniversaryZj(id))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.AnniversaryZhangJiao!;
                if (r.Op == SkillProgramEffectOp.ResolveAnniversaryZhangJiaoSijun && r.Stage == AnniversaryZhangJiaoStage.Obtained)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangJiao = r with { Stage = AnniversaryZhangJiaoStage.Shuffled, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                    _corePhysicalMovementFacadesOperations.MoveAllCards(CardLocation.DiscardPile, CardLocation.DrawPile, CardMoveReasons.Reshuffle);
                    _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
                    _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { AnniversaryZhangJiao = f.AnniversaryZhangJiao!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangJiaoShuffledEvent(id, r.Source, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
                    continue;
                }

                FinishAnniversaryZj(f);
                return;
            }

            if (r.Stage == AnniversaryZhangJiaoStage.Choosing)
            {
                PublishAnniversaryTianjie(f);
                return;
            }

            if (r.Stage == AnniversaryZhangJiaoStage.Damaging)
            {
                if (f.AttackAttempt is not null)
                    return;
                if (DrainAnniversaryZj(id))
                    return;
                if (r.Cursor == r.Targets.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishAnniversaryZj(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                    return;
                }

                var target = r.Targets[r.Cursor];
                _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { AnniversaryZhangJiao = r with { Cursor = r.Cursor + 1 } });
                if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                    continue;
                var amount = Math.Max(1, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count(c => _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[target], c) == CardKind.Dodge));
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, amount, nature: DamageNature.Thunder);
                return;
            }

            return;
        }
    }

    internal void FinishAnniversaryZj(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryZhangJiao = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal PromptChoice AnniversaryTianjieChoice(ProgramSkillFrame f, int? target) => new(new($"anniversary-tianjie.{(f.Id)}.{(f.AnniversaryZhangJiao!.Targets.Count)}.{(target?.ToString(CultureInfo.InvariantCulture) ?? "finish")}"), target is { } s ? "选择" + _coreMatchParticipantsPlayersAccess._players[s].Name : "结束选择", [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "anniversary-tianjie", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = target?.ToString(CultureInfo.InvariantCulture) ?? "finish" });
    internal IReadOnlyList<PromptChoice> AnniversaryTianjieChoices(ProgramSkillFrame f) => f.AnniversaryZhangJiao!.Targets.Count >= 3 ? [AnniversaryTianjieChoice(f, null)] : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && !f.AnniversaryZhangJiao.Targets.Contains(p.Seat)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, p.Seat)).Select(p => AnniversaryTianjieChoice(f, p.Seat)).Append(AnniversaryTianjieChoice(f, null)).ToArray();
    internal void PublishAnniversaryTianjie(ProgramSkillFrame f)
    {
        var choices = AnniversaryTianjieChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "请选择至多三名其他角色承受天劫。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-tianjie")]
    internal void AnswerAnniversaryTianjie(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.AnniversaryZhangJiao ?? throw new InvalidOperationException("Tianjie lost its original choice frame.");
        if (r.Stage != AnniversaryZhangJiaoStage.Choosing || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !AnniversaryTianjieChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Tianjie requires its exact currently published target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["option"] == "finish")
            r = r with
            {
                Stage = AnniversaryZhangJiaoStage.Damaging,
                Targets = r.Targets.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, s)).ToArray()
            };
        else
            r = r with
            {
                Targets = r.Targets.Append(choice.Targets.Single()).ToArray()
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryZhangJiao = r });
        ContinueAnniversaryZj(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryZhangJiaoTianjie)]
    internal PromptChoice ChooseAnniversaryTianjie(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    [ProgramContinuation(ProgramContinuationKind.Resume, 39400)]
    internal bool ResumeAnniversaryZj(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryZhangJiao: { } r } f || f.Id != id || r.Stage == AnniversaryZhangJiaoStage.SanshouSnapshot)
            return false;
        ContinueAnniversaryZj(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 39400)]
    internal bool ReturnAnniversaryZjMovement(ProgramSkillFrame f) => f.AnniversaryZhangJiao is { Stage: not AnniversaryZhangJiaoStage.SanshouSnapshot } && ResumeAnniversaryZj(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 39400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 39400)]
    internal bool ContinueIssuedAnniversaryZj(ProgramSkillFrame f) => f.AnniversaryZhangJiao is not null;

    [ProgramTriggerProof(ProgramTriggerProofPhase.DeadOwnerEnding, 20)]
    internal bool ProveIsAnniversaryTianjieDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate) => IsAnniversaryTianjieDeadOwnerEndingCandidate(candidate, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
}
