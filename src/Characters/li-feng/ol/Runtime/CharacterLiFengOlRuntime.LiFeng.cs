#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiFengOlRuntime
{
    SkillProgramStepOutcome ILiFengHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginLiFeng(f, op);
    internal SkillProgramStepOutcome BeginLiFeng(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LiFeng is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } c || !LiFengComposition.Handles(op))
            throw new InvalidOperationException("Li Feng requires his exact unissued native trigger.");
        LiFengComposition.ValidateProgram(f.SkillId, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        var expected = op switch
        {
            SkillProgramEffectOp.InitializeJiyunCategoryGrain => SkillProgramTriggerWindow.GameStarting,
            SkillProgramEffectOp.ResolveJiyunPreparationDrawLeases => SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
            SkillProgramEffectOp.CollectJiyunCompletedUseEntities => SkillProgramTriggerWindow.CardUseCompleted,
            _ => SkillProgramTriggerWindow.TurnEnding
        };
        PublicPersistentPileSource? pile;
        if (op == SkillProgramEffectOp.CollectJiyunCompletedUseEntities)
        {
            var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
            if (!IsIssuedJiyunCompletedUseCandidate(candidate, c) || c.Jiyun is not { } issued)
                throw new InvalidOperationException("Jiyun lost its exact completed-use entitlement.");
            pile = issued.Pile;
        }
        else if (op == SkillProgramEffectOp.DistributeShuliangEndingGrain)
            pile = _corePublicPileSourcesOperations.ReferencedPublicPileSources(f.OwnerSeat, "ol:jiyun", f.SkillInstanceId).Where(IsLiveJiyunPile).Where(p => _corePublicPileSourcesOperations.PublicPileCards(p).Count > 0).OrderBy(p => p.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
        else
            pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, 3);
        if (pile is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (op != SkillProgramEffectOp.CollectJiyunCompletedUseEntities && !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return SkillProgramStepOutcome.Continue;
        if (op == SkillProgramEffectOp.InitializeJiyunCategoryGrain && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LiFengStartedEvent>().Any(e => e.Op == op && e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillInstanceId == f.SkillInstanceId))
            throw new InvalidOperationException("Jiyun cannot initialize one source twice.");
        var stage = op switch
        {
            SkillProgramEffectOp.InitializeJiyunCategoryGrain => LiFengStage.InitialChildren,
            SkillProgramEffectOp.ResolveJiyunPreparationDrawLeases => LiFengStage.PreparationTargets,
            SkillProgramEffectOp.CollectJiyunCompletedUseEntities => LiFengStage.CollectNext,
            _ => LiFengStage.DistributionChoice
        };
        var r = new LiFengReceipt(LiFengSource(f), f.GameplayHash, op, c.ParentFrameId, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, pile, stage)
        {
            MaximumRecipients = op == SkillProgramEffectOp.ResolveJiyunPreparationDrawLeases ? Math.Max(1, _corePublicPileSourcesOperations.PublicPileCards(pile).Count) : 0,
            CompletedUse = c.Jiyun,
            CompletedEntities = c.Jiyun is { } completed ? JiyunCompletedEntities(completed) : [],
            Entities = op == SkillProgramEffectOp.DistributeShuliangEndingGrain ? _corePublicPileSourcesOperations.ReferencedPublicPileSources(f.OwnerSeat, "ol:jiyun", f.SkillInstanceId).Where(IsLiveJiyunPile).OrderBy(p => p.SkillInstanceId, StringComparer.Ordinal).SelectMany(p => _corePublicPileSourcesOperations.PublicPileCards(p).Select(card => new LiFengEntity(card.Id, card.Kind, p.Location))).ToArray() : []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiFengStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.TurnOwner, pile));
        if (op == SkillProgramEffectOp.ResolveJiyunPreparationDrawLeases)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunRecipientLimitFrozenEvent(f.Id, r.MaximumRecipients));
        if (op == SkillProgramEffectOp.DistributeShuliangEndingGrain)
            for (var i = 0; i < r.Entities.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShuliangDistributionEntityFrozenEvent(f.Id, i, r.Entities[i].CardId, r.Entities[i].Kind, r.Entities[i].From));
        if (op is SkillProgramEffectOp.InitializeJiyunCategoryGrain or SkillProgramEffectOp.CollectJiyunCompletedUseEntities)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r = r with { Paid = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiFengPaidEvent(f.Id, op));
        }

        if (op == SkillProgramEffectOp.InitializeJiyunCategoryGrain)
        {
            var entities = new List<LiFengEntity>();
            foreach (var category in new[]
            {
                SkillProgramCardCategory.Basic,
                SkillProgramCardCategory.Trick,
                SkillProgramCardCategory.Equipment
            }

            )
            {
                var cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind) == category && !entities.Any(e => e.CardId == card.Id)).OrderBy(card => card.Id).ToArray();
                if (cards.Length > 0)
                {
                    var card = cards[_randomOperations._random.Next(cards.Length)];
                    entities.Add(new(card.Id, card.Kind, CardLocation.DrawPile));
                }
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r with { Entities = entities, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (entities.Count > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(entities.Select(e => e.CardId).ToArray(), pile.Location, new(JiyunInitialReason), (_, movements) =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { LiFeng = current.LiFeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                    for (var i = 0; i < entities.Count; i++)
                    {
                        var m = movements.Single(m => m.CardId == entities[i].CardId && m.From == CardLocation.DrawPile && m.To == pile.Location);
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunInitialEntityStoredEvent(f.Id, i, m.CardId, m.CardKind, m.Sequence));
                    }
                });
        }

        ContinueLiFeng(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> LiFengChoices(ProgramSkillFrame f)
    {
        var r = f.LiFeng!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"li-feng.{(f.Id)}.{(branch)}.{(string.Join('-', cards ?? []))}.{(string.Join('-', targets ?? []))}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "li-feng", ["branch"] = branch });
        if (r.Stage == LiFengStage.PreparationTargets)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            {
                if (r.Recipients.Count < r.MaximumRecipients)
                    foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !r.Recipients.Contains(p.Seat)))
                        choices.Add(Choice("recipient", $"令{(p.Name)}摸一张牌", targets: [p.Seat]));
                if (r.Recipients.Count > 0)
                    choices.Add(Choice("start", $"令已选的{(r.Recipients.Count)}名角色依次摸牌"));
            }

            choices.Add(Choice("cancel", "不发动集运"));
        }
        else if (r.Stage == LiFengStage.DistributionChoice)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && (r.Paid || _coreProgramLifecycleOperations.CanInvokeProgramSkill(f)))
                foreach (var entity in r.Entities.Where(e => !r.DistributedIds.Contains(e.CardId) && _cardZonesOperations._cardZones.GetLocation(e.CardId) == e.From))
                    foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                        choices.Add(Choice("give", $"将粮【{(CardCatalog.Get(entity.Kind).DisplayName)}】交给{(p.Name)}", [entity.CardId], [p.Seat]));
            choices.Add(Choice("finish", "结束分配（其余粮保留）"));
        }

        return r.Stage == LiFengStage.DistributionChoice ? _coreCardDistributionPresentationOperations.PresentCardDistribution(f, choices, "shuliang") : Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishLiFeng(ProgramSkillFrame f)
    {
        var choices = LiFengChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
        {
            FinishLiFeng(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.LiFeng!.Stage == LiFengStage.PreparationTargets ? "集运：选择至多指定数量的不同角色，逐人摸一张牌。" : "输粮：将任意粮分配给任意存活角色。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("li-feng")]
    internal void AnswerLiFeng(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiFeng: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !LiFengChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Li Feng requires his current published recipient or exact grain choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (branch is "cancel" or "finish")
        {
            FinishLiFeng(f);
            return;
        }

        if (branch == "recipient")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r with { Recipients = r.Recipients.Append(selected.Targets.Single()).ToArray() } });
            PublishLiFeng(f);
            return;
        }

        if (branch == "start")
        {
            if (r.Stage != LiFengStage.PreparationTargets || r.Paid || r.Recipients.Count == 0 || r.Recipients.Count > r.MaximumRecipients || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
                throw new InvalidOperationException("Jiyun cannot issue a canceled recipient queue.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r with { Paid = true, Stage = LiFengStage.PreparationDraw } });
            for (var i = 0; i < r.Recipients.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunRecipientFrozenEvent(f.Id, i, r.Recipients[i]));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiFengPaidEvent(f.Id, r.Op));
            ContinueLiFeng(f.Id);
            return;
        }

        if (branch != "give" || r.Stage != LiFengStage.DistributionChoice || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !r.Paid && !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            throw new InvalidOperationException("Shuliang cannot issue a new unqualified grain distribution.");
        var card = selected.Cards.Single();
        var target = selected.Targets.Single();
        var from = r.Entities.Single(e => e.CardId == card).From;
        if (!r.Paid)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r = r with { Paid = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiFengPaidEvent(f.Id, r.Op));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r with { Stage = LiFengStage.DistributionChildren, ActiveCardId = card, ActiveRecipient = target, ActiveFrom = from, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(card), from, CardLocation.Hand(target), new(ShuliangGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, target, [card], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, movement =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { LiFeng = current.LiFeng!with { After = movement.Sequence, DistributedIds = current.LiFeng.DistributedIds.Append(card).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShuliangGrainDistributedEvent(f.Id, card, target, from, movement.Sequence));
        }));
        ContinueLiFeng(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveJiyunPreparationDrawLeases)]
    internal void ReturnJiyunPreparationDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.LiFeng is not { Paid: true, Stage: LiFengStage.PreparationDraw } r || r.Cursor < 0 || r.Cursor >= r.Recipients.Count || outcome.RecipientSeat != r.Recipients[r.Cursor] || outcome.RequestedCount != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JiyunPreparationDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Cursor))
            throw new InvalidOperationException("Jiyun draw lost its exact frozen recipient cursor.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r with { Stage = LiFengStage.PreparationChildren, DrawOutcome = outcome, ActualDrawIds = outcome.ActualDrawIds, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var id in outcome.ActualDrawIds)
        {
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.CardId == id && m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason.Value == JiyunDrawReason);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunLeaseIssuedEvent(m.Sequence, f.Id, r.Cursor, r.Source, r.GameplayHash, r.Pile, outcome.RecipientSeat, id, m.CardKind, m.Sequence));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunPreparationDrawReturnedEvent(f.Id, r.Cursor, outcome.RecipientSeat, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueLiFeng(f.Id);
    }

    internal bool DrainLiFeng(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueLiFeng(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { LiFeng: { } r } f && f.Id == id)
        {
            if (r.Stage is LiFengStage.InitialChildren or LiFengStage.PreparationChildren or LiFengStage.CollectChildren or LiFengStage.DistributionChildren)
            {
                if (DrainLiFeng(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.LiFeng!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
                if (r.Stage == LiFengStage.InitialChildren || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishLiFeng(f);
                    return;
                }

                r = r.Stage switch
                {
                    LiFengStage.PreparationChildren => r with
                    {
                        Stage = LiFengStage.PreparationDraw,
                        Cursor = r.Cursor + 1,
                        DrawOutcome = null,
                        ActualDrawIds = []
                    },
                    LiFengStage.CollectChildren => r with
                    {
                        Stage = LiFengStage.CollectNext,
                        Cursor = r.Cursor + 1,
                        ActiveCardId = 0,
                        ActiveFrom = null
                    },
                    _ => r with
                    {
                        Stage = LiFengStage.DistributionChoice,
                        ActiveCardId = 0,
                        ActiveFrom = null
                    }
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r });
            }

            if (r.Stage is LiFengStage.PreparationTargets or LiFengStage.DistributionChoice)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !r.Paid && !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
                {
                    FinishLiFeng(f);
                    return;
                }

                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishLiFeng(f);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishLiFeng(f);
                return;
            }

            if (r.Stage == LiFengStage.PreparationDraw)
            {
                if (f.NativeDraw is not null)
                    return;
                if (r.Cursor >= r.Recipients.Count)
                {
                    FinishLiFeng(f);
                    return;
                }

                if (!_coreMatchParticipantsPlayersAccess._players[r.Recipients[r.Cursor]].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r with { Cursor = r.Cursor + 1 } });
                    continue;
                }

                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Recipients[r.Cursor], 1, new(JiyunDrawReason));
                return;
            }

            if (r.Stage != LiFengStage.CollectNext || r.CompletedUse is not { } completed)
                return;
            if (r.Cursor >= r.CompletedEntities.Count)
            {
                FinishLiFeng(f);
                return;
            }

            var entity = r.CompletedEntities[r.Cursor];
            var lease = JiyunLeases().Single(l => l.LeaseId == entity.LeaseId);
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == r.WindowId);
            if (!IsJiyunPile(r.Pile) || _corePublicPileSourcesOperations.PublicPileCards(r.Pile).Count >= 3 || JiyunActualPayment(lease, parent.Action, completed.NativeUseId, completed.ResponseKind)is null || !JiyunTerminalEntity(lease, parent.Action, completed.NativeUseId, completed.ResponseKind))
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunLeaseResolvedEvent(f.Id, completed.ActionId, entity.LeaseId, false));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r with { Cursor = r.Cursor + 1 } });
                continue;
            }

            var from = _cardZonesOperations._cardZones.GetLocation(entity.CardId);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = r with { Stage = LiFengStage.CollectChildren, ActiveCardId = entity.CardId, ActiveFrom = from, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(entity.CardId), from, r.Pile.Location, new(JiyunCollectReason), movement =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { LiFeng = current.LiFeng!with { After = movement.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunClaimedEntityEvent(id, completed.NativeUseId, completed.ActionId, completed.ResponseKind, entity.LeaseId, entity.CardId, entity.Kind, from, r.Pile.Location, movement.Sequence));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunLeaseResolvedEvent(id, completed.ActionId, entity.LeaseId, true));
            });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiFeng = f.LiFeng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        }
    }

    internal void FinishLiFeng(ProgramSkillFrame f)
    {
        var r = f.LiFeng!;
        if (r.CompletedUse is { } completed)
            foreach (var e in r.CompletedEntities.Where(e => !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<JiyunLeaseResolvedEvent>().Any(s => s.LeaseId == e.LeaseId)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiyunLeaseResolvedEvent(f.Id, completed.ActionId, e.LeaseId, false));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiFengCompletedEvent(f.Id, r.Op, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiFeng = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, r.Paid);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27591)]
    internal bool ResumeLiFeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiFeng: not null } f || f.Id != id)
            return false;
        ContinueLiFeng(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27591)]
    internal bool ReturnLiFengMovement(ProgramSkillFrame f)
    {
        if (f.LiFeng is null)
            return false;
        ContinueLiFeng(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.InitializeJiyunCategoryGrain, SkillProgramEffectOp.ResolveJiyunPreparationDrawLeases, SkillProgramEffectOp.CollectJiyunCompletedUseEntities, SkillProgramEffectOp.DistributeShuliangEndingGrain)]
    internal PromptChoice ChooseLiFengAi(PendingDecision prompt, ProgramSkillFrame f)
    {
        if (f.LiFeng!.Stage == LiFengStage.PreparationTargets)
            return prompt.Choices.Where(c => c.Parameters["branch"] == "recipient").OrderBy(c => c.Targets.Single() == f.OwnerSeat ? 0 : 1).FirstOrDefault() ?? prompt.Choices.FirstOrDefault(c => c.Parameters["branch"] == "start") ?? prompt.Choices.Single(c => c.Parameters["branch"] == "cancel");
        return prompt.Choices.Where(c => c.Parameters["branch"] == "give").OrderBy(c => c.Targets.Single() == f.OwnerSeat ? 0 : 1).FirstOrDefault() ?? prompt.Choices.Single(c => c.Parameters["branch"] == "finish");
    }
}
