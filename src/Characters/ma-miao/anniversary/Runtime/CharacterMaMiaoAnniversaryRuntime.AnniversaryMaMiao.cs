#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaMiaoAnniversaryRuntime
{
    internal static CardConversionSource MaMiaoSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static CardMoveReason MaMiaoDrawReason => new("skill-program.anniversary:zhangguan.adjust-draw");
    internal static CardMoveReason MaMiaoDiscardReason => new("skill-program.anniversary:zhangguan.adjust-discard");

    internal static CardMoveReason MaMiaoGainReason(ProgramSkillFrame f) => new(f.SkillId == AnniversaryMaMiaoComposition.Congfeng ? "skill-program.anniversary:congfeng.extra" : "skill-program.anniversary:zhangguan.obtain");
    internal static ProgramTriggerCandidate MaMiaoCandidate(ProgramSkillFrame f) => new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext?.OccurrenceIndex ?? 0);
    internal AnniversaryMaMiaoEntity[] MaMiaoObtainPool(int recipient, int holder) => !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || !_coreMatchParticipantsPlayersAccess._players[holder].IsAlive ? [] : _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[holder])).Select(c => new AnniversaryMaMiaoEntity(c.Id, _cardZonesOperations._cardZones.GetLocation(c.Id))).Where(e => e.From != CardLocation.Hand(recipient) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(recipient, _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId), e.From, OwnedCardMoveIntent.Obtain)).OrderBy(e => e.From.Zone).ThenBy(e => e.CardId).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 48800, SkillProgramEffectOp.ResolveAnniversaryMaMiaoZhangguan, SkillProgramEffectOp.ResolveAnniversaryMaMiaoCongfeng)]
    internal bool CanOfferMaMiao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (t.Effects is [{ Op: SkillProgramEffectOp.ResolveAnniversaryMaMiaoCongfeng }])
            return MaMiaoGainQualification(c, w)is { } e && MaMiaoObtainPool(e.Recipient, e.Holder).Length > 0;
        return w.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && w.OwnerSeat == c.OwnerSeat && w.SourceSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HasHpValue;
    }

    SkillProgramStepOutcome IAnniversaryMaMiaoHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMaMiao(f, op);
    internal SkillProgramStepOutcome BeginMaMiao(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryMaMiao is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op || !CanOfferMaMiao(MaMiaoCandidate(f), _coreProgramLifecycleOperations.GetProgramTrigger(f), w))
            throw new InvalidOperationException("Ma Miao lost his real original turn-start or paired native gain candidate.");
        var gain = op == SkillProgramEffectOp.ResolveAnniversaryMaMiaoCongfeng ? MaMiaoGainQualification(MaMiaoCandidate(f), w)! : null;
        var r = new AnniversaryMaMiaoReceipt(MaMiaoSource(f), f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, gain is null ? "zhangguan" : "congfeng", gain?.Holder ?? _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, gain?.Recipient ?? f.OwnerSeat)
        {
            InitialHand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count,
            AdjustmentHp = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp),
            Stage = gain is null ? "adjust" : "extra-choice"
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaMiaoStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.Mode, r.Holder, r.Recipient, r.InitialHand, r.AdjustmentHp));
        ContinueMaMiao(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int MaMiaoChooser(ProgramSkillFrame f) => f.AnniversaryMaMiao!.Mode == "congfeng" ? f.AnniversaryMaMiao.Recipient : f.OwnerSeat;
    internal IReadOnlyList<PromptChoice> MaMiaoChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryMaMiao!;
        PromptChoice C(string key, string text) => new(new($"anniversary-ma-miao.{(f.Id)}.{(r.Stage)}.{(key)}"), text, [], [], new Dictionary<string, string> { ["program-action"] = "anniversary-ma-miao", ["branch"] = key });
        if (r.Stage == "extra-choice")
            return[C("accept", "额外随机获得一张牌"), C("decline", "不额外获得")];
        var discard = r.Stage == "discard";
        if (!discard && r.Stage != "obtain-choice")
            return[];
        var holder = discard ? f.OwnerSeat : r.Holder;
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, holder, discard ? [CardZoneKind.Hand] : [CardZoneKind.Hand, CardZoneKind.Equipment], discard ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Obtain, canSelect: (zone, card) => (!discard || !r.Cards.Contains(card.Id)) && (discard || holder != r.Recipient || zone != CardZoneKind.Hand)).Select(c => c with { Id = new($"anniversary-ma-miao.{(f.Id)}.{(r.Stage)}.{(r.Cards.Count)}.{(c.Id.Value)}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "anniversary-ma-miao", ["branch"] = "card" } }).ToArray());
    }

    internal void PublishMaMiao(ProgramSkillFrame f)
    {
        var choices = MaMiaoChoices(f);
        var chooser = MaMiaoChooser(f);
        if (!_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive || choices.Count == 0)
        {
            FinishMaMiao(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, f.AnniversaryMaMiao!.Stage switch
        {
            "discard" => "仗关：弃置超出体力值的手牌。",
            "obtain-choice" => "仗关：获得当前回合角色一张牌。",
            _ => "从风：是否额外随机获得原持有者一张牌？"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-ma-miao")]
    internal void AnswerMaMiao(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryMaMiao: { } r } f || !ValidMaMiao(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != MaMiaoChooser(f) || !MaMiaoChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Ma Miao rejects another recipient's choice or a changed original card slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == "extra-choice")
        {
            var accepted = choice.Parameters["branch"] == "accept";
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaMiaoDecisionEvent(f.Id, r.Recipient, accepted));
            if (!accepted)
            {
                FinishMaMiao(f);
                return;
            }

            var pool = MaMiaoObtainPool(r.Recipient, r.Holder);
            if (pool.Length == 0)
            {
                FinishMaMiao(f);
                return;
            }

            var before = _randomOperations._random.State;
            var index = _randomOperations._random.Next(pool.Length);
            var card = pool[index];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = r with { RandomPool = pool, RandomIndex = index, RandomBefore = before, RandomAfter = _randomOperations._random.State } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaMiaoRandomEvent(f.Id, r.Recipient, r.Holder, card.CardId, card.From, before, _randomOperations._random.State, index, pool.Length));
            PayMaMiaoGain(f, card);
            return;
        }

        var holder = r.Stage == "discard" ? f.OwnerSeat : r.Holder;
        var from = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), holder);
        var cardId = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)].Id;
        if (r.Stage == "obtain-choice")
        {
            PayMaMiaoGain(f, new(cardId, from));
            return;
        }

        var ids = r.Cards.Append(cardId).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = r with { Cards = ids } });
        if (ids.Length < r.Count)
        {
            PublishMaMiao(f);
            return;
        }

        if (ids.Length != r.Count || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat) || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Hand(f.OwnerSeat), OwnedCardMoveIntent.Discard)))
            throw new InvalidOperationException("Zhangguan must pay the original complete currently legal excess Hand selection.");
        var sequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = f.AnniversaryMaMiao!with { Stage = "adjust-children", Before = sequence, After = sequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, ids, MaMiaoDiscardReason, (_, _) => RecordMaMiaoMovement(f.Id, "discard", ids.Length, sequence));
        ContinueMaMiao(f.Id);
    }

    internal void PayMaMiaoGain(ProgramSkillFrame f, AnniversaryMaMiaoEntity card)
    {
        var r = f.AnniversaryMaMiao!;
        if (!MaMiaoObtainPool(r.Recipient, r.Holder).Contains(card))
        {
            FinishMaMiao(f);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var reason = MaMiaoGainReason(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = r with { Stage = "gain-children", CardId = card.CardId, From = card.From, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.CardId], CardLocation.Hand(r.Recipient), reason, afterRequestSettled: settlement =>
        {
            _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(settlement, f.Id, [card.CardId], CardLocation.Hand(r.Recipient), before);
            var live = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(live with { AnniversaryMaMiao = live.AnniversaryMaMiao!with { Gain = settlement } });
            RecordMaMiaoMovement(f.Id, "gain", 1, before);
        }, producerFrameId: f.Id);
        ContinueMaMiao(f.Id);
    }

    internal void RecordMaMiaoMovement(long id, string mode, int count, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryMaMiao = f.AnniversaryMaMiao!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaMiaoMovementEvent(id, mode, count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal bool DrainMaMiao(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueMaMiao(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryMaMiao: { } r } f && f.Id == id)
        {
            if (!ValidMaMiao(f))
                throw new InvalidOperationException("Ma Miao lost his original native adjustment, recovery or gain invoice.");
            if (r.Stage == "drawing")
                return;
            if (r.Stage is "adjust-children" or "gain-children" or "recovery-children")
            {
                if (DrainMaMiao(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.AnniversaryMaMiao!;
                if (r.Stage == "gain-children")
                {
                    FinishMaMiao(f);
                    return;
                }

                if (r.Stage == "recovery-children")
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, AnniversaryMaMiao = r with { Stage = "obtain-choice" } });
                    continue;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, AnniversaryMaMiao = r with { Stage = "after-adjust" } });
                continue;
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.Recipient].IsAlive)
            {
                FinishMaMiao(f);
                return;
            }

            if (r.Stage == "adjust")
            {
                var difference = r.AdjustmentHp - r.InitialHand;
                if (difference > 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = r with { Stage = "drawing", Count = difference } });
                    _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, difference, MaMiaoDrawReason);
                    return;
                }

                var count = Math.Min(-difference, _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard).Count);
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryMaMiao = r with { Count = count, Stage = count > 0 ? "discard" : "after-adjust" } });
                continue;
            }

            if (r.Stage == "after-adjust")
            {
                var after = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaMiaoAdjustedEvent(id, r.InitialHand, after, r.AdjustmentHp));
                if (after > r.InitialHand)
                {
                    FinishMaMiao(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = r with { Stage = "recovery-children", HandAfter = after } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaMiaoRecoveryIssuedEvent(id, f.OwnerSeat, 1));
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
                continue;
            }

            PublishMaMiao(f);
            return;
        }
    }

    internal void FinishMaMiao(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaMiaoCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryMaMiaoZhangguan)]
    internal void ReturnMaMiaoDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.AnniversaryMaMiao is not { Stage: "drawing", Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.Count || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Zhangguan lost its original native adjustment draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryMaMiao = r with { Stage = "adjust-children", Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMaMiaoMovementEvent(f.Id, "draw", r.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueMaMiao(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 48800)]
    internal bool ResumeMaMiao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryMaMiao: not null } f || f.Id != id)
            return false;
        ContinueMaMiao(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 48800)]
    internal bool ReturnMaMiaoMovement(ProgramSkillFrame f)
    {
        if (f.AnniversaryMaMiao is null)
            return false;
        ContinueMaMiao(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryMaMiaoZhangguan, SkillProgramEffectOp.ResolveAnniversaryMaMiaoCongfeng)]
    internal PromptChoice ChooseMaMiao(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "accept") ?? p.Choices.First();
}
