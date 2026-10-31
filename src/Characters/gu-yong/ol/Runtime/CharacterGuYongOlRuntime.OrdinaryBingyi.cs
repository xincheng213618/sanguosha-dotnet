#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuYongOlRuntime
{
    internal SkillProgramStepOutcome BeginOrdinaryBingyi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.OrdinaryBingyi is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.TriggerId != "ordinary-discard-reveal-share" || f.WindowContext is not { } context || !CanOfferOrdinaryBingyi(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Ordinary Bingyi requires its original actual-phase owned discard and nonempty current hand.");
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => new OrdinaryBingyiShownCard(c.Id, c.Kind, _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c))).ToArray();
        var colors = cards.Select(c => c.EffectiveSuit is Suit.Heart or Suit.Diamond ? (bool? )true : c.EffectiveSuit is Suit.Spade or Suit.Club ? false : null).Distinct().ToArray();
        var r = new OrdinaryBingyiReceipt(OrdinaryGuYongSource(f), f.GameplayHash, context.MovementBatch!.DiscardRecoveryPhase!, context.ParentFrameId, context.MovementBatch.Id, context.OccurrenceIndex, colors.Length == 1, colors.Length == 1 ? colors[0] : null, OrdinaryBingyiStage.SelectingRecipients)
        {
            Paid = true,
            ShownCards = cards
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrdinaryBingyi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryBingyiShownEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.WindowId, r.BatchId, r.OccurrenceIndex, cards.Length, r.UniformColor, r.Color));
        for (var i = 0; i < cards.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryBingyiShownEntityEvent(f.Id, i, cards[i].CardId, cards[i].PrintedKind, cards[i].EffectiveSuit));
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, OrdinaryBingyiShownBind, cards.Select(c => c.CardId).ToArray(), SkillProgramCardSetVisibility.Private, cards.Select(_ => CardLocation.Hand(f.OwnerSeat)).ToArray());
        _coreProgramCategoryChallengesOperations.RevealProgramBoundCards(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), OrdinaryBingyiShownBind);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【秉壹】，展示全部 {(cards.Length)} 张手牌。", f.OwnerSeat);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (!r.UniformColor)
        {
            FinishOrdinaryBingyi(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishOrdinaryBingyi(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> OrdinaryBingyiChoices(ProgramSkillFrame f)
    {
        if (f.OrdinaryBingyi is not { Paid: true, UniformColor: true, Stage: OrdinaryBingyiStage.SelectingRecipients, RecipientsCommitted: false } r || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        PromptChoice Choice(string branch, int? peer, IReadOnlyList<int> after, string label) => new(new($"ordinary-bingyi.{(f.Id)}.{(branch)}.{(peer?.ToString(CultureInfo.InvariantCulture) ?? "done")}"), label, [], after.Prepend(f.OwnerSeat).ToArray(), new Dictionary<string, string> { ["program-action"] = "ordinary-bingyi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["peer-seat"] = peer?.ToString(CultureInfo.InvariantCulture) ?? "" });
        var result = new List<PromptChoice>();
        foreach (var peer in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
        {
            var chosen = r.OtherRecipients.Contains(peer.Seat);
            if (!chosen && r.OtherRecipients.Count >= r.ShownCards.Count)
                continue;
            var next = chosen ? r.OtherRecipients.Where(s => s != peer.Seat).ToArray() : r.OtherRecipients.Append(peer.Seat).ToArray();
            result.Add(Choice("toggle", peer.Seat, next, (chosen ? "取消选择 " : "选择 ") + peer.Name));
        }

        result.Add(Choice("confirm", null, r.OtherRecipients, $"确定：自己与 {(r.OtherRecipients.Count)} 名其他角色各摸一张牌"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishOrdinaryBingyi(ProgramSkillFrame f)
    {
        var choices = OrdinaryBingyiChoices(f);
        if (choices.Count == 0)
        {
            FinishOrdinaryBingyi(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"秉壹：选择至多 {(f.OrdinaryBingyi!.ShownCards.Count)} 名其他角色，自己与其各摸一张牌。", [], _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => p.Seat).ToArray(), SourceSeat: f.OwnerSeat)
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

    [ProgramAnswer("ordinary-bingyi")]
    internal void AnswerOrdinaryBingyi(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OrdinaryBingyi: { Stage: OrdinaryBingyiStage.SelectingRecipients } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !OrdinaryBingyiChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Ordinary Bingyi requires a current original recipient choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters["branch"] == "toggle")
        {
            var peer = int.Parse(selected.Parameters["peer-seat"], CultureInfo.InvariantCulture);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryBingyi = r with { OtherRecipients = r.OtherRecipients.Contains(peer) ? r.OtherRecipients.Where(s => s != peer).ToArray() : r.OtherRecipients.Append(peer).ToArray() } });
            PublishOrdinaryBingyi(f);
            return;
        }

        var recipients = new[]
        {
            f.OwnerSeat
        }.Concat(r.OtherRecipients.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(r.Phase.ActualTurnOwnerSeat, s))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, OrdinaryBingyi = r with { RecipientsCommitted = true, Recipients = recipients, Stage = OrdinaryBingyiStage.Drawing } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryBingyiRecipientsCommittedEvent(f.Id, recipients.Length));
        for (var i = 0; i < recipients.Length; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryBingyiRecipientEvent(f.Id, i, recipients[i]));
        ContinueOrdinaryBingyi(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveOrdinaryGuYongBingyi)]
    internal void ReturnOrdinaryBingyiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.OrdinaryBingyi is not { Paid: true, RecipientsCommitted: true, Stage: OrdinaryBingyiStage.Drawing } r || r.Cursor < 0 || r.Cursor >= r.Recipients.Count || outcome.RecipientSeat != r.Recipients[r.Cursor] || outcome.RequestedCount != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryBingyiDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.DrawIndex == r.Cursor))
            throw new InvalidOperationException("Ordinary Bingyi lost or repeated its exact native recipient draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrdinaryBingyi = r with { Stage = OrdinaryBingyiStage.DrawChildren, DrawOutcome = outcome }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryBingyiDrawReturnedEvent(f.Id, r.Cursor, outcome.RecipientSeat, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueOrdinaryBingyi(f.Id);
    }

    internal bool DrainOrdinaryBingyi(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueOrdinaryBingyi(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { OrdinaryBingyi: { } r } f && f.Id == id)
        {
            if (r.Stage == OrdinaryBingyiStage.SelectingRecipients)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishOrdinaryBingyi(f);
                    return;
                }

                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishOrdinaryBingyi(f);
                return;
            }

            if (r.Stage == OrdinaryBingyiStage.DrawChildren)
            {
                if (DrainOrdinaryBingyi(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.OrdinaryBingyi!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, OrdinaryBingyi = r with { Cursor = r.Cursor + 1, Stage = OrdinaryBingyiStage.Drawing, DrawOutcome = null } });
                continue;
            }

            if (r.Stage != OrdinaryBingyiStage.Drawing)
                throw new InvalidOperationException("Ordinary Bingyi has no extra resolution stage.");
            if (f.NativeDraw is not null)
                return;
            if (r.Cursor >= r.Recipients.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishOrdinaryBingyi(f);
                return;
            }

            var seat = r.Recipients[r.Cursor];
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryBingyiRecipientSkippedEvent(f.Id, r.Cursor, seat));
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { OrdinaryBingyi = r with { Cursor = r.Cursor + 1 } });
                continue;
            }

            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, seat, 1, new(OrdinaryBingyiDrawReason));
            return;
        }
    }

    internal void FinishOrdinaryBingyi(ProgramSkillFrame f)
    {
        var r = f.OrdinaryBingyi ?? throw new InvalidOperationException("Ordinary Bingyi completion lost its owning reveal.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OrdinaryBingyiCompletedEvent(f.Id, r.UniformColor, r.Recipients.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OrdinaryBingyi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27598)]
    internal bool ResumeOrdinaryBingyi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OrdinaryBingyi: not null } f || f.Id != id)
            return false;
        ContinueOrdinaryBingyi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27598)]
    internal bool ReturnOrdinaryBingyiMovement(ProgramSkillFrame f)
    {
        if (f.OrdinaryBingyi is null)
            return false;
        ContinueOrdinaryBingyi(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveOrdinaryGuYongBingyi)]
    internal PromptChoice ChooseOrdinaryBingyiAi(PendingDecision decision, ProgramSkillFrame f) => _coreProgramLifecycleOperations.SelectAiProgramTargets(decision, SkillProgramTargetAiOrder.SupportDraw);
}
