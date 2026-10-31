#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoYuanMobileShiRuntime
{
    internal const string ShiGyQingdao = "mobile:shi-qingdao", ShiGyXiugeng = "mobile:shi-xiugeng", ShiGyChenshe = "mobile:shi-chenshe";
    internal static CardMoveReason ShiGyDiscardReason => new("skill.mobile-shi-guo-yuan.discard");

    internal static CardConversionSource ShiGySource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IShiGuoYuanHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShiGy(f, op);
    internal ShiGyRecordCommittedEvent? ShiGyRecord(int owner, string instance) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiGyRecordCommittedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner && e.Source.SkillInstanceId == instance);
    internal ShiGyHandRecordedEvent? ShiGyRecordedHand(int owner, string instance, int target) => ShiGyRecord(owner, instance)is { } record ? _coreProgramEventHistoryOperations.ProgramEventHistory<ShiGyHandRecordedEvent>().SingleOrDefault(e => e.FrameId == record.FrameId && e.Target == target) : null;
    internal int ShiGyHandLimitBonus(int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiGyHandLimitRaisedEvent>().Count(e => e.Target == target);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeShiGyDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng))
            return existing;
        var extra = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != subject.Seat).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.DrawPhaseStarting)).Where(c => c.SkillId == ShiGyXiugeng && c.BindingId == "compare-record" && ShiGyRecordedHand(c.OwnerSeat, c.SkillInstanceId, subject.Seat)is not null);
        return existing.Concat(extra).Distinct().OrderByDescending(c => c.Priority).ThenBy(c => _coreTableOrderOperations.RelativeTableOrder(subject.Seat, c.OwnerSeat)).ThenBy(c => c.SkillId, StringComparer.Ordinal).ToArray();
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.ForeignDraw, 30)]
    internal bool IsShiGyDrawContext(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == ShiGyXiugeng && c.BindingId == "compare-record" && w.Window == SkillProgramTriggerWindow.DrawPhaseStarting && w.OwnerSeat == c.OwnerSeat && w.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && w.TargetSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat].IsAlive && ShiGyRecordedHand(c.OwnerSeat, c.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)is not null && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DrawPhaseStarting, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase, ActualPhaseSubstitution: null } parent && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == c;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 48100)]
    internal bool CanOfferShiGy(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (c.SkillId == ShiGyQingdao)
            return w.CardUse is { } a && a.ActorSeat != c.OwnerSeat && global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(a.EffectiveKind);
        if (c.SkillId == ShiGyXiugeng)
            return t.Id == "record-hands" ? w.OwnerSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == c.OwnerSeat : w.TargetSeat is { } target && ShiGyRecordedHand(c.OwnerSeat, c.SkillInstanceId, target)is not null;
        return c.SkillId != ShiGyChenshe || w.TargetSeat is { } victim && victim != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _coreMatchParticipantsPlayersAccess._players[victim].Hp <= 0;
    }

    internal SkillProgramStepOutcome BeginShiGy(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShiGuoYuan is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w)
            throw new InvalidOperationException("Shi Guo Yuan requires its original unpaid trigger.");
        var stage = op == SkillProgramEffectOp.ResolveMobileShiGuoYuanQingdao ? ShiGyStage.Benefit : op == SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng ? f.TriggerId == "record-hands" ? ShiGyStage.Record : ShiGyStage.DrawChildren : ShiGyStage.Discard;
        var r = new ShiGyReceipt(ShiGySource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, w.ParentFrameId, stage)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (f.SkillId == ShiGyQingdao && op == SkillProgramEffectOp.ResolveMobileShiGuoYuanQingdao && w is { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } a } && a.ActorSeat != f.OwnerSeat && global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(a.EffectiveKind))
            r = r with
            {
                OriginalUse = a.ParentCardUseFrameId,
                Damaged = _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Any(e => e.SourceCardUseResolutionId == a.ParentCardUseFrameId && e.TargetSeat == f.OwnerSeat && e.Amount > 0)
            };
        else if (f.SkillId == ShiGyXiugeng && op == SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng && w.Window is SkillProgramTriggerWindow.TurnBeforeStart or SkillProgramTriggerWindow.DrawPhaseStarting)
        {
        }
        else if (f.SkillId == ShiGyChenshe && op == SkillProgramEffectOp.ResolveMobileShiGuoYuanChenshe && _coreDyingOwnedCardSelectionsOperations.ExactDyingOwnedCardEntry(f, out var dying, out _) && dying.VictimSeat != f.OwnerSeat)
            r = r with
            {
                DyingId = dying.Id,
                Victim = dying.VictimSeat,
                Killer = dying.KillerSeat ?? -1,
                Seats = [f.OwnerSeat, dying.VictimSeat, dying.KillerSeat ?? -1]
            };
        else
            throw new InvalidOperationException("Shi Guo Yuan lost its original card use, turn/draw boundary or other-dying entry.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.WindowId, f.TriggerId!));
        if (f.SkillId == ShiGyXiugeng && f.TriggerId == "compare-record")
        {
            var target = w.TargetSeat!.Value;
            var record = ShiGyRecordedHand(f.OwnerSeat, f.SkillInstanceId, target) ?? throw new InvalidOperationException("Xiugeng lost its current recorded target.");
            var actual = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count;
            var draw = actual <= record.HandCount;
            var limit = actual >= record.HandCount;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyComparedEvent(f.Id, record.FrameId, target, actual, record.HandCount, draw, limit));
            if (limit)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyHandLimitRaisedEvent(f.Id, target));
            if (draw)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.Drawing, Victim = target } });
                return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, 2, new("skill.mobile-shi-guo-yuan.xiugeng.draw"));
            }

            FinishShiGy(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        ContinueShiGy(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShiGyChoices(ProgramSkillFrame f)
    {
        var r = f.ShiGuoYuan!;
        PromptChoice C(string branch, string text, int? target = null) => new(new($"shi-guo-yuan.{(f.Id)}.{(r.Stage)}.{(r.Index)}.{(branch)}.{(target ?? -1)}"), text, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "shi-guo-yuan", ["branch"] = branch });
        if (r.Stage == ShiGyStage.Benefit)
        {
            var choices = new List<PromptChoice>
            {
                C("obtain", r.Damaged ? "从牌堆或弃牌堆获得一张【闪】" : "从牌堆或弃牌堆获得一张【杀】")
            };
            if (r.Damaged)
                choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => ShiGyDiscardChoices(f, p.Seat)));
            else
                choices.Add(C("use", "无距离限制使用一张手牌"));
            return choices.ToArray();
        }

        if (r.Stage == ShiGyStage.Record)
            return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !r.Seats.Contains(p.Seat) && r.Seats.Count < 2).Select(p => C("record", "记录 " + p.Name + $" 的手牌数（当前{(_corePhysicalOwnedCardQueriesOperations.GetHand(p).Count)}张）", p.Seat)).Append(C("finish", $"确认记录（已选{(r.Seats.Count)}/2）")).ToArray();
        if (r.Stage == ShiGyStage.Discard && r.Index < r.Seats.Count && r.Seats[r.Index] is >= 0 and var payer && _coreMatchParticipantsPlayersAccess._players[payer].IsAlive)
            return ShiGyDiscardChoices(f, payer);
        if (r.Stage == ShiGyStage.UseChoice)
            return ShiGyUseChoices(f);
        return[];
    }

    internal IReadOnlyList<PromptChoice> ShiGyDiscardChoices(ProgramSkillFrame f, int target) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => target != f.OwnerSeat || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(target, card, new(zone, target), OwnedCardMoveIntent.Discard)).Select(c => c with { Id = new($"shi-guo-yuan.{(f.Id)}.{(f.ShiGuoYuan!.Stage)}.{(f.ShiGuoYuan.Index)}.{(target)}.{(c.Parameters["source-zone"])}.{(c.Parameters["slot-index"])}"), Description = _coreMatchParticipantsPlayersAccess._players[target].Name + "：" + c.Description, Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "shi-guo-yuan", ["branch"] = "discard" } }).ToArray();
    internal void PublishShiGy(ProgramSkillFrame f)
    {
        var r = f.ShiGuoYuan!;
        var choices = ShiGyChoices(f);
        if (choices.Count == 0)
        {
            FinishShiGy(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage switch
        {
            ShiGyStage.Record => "修耕：记录至多两名角色当前的手牌数；确认后替换上次记录。",
            ShiGyStage.Discard => $"陈赦：依次弃置本人、濒死角色、伤害来源各一张区域牌（第{(r.Index + 1)}/3项）。",
            ShiGyStage.UseChoice => "清蹈：可以无距离限制使用一张手牌。",
            _ => r.Damaged ? "清蹈：你受过此牌伤害，选择获得闪或弃置一名角色的一张区域牌。" : "清蹈：你未受此牌伤害，选择获得杀或无距离使用手牌。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
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

    [ProgramAnswer("shi-guo-yuan")]
    internal void AnswerShiGy(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiGuoYuan: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiGyChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shi Guo Yuan rejects an unpublished hand, field slot or native use choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "record")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Seats = r.Seats.Append(choice.Targets.Single()).ToArray() } });
            PublishShiGy(f);
            return;
        }

        if (r.Stage == ShiGyStage.Record && branch == "finish")
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyRecordCommittedEvent(f.Id, r.Source, r.Seats.Count));
            foreach (var seat in r.Seats)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyHandRecordedEvent(f.Id, seat, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count));
            FinishShiGy(f);
            return;
        }

        if (branch == "obtain")
        {
            var kind = r.Damaged ? CardKind.Dodge : CardKind.Slash;
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(c => kind == CardKind.Slash ? global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind) : c.Kind == kind) ?? _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).FirstOrDefault(c => kind == CardKind.Slash ? global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind) : c.Kind == kind);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.Movement, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (card is not null)
                _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.Hand(f.OwnerSeat), new("skill.mobile-shi-guo-yuan.qingdao.obtain"));
            SealShiGy(f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyBenefitObtainedEvent(f.Id, kind, card?.Id));
            ContinueShiGy(f.Id);
            return;
        }

        if (branch == "use")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.UseChoice } });
            PublishShiGy(f);
            return;
        }

        if (r.Stage == ShiGyStage.UseChoice)
        {
            AnswerShiGyUse(f, choice);
            return;
        }

        if (branch != "discard")
            throw new InvalidOperationException("Unknown Shi Guo Yuan branch.");
        var payer = int.Parse(choice.Parameters["card-owner-seat"], CultureInfo.InvariantCulture);
        var from = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), payer);
        var cardToDiscard = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[payer], cardToDiscard);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.Movement, Before = before, After = before, Index = r.Index + 1 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, cardToDiscard, from, ShiGyDiscardReason, movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id), parentFrameId: f.Id);
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == cardToDiscard.Id && m.From == from && m.Reason == ShiGyDiscardReason);
        var paid = new ShiGyPaidCard(r.Index, payer, cardToDiscard.Id, from, movement.To, suit, movement.Sequence);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiGuoYuan = f.ShiGuoYuan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = r.Paid.Append(paid).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyDiscardPaidEvent(f.Id, paid));
        ContinueShiGy(f.Id);
    }

    internal void SealShiGy(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiGuoYuan = f.ShiGuoYuan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainShiGy(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 48100)]
    internal bool ContinueShiGy(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiGuoYuan: { } r } f || f.Id != id)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || r.Stage is ShiGyStage.Drawing or ShiGyStage.IssuingUse or ShiGyStage.NativeUse)
            return true;
        if (r.Stage is ShiGyStage.Movement or ShiGyStage.DrawChildren or ShiGyStage.RecoveryChildren or ShiGyStage.LostSkillChildren or ShiGyStage.NativeTail)
        {
            if (DrainShiGy(id))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            r = f.ShiGuoYuan!;
            if (r.Stage == ShiGyStage.RecoveryChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.LostSkillChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _coreProgramSkillChangesOperations.ReplaceProgramSkills(f, [ShiGyChenshe], [], ProgramSkillSourceRemoval.Enabled, acquireFirst: false);
                _coreProgramSkillChangesOperations.CommitProgramSkillChangeFact(f, new ShiGyChensheLostEvent(f.Id, f.OwnerSeat));
                ContinueShiGy(id);
                return true;
            }

            if (f.SkillId != ShiGyChenshe || r.Stage != ShiGyStage.Movement)
            {
                FinishShiGy(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.Discard } });
            r = f.ShiGuoYuan!;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishShiGy(f);
            return true;
        }

        if (r.Stage == ShiGyStage.Discard)
        {
            while (r.Index < r.Seats.Count && (r.Seats[r.Index] < 0 || !_coreMatchParticipantsPlayersAccess._players[r.Seats[r.Index]].IsAlive || ShiGyDiscardChoices(f, r.Seats[r.Index]).Count == 0))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r = r with { Index = r.Index + 1 } });
            }

            if (r.Index == r.Seats.Count)
            {
                if (r.Paid.Count == 3 && r.Paid.Select(p => p.ParticipantIndex).SequenceEqual([0, 1, 2]) && r.Paid.Select(p => p.Suit).Distinct().Count() == 1 && _coreMatchParticipantsPlayersAccess._players[r.Victim].IsAlive)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = r with { Stage = ShiGyStage.RecoveryChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiGyRescueIssuedEvent(f.Id, r.Victim, _coreMatchParticipantsPlayersAccess._players[r.Victim].MaxHp));
                    _coreProgramLifecycleOperations.RecoverProgramTargetTo(f.Id, f.OwnerSeat, r.Victim, SkillProgramNumberExpression.IntegerConstant, _coreMatchParticipantsPlayersAccess._players[r.Victim].MaxHp, true);
                    ContinueShiGy(id);
                    return true;
                }

                FinishShiGy(f);
                return true;
            }
        }

        PublishShiGy(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 48100)]
    internal bool ReturnShiGyMovement(ProgramSkillFrame f) => f.ShiGuoYuan is not null && ContinueShiGy(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng)]
    internal void ReturnShiGyDraw(ProgramSkillFrame supplied, NativeDrawOutcome d)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShiGuoYuan!;
        if (r.Stage != ShiGyStage.Drawing || d.RecipientSeat != r.Victim || d.RequestedCount != 2)
            throw new InvalidOperationException("Xiugeng lost its frozen actual recipient draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShiGuoYuan = r with { Stage = ShiGyStage.DrawChildren, Before = d.SequenceBefore, After = d.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueShiGy(f.Id);
    }

    internal void FinishShiGy(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiGuoYuan = f.ShiGuoYuan!with { Stage = ShiGyStage.Finished }, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShiGuoYuanQingdao, SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng, SkillProgramEffectOp.ResolveMobileShiGuoYuanChenshe)]
    internal PromptChoice ChooseShiGy(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.ShiGuoYuan!.Stage == ShiGyStage.Record)
            return p.Choices.FirstOrDefault(c => c.Targets is [var seat] && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[seat])) ?? p.Choices.Last();
        if (f.ShiGuoYuan.Stage == ShiGyStage.Benefit)
            return p.Choices.First(c => c.Parameters["branch"] == "obtain");
        // Opaque foreign Hand slots have no face-based score.
        return p.Choices.OrderBy(c => c.Cards is [var id] ? global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) : 0).First();
    }
}
