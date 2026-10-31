#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanJiOlRuntime
{
    internal const string YuanJiPairReason = "skill-program.yuan-ji.jieyan.draw", YuanJiJinghuaReason = "skill-program.yuan-ji.jinghua.draw", YuanJiShuiyueReason = "skill-program.yuan-ji.shuiyue.draw", YuanJiBottomReason = "skill-program.yuan-ji.jinghua.return", YuanJiTopReason = "skill-program.yuan-ji.shuiyue.return";
    SkillProgramStepOutcome IYuanJiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => ResolveYuanJi(f, op);
    internal SkillProgramStepOutcome ResolveYuanJi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.YuanJi is not null || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op || f.WindowContext is not { } c)
            throw new InvalidOperationException("YuanJi requires the exact first committed native instruction and lifecycle window.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex);
        if (!YuanJiWindowCandidate(candidate, c, out var parent) || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2].Id != parent.Id)
            throw new InvalidOperationException("YuanJi lost its immediate native owning boundary.");
        var loss = op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair ? JieyanOpportunity(candidate) : null;
        if (op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair && !IsIssuedJieyanCandidate(candidate, c) || IsYuanJiReturnOp(op) && !IsIssuedYuanJiReturn(candidate, c))
            throw new InvalidOperationException("YuanJi cannot invent an atomic Hand pair or a previous exact mark issuance.");
        var r = new YuanJiReceipt(YuanJiSource(f), f.GameplayHash, op, parent.Id, c.Window, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber, IsYuanJiReturnOp(op) ? YuanJiStage.ReturnOrder : YuanJiStage.Drawing, loss?.LoserSeat ?? -1, loss?.Index ?? -1);
        if (IsYuanJiReturnOp(op))
        {
            var claims = YuanJiActiveClaims(f.OwnerSeat, f.SkillId).ToArray();
            r = r with
            {
                Claims = claims,
                Eligible = claims.Where(a => _cardZonesOperations._cardZones.GetLocation(a.CardId) == CardLocation.Hand(f.OwnerSeat)).Select(a => a.CardId).Distinct().ToArray()
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanJi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.Window, r.ActualTurn, r.TurnOwner, r.Round, r.LoserSeat, r.OpportunityIndex));
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        ContinueYuanJi(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<int> DrawYuanJiEnd(int seat, bool top, CardConversionSource source)
    {
        var reason = new CardMoveReason(YuanJiPairReason);
        var draft = _coreOutsidePhaseDrawDiscardOperations.CaptureNativeDrawInvocation(seat, 1, reason, source);
        var ids = new List<int>();
        if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalDrawFacadesOperations.EnsureDrawPile())
        {
            var pile = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile);
            var card = top ? pile[^1] : pile[0];
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(seat), reason);
            ids.Add(card.Id);
        }

        _coreOutsidePhaseDrawDiscardOperations.CompleteNativeDrawInvocation(draft, ids);
        return ids;
    }

    internal void BeginYuanJiDraw(ProgramSkillFrame f)
    {
        var r = f.YuanJi!;
        var pair = r.Op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair;
        var seat = pair && r.Cursor == 1 ? r.LoserSeat : f.OwnerSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishYuanJi(f);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var top = pair ? r.Cursor == 0 : !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.DrawFromBottom);
        var requested = pair ? 1 : 2;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanJi = r with { Stage = YuanJiStage.DrawChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var ids = pair ? DrawYuanJiEnd(seat, top, r.Source) : _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 2, true, new(r.Op == SkillProgramEffectOp.ResolveJinghuaRoundDraw ? YuanJiJinghuaReason : YuanJiShuiyueReason), r.Source);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var card = pair && ids.Count == 1 ? _coreAdvancedLifecycleOperations.GetAdvancedCard(ids[0]) : null;
        var step = new YuanJiDrawStep(r.Cursor, seat, top, requested, ids.Count, before, after, card?.Id, card?.Suit);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanJi = f.YuanJi!with { After = after, Draws = f.YuanJi!.Draws.Append(step).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiDrawCompletedEvent(f.Id, r.Cursor, seat, top, requested, ids.Count, before, after));
        if (pair && card is not null)
        {
            var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= after && m.CardId == card.Id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat) && m.Reason.Value == YuanJiPairReason);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiShownEvent(f.Id, r.Cursor, seat, card.Id, card.Suit, card.Rank, move.Sequence));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, [_coreSnapshotValueProjectionOperations.ToSnapshot(card)]));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "yuan-ji-drawn-hand"))
                return;
        }
        else if (!pair)
            for (var i = 0; i < ids.Count; i++)
            {
                var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= after && m.CardId == ids[i] && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat) && m.Reason.Value == (r.Op == SkillProgramEffectOp.ResolveJinghuaRoundDraw ? YuanJiJinghuaReason : YuanJiShuiyueReason));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiAcquiredEvent(f.Id, i, ids[i], move.Sequence));
            }
    }

    [PublicHandRevealReturn("yuan-ji-drawn-hand")]
    internal void ReturnYuanJiPublicHand(ProgramSkillFrame f)
    {
        if (f.YuanJi is not { Stage: YuanJiStage.DrawChildren })
            throw new InvalidOperationException("YuanJi lost its native draw/reveal cursor.");
        ContinueYuanJi(f.Id);
    }

    internal void ContinueYuanJi(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { YuanJi: { } r } f && f.Id == id)
        {
            if (r.Stage is YuanJiStage.DrawChildren or YuanJiStage.ReturnChildren)
            {
                if (DrainYuanJi(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.YuanJi!;
                if (r.Stage == YuanJiStage.ReturnChildren)
                {
                    FinishYuanJi(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, YuanJi = r with { Cursor = r.Cursor + 1, Stage = YuanJiStage.Drawing, Before = 0, After = 0 } });
                r = f.YuanJi!;
            }

            if (r.Stage == YuanJiStage.ReturnOrder)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                {
                    if (r.Ordered.Count == r.Eligible.Count)
                        PayYuanJiReturn(f);
                    else
                        PublishYuanJiOrder(f);
                }

                return;
            }

            var count = r.Op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair ? 2 : 1;
            if (r.Cursor < count)
            {
                BeginYuanJiDraw(f);
                continue;
            }

            if (r.Op == SkillProgramEffectOp.ResolveJieyanOppositeEndDrawPair && r.Draws is [{ Suit: { } first }, { Suit: { } second }] && first != second)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiJieyanDisabledEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.Round, first, second));
            FinishYuanJi(f);
            return;
        }
    }

    internal IReadOnlyList<PromptChoice> YuanJiOrderChoices(ProgramSkillFrame f) => Array.AsReadOnly(f.YuanJi!.Eligible.Where(id => !f.YuanJi.Ordered.Contains(id)).Select(id => new PromptChoice(new($"yuan-ji.{(f.Id)}.{(f.YuanJi.Ordered.Count)}.{(id)}"), $"先置{((f.YuanJi.Op == SkillProgramEffectOp.ResolveJinghuaTurnReturn ? "底" : "顶"))}：{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}", [id], [], new Dictionary<string, string> { { "program-action", "yuan-ji-order" }, { "card-id", id.ToString(CultureInfo.InvariantCulture) } })).ToArray());
    internal void PublishYuanJiOrder(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择原标记牌回置次序，第一张最先从该端摸到。", f.YuanJi!.Eligible, [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = YuanJiOrderChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("yuan-ji-order")]
    internal void AnswerYuanJiOrder(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YuanJi: { Stage: YuanJiStage.ReturnOrder } r } f || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(YuanJiOrderChoices(f).Where(c => c.Id == choice.Id).ToArray(), [choice]))
            throw new InvalidOperationException("YuanJi return order requires the exact current private original entities.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var id = choice.Cards.Single();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiReturnSelectedEvent(f.Id, r.Ordered.Count, id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, YuanJi = r with { Ordered = r.Ordered.Append(id).ToArray() } });
        if (f.YuanJi!.Ordered.Count == f.YuanJi.Eligible.Count)
            PayYuanJiReturn(f);
        else
            PublishYuanJiOrder(f);
    }

    internal void PayYuanJiReturn(ProgramSkillFrame f)
    {
        var r = f.YuanJi!;
        if (r.Ordered.Count != r.Eligible.Count || r.Ordered.Distinct().Count() != r.Ordered.Count || !r.Ordered.Order().SequenceEqual(r.Eligible.Order()) || r.Ordered.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat)))
            throw new InvalidOperationException("YuanJi cannot replace or reclaim a foreign-held mark entity.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var top = r.Op == SkillProgramEffectOp.ResolveShuiyueRoundReturn;
        foreach (var c in r.Claims)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiMarkClosedEvent(f.Id, c.DrawFrameId, c.Index, c.CardId, r.Eligible.Contains(c.CardId)));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanJi = r with { Stage = YuanJiStage.ReturnChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (r.Ordered.Count > 0)
        {
            _corePhysicalMovementFacadesOperations.MoveCards(r.Ordered.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.Hand(f.OwnerSeat), CardLocation.DrawPile, new(top ? YuanJiTopReason : YuanJiBottomReason));
            if (top)
                _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(r.Ordered);
            else
                _cardZonesOperations._cardZones.PlaceDrawPileCardsAtBottom(r.Ordered);
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanJi = f.YuanJi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiReturnPaidEvent(f.Id, r.Ordered.Count, top, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueYuanJi(f.Id);
    }

    internal bool DrainYuanJi(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void FinishYuanJi(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuanJiCompletedEvent(f.Id, f.YuanJi!.Op));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YuanJi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27540)]
    internal bool ResumeYuanJi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YuanJi: not null } f || f.Id != id)
            return false;
        ContinueYuanJi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27540)]
    internal bool ReturnYuanJiMovement(ProgramSkillFrame f)
    {
        if (f.YuanJi is null)
            return false;
        ContinueYuanJi(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27540)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27540)]
    internal bool ContinueIssuedYuanJi(ProgramSkillFrame f)
    {
        if (f.YuanJi is not null)
        {
            return true;
        }

        if (f.InstructionIndex is not (0 or 1) || f.TriggerId is null || f.WindowContext is not { } c)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex);
        return IsIssuedJieyanCandidate(candidate, c) || IsIssuedYuanJiReturn(candidate, c);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveJinghuaTurnReturn, SkillProgramEffectOp.ResolveShuiyueRoundReturn)]
    internal PromptChoice AiYuanJi(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
