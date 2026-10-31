#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanMouMobileRuntime
{
    internal const string MouDiaoChanLijian = "mou-mobile:lijian", MouDiaoChanBiyue = "mou-mobile:biyue";
    internal const string MouDiaoChanCostReason = "skill-program.mou-mobile:lijian.discard";
    internal static CardConversionSource MouDiaoChanSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_diao_chan_mou_mobile_MouDiaoChanMobile.MouDiaoChanSource(f);
    SkillProgramStepOutcome IMouDiaoChanMobileHost.Start(ProgramSkillFrame f, SkillProgramEffectOp op) => StartMouDiaoChanMobile(f, op);
    internal Card[] MouDiaoChanCosts(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal int MouDiaoChanPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouDiaoChanMobileStartedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Op == SkillProgramEffectOp.ResolveMouDiaoChanLijian && e.Phase == phase);
    [ProgramPhaseUsedProjection(31700)]
    internal int ProjectMouDiaoChanPhaseUsed(int seat, string skill, int fallback) => skill == MouDiaoChanLijian && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p ? MouDiaoChanPhaseUses(seat, p) : fallback;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 31700, SkillProgramEffectOp.ResolveMouDiaoChanLijian)]
    internal bool CanStartMouDiaoChanLijian(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && MouDiaoChanPhaseUses(owner.Seat, p) == 0 && MouDiaoChanCosts(owner.Seat).Length > 0 && _coreMatchParticipantsPlayersAccess._players.Count(x => x.IsAlive && x.Seat != owner.Seat) >= 2;
    internal SkillProgramStepOutcome StartMouDiaoChanMobile(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouDiaoChanMobile is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            throw new InvalidOperationException("Mou Diao Chan requires her exact original native phase entry.");
        if (op == SkillProgramEffectOp.ResolveMouDiaoChanLijian ? f.SkillId != MouDiaoChanLijian || f.ActivationId != "ring-duels" || f.TriggerId is not null || !CanStartMouDiaoChanLijian(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : f.SkillId != MouDiaoChanBiyue || f.TriggerId != "ending-damaged-roles" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } || phase.Kind != ActualDiscardRecoveryPhaseKind.Ending)
            throw new InvalidOperationException("Mou Diao Chan cannot borrow another phase or binding.");
        var r = new MouDiaoChanMobileReceipt(MouDiaoChanSource(f), f.GameplayHash, op, phase)
        {
            Stage = op == SkillProgramEffectOp.ResolveMouDiaoChanLijian ? MouDiaoChanMobileStage.Targets : MouDiaoChanMobileStage.Drawing
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDiaoChanMobileStartedEvent(f.Id, r.Source, r.GameplayHash, op, phase));
        if (op == SkillProgramEffectOp.ResolveMouDiaoChanLijian)
        {
            PublishMouDiaoChan(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var prefix = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Reverse().TakeWhile(e => e is not TurnStartedEvent).OfType<DamageAppliedEvent>();
        var damaged = prefix.Where(e => e.Amount > 0).Select(e => e.TargetSeat).Distinct().Count();
        var draw = Math.Min(4, damaged + 1);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r with { Paid = true, DrawCount = draw } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDiaoChanMobileBiyueEvent(f.Id, _coreActualTurnProgressTurnNumberAccess._turnNumber, damaged, draw));
        ContinueMouDiaoChan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice MouDiaoChanChoice(ProgramSkillFrame f, string branch, string text, int? target = null, int? card = null) => new(new($"mou-diao-chan.{(f.Id)}.{(branch)}.{(target ?? card ?? 0)}"), text, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mou-diao-chan-mobile", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> MouDiaoChanChoices(ProgramSkillFrame f)
    {
        var r = f.MouDiaoChanMobile!;
        var choices = new List<PromptChoice>();
        if (r.Stage == MouDiaoChanMobileStage.Targets)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                if (r.Targets.Contains(p.Seat) || r.Targets.Count < MouDiaoChanCosts(f.OwnerSeat).Length + 1)
                    choices.Add(MouDiaoChanChoice(f, "target", (r.Targets.Contains(p.Seat) ? "取消：" : "选择：") + p.Name, target: p.Seat));
            if (r.Targets.Count >= 2)
                choices.Add(MouDiaoChanChoice(f, "targets-done", $"确定{(r.Targets.Count)}名角色"));
        }

        if (r.Stage == MouDiaoChanMobileStage.Cards)
        {
            foreach (var c in MouDiaoChanCosts(f.OwnerSeat))
                if (r.Cards.Contains(c.Id) || r.Cards.Count < r.Targets.Count - 1)
                    choices.Add(MouDiaoChanChoice(f, "card", (r.Cards.Contains(c.Id) ? "取消：" : "弃置：") + c.DisplayName, card: c.Id));
            if (r.Cards.Count == r.Targets.Count - 1)
                choices.Add(MouDiaoChanChoice(f, "pay", $"弃置{(r.Cards.Count)}张牌并结算全部离间"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouDiaoChan(ProgramSkillFrame f)
    {
        var choices = MouDiaoChanChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-diao-chan-mobile")]
    internal void AnswerMouDiaoChan(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDiaoChanMobile: { } r } f || !ValidMouDiaoChan(f) || !MouDiaoChanChoices(f).Any(c => c.Id == choice.Id && CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Mou Diao Chan lost her exact private selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "target")
        {
            var seat = choice.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r with { Targets = r.Targets.Contains(seat) ? r.Targets.Where(x => x != seat).ToArray() : r.Targets.Append(seat).ToArray() } });
            PublishMouDiaoChan(f);
            return;
        }

        if (branch == "targets-done")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r with { Stage = MouDiaoChanMobileStage.Cards } });
            PublishMouDiaoChan(f);
            return;
        }

        if (branch == "card")
        {
            var id = choice.Cards.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r with { Cards = r.Cards.Contains(id) ? r.Cards.Where(x => x != id).ToArray() : r.Cards.Append(id).ToArray() } });
            PublishMouDiaoChan(f);
            return;
        }

        if (branch != "pay" || r.Cards.Count != r.Targets.Count - 1 || !r.Cards.All(id => MouDiaoChanCosts(f.OwnerSeat).Any(c => c.Id == id)) || r.Targets.Any(s => !_coreMatchParticipantsPlayersAccess._players[s].IsAlive))
            throw new InvalidOperationException("Lijian must pay exactly its complete selected roster minus one.");
        var roster = r.Targets.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, s)).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r = r with { Paid = true, Targets = roster, Stage = MouDiaoChanMobileStage.CostChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < roster.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDiaoChanMobileRosterEvent(f.Id, i, roster[i]));
        foreach (var id in r.Cards)
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            var from = _cardZonesOperations._cardZones.GetLocation(id);
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new(MouDiaoChanCostReason), parentFrameId: f.Id);
            var moved = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id && m.From == from && m.Sequence > before);
            if (moved is not null)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDiaoChanMobileCostEvent(f.Id, id, card.Kind, from, moved.To, moved.Sequence));
        }

        SealMouDiaoChanMovement(f.Id, before);
        ContinueMouDiaoChan(f.Id);
    }

    internal bool ValidMouDiaoChan(ProgramSkillFrame f) => f.MouDiaoChanMobile is { } r && r.Source == MouDiaoChanSource(f) && r.GameplayHash == f.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && r.Phase.ActorSeat == f.OwnerSeat && r.Targets.Distinct().Count() == r.Targets.Count && r.Cards.Distinct().Count() == r.Cards.Count && r.Index >= 0 && r.Index <= r.Targets.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDiaoChanMobileStartedEvent>().Count(e => e == new MouDiaoChanMobileStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.Phase)) == 1 && (!r.Paid || r.Op != SkillProgramEffectOp.ResolveMouDiaoChanLijian || r.Targets.Count >= 2 && r.Cards.Count == r.Targets.Count - 1 && r.Targets.Select((seat, index) => new MouDiaoChanMobileRosterEvent(f.Id, index, seat)).All(e => _coreProgramEventHistoryOperations.ProgramEventHistory<MouDiaoChanMobileRosterEvent>().Count(p => p == e) == 1));
    internal void ContinueMouDiaoChan(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDiaoChanMobile: { } r } f || f.Id != id)
            return;
        if (!ValidMouDiaoChan(f))
            throw new InvalidOperationException("Mou Diao Chan lost her original accepted cursor.");
        if (r.Stage is MouDiaoChanMobileStage.Targets or MouDiaoChanMobileStage.Cards)
        {
            PublishMouDiaoChan(f);
            return;
        }

        if (r.Stage == MouDiaoChanMobileStage.Drawing)
        {
            if (f.NativeDraw is null)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.DrawCount, new("skill-program.mou-mobile:biyue.draw"));
            return;
        }

        if (r.Stage == MouDiaoChanMobileStage.Duels)
        {
            if (r.UseId == 0)
                IssueMouDiaoChanDuel(f);
            return;
        }

        if (DrainMouDiaoChanChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouDiaoChanMobile!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == MouDiaoChanMobileStage.DrawChildren || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouDiaoChan(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r with { Stage = MouDiaoChanMobileStage.Duels } });
        IssueMouDiaoChanDuel(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouDiaoChanBiyue)]
    internal void ReturnMouDiaoChanBiyue(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (!ValidMouDiaoChan(f) || f.MouDiaoChanMobile is not { Stage: MouDiaoChanMobileStage.Drawing } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.DrawCount)
            throw new InvalidOperationException("Biyue must return its original native Ending draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = r with { Stage = MouDiaoChanMobileStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDiaoChanMobileDrawReturnedEvent(f.Id, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.AttemptFrameId));
        SealMouDiaoChanMovement(f.Id, outcome.SequenceBefore);
        ContinueMouDiaoChan(f.Id);
    }

    internal void FinishMouDiaoChan(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDiaoChanMobile = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouDiaoChanLijian)]
    internal PromptChoice AiMouDiaoChan(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MouDiaoChanMobile!;
        if (r.Stage == MouDiaoChanMobileStage.Targets)
        {
            if (r.Targets.Count >= 2)
                return p.Choices.Single(c => c.Parameters["branch"] == "targets-done");
            return p.Choices.Where(c => c.Targets.Count == 1 && !r.Targets.Contains(c.Targets[0])).OrderBy(c => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]) ? 1 : 0).ThenBy(c => c.Targets[0]).First();
        }

        if (r.Cards.Count == r.Targets.Count - 1)
            return p.Choices.Single(c => c.Parameters["branch"] == "pay");
        return p.Choices.Where(c => c.Cards.Count == 1 && !r.Cards.Contains(c.Cards[0])).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Cards[0]).First();
    }
}
