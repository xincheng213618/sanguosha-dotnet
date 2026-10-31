#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoYouZhiMouMobileRuntime
{
    internal const string MouGyzGiftReason = "skill-program.mou-mobile:bique.give", MouGyzDrawReason = "skill-program.mou-mobile:bique.draw", MouGyzDiscardReason = "skill-program.mou-mobile:bique.discard", MouGyzObtainReason = "skill-program.mou-mobile:zhongchun.obtain";
    SkillProgramStepOutcome global::CardGame.Core.IMouGuoYouZhiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMouGyz(f, op);
    internal CardConversionSource MouGyzSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramStepOutcome BeginMouGyz(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouGuoYouZhi is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Guo You Zhi cannot repeat an accepted instruction.");
        var bique = op == SkillProgramEffectOp.ResolveMouGuoYouZhiBique;
        var actor = bique ? f.SelectedTargetSeats.Single() : f.WindowContext?.SourceSeat ?? _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || bique && (f.TriggerId is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || actor == f.OwnerSeat) || !bique && !ExactMouGyzEndingParent(f))
            throw new InvalidOperationException("Guo You Zhi lost its actual Play target or Ending actor.");
        var r = new MouGuoYouZhiReceipt(MouGyzSource(f), f.GameplayHash, op, actor, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
        {
            Stage = bique ? MouGuoYouZhiStage.GiftChoice : MouGuoYouZhiStage.DiscardChoice,
            WatchedHand = bique ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Select(c => c.Id).ToArray() : [],
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiStartedEvent(f.Id, r.Source, r.GameplayHash, op, actor, r.ActualTurn, r.ActualPhase));
        if (bique)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiViewedEvent(f.Id, f.OwnerSeat, actor, r.WatchedHand.Count));
        PublishMouGyz(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice MouGyzChoice(ProgramSkillFrame f, string branch, string text, int? card = null, int? option = null) => new(new($"mou-guo-you-zhi.{(f.Id)}.{(branch)}.{(card ?? option ?? -1)}"), text, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "mou-guo-you-zhi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["option"] = (option ?? -1).ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<Card> MouGyzEligibleGifts(ProgramSkillFrame f)
    {
        var r = f.MouGuoYouZhi!;
        var names = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat]).Select(c => _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], c)).ToHashSet();
        return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => !names.Contains(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], c))).ToArray();
    }

    internal IReadOnlyList<PromptChoice> MouGyzChoices(ProgramSkillFrame f)
    {
        var r = f.MouGuoYouZhi!;
        var list = new List<PromptChoice>();
        if (r.Stage == MouGuoYouZhiStage.GiftChoice)
        {
            list.AddRange(MouGyzEligibleGifts(f).Select(c => MouGyzChoice(f, "gift-card", (r.Selected.Contains(c.Id) ? "取消：" : "交给：") + c.DisplayName, c.Id)));
            list.Add(MouGyzChoice(f, "gift-confirm", $"确认交给{(r.Selected.Count)}张牌"));
        }
        else if (r.Stage == MouGuoYouZhiStage.AdjustChoice)
        {
            list.Add(MouGyzChoice(f, "adjust", $"将手牌数调整至{(r.DistinctNames)}"));
            list.Add(MouGyzChoice(f, "decline", "不调整手牌"));
        }
        else if (r.Stage == MouGuoYouZhiStage.AdjustDiscard)
        {
            list.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => MouGyzChoice(f, "discard-card", (r.Selected.Contains(c.Id) ? "取消：" : "弃置：") + c.DisplayName, c.Id)));
            if (r.Selected.Count == _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count - r.DistinctNames)
                list.Add(MouGyzChoice(f, "discard-confirm", $"确认弃置{(r.Selected.Count)}张手牌"));
        }
        else if (r.Stage == MouGuoYouZhiStage.DiscardChoice)
        {
            list.AddRange(MouGyzDiscardCards(r.ActorSeat, r.ActualTurn).Select(id => MouGyzChoice(f, "obtain", $"令{(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].Name)}获得【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", id)));
            list.Add(MouGyzChoice(f, "decline", "不发动忠纯"));
        }
        else if (r.Stage == MouGuoYouZhiStage.UseChoice)
        {
            list.AddRange(MouGyzNativeActions(f).Select((a, i) => new PromptChoice(new($"mou-guo-you-zhi.{(f.Id)}.use.{(i)}"), a.Description, [r.CardId], a.TargetSeats, new Dictionary<string, string> { ["program-action"] = "mou-guo-you-zhi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "use", ["option"] = i.ToString(CultureInfo.InvariantCulture) })));
            list.Add(MouGyzChoice(f, "decline", "不使用此牌"));
        }

        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishMouGyz(ProgramSkillFrame f)
    {
        var r = f.MouGuoYouZhi!;
        var chooser = r.Stage == MouGuoYouZhiStage.UseChoice ? r.ActorSeat : f.OwnerSeat;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive || r.Stage == MouGuoYouZhiStage.GiftChoice && !_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive)
        {
            FinishMouGyz(f);
            return;
        }

        var choices = MouGyzChoices(f);
        if (choices.Count == 0 || r.Stage == MouGuoYouZhiStage.UseChoice && choices.Count == 1)
        {
            FinishMouGyz(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MouGuoYouZhiStage.GiftChoice ? "裨阙：已观看对方手牌，选择交出的手牌（可为零张）。" : r.Stage == MouGuoYouZhiStage.UseChoice ? "忠纯：可以使用刚获得的原牌，或不使用。" : skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            TargetSeat = r.ActorSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-guo-you-zhi")]
    internal void AnswerMouGyz(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGuoYouZhi: { } r } f || !ValidMouGyz(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouGuoYouZhiStage.UseChoice ? r.ActorSeat : f.OwnerSeat) || !MouGyzChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Guo You Zhi requires its actual chooser and unchanged published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch is "gift-card" or "discard-card")
        {
            var id = choice.Cards.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r with { Selected = r.Selected.Contains(id) ? r.Selected.Where(c => c != id).ToArray() : r.Selected.Append(id).ToArray() } });
            PublishMouGyz(f);
            return;
        }

        if (branch == "decline")
        {
            if (r.Stage == MouGuoYouZhiStage.AdjustChoice)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiAdjustChosenEvent(f.Id, r.DistinctNames, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count, false));
            FinishMouGyz(f);
            return;
        }

        if (branch == "gift-confirm")
        {
            PayMouGyzGift(f);
            return;
        }

        if (branch == "adjust")
        {
            var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiAdjustChosenEvent(f.Id, r.DistinctNames, count, true));
            if (count < r.DistinctNames)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.Drawing, Requested = r.DistinctNames - count } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiDrawIssuedEvent(f.Id, f.OwnerSeat, r.DistinctNames - count));
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.DistinctNames - count, new(MouGyzDrawReason));
                return;
            }

            if (count == r.DistinctNames)
            {
                FinishMouGyz(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.AdjustDiscard, Selected = [] } });
            PublishMouGyz(f);
            return;
        }

        if (branch == "discard-confirm")
        {
            if (r.Selected.Count != _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count - r.DistinctNames || r.Selected.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat)))
                throw new InvalidOperationException("Bique must really discard the excess Hand.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.DiscardChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(MouGyzDiscardReason));
            SealMouGyz(f.Id);
            ContinueMouGyz(f.Id);
            return;
        }

        if (branch == "obtain")
        {
            PayMouGyzObtain(f, choice.Cards.Single());
            return;
        }

        if (branch == "use")
        {
            IssueMouGyzNativeUse(f, int.Parse(choice.Parameters["option"], CultureInfo.InvariantCulture));
            return;
        }

        throw new InvalidOperationException("Unknown Guo You Zhi branch.");
    }

    internal void PayMouGyzGift(ProgramSkillFrame f)
    {
        var r = f.MouGuoYouZhi!;
        var allowed = MouGyzEligibleGifts(f).Select(c => c.Id).ToHashSet();
        if (r.Selected.Any(id => !allowed.Contains(id)) || r.Selected.Distinct().Count() != r.Selected.Count)
            throw new InvalidOperationException("Bique cannot give a newly owned target name or another location.");
        var gifts = r.Selected.Select(id => new MouGuoYouZhiGift(id, _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.GiftChildren, Gifts = gifts, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(r.ActorSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiGiftAcceptedEvent(f.Id, r.ActorSeat, gifts.Length));
        if (gifts.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(gifts.Select(g => g.CardId).ToArray(), CardLocation.Hand(r.ActorSeat), new(MouGyzGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, r.ActorSeat, gifts.Select(g => g.CardId).ToArray(), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: CardMovementDisclosure.FaceDown);
        SealMouGyz(f.Id);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.MouGuoYouZhi!;
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After && m.To == CardLocation.Hand(r.ActorSeat) && m.Reason.Value == MouGyzGiftReason && gifts.Any(g => g.CardId == m.CardId)).Select(m => m.CardId).ToHashSet();
        var names = gifts.Where(g => actual.Contains(g.CardId)).Select(g => g.EffectiveKind).Distinct().Count();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouGuoYouZhi = r with { DistinctNames = names, WatchedHand = [], Selected = [] } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiGiftReturnedEvent(f.Id, actual.Count, names));
        ContinueMouGyz(f.Id);
    }

    internal void ContinueMouGyz(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouGuoYouZhi!;
        if (r.Stage is MouGuoYouZhiStage.Drawing or MouGuoYouZhiStage.IssuingUse or MouGuoYouZhiStage.NativeUse)
            return;
        if (r.Stage is MouGuoYouZhiStage.GiftChildren or MouGuoYouZhiStage.DrawChildren or MouGuoYouZhiStage.DiscardChildren or MouGuoYouZhiStage.ObtainChildren)
        {
            if (DrainMouGyz(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.MouGuoYouZhi!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishMouGyz(f);
                return;
            }

            if (r.Stage == MouGuoYouZhiStage.GiftChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count == r.DistinctNames)
                {
                    FinishMouGyz(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.AdjustChoice } });
                PublishMouGyz(f);
                return;
            }

            if (r.Stage == MouGuoYouZhiStage.ObtainChildren)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive || _cardZonesOperations._cardZones.GetLocation(r.CardId) != CardLocation.Hand(r.ActorSeat))
                {
                    FinishMouGyz(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.UseChoice } });
                PublishMouGyz(f);
                return;
            }

            FinishMouGyz(f);
            return;
        }

        if (r.Stage == MouGuoYouZhiStage.Returned)
        {
            FinishMouGyz(f);
            return;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishMouGyz(f);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouGuoYouZhiBique)]
    internal void ReturnMouGyzDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MouGuoYouZhi is not { Stage: MouGuoYouZhiStage.Drawing, Draw: null } r || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.Requested)
            throw new InvalidOperationException("Bique draw must return its real Hand normalization.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = r with { Stage = MouGuoYouZhiStage.DrawChildren, Draw = o, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiDrawReturnedEvent(f.Id, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.AttemptFrameId));
        SealMouGyz(f.Id);
        ContinueMouGyz(f.Id);
    }

    internal void FinishMouGyz(ProgramSkillFrame f)
    {
        var r = f.MouGuoYouZhi!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuoYouZhiCompletedEvent(f.Id, r.Op, r.DistinctNames, r.Native?.UseId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouGuoYouZhi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 32400)]
    internal CardSnapshot[] MouGyzPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.MouGuoYouZhi is { Stage: MouGuoYouZhiStage.GiftChoice }).SelectMany(f => f.MouGuoYouZhi!.WatchedHand.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.MouGuoYouZhi.ActorSeat)).Select(id => _closedHandCardViewQueriesSource.Get().Project(_coreMatchParticipantsPlayersAccess._players[f.MouGuoYouZhi.ActorSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)))).ToArray();
}
