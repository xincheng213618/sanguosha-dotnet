#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouQunOlRuntime
{
    internal const string ChenshuoRevealReason = "skill-program.zhou-qun.chenshuo.reveal", ChenshuoGainReason = "skill-program.zhou-qun.chenshuo.gain", TianhouExchangeReason = "skill-program.zhou-qun.tianhou.exchange", ZhouQunCleanupReason = "skill-program.zhou-qun.cleanup";
    internal bool HasZhouQunCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveChenshuoRepeatedReveal) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveTianhouTopExchangeAndGrant) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveLieshuEndingLoss) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveNingwuSingletonSlashJudgment) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveZhouyuThunderNeighbors) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveYanshuangEndingLoss);

    internal static CardConversionSource ZhouQunSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_zhou_qun_ol_ZhouQun.ZhouQunSource(f);
    internal static int ZhouQunNameLength(CardKind kind) => RuntimeSkills.RuntimeRules_zhou_qun_ol_ZhouQun.ZhouQunNameLength(kind);
    internal int[] ZhouQunNeighbors(int seat)
    {
        // A just-dead damaged participant still anchors the two directions;
        // its former seat is not silently removed from the after-damage rule.
        var living = _coreTableOrderOperations.CurrentTableOrder().Where(id => _coreMatchParticipantsPlayersAccess._players[id].IsAlive || id == seat).ToArray();
        var index = Array.IndexOf(living, seat);
        return index < 0 || living.Length < 2 ? [] : new[]
        {
            living[(index + living.Length - 1) % living.Length],
            living[(index + 1) % living.Length]
        }.Distinct().OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(seat, s)).ToArray();
    }

    internal bool ZhouQunParent(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.OwnerSeat != c.OwnerSeat)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == context.ParentFrameId);
        if (parent is ProgramLifecycleTriggerWindowFrame lifecycle)
            return context.Window == lifecycle.Window && lifecycle.CandidateIndex >= 0 && lifecycle.CandidateIndex < lifecycle.Candidates.Count && lifecycle.Candidates[lifecycle.CandidateIndex] == c;
        if (parent is TurnEndingBoundaryFrame ending)
            return context.Window == SkillProgramTriggerWindow.TurnEnding && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate == c;
        if (parent is ProgramCardTriggerWindowFrame card)
            return context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && card.CandidateIndex >= 0 && card.CandidateIndex < card.Candidates.Count && card.Candidates[card.CandidateIndex] is { } candidate && candidate.OwnerSeat == c.OwnerSeat && candidate.SkillId == c.SkillId && candidate.TriggerId == c.BindingId && candidate.SkillInstanceId == c.SkillInstanceId && candidate.GameplayHash == c.GameplayHash && context.CardUse?.ParentCardUseFrameId == card.ParentFrameId;
        if (parent is DamageTriggerWindowFrame damage)
            return context.Window == damage.TriggerWindow && damage.CandidateIndex >= 0 && damage.CandidateIndex < damage.Candidates.Count && damage.Candidates[damage.CandidateIndex].ToProgramCandidate() == c;
        return parent is BeforeDamageProgramWindowFrame before && context.Window == SkillProgramTriggerWindow.BeforeDamageApplied && before.CandidateIndex >= 0 && before.CandidateIndex < before.Candidates.Count && before.Candidates[before.CandidateIndex].Candidate == c;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27572)]
    internal bool CanOfferZhouQun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId == "ol:zhouyu" && trigger.Effects is [{ Op: SkillProgramEffectOp.PreventCurrentDamage }])
            return _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is BeforeDamageProgramWindowFrame { Nature: DamageNature.Fire, Prevented: false } before && _coreTableSeatQueriesOperations.IsValidPlayerSeat(before.SourceSeat) && before.SourceSeat != candidate.OwnerSeat && _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.IsSourceLess != true;
        if (trigger.Effects is not [var effect] || !ZhouQunComposition.Handles(effect.Op))
            return true;
        if (effect.Op == SkillProgramEffectOp.ResolveChenshuoRepeatedReveal)
            return _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0);
        if (effect.Op == SkillProgramEffectOp.ResolveNingwuSingletonSlashJudgment)
            return NingwuEligibleUse(candidate.OwnerSeat, context)is not null;
        if (effect.Op == SkillProgramEffectOp.ResolveZhouyuThunderNeighbors)
            return context.Amount > 0 && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is DamageTriggerWindowFrame d && _coreNativeCardClassificationsOperations.GetDamageNature(_coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(d)) == DamageNature.Thunder;
        if (effect.Op == SkillProgramEffectOp.ResolveTianhouTopExchangeAndGrant)
            return true;
        var target = context.TargetSeat ?? context.SourceSeat;
        if (target is not { } seat || seat == candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            return false;
        var hp = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue).Select(p => p.Hp).ToArray();
        return hp.Length > 0 && _coreMatchParticipantsPlayersAccess._players[seat].HasHpValue && _coreMatchParticipantsPlayersAccess._players[seat].Hp == (effect.Op == SkillProgramEffectOp.ResolveLieshuEndingLoss ? hp.Max() : hp.Min());
    }

    SkillProgramStepOutcome IZhouQunHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginZhouQun(frame, op);
    internal SkillProgramStepOutcome BeginZhouQun(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ZhouQun is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || !ZhouQunComposition.Handles(op))
            throw new InvalidOperationException("Zhou Qun requires his original living native trigger.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex);
        if (!ZhouQunParent(candidate, context) || !CanOfferZhouQun(candidate, _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("The current weather binding lost its exact native parent or qualification.");
        var stage = op == SkillProgramEffectOp.ResolveChenshuoRepeatedReveal ? ZhouQunStage.ChooseHandOwner : op == SkillProgramEffectOp.ResolveTianhouTopExchangeAndGrant ? ZhouQunStage.LeaseChildren : op == SkillProgramEffectOp.ResolveNingwuSingletonSlashJudgment ? ZhouQunStage.Judgment : ZhouQunStage.HpIssuing;
        var r = new ZhouQunReceipt(ZhouQunSource(f), f.GameplayHash, op, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, stage)
        {
            Paid = true
        };
        if (stage == ZhouQunStage.HpIssuing)
            r = r with
            {
                Targets = op == SkillProgramEffectOp.ResolveZhouyuThunderNeighbors ? ZhouQunNeighbors(context.TargetSeat!.Value) : [context.TargetSeat ?? context.SourceSeat!.Value]
            };
        if (stage == ZhouQunStage.Judgment)
        {
            var use = NingwuEligibleUse(f.OwnerSeat, context)!;
            r = r with
            {
                UseId = use.Id,
                ActionId = use.Action!.ActionId,
                Peer = use.SourceSeat,
                Targets = use.Action.EffectiveDesignatedTargetSeats,
                SlashRank = use.Action.EffectiveRank,
                JudgmentId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhouQunStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.ActualTurn, r.TurnOwner));
        for (var i = 0; i < r.Targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhouQunHpTargetEvent(f.Id, i, r.Targets[i]));
        if (stage == ZhouQunStage.LeaseChildren)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianhouInvokedEvent(f.Id, r.Source, r.GameplayHash));
            ExpireTianhouWeather(f);
        }

        if (stage == ZhouQunStage.Judgment)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NingwuJudgmentIssuedEvent(f.Id, r.JudgmentId, r.UseId, r.ActionId, r.Peer, r.Targets.Single(), r.SlashRank));
            _ = _coreNativeJudgmentOperations.BeginJudgment(null, r.Peer, "ningwu", f.Id, context.CardUse!.EffectiveKind, JudgmentContinuationKind.ProgramSkill, sourceSeat: f.OwnerSeat, programResultBind: "ningwu-result", programResultVisibility: SkillProgramCardSetVisibility.Public);
        }
        else
            ContinueZhouQun(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int ZhouQunChooser(ProgramSkillFrame f) => f.ZhouQun is { Stage: ZhouQunStage.ChooseShownHand, Peer: >= 0 } r ? r.Peer : f.OwnerSeat;
    internal PromptChoice ZhouQunChoice(ProgramSkillFrame f, string branch, string label, int? card = null, int? target = null) => new(new($"zhou-qun.{(f.Id)}.{(f.ZhouQun!.Step)}.{(f.ZhouQun.Stage)}.{(branch)}.{(card ?? target ?? -1)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "zhou-qun", ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> ZhouQunChoices(ProgramSkillFrame f)
    {
        var r = f.ZhouQun!;
        var choices = new List<PromptChoice>();
        if (r.Stage == ZhouQunStage.ChooseHandOwner)
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).Select(p => ZhouQunChoice(f, "hand-owner", "令" + p.Name + "展示一张手牌", target: p.Seat)));
        else if (r.Stage == ZhouQunStage.ChooseShownHand)
            choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Peer]).Select(c => ZhouQunChoice(f, "show-hand", "展示【" + c.DisplayName + "】", c.Id)));
        else if (r.Stage == ZhouQunStage.ExchangeHand)
        {
            if (r.SelectedHand.Count < r.Viewed.Count)
                choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !r.SelectedHand.Contains(c.Id)).Select(c => ZhouQunChoice(f, "hand", "换入牌堆【" + c.DisplayName + "】", c.Id)));
            choices.AddRange(r.SelectedHand.Select(id => ZhouQunChoice(f, "unhand", "撤回【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName + "】", id)));
            choices.Add(ZhouQunChoice(f, "hand-done", "确定等量交换（可不交换）"));
        }
        else if (r.Stage == ZhouQunStage.ExchangeTop)
            choices.AddRange(r.Viewed.Where(id => !r.SelectedTop.Contains(id)).Select(id => ZhouQunChoice(f, "top", "换取【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName + "】", id)));
        else if (r.Stage == ZhouQunStage.ChooseWeatherCard)
            choices.AddRange(r.Viewed.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile).Select(id => ZhouQunChoice(f, "weather-card", "展示【" + _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName + "】授予" + ZhouQunWeather(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Suit).Name, id)));
        else if (r.Stage == ZhouQunStage.ChooseWeatherRecipient)
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => ZhouQunChoice(f, "weather-target", "令" + p.Name + "获得" + ZhouQunWeather(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.ChosenCard).Suit).Name, target: p.Seat)));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishZhouQun(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            CleanupAndFinishZhouQun(f);
            return;
        }

        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(ZhouQunChooser(f)) || !_coreMatchParticipantsPlayersAccess._players[ZhouQunChooser(f)].IsAlive || ZhouQunChoices(f).Count == 0)
        {
            if (f.ZhouQun is { Op: SkillProgramEffectOp.ResolveChenshuoRepeatedReveal } r && r.Revealed.Count > 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhouQun = r with { Stage = ZhouQunStage.RevealChildren, Repeat = false } });
                ContinueZhouQun(f.Id);
            }
            else
                CleanupAndFinishZhouQun(f);
            return;
        }

        var choices = ZhouQunChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var chooser = ZhouQunChooser(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name + "：请选择真实牌或角色。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("zhou-qun")]
    internal void AnswerZhouQun(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhouQun: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != ZhouQunChooser(f) || !ZhouQunChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Weather choices require the real current private chooser and exact entities.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "hand-owner")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r with { Peer = choice.Targets.Single(), Stage = ZhouQunStage.ChooseShownHand } });
            PublishZhouQun(f);
            return;
        }

        if (branch == "show-hand")
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
            var kind = _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.Peer], card);
            var shown = new ZhouQunShownCard(card.Id, r.Peer, kind, _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.Peer], card), card.Rank, ZhouQunNameLength(kind));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r with { Shown = shown, ShownCards = r.ShownCards.Append(shown).ToArray(), Stage = ZhouQunStage.RevealChildren, Repeat = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChenshuoHandShownEvent(f.Id, r.Revealed.Count, shown));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.TriggerId!, r.Peer, "chenshuo-hand", [_coreSnapshotValueProjectionOperations.ToSnapshot(card)]));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "chenshuo-selected-hand"))
                return;
            ContinueZhouQun(f.Id);
            return;
        }

        if (branch == "hand")
            r = r with
            {
                SelectedHand = r.SelectedHand.Append(choice.Cards.Single()).ToArray()
            };
        else if (branch == "unhand")
            r = r with
            {
                SelectedHand = r.SelectedHand.Where(id => id != choice.Cards.Single()).ToArray()
            };
        else if (branch == "hand-done")
        {
            r = r with
            {
                Stage = ZhouQunStage.ExchangeTop
            };
            if (r.SelectedHand.Count == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r });
                PayTianhouExchange(f);
                return;
            }
        }
        else if (branch == "top")
        {
            r = r with
            {
                SelectedTop = r.SelectedTop.Append(choice.Cards.Single()).ToArray()
            };
            if (r.SelectedTop.Count == r.SelectedHand.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r });
                PayTianhouExchange(f);
                return;
            }
        }
        else if (branch == "weather-card")
        {
            var id = choice.Cards.Single();
            r = r with
            {
                ChosenCard = id,
                Stage = ZhouQunStage.ChooseWeatherRecipient
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianhouCardShownEvent(f.Id, f.OwnerSeat, id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Suit));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, f.TriggerId!, f.OwnerSeat, "tianhou-weather", [_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))]));
            PublishZhouQun(f);
            return;
        }
        else if (branch == "weather-target")
        {
            GrantTianhouWeather(f, choice.Targets.Single());
            ContinueZhouQun(f.Id);
            return;
        }
        else
            throw new InvalidOperationException("Unknown Zhou Qun choice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r });
        PublishZhouQun(f);
    }

    [PublicHandRevealReturn("chenshuo-selected-hand")]
    internal void ReturnChenshuoPublicHand(ProgramSkillFrame f)
    {
        ContinueZhouQun(f.Id);
    }

    internal void BeginZhouQunMovement(ref ProgramSkillFrame f, ZhouQunStage stage)
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.GrantRevision);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = f.ZhouQun!with { Stage = stage, Step = f.ZhouQun.Step + 1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
    }

    internal void CompleteZhouQunMovement(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.ZhouQun!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhouQunMoveIssuedEvent(id, r.Step, r.Stage, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal void RecordZhouQunMoves(ProgramSkillFrame f, long batch, IReadOnlyList<CardMovementRecord> records)
    {
        foreach (var m in records)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhouQunMovedEvent(f.Id, f.ZhouQun!.Step, batch, m.CardId, m.From, m.To, m.Reason, m.Sequence));
    }

    internal bool DrainZhouQun(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueZhouQun(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ZhouQun: { } r } f && f.Id == id)
        {
            if (r.Stage == ZhouQunStage.Judgment)
                return;
            if (r.Stage is ZhouQunStage.ChooseHandOwner or ZhouQunStage.ChooseShownHand or ZhouQunStage.ExchangeHand or ZhouQunStage.ExchangeTop or ZhouQunStage.ChooseWeatherCard or ZhouQunStage.ChooseWeatherRecipient)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishZhouQun(f);
                return;
            }

            if (r.Stage == ZhouQunStage.HpIssuing)
            {
                if (r.Cursor == r.Targets.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishZhouQun(f);
                    return;
                }

                if (!_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhouQun = r with { Cursor = r.Cursor + 1 } });
                    continue;
                }

                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, r.Targets[r.Cursor], 1);
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                if (f.ZhouQun!.Stage == ZhouQunStage.HpIssuing)
                    return;
            }

            if (DrainZhouQun(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ZhouQun!;
            if (f.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == ZhouQunStage.HpChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhouQun = r with { Stage = ZhouQunStage.HpIssuing, Cursor = r.Cursor + 1 } });
                continue;
            }

            if (r.Stage == ZhouQunStage.LeaseChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishZhouQun(f);
                    return;
                }

                BeginZhouQunMovement(ref f, ZhouQunStage.ViewChildren);
                _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
                var viewed = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(3).Reverse().Select(c => c.Id).ToArray();
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhouQun = f.ZhouQun!with { Viewed = viewed } });
                for (var i = 0; i < viewed.Length; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TianhouViewedEvent(id, i, viewed[i]));
                CompleteZhouQunMovement(id);
                continue;
            }

            if (r.Stage == ZhouQunStage.ViewChildren || r.Stage == ZhouQunStage.ExchangeChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r with { Viewed = r.Stage == ZhouQunStage.ExchangeChildren ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(3).Reverse().Select(c => c.Id).ToArray() : r.Viewed, Stage = r.Stage == ZhouQunStage.ViewChildren ? ZhouQunStage.ExchangeHand : ZhouQunStage.ChooseWeatherCard } });
                PublishZhouQun(f);
                return;
            }

            if (r.Stage == ZhouQunStage.RevealChildren)
            {
                if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && r.Repeat && r.Revealed.Count < 3)
                {
                    if (r.ShownCards.Count == r.Revealed.Count)
                    {
                        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = r with { Stage = ZhouQunStage.ChooseHandOwner } });
                        PublishZhouQun(f);
                        return;
                    }

                    RevealNextChenshuo(f);
                    continue;
                }

                BeginZhouQunMovement(ref f, ZhouQunStage.GainChildren);
                var ids = r.Revealed.Where(card => _cardZonesOperations._cardZones.GetLocation(card) == CardLocation.Processing).ToArray();
                if (ids.Length > 0)
                    _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? CardLocation.Hand(f.OwnerSeat) : CardLocation.DiscardPile, new(ChenshuoGainReason), (batch, records) => RecordZhouQunMoves(f, batch, records));
                CompleteZhouQunMovement(id);
                continue;
            }

            if (r.Stage == ZhouQunStage.JudgmentChildren)
            {
                ApplyNingwuJudgment(f);
                FinishZhouQun(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            }

            FinishZhouQun(f);
            return;
        }
    }

    internal void CleanupAndFinishZhouQun(ProgramSkillFrame f)
    {
        var ids = f.ZhouQun!.Revealed.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).ToArray();
        if (ids.Length == 0)
        {
            FinishZhouQun(f);
            return;
        }

        BeginZhouQunMovement(ref f, ZhouQunStage.CleanupChildren);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.DiscardPile, new(ZhouQunCleanupReason), (batch, records) => RecordZhouQunMoves(f, batch, records));
        CompleteZhouQunMovement(f.Id);
        ContinueZhouQun(f.Id);
    }

    internal void FinishZhouQun(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhouQunCompletedEvent(f.Id, true));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhouQun = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27572)]
    internal bool ResumeZhouQun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhouQun: not null } f || f.Id != id)
            return false;
        ContinueZhouQun(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27572)]
    internal bool ReturnZhouQunMovement(ProgramSkillFrame f)
    {
        if (f.ZhouQun is null)
            return false;
        ContinueZhouQun(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27572)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27572)]
    internal bool ContinuePaidZhouQun(ProgramSkillFrame f)
    {
        if (f.ZhouQun is not { Paid: true })
            return false;
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27572)]
    internal void RecordZhouQunHpPayment(ProgramSkillHpLostEvent e)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ZhouQun: { Stage: ZhouQunStage.HpIssuing } r } f && e.FrameId == f.Id && e.SkillId == f.SkillId && r.Cursor < r.Targets.Count && e.TargetSeat == r.Targets[r.Cursor])
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ZhouQun = r with { Stage = ZhouQunStage.HpChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhouQunHpPaidEvent(f.Id, r.Cursor, e.TargetSeat, e.Amount, e.RemainingHp));
        }
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveChenshuoRepeatedReveal, SkillProgramEffectOp.ResolveTianhouTopExchangeAndGrant)]
    internal PromptChoice ChooseZhouQun(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "hand-done") ?? p.Choices.First();
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 80)]
    internal IReadOnlyList<CardSnapshot> ZhouQunPrivatelyViewedTopCards(int viewerSeat) => Array.AsReadOnly(_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewerSeat && f.ZhouQun is { Stage: ZhouQunStage.ExchangeHand or ZhouQunStage.ExchangeTop or ZhouQunStage.ChooseWeatherCard or ZhouQunStage.ChooseWeatherRecipient }).SelectMany(f => f.ZhouQun!.Viewed).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray());
    [ProgramViewContributor(ProgramViewContribution.PublicCardsBeforeResolution, 30)]
    internal IEnumerable<CardSnapshot> ProjectZhouQunPublicShownCards() => ZhouQunPublicShownCardIds().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))with { PublicHandOwnerSeat = _cardZonesOperations._cardZones.GetLocation(id).Zone == CardZoneKind.Hand ? _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat : null });
    internal IEnumerable<int> ZhouQunPublicShownCardIds() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.ZhouQun is not null).SelectMany(f => f.ZhouQun!.Revealed.Concat(f.ZhouQun.ShownCards.Select(c => c.CardId)).Concat(f.ZhouQun.ChosenCard > 0 ? [f.ZhouQun.ChosenCard] : [])).Distinct();
}
