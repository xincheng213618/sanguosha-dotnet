#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanDangMouMobileRuntime
{
    internal const string MouHanDangGongqi = "mou-mobile:gongqi", MouHanDangJiefan = "mou-mobile:jiefan";
    internal const string MouHanDangDiscardReason = RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.MouHanDangDiscardReason, MouHanDangDrawReason = "skill-program.mou-mobile:jiefan.Draw";
    internal bool HasMouHanDangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHanDangGongqi) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouHanDangJiefan);

    internal static CardConversionSource MouHanDangSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_han_dang_mou_mobile_MouHanDang.MouHanDangSource(f);
    internal Card[] MouHanDangDiscardCards(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).OrderBy(c => c.Id).ToArray();
    internal SkillProgramStepOutcome StartMouHanDang(ProgramSkillFrame supplied, bool gongqi)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouHanDang is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || (gongqi ? f.SkillId != MouHanDangGongqi || !MouHanDangPlayStartParent(f) || f.SelectedTargetSeats.Count != 0 || MouHanDangDiscardCards(f.OwnerSeat).Length == 0 : f.SkillId != MouHanDangJiefan || f.ActivationId != "aid-by-range" || f.TriggerId is not null || f.WindowContext is not null || f.SelectedTargetSeats is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !CanStartMouHanDangJiefan(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])))
            throw new InvalidOperationException("Mou Han Dang requires its exact own native Play entry and live source.");
        var r = new MouHanDangReceipt(MouHanDangSource(f), f.GameplayHash, phase, gongqi)
        {
            Stage = gongqi ? MouHanDangStage.CostChoice : MouHanDangStage.ModeChoice,
            TargetSeat = gongqi ? f.OwnerSeat : f.SelectedTargetSeats.Single(),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangStartedEvent(f.Id, r.Source, r.GameplayHash, phase, gongqi, r.TargetSeat));
        PublishMouHanDang(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MouHanDangChoice(ProgramSkillFrame f, string branch, string label, int? card = null) => new(new($"mou-han-dang.{(f.Id)}.{(f.MouHanDang!.Index)}.{(branch)}.{(card ?? 0)}"), label, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "mou-han-dang", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> MouHanDangChoices(ProgramSkillFrame f)
    {
        var r = f.MouHanDang!;
        if (r.Stage == MouHanDangStage.ModeChoice)
            return Array.AsReadOnly(new[] { MouHanDangChoice(f, "discard", "令攻击范围包含你的其他角色依次弃置一张牌"), MouHanDangChoice(f, "draw", "摸此时攻击范围包含你的其他角色数的牌"), MouHanDangChoice(f, "backwater", "背水：令解烦失效直至发动者击杀角色，然后执行两项") });
        var chooser = r.Gongqi ? f.OwnerSeat : r.Participants[r.Index];
        return Array.AsReadOnly(MouHanDangDiscardCards(chooser).Select(c => MouHanDangChoice(f, "discard-card", $"弃置【{(c.DisplayName)}】", c.Id)).ToArray());
    }

    internal void PublishMouHanDang(ProgramSkillFrame f)
    {
        var r = f.MouHanDang!;
        var chooser = r.Stage == MouHanDangStage.ModeChoice ? r.TargetSeat : r.Gongqi ? f.OwnerSeat : r.Participants[r.Index];
        var choices = MouHanDangChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("Mou Han Dang lost its currently payable choice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MouHanDangStage.ModeChoice ? "解烦：选择一项，或背水执行两项。" : "弃置一张自己的手牌或装备牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-han-dang")]
    internal void AnswerMouHanDang(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouHanDang;
        if (r is null || !ValidMouHanDang(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouHanDangChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Mou Han Dang accepts only its current published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MouHanDangStage.ModeChoice)
        {
            var branch = selected.Parameters["branch"];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = r = r with { Branch = branch, Stage = MouHanDangStage.DiscardChildren } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangBranchEvent(f.Id, r.TargetSeat, branch));
            if (branch == "backwater")
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangJiefanDisabledEvent(f.Id, f.OwnerSeat));
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的【解烦】因背水失效，直至其击杀一名角色。", f.OwnerSeat);
            }

            if (branch != "draw")
            {
                var seats = MouHanDangRangeSeats(r.TargetSeat, f.OwnerSeat);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = r = r with { Participants = seats } });
                for (var i = 0; i < seats.Count; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangParticipantEvent(f.Id, i, seats[i]));
            }

            ContinueMouHanDang(f.Id);
            return;
        }

        var seat = r.Gongqi ? f.OwnerSeat : r.Participants[r.Index];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single());
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = r with { Stage = MouHanDangStage.Paying, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, seat, card, from, new(MouHanDangDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.MouHanDang!;
        var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == card.Id && m.From == from && m.Reason.Value == MouHanDangDiscardReason);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = r = r with { Stage = MouHanDangStage.DiscardChildren, PaidCardId = card.Id, PaidSuit = suit, PaymentSequence = m.Sequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangDiscardedEvent(f.Id, r.Index, seat, card.Id, from, m.To, m.Sequence, suit));
        if (r.Gongqi)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangGongqiGrantedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, card.Id, suit, m.Sequence));
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【弓骑】：本阶段其用牌只能以虚拟牌或{((CoreNativeCardClassificationsRuntime.IsRedSuit(suit) ? "红色" : "黑色"))}手牌响应。", f.OwnerSeat);
        }

        SealMouHanDangMovements(f.Id, before);
        ContinueMouHanDang(f.Id);
    }

    internal void ContinueMouHanDang(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouHanDang: { } r } f || f.Id != id)
            return;
        if (!ValidMouHanDang(f))
            throw new InvalidOperationException("Mou Han Dang lost its accepted source, phase or paid native child.");
        if (r.Stage is MouHanDangStage.CostChoice or MouHanDangStage.Paying or MouHanDangStage.ModeChoice or MouHanDangStage.ParticipantChoice or MouHanDangStage.Drawing)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouHanDang!;
        if (r.Gongqi || r.Stage == MouHanDangStage.DrawChildren || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouHanDang(f);
            return;
        }

        var index = r.Index + (r.PaidCardId is null ? 0 : 1);
        while (index < r.Participants.Count && (!_coreMatchParticipantsPlayersAccess._players[r.Participants[index]].IsAlive || MouHanDangDiscardCards(r.Participants[index]).Length == 0))
            index++;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, MouHanDang = r = r with { Index = index, PaidCardId = null, PaymentSequence = 0, PaidSuit = Suit.None } });
        if (index < r.Participants.Count)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = r with { Stage = MouHanDangStage.ParticipantChoice } });
            PublishMouHanDang(f);
            return;
        }

        if (r.Branch == "discard" || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
        {
            FinishMouHanDang(f);
            return;
        }

        var count = MouHanDangRangeSeats(r.TargetSeat, f.OwnerSeat).Count;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = r with { Stage = MouHanDangStage.Drawing, RequestedDraw = count } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.TargetSeat, count, new(MouHanDangDrawReason));
    }

    internal void FinishMouHanDang(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHanDangCompletedEvent(f.Id, f.MouHanDang!.Gongqi));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHanDang = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouHanDangGongqi, SkillProgramEffectOp.ResolveMouHanDangJiefan)]
    internal PromptChoice AiMouHanDang(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "draw") ?? p.Choices.First();
    SkillProgramStepOutcome IMouHanDangHost.Gongqi(ProgramSkillFrame f) => StartMouHanDang(f, true);
    SkillProgramStepOutcome IMouHanDangHost.Jiefan(ProgramSkillFrame f) => StartMouHanDang(f, false);
}
