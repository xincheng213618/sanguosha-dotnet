#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanYuOlRuntime
{
    internal const string RuanYuExchangeReason = "skill-program.ruan-yu.xingzuo.exchange", MiaoxianDrawReason = "skill-program.ruan-yu.miaoxian.draw";
    internal bool HasRuanYuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveXingzuoPrivateBottomExchange) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMiaoxianUniqueRedUseDraw);

    internal static CardConversionSource RuanYuSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_ruan_yu_ol_RuanYu.RuanYuSource(f);
    SkillProgramStepOutcome IRuanYuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginRuanYu(f, op);
    internal SkillProgramStepOutcome BeginRuanYu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var context = f.WindowContext ?? throw new InvalidOperationException("Ruan Yu requires a genuine native trigger.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (!CanOfferRuanYu(candidate, _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Ruan Yu lost his original source, actual phase or prepayment true-use qualification.");
        var stage = op == SkillProgramEffectOp.ResolveXingzuoPrivateBottomExchange ? RuanYuStage.Hand : op == SkillProgramEffectOp.ResolveXingzuoIssuedEndingExchange ? RuanYuStage.EndingTarget : RuanYuStage.RedChildren;
        var r = new RuanYuReceipt(RuanYuSource(f), f.GameplayHash, op, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, stage, f.OwnerSeat);
        if (stage == RuanYuStage.Hand)
            r = r with
            {
                ViewedBottom = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Take(3).Select(c => c.Id).ToArray()
            };
        if (stage == RuanYuStage.EndingTarget)
            r = r with
            {
                DebtId = CurrentRuanYuEndingDebt(candidate, context)!.DebtId,
                Paid = true
            };
        if (stage == RuanYuStage.RedChildren)
            r = r with
            {
                ActionId = context.CardUse!.CardActionId,
                Paid = true
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanYuStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.ActualTurn, r.TurnOwner));
        if (r.DebtId is { } debt)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingzuoEndingConsumedEvent(debt, f.Id, r.ParentId));
        if (stage == RuanYuStage.RedChildren)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MiaoxianRedDrawIssuedEvent(f.Id, r.ActionId!.Value, r.Source, r.ActualTurn));
            PrepareRuanYuMovement(ref f, RuanYuStage.RedChildren, []);
            var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(MiaoxianDrawReason)).Count;
            CompleteRuanYuMovement(f.Id, count);
        }
        else
            PublishRuanYu(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int[] RuanYuEndingTargets() => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).Select(p => p.Seat).ToArray();
    internal PromptChoice RuanYuChoice(ProgramSkillFrame f, string branch, string label, int? card = null, int? seat = null) => new(new($"ruan-yu.{(f.Id)}.{(branch)}.{(card ?? seat ?? -1)}"), label, card is { } id ? [id] : [], seat is { } s ? [s] : [], new Dictionary<string, string> { { "program-action", "ruan-yu" }, { "branch", branch } });
    internal int[] RuanYuDraftBottom(RuanYuReceipt r) => r.SelectedHand.Concat(r.ViewedBottom.Where(id => !r.SelectedBottom.Contains(id))).ToArray();
    internal IReadOnlyList<PromptChoice> RuanYuChoices(ProgramSkillFrame f)
    {
        var r = f.RuanYu!;
        var choices = new List<PromptChoice>();
        if (r.Stage == RuanYuStage.Hand)
        {
            if (r.SelectedHand.Count < r.ViewedBottom.Count)
                choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !r.SelectedHand.Contains(c.Id)).Select(c => RuanYuChoice(f, "hand", "换入【" + c.DisplayName + "】", c.Id)));
            choices.Add(RuanYuChoice(f, "done", "选齐手牌，确定等量底牌"));
            choices.Add(RuanYuChoice(f, "cancel", "取消兴作"));
        }

        if (r.Stage == RuanYuStage.Bottom)
            choices.AddRange(r.ViewedBottom.Where(id => !r.SelectedBottom.Contains(id)).Select(id => RuanYuChoice(f, "bottom", "取得【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName + "】", id)));
        if (r.Stage == RuanYuStage.Order)
            choices.AddRange(RuanYuDraftBottom(r).Where(id => !r.OrderedBottom.Contains(id)).Select(id => RuanYuChoice(f, "order", "依次置于底牌栏【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName + "】", id)));
        if (r.Stage == RuanYuStage.EndingTarget)
            choices.AddRange(RuanYuEndingTargets().Select(s => RuanYuChoice(f, "target", _coreMatchParticipantsPlayersAccess._players[s].Name, seat: s)));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishRuanYu(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishRuanYu(f);
            return;
        }

        var choices = RuanYuChoices(f);
        if (choices.Count == 0)
        {
            if (f.RuanYu!.Stage == RuanYuStage.Order)
                PayRuanYuFront(f);
            else
                FinishRuanYu(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "兴作：选择真实原手牌、底牌及最终底牌次序。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("ruan-yu")]
    internal void AnswerRuanYu(PromptChoice supplied)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RuanYu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !RuanYuChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [supplied])))
            throw new InvalidOperationException("Xingzuo requires the exact private current entity choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishRuanYu(f);
            return;
        }

        var branch = supplied.Parameters["branch"];
        if (branch == "cancel")
        {
            FinishRuanYu(f);
            return;
        }

        if (branch == "hand")
            r = r with
            {
                SelectedHand = r.SelectedHand.Append(supplied.Cards.Single()).ToArray()
            };
        else if (branch == "done")
            r = r with
            {
                Stage = r.SelectedHand.Count == 0 ? RuanYuStage.Order : RuanYuStage.Bottom
            };
        else if (branch == "bottom")
        {
            var ids = r.SelectedBottom.Append(supplied.Cards.Single()).ToArray();
            r = r with
            {
                SelectedBottom = ids,
                Stage = ids.Length == r.SelectedHand.Count ? RuanYuStage.Order : RuanYuStage.Bottom
            };
        }
        else if (branch == "order")
        {
            r = r with
            {
                OrderedBottom = r.OrderedBottom.Append(supplied.Cards.Single()).ToArray()
            };
        }
        else if (branch == "target")
        {
            PayRuanYuEnding(f, supplied.Targets.Single());
            return;
        }
        else
            throw new InvalidOperationException("Unknown Xingzuo choice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = r });
        if (r.Stage == RuanYuStage.Order && r.OrderedBottom.Count == r.ViewedBottom.Count)
            PayRuanYuFront(f);
        else
            PublishRuanYu(f);
    }

    internal void PrepareRuanYuMovement(ref ProgramSkillFrame f, RuanYuStage stage, IReadOnlyList<RuanYuEntity> entities)
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        var r = f.RuanYu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = r with { Paid = true, Stage = stage, Step = r.Step + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = 0, Entities = entities }, PendingMovementContinuation = new(r.Recipient, 0, null) });
    }

    internal void RecordRuanYuMovement(ProgramSkillFrame f, long batch, IReadOnlyList<CardMovementRecord> records)
    {
        foreach (var m in records)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanYuEntityMovedEvent(f.Id, f.RuanYu!.Step, batch, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
    }

    internal void CompleteRuanYuMovement(long id, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.RuanYu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanYuMovementPaidEvent(id, r.Step, r.Stage, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
        ContinueRuanYu(id);
    }

    internal void MoveRuanYuExchange(ProgramSkillFrame f, IReadOnlyList<RuanYuEntity> entities, IReadOnlyList<int> desired)
    {
        var r = f.RuanYu!;
        var old = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => c.Id).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = r with { DeckBefore = old, DesiredDeck = desired } });
        PrepareRuanYuMovement(ref f, r.Stage, entities);
        void Reorder()
        {
            var now = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Select(c => c.Id).ToArray();
            _cardZonesOperations._cardZones.ReorderDrawPileTop(now, desired.Reverse().ToArray(), []);
        }

        if (entities.Count > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(entities.Select(c => c.CardId).ToArray(), CardLocation.DrawPile, new(RuanYuExchangeReason), (batch, records) =>
            {
                RecordRuanYuMovement(f, batch, records);
                Reorder();
            }, (_, from) => from == CardLocation.DrawPile ? CardLocation.Hand(r.Recipient) : CardLocation.DrawPile);
        else
            Reorder();
        for (var i = 0; i < (r.Stage == RuanYuStage.FrontChildren ? r.OrderedBottom.Count : r.SelectedHand.Count); i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingzuoBottomOrderedEvent(f.Id, i, desired[i], r.Stage == RuanYuStage.EndingChildren));
        CompleteRuanYuMovement(f.Id, entities.Count);
    }

    internal void PayRuanYuFront(ProgramSkillFrame f)
    {
        var r = f.RuanYu!;
        var deck = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => c.Id).ToArray();
        if (!r.ViewedBottom.SequenceEqual(deck.Take(r.ViewedBottom.Count)) || r.SelectedHand.Count != r.SelectedBottom.Count || !r.SelectedHand.All(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat)) || !r.SelectedBottom.All(r.ViewedBottom.Contains) || r.OrderedBottom.Count != r.ViewedBottom.Count || !r.OrderedBottom.Order().SequenceEqual(RuanYuDraftBottom(r).Order()))
            throw new InvalidOperationException("Xingzuo cannot replace or reorder different original physical sets.");
        var entities = r.SelectedHand.Select(id => new RuanYuEntity(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, CardLocation.Hand(f.OwnerSeat))).Concat(r.SelectedBottom.Select(id => new RuanYuEntity(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, CardLocation.DrawPile))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = r with { Stage = RuanYuStage.FrontChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingzuoEndingIssuedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn));
        MoveRuanYuExchange(f, entities, r.OrderedBottom.Concat(deck.Skip(r.ViewedBottom.Count)).ToArray());
    }

    internal void PayRuanYuEnding(ProgramSkillFrame f, int recipient)
    {
        var r = f.RuanYu!;
        if (!RuanYuEndingTargets().Contains(recipient))
            throw new InvalidOperationException("Xingzuo Ending requires a genuinely living nonempty-Hand participant.");
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[recipient]).ToArray();
        var bottom = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Take(3).ToArray();
        var deck = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Select(c => c.Id).ToArray();
        var randomized = hand.Select(c => c.Id).ToArray();
        for (var i = randomized.Length - 1; i > 0; i--)
        {
            var j = _randomOperations._random.Next(i + 1);
            (randomized[i], randomized[j]) = (randomized[j], randomized[i]);
        }

        var entities = hand.Select(c => new RuanYuEntity(c.Id, c.Kind, CardLocation.Hand(recipient))).Concat(bottom.Select(c => new RuanYuEntity(c.Id, c.Kind, CardLocation.DrawPile))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = r with { Recipient = recipient, Stage = RuanYuStage.EndingChildren, SelectedHand = hand.Select(c => c.Id).ToArray(), ViewedBottom = bottom.Select(c => c.Id).ToArray(), OrderedBottom = randomized } });
        MoveRuanYuExchange(f, entities, randomized.Concat(deck.Skip(bottom.Length)).ToArray());
    }

    internal bool DrainRuanYu(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.RuanYu!.Stage == RuanYuStage.LoseHpChildren ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void ContinueRuanYu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RuanYu: { } r } f || f.Id != id)
            return;
        if (r.Stage is RuanYuStage.Hand or RuanYuStage.Bottom or RuanYuStage.Order or RuanYuStage.EndingTarget)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishRuanYu(f);
            return;
        }

        if (r.Stage == RuanYuStage.LoseHpIssuing)
        {
            _ = _coreParticipantAndReserveProgramsOperations.ExecuteParticipantReserve(new(SkillProgramEffectOp.LoseHpUnclamped, SkillProgramEffectTarget.Owner, 1, new(SkillProgramConditionKind.Always, 0, [])), f);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.RuanYu!;
            if (r.Stage == RuanYuStage.LoseHpIssuing)
                return;
        }

        if (DrainRuanYu(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.RuanYu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == RuanYuStage.EndingChildren && r.SelectedHand.Count > 3 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = r with { Stage = RuanYuStage.LoseHpIssuing } });
            ContinueRuanYu(id);
            return;
        }

        FinishRuanYu(f);
    }

    internal void FinishRuanYu(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RuanYuCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { RuanYu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27430)]
    internal bool ResumeRuanYu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { RuanYu: not null } f || f.Id != id)
            return false;
        ContinueRuanYu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27430)]
    internal bool ReturnRuanYuMovement(ProgramSkillFrame f)
    {
        if (f.RuanYu is null)
            return false;
        ContinueRuanYu(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27430)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27430)]
    internal bool ContinuePaidRuanYu(ProgramSkillFrame f)
    {
        if (f.RuanYu is { Paid: true })
            return ValidRuanYu(f);
        if (f.RuanYu is not null || f.TriggerId is null || f.WindowContext is not { } context || f.InstructionIndex is not (0 or 1) || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId))
            return false;
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        return IsIssuedRuanYuXingzuoEndingCandidate(c, context) || IsIssuedRuanYuMiaoxianRedCandidate(c, context);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27430)]
    internal void CaptureRuanYuHpLoss(ProgramSkillHpLostEvent paid)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { RuanYu: { Stage: RuanYuStage.LoseHpIssuing } } f && paid.FrameId == f.Id && paid.SkillId == f.SkillId && paid.TargetSeat == f.OwnerSeat && paid.Amount == 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { RuanYu = f.RuanYu!with { Stage = RuanYuStage.LoseHpChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingzuoHpLossPaidEvent(f.Id, paid.RemainingHp));
        }
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveXingzuoPrivateBottomExchange, SkillProgramEffectOp.ResolveXingzuoIssuedEndingExchange)]
    internal PromptChoice ChooseRuanYu(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.RuanYu!;
        if (r.Stage == RuanYuStage.Hand)
        {
            var c = p.Choices.Where(c => c.Parameters["branch"] == "hand").OrderBy(c => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Kind).HandKeepValue).FirstOrDefault();
            var good = r.ViewedBottom.Select(id => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind).HandKeepValue).OrderDescending().Skip(r.SelectedHand.Count).FirstOrDefault();
            return c is not null && CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Kind).HandKeepValue < good ? c : p.Choices.First(c => c.Parameters["branch"] == "done");
        }

        if (r.Stage == RuanYuStage.Bottom)
            return p.Choices.OrderByDescending(c => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Kind).HandKeepValue).First();
        if (r.Stage == RuanYuStage.Order)
            return p.Choices.OrderBy(c => Array.IndexOf(RuanYuDraftBottom(r), c.Cards.Single())).First();
        return p.Choices.OrderByDescending(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count).ThenBy(c => c.Targets.Single()).First();
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 60)]
    internal IReadOnlyList<CardSnapshot> RuanYuPrivatelyViewedBottomCards(int viewerSeat) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { RuanYu: { Stage: RuanYuStage.Hand or RuanYuStage.Bottom or RuanYuStage.Order } r } f && f.OwnerSeat == viewerSeat && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { IsPrivate: true, PlayerSeat: var actor } && actor == viewerSeat ? Array.AsReadOnly(r.ViewedBottom.Concat(r.OrderedBottom).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray()) : [];
}
