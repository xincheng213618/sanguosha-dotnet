#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiMobileShiRuntime
{
    internal const string ShiDaTuntian = "mobile:shi-tuntian", ShiDaZaoxian = "mobile:shi-zaoxian", ShiDaJixi = "mobile:shi-jixi";
    internal const string ShiDaHeartReason = "skill.mobile-shi-deng-ai.tuntian.obtain", ShiDaRewardReason = "skill.mobile-shi-deng-ai.zaoxian.obtain", ShiDaCostReason = "skill.mobile-shi-deng-ai.jixi.discard";
    internal static CardConversionSource ShiDaSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal (int Current, int Capacity) ShiDaCharge(int owner)
    {
        var e = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaChargeChangedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner);
        return e is null ? (0, 0) : (e.After, e.CapacityAfter);
    }

    internal void ChangeShiDaCharge(ProgramSkillFrame f, int amount, int capacity, long batch = 0) => ChangeShiDaCharge(f.Id, ShiDaSource(f), f.GameplayHash, amount, capacity, batch);
    internal void ChangeShiDaCharge(long frameId, CardConversionSource source, string hash, int amount, int capacity, long batch)
    {
        if (source.SkillId != ShiDaTuntian || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat) || _contentRegistryOperations._contentRegistry.GetSkill(ShiDaTuntian).Program?.GameplayHash != hash)
            throw new InvalidOperationException("Tuntian progression requires its frozen actual source and current content hash.");
        var before = ShiDaCharge(source.OwnerSeat);
        var cap = checked(before.Capacity + capacity);
        if (cap < 0)
            throw new InvalidOperationException("Tuntian cannot have negative capacity.");
        var after = Math.Clamp(checked(before.Current + amount), 0, cap);
        if (before.Current != after || before.Capacity != cap)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaChargeChangedEvent(frameId, source, before.Current, after, before.Capacity, cap, batch));
    }

    internal static bool ShiDaOwns(CardLocation location, int owner) => location.OwnerSeat == owner && location.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment;
    internal static bool ShiDaDamageCard(CardKind kind) => CoreGameEngineMouLuMengRuntime.IsDamageCardKind(kind) || CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(kind) || kind == CardKind.Lightning;
    internal IReadOnlyList<CardMovementRecord> ShiDaLosses(int owner, CardMovementBatchContext? batch) => batch is null ? [] : batch.Movements.Where(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && ShiDaOwns(m.From, owner) && !ShiDaOwns(m.To, owner) && !ShiDaDamageCard(m.CardKind) && !IsShiDaDeferredEquipmentEntry(batch, m)).GroupBy(m => m.CardId).Select(group => group.OrderBy(m => m.Sequence).First()).ToArray();
    internal IReadOnlyList<int> ShiDaHistoricalTargets(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaTargetRecordedEvent>().Where(e => e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Actor == owner && e.Target != owner).Select(e => e.Target).Distinct().ToArray();
    internal bool ShiDaHasJixiTarget(int owner) => !_coreCardUseModulesOperations.IsCardUseForbidden(owner, CardKind.Snatch, CardActionType.Use) && ShiDaHistoricalTargets(owner).Any(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalOwnedCardQueriesOperations.HasTargetCard(_coreMatchParticipantsPlayersAccess._players[seat]) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.Snatch, Suit.None, null) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner, seat, CardKind.Snatch));
    internal IReadOnlyList<PromptChoice> ShiDaCostChoices(long id, int owner, int payer) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(id, owner, payer, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard);
    internal bool ShiDaHasJixiDiscard(ProgramTriggerCandidate candidate, int payer) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(payer) && new[]
    {
        CardZoneKind.Hand,
        CardZoneKind.Equipment,
        CardZoneKind.Judgment
    }.Any(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, payer)).Any(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(candidate.OwnerSeat, card, new(zone, payer), OwnedCardMoveIntent.Discard) && !(zone == CardZoneKind.Equipment && payer == candidate.OwnerSeat && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(payer, candidate.SkillId, candidate.SkillInstanceId, card))));
    SkillProgramStepOutcome IShiDengAiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShiDa(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 50300)]
    internal bool CanOfferShiDa(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId == ShiDaTuntian)
        {
            var charge = ShiDaCharge(candidate.OwnerSeat);
            return trigger.Id == "full-charge" ? charge.Current == charge.Capacity : trigger.Id != "non-damage-loss" || charge.Current < charge.Capacity && ShiDaLosses(candidate.OwnerSeat, context.MovementBatch).Count > 0;
        }

        return candidate.SkillId != ShiDaJixi || trigger.Id != "ending-jixi" || _coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && ShiDaHasJixiTarget(candidate.OwnerSeat) && ShiDaHasJixiDiscard(candidate, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginShiDa(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.InstructionIndex != 1 || f.ShiDengAi is not null)
            throw new InvalidOperationException("Shi Deng Ai requires its original unstarted instruction.");
        if (op == SkillProgramEffectOp.ResolveMobileShiDengAiGain)
        {
            if (f.SkillId != ShiDaTuntian || f.TriggerId != "non-damage-loss" || !ShiDaWindowMatches(f, SkillProgramTriggerWindow.CardsMoved) || f.WindowContext?.MovementBatch is not { } b)
                throw new InvalidOperationException("Tuntian gain needs its exact real owner loss batch.");
            ChangeShiDaCharge(f, ShiDaLosses(f.OwnerSeat, b).Count, 0, b.Id);
            return SkillProgramStepOutcome.Continue;
        }

        if (op == SkillProgramEffectOp.ResolveMobileShiDengAiTargets)
        {
            var w = f.WindowContext;
            if (f.SkillId != ShiDaJixi || w is null || w.Window is not (SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseCompleted) || !ShiDaWindowMatches(f, w.Window) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame parent || parent.Action is not { Type: CardActionType.Use } a || a.ActorSeat != f.OwnerSeat || w.CardUse?.CardActionId != a.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { } use || use.Action?.ActionId != a.ActionId)
                throw new InvalidOperationException("Jixi target history needs the same native actor and actual-use window.");
            RecordShiDaTargets(use.Id, a);
            return SkillProgramStepOutcome.Continue;
        }

        var stage = op == SkillProgramEffectOp.ResolveMobileShiDengAiFull ? ShiDaStage.Drawing : op == SkillProgramEffectOp.ResolveMobileShiDengAiTuntian ? ShiDaStage.Amount : ShiDaStage.Cost;
        if (op == SkillProgramEffectOp.ResolveMobileShiDengAiFull ? f.SkillId != ShiDaTuntian || f.TriggerId != "full-charge" || !ShiDaWindowMatches(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || ShiDaCharge(f.OwnerSeat)is var charge && charge.Current != charge.Capacity : op == SkillProgramEffectOp.ResolveMobileShiDengAiTuntian ? f.SkillId != ShiDaTuntian || f.TriggerId is not null || f.ActivationId != "spend-charge" || f.WindowContext is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != f.OwnerSeat : op != SkillProgramEffectOp.ResolveMobileShiDengAiJixi || f.SkillId != ShiDaJixi || f.TriggerId != "ending-jixi" || !ShiDaWindowMatches(f, SkillProgramTriggerWindow.AfterTurnEnded))
            throw new InvalidOperationException("Shi Deng Ai lost its actual Play or turn boundary.");
        var r = new ShiDaReceipt(ShiDaSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, f.WindowContext?.ParentFrameId ?? 0, stage)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            History = op == SkillProgramEffectOp.ResolveMobileShiDengAiJixi ? ShiDaHistoricalTargets(f.OwnerSeat) : [],
            Paid = op == SkillProgramEffectOp.ResolveMobileShiDengAiFull
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaStartedEvent(f.Id, r.Source, r.Hash, op, r.Turn, r.TurnOwner, r.WindowId));
        ContinueShiDa(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void RecordShiDaTargets(long useId, CardActionContext action)
    {
        foreach (var seat in action.EffectiveDesignatedTargetSeats.Where(seat => seat != action.ActorSeat).Distinct())
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<ShiDaTargetRecordedEvent>().Any(e => e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.UseId == useId && e.ActionId == action.ActionId && e.Actor == action.ActorSeat && e.Target == seat))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaTargetRecordedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, action.ActorSeat, useId, action.ActionId, seat));
    }

    // Additions can themselves create observer windows before the whole Use completes.
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 50300)]
    internal void CaptureShiDaTargetUnion(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiDengAiTargets) || action.Type != CardActionType.Use || window is not (SkillProgramTriggerWindow.CardUseBeforeTargetEffects or SkillProgramTriggerWindow.CardEffectBeforeApply or SkillProgramTriggerWindow.CardUseCompleted) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId)is not { } use)
            return;
        RecordShiDaTargets(use.Id, action);
    }

    internal PromptChoice ShiDaChoice(ProgramSkillFrame f, string branch, string label, int number = -1, int? target = null, int? card = null) => new(new($"shi-deng-ai.{(f.Id)}.{(f.ShiDengAi!.Stage)}.{(branch)}.{(number)}.{(target ?? -1)}.{(card ?? -1)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mobile-shi-deng-ai", ["branch"] = branch, ["number"] = number.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> ShiDaChoices(ProgramSkillFrame f)
    {
        var r = f.ShiDengAi!;
        var choices = new List<PromptChoice>();
        if (r.Stage == ShiDaStage.Amount)
            for (var n = 0; n <= ShiDaCharge(f.OwnerSeat).Current; n++)
                choices.Add(ShiDaChoice(f, "amount", $"消耗{(n)}点蓄力", n));
        if (r.Stage == ShiDaStage.Recipients)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (r.Targets.Contains(p.Seat) || r.Targets.Count < r.Amount)))
                choices.Add(ShiDaChoice(f, "target", (r.Targets.Contains(p.Seat) ? "取消：" : "选择：") + p.Name, target: p.Seat));
            choices.Add(ShiDaChoice(f, "spend", $"确认消耗{(r.Amount)}点，令{(r.Targets.Count)}人随机获得红桃牌"));
        }

        if (r.Stage == ShiDaStage.Rewards && r.Zaoxian is not null)
        {
            var kind = ShiDaRewardKind(r.RewardCursor);
            choices.AddRange(_cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => c.Kind == kind).Select(c => ShiDaChoice(f, "reward", "凿险：获得【" + c.DisplayName + "】（" + c.Suit + c.RankText + "）", card: c.Id)));
        }

        if (r.Stage == ShiDaStage.Cost)
            choices.AddRange(ShiDaCostChoices(f.Id, f.OwnerSeat, r.TurnOwner).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mobile-shi-deng-ai", ["branch"] = "cost" } }));
        if (r.Stage == ShiDaStage.Targets)
        {
            foreach (var seat in ShiDaSnatchTargets(f).Where(seat => r.History.Contains(seat)))
                choices.Add(ShiDaChoice(f, "target", (r.Targets.Contains(seat) ? "取消：" : "选择：") + _coreMatchParticipantsPlayersAccess._players[seat].Name, target: seat));
            if (r.Targets.Count > 0 && r.Targets.All(seat => ShiDaSnatchTargets(f).Contains(seat)))
                choices.Add(ShiDaChoice(f, "snatch", $"对{(r.Targets.Count)}名角色使用一张无距离限制的顺手牵羊"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishShiDa(ProgramSkillFrame f)
    {
        var r = f.ShiDengAi!;
        var choices = ShiDaChoices(f);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || choices.Count == 0)
        {
            FinishShiDa(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage switch
        {
            ShiDaStage.Amount => "屯田：选择消耗的蓄力点数。",
            ShiDaStage.Recipients => "屯田：选择至多等量角色；确认前不消耗。",
            ShiDaStage.Rewards => "凿险：从弃牌堆获得对应牌。",
            ShiDaStage.Cost => "急袭：弃置当前回合角色的一张牌。",
            _ => "急袭：选择本回合成为过你牌目标的其他角色。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-shi-deng-ai")]
    internal void AnswerShiDa(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiDengAi: { } r } f || !ValidShiDa(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiDaChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Shi Deng Ai requires the exact current chooser, original cost slot and unpaid selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (branch == "amount")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r with { Amount = int.Parse(selected.Parameters["number"], CultureInfo.InvariantCulture), Stage = ShiDaStage.Recipients } });
            PublishShiDa(f);
            return;
        }

        if (branch == "target")
        {
            var seat = selected.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r with { Targets = r.Targets.Contains(seat) ? r.Targets.Where(s => s != seat).ToArray() : r.Targets.Append(seat).ToArray() } });
            PublishShiDa(f);
            return;
        }

        if (branch == "spend")
        {
            if (r.Paid || r.Amount < 0 || r.Amount > ShiDaCharge(f.OwnerSeat).Current || r.Targets.Count > r.Amount)
                throw new InvalidOperationException("Tuntian cannot spend missing or already paid charge.");
            var zaoxian = _coreNationalSkillsOperations.EnabledRuntimeSkillGrants(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(grant => grant.SkillId == ShiDaZaoxian).OrderBy(grant => grant.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
            var source = zaoxian is null ? null : new CardConversionSource(ShiDaZaoxian, "charge-thresholds", f.OwnerSeat, zaoxian.SkillInstanceId);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r with { Paid = true, Zaoxian = source, Stage = ShiDaStage.Rewards } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaSpentEvent(f.Id, r.Source, r.Amount, r.Targets.Count, source));
            foreach (var pair in r.Targets.Select((seat, index) => (seat, index)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaRecipientEvent(f.Id, pair.index, pair.seat));
            ChangeShiDaCharge(f, -r.Amount, 0);
            ContinueShiDa(f.Id);
            return;
        }

        if (branch == "reward")
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single());
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r with { Stage = ShiDaStage.RewardChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, CardId = card.Id, CardFrom = CardLocation.DiscardPile }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaThresholdObtainedEvent(f.Id, r.Zaoxian!, 3 + 2 * r.RewardCursor, card.Kind, card.Id));
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new(ShiDaRewardReason), move => SealShiDaMove(f.Id, move));
            ContinueShiDa(f.Id);
            return;
        }

        if (branch == "cost")
        {
            var from = new CardLocation(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), r.TurnOwner);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r with { Paid = true, Stage = ShiDaStage.CostChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, CardId = card.Id, CardFrom = from }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TurnOwner, [card.Id], new(ShiDaCostReason), (batch, moves) =>
            {
                var move = moves.Single();
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { ShiDengAi = active.ShiDengAi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, PaymentSequence = move.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaCostPaidEvent(f.Id, batch, r.TurnOwner, card.Id, move.From, move.To, move.Sequence));
            });
            ContinueShiDa(f.Id);
            return;
        }

        if (branch == "snatch")
        {
            IssueShiDaSnatch(f);
            return;
        }

        throw new InvalidOperationException("Unknown Shi Deng Ai choice.");
    }

    internal static CardKind ShiDaRewardKind(int index) => index switch
    {
        0 => CardKind.DrawTwo,
        1 => CardKind.Nullification,
        _ => CardKind.FiveGrains
    };
    internal bool DrainShiDa(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueShiDa(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiDengAi: { } r } f || f.Id != id)
            return;
        if (r.Stage is ShiDaStage.DrawChildren or ShiDaStage.RewardChildren or ShiDaStage.HeartChildren or ShiDaStage.CostChildren or ShiDaStage.NativeChildren)
        {
            if (DrainShiDa(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShiDengAi!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == ShiDaStage.DrawChildren)
            {
                ChangeShiDaCharge(f, 0, 1);
                FinishShiDa(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            }

            if (r.Stage == ShiDaStage.NativeChildren)
            {
                FinishShiDa(f);
                return;
            }

            r = r.Stage switch
            {
                ShiDaStage.RewardChildren => r with
                {
                    Stage = ShiDaStage.Rewards,
                    RewardCursor = r.RewardCursor + 1
                },
                ShiDaStage.HeartChildren => r with
                {
                    Stage = ShiDaStage.Hearts,
                    Cursor = r.Cursor + 1,
                    Eligible = [],
                    RandomIndex = -1
                },
                _ => r with
                {
                    Stage = ShiDaStage.Targets
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r });
        }

        if (r.Stage == ShiDaStage.Drawing)
        {
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new("skill.mobile-shi-deng-ai.tuntian.full"));
            return;
        }

        if (r.Stage == ShiDaStage.Rewards)
        {
            while (r.RewardCursor < 3 && (r.Zaoxian is null || r.Amount < 3 + 2 * r.RewardCursor || !_cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Any(c => c.Kind == ShiDaRewardKind(r.RewardCursor))))
                r = r with
                {
                    RewardCursor = r.RewardCursor + 1
                };
            if (r.RewardCursor < 3)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r });
                PublishShiDa(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r = r with { Stage = ShiDaStage.Hearts } });
        }

        if (r.Stage == ShiDaStage.Hearts)
        {
            while (r.Cursor < r.Targets.Count && (!_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive))
                r = r with
                {
                    Cursor = r.Cursor + 1
                };
            if (r.Cursor >= r.Targets.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishShiDa(f);
                return;
            }

            var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile)).Where(c => c.Suit == Suit.Heart).OrderBy(c => c.Id).ToArray();
            var before = _randomOperations._random.State;
            var index = candidates.Length == 0 ? -1 : _randomOperations._random.Next(candidates.Length);
            var after = _randomOperations._random.State;
            var card = index < 0 ? null : candidates[index];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r with { Stage = ShiDaStage.HeartChildren, Eligible = candidates.Select(c => c.Id).ToArray(), RandomIndex = index, RandomBefore = before, RandomAfter = after, CardId = card?.Id ?? 0, CardFrom = card is null ? null : _cardZonesOperations._cardZones.GetLocation(card.Id), Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            for (var n = 0; n < candidates.Length; n++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaRandomCandidateEvent(id, r.Cursor, n, candidates[n].Id));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaRandomIssuedEvent(id, r.Cursor, r.Targets[r.Cursor], card?.Id ?? 0, index, candidates.Length, before, after));
            if (card is not null)
                _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.Hand(r.Targets[r.Cursor]), new(ShiDaHeartReason), move => SealShiDaMove(id, move));
            ContinueShiDa(id);
            return;
        }

        PublishShiDa(f);
    }

    internal void SealShiDaMove(long id, CardMovementRecord move)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.ShiDengAi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiDengAi = r with { After = move.Sequence, PaymentSequence = move.Sequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaCardMovedEvent(id, r.Stage, r.Cursor, move.CardId, move.From, move.To, move.Sequence));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiDengAiFull)]
    internal void ReturnShiDaDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.ShiDengAi is not { Stage: ShiDaStage.Drawing, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1)
            throw new InvalidOperationException("Tuntian lost its original full-charge native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = r with { Stage = ShiDaStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaDrawPaidEvent(f.Id, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueShiDa(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 50300)]
    internal bool ResumeShiDa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiDengAi: not null } f || f.Id != id)
            return false;
        ContinueShiDa(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 50300)]
    internal bool ReturnShiDaMovement(ProgramSkillFrame f) => f.ShiDengAi is not null && ResumeShiDa(f.Id);
    internal void FinishShiDa(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiDaCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiDengAi = f.ShiDengAi!with { Stage = ShiDaStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiDengAiTuntian, SkillProgramEffectOp.ResolveMobileShiDengAiJixi)]
    internal PromptChoice ChooseShiDa(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.ShiDengAi!;
        if (r.Stage == ShiDaStage.Amount)
            return p.Choices.Last();
        if (r.Stage == ShiDaStage.Recipients)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "target" && !r.Targets.Contains(c.Targets.Single()) && CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])) ?? p.Choices.Single(c => c.Parameters["branch"] == "spend");
        if (r.Stage == ShiDaStage.Targets)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "target" && !r.Targets.Contains(c.Targets.Single())) ?? p.Choices.Single(c => c.Parameters["branch"] == "snatch");
        return p.Choices[0];
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiDengAiPublicStates(CharacterState owner, string skillId)
    {
        if (skillId is not (ShiDaTuntian or ShiDaJixi))
            return[];
        var charge = ShiDaCharge(owner.Seat);
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId).SelectMany(i => skillId == ShiDaTuntian ? new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "charge", ProgramPublicRuleStateKind.GameActivationProgress, charge.Current), new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "capacity", ProgramPublicRuleStateKind.GameActivationProgress, charge.Capacity) } : new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "eligible-targets", ProgramPublicRuleStateKind.GameActivationProgress, ShiDaHistoricalTargets(owner.Seat).Count) }).ToArray();
    }
}
