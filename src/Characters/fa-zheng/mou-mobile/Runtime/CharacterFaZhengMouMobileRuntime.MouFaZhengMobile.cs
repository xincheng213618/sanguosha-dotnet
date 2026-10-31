#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFaZhengMouMobileRuntime
{
    internal const string MouFaZhengGiftReason = "skill-program.mou-mobile:xuanhuo.give", MouFaZhengTakeReason = "skill-program.mou-mobile:xuanhuo.take", MouFaZhengRepayReason = "skill-program.mou-mobile:enyuan.give";
    SkillProgramStepOutcome IMouFaZhengHost.Start(ProgramSkillFrame f, SkillProgramEffectOp op) => StartMouFaZheng(f, op);
    internal bool CanMouFaZhengMark(int owner, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && target != owner && !MouFaZhengMarks().Any(m => m.TargetSeat == target);
    internal IReadOnlyList<PromptChoice> MouFaZhengPayments(ProgramSkillFrame f, int target) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.SelectedTargetHand, destinationSeat: target);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 31800, SkillProgramEffectOp.IssueMouFaZhengXuanhuo)]
    internal bool CanStartMouFaZheng(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner.Seat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengStartedEvent>().Any(e => e.Op == SkillProgramEffectOp.IssueMouFaZhengXuanhuo && e.Source.OwnerSeat == owner.Seat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengActivationPhaseEvent>().Any(p => p.FrameId == e.FrameId && p.Phase == phase)) && _coreMatchParticipantsPlayersAccess._players.Any(p => CanMouFaZhengMark(owner.Seat, p.Seat)) && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count > 0;
    [ProgramPhaseUsedProjection(31800)]
    internal int MouFaZhengUsed(int owner, string skill, int fallback) => skill == MouFaZhengComposition.Xuanhuo && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { } phase ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouFaZhengActivationPhaseEvent>().Count(e => e.OwnerSeat == owner && e.Phase == phase) : fallback;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 31800)]
    internal bool CanOfferMouFaZheng(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (IsMouFaZhengGainTrigger(t))
            return MouFaZhengQualification(c, context)is { } q && MouFaZhengMarks().Any(m => m.MarkId == q.MarkId) && MouFaZhengExtracted(q.MarkId) < 5 && _coreMatchParticipantsPlayersAccess._players[q.TargetSeat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[q.TargetSeat]).Count > 0;
        return t.Effects is not [{ Op: SkillProgramEffectOp.ResolveMouFaZhengEnyuan }] || context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && context.SourceSeat == c.OwnerSeat && MouFaZhengMarks().Any(m => m.Source.OwnerSeat == c.OwnerSeat);
    }

    internal SkillProgramStepOutcome StartMouFaZheng(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouFaZheng is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Mou Fa Zheng requires his original empty-input native entry.");
        var parent = f.WindowContext?.ParentFrameId ?? 0L;
        var r = new MouFaZhengReceipt(MouFaZhengSource(f), f.GameplayHash, op, parent);
        if (op == SkillProgramEffectOp.IssueMouFaZhengXuanhuo)
        {
            if (f.SkillId != MouFaZhengComposition.Xuanhuo || f.ActivationId != "give-xuan" || f.TriggerId is not null || !CanStartMouFaZheng(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                throw new InvalidOperationException("Xuanhuo requires its unused actual Play activation.");
            r = r with
            {
                Stage = MouFaZhengStage.Target,
                Phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()
            };
        }
        else
        {
            if (f.ActivationId != f.TriggerId || f.WindowContext is not { } w || !CanOfferMouFaZheng(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, w.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), w))
                throw new InvalidOperationException("Mou Fa Zheng cannot borrow another gain or Preparation window.");
            r = r with
            {
                Accepted = true,
                Stage = MouFaZhengStage.NextMark
            };
            if (op == SkillProgramEffectOp.ResolveMouFaZhengEnyuan)
                r = r with
                {
                    Marks = MouFaZhengMarks().Where(m => m.Source.OwnerSeat == f.OwnerSeat).OrderBy(m => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, m.TargetSeat)).Select(m => m.MarkId).ToArray()
                };
            else
            {
                var q = MouFaZhengQualification(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, w.OccurrenceIndex), w)!;
                r = r with
                {
                    MarkId = q.MarkId,
                    Target = q.TargetSeat
                };
            }
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengStartedEvent(f.Id, r.Source, r.GameplayHash, op, parent));
        if (r.Phase is { } phase)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengActivationPhaseEvent(f.Id, f.OwnerSeat, phase));
        for (var n = 0; n < r.Marks.Count; n++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengRosterEvent(f.Id, n, r.Marks[n]));
        if (op == SkillProgramEffectOp.IssueMouFaZhengXuanhuo)
            PublishMouFaZheng(f);
        else if (op == SkillProgramEffectOp.TakeMouFaZhengXuanhuoGain)
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]);
            var card = hand[_randomOperations._random.Next(hand.Count)];
            PayMouFaZhengMovement(f, card, CardLocation.Hand(r.Target), f.OwnerSeat, "take");
        }
        else
            ContinueMouFaZheng(f.Id);
        return op == SkillProgramEffectOp.IssueMouFaZhengXuanhuo ? SkillProgramStepOutcome.AwaitChoice : SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> MouFaZhengChoices(ProgramSkillFrame f)
    {
        var r = f.MouFaZheng!;
        if (r.Stage == MouFaZhengStage.Target)
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => CanMouFaZhengMark(f.OwnerSeat, p.Seat) && MouFaZhengPayments(f, p.Seat).Count > 0).Select(p => new PromptChoice(new($"mou-fa-zheng.{(f.Id)}.target.{(p.Seat)}"), $"交牌并令{(p.Name)}获得眩", [], [p.Seat], new Dictionary<string, string> { ["program-action"] = "mou-fa-zheng", ["branch"] = "target" })).ToArray());
        if (r.Stage is MouFaZhengStage.Gift or MouFaZhengStage.Repay)
            return Array.AsReadOnly(MouFaZhengPayments(f, r.Target).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-fa-zheng", ["branch"] = "card" } }).ToArray());
        return[];
    }

    internal void PublishMouFaZheng(ProgramSkillFrame f)
    {
        var choices = MouFaZhengChoices(f);
        if (choices.Count == 0)
        {
            if (f.MouFaZheng!.Op == SkillProgramEffectOp.ResolveMouFaZhengEnyuan)
            {
                NextMouFaZhengMark(f);
                return;
            }

            FinishMouFaZheng(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.MouFaZheng!.Stage == MouFaZhengStage.Repay ? "恩怨：交给该角色三张牌，不足则交给其全部可交牌。" : "眩惑：选择其他角色并交给其一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-fa-zheng")]
    internal void AnswerMouFaZheng(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouFaZheng: { } r } f || !ValidMouFaZheng(f) || !MouFaZhengChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Mou Fa Zheng lost his exact current HE payment choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MouFaZhengStage.Target)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r with { Target = choice.Targets.Single(), Stage = MouFaZhengStage.Gift } });
            PublishMouFaZheng(f);
            return;
        }

        if (r.Stage == MouFaZhengStage.Gift && !CanMouFaZhengMark(f.OwnerSeat, r.Target))
        {
            FinishMouFaZheng(f);
            return;
        }

        var from = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), f.OwnerSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        PayMouFaZhengMovement(f, card, from, r.Target, r.Stage == MouFaZhengStage.Gift ? "gift" : "repay");
    }

    internal void PayMouFaZhengMovement(ProgramSkillFrame f, Card card, CardLocation from, int target, string purpose)
    {
        var r = f.MouFaZheng!;
        var reason = purpose == "take" ? MouFaZhengTakeReason : purpose == "gift" ? MouFaZhengGiftReason : MouFaZhengRepayReason;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r with { Accepted = true, Stage = MouFaZhengStage.MovementChildren, MovementPurpose = purpose, PaidCardId = card.Id, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(target), new CardMoveReason(reason), move =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = current.MouFaZheng!;
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouFaZheng = paid with { After = move.Sequence, MarkId = purpose == "gift" ? f.Id : paid.MarkId } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengMovementPaidEvent(f.Id, purpose, purpose == "gift" ? f.Id : paid.MarkId, from.OwnerSeat!.Value, target, move.Sequence));
            if (purpose == "gift")
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengMarkEvent(f.Id, paid.Source, paid.GameplayHash, target, move.Sequence));
            if (purpose == "take")
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengObtainedEvent(f.Id, paid.MarkId, from.OwnerSeat.Value, move.To == CardLocation.Hand(f.OwnerSeat) ? 1 : 0, move.Sequence));
            if (purpose != "take")
                _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, from.OwnerSeat!.Value, target, [card.Id], f.MouFaZheng!.Before);
        }, disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouFaZheng = f.MouFaZheng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueMouFaZheng(f.Id);
    }

    internal void RemoveMouFaZhengMark(ProgramSkillFrame f)
    {
        var r = f.MouFaZheng!;
        if (MouFaZhengMarks().Any(m => m.MarkId == r.MarkId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengRemovedEvent(f.Id, r.MarkId, r.Target, MouFaZhengReceived(r.MarkId)));
        SynchronizeMouFaZhengMarks();
    }

    internal void NextMouFaZhengMark(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = f.MouFaZheng!with { Index = f.MouFaZheng.Index + 1, Stage = MouFaZhengStage.NextMark }, PendingMovementContinuation = null });
        ContinueMouFaZheng(f.Id);
    }

    internal void ContinueMouFaZheng(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouFaZheng: { } r } f || f.Id != id)
            return;
        if (!ValidMouFaZheng(f))
            throw new InvalidOperationException("Mou Fa Zheng lost his issued native effect cursor.");
        if (r.Stage is MouFaZhengStage.Target or MouFaZhengStage.Gift or MouFaZhengStage.Repay)
        {
            PublishMouFaZheng(f);
            return;
        }

        if (DrainMouFaZhengChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouFaZheng!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouFaZheng(f);
            return;
        }

        if (r.Stage == MouFaZhengStage.MovementChildren)
        {
            if (r.MovementPurpose != "repay")
            {
                FinishMouFaZheng(f);
                return;
            }

            r = r with
            {
                Remaining = r.Remaining - 1,
                Stage = MouFaZhengStage.Repay
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r });
            if (r.Remaining > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            {
                PublishMouFaZheng(f);
                return;
            }

            NextMouFaZhengMark(f);
            return;
        }

        if (r.Stage == MouFaZhengStage.LosingHp)
        {
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<IdentityAppearanceCompletedEvent>().Any(done => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<IdentityGeneralAppearedEvent>().Any(e => e.AppearanceFrameId == done.AppearanceFrameId && e.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && e.Receipt.ParentFrameId == f.Id && e.Receipt.OwnerSeat == r.Target && e.Receipt.RequestedDeduction == 1)))
                throw new InvalidOperationException("Enyuan requires its native HP-loss return.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r = r with { Stage = MouFaZhengStage.LossChildren } });
        }

        if (r.Stage == MouFaZhengStage.LossChildren)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r with { Stage = MouFaZhengStage.RecoveryChildren, RecoveryHpBefore = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouFaZheng = f.MouFaZheng!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                ContinueMouFaZheng(id);
                return;
            }

            RemoveMouFaZhengMark(f);
            NextMouFaZhengMark(f);
            return;
        }

        if (r.Stage == MouFaZhengStage.RecoveryChildren)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengRecoveryReturnedEvent(f.Id, f.OwnerSeat));
            RemoveMouFaZhengMark(f);
            NextMouFaZhengMark(f);
            return;
        }

        if (r.Stage != MouFaZhengStage.NextMark)
            throw new InvalidOperationException("Unexpected Enyuan cursor.");
        if (r.Index >= r.Marks.Count)
        {
            FinishMouFaZheng(f);
            return;
        }

        var mark = MouFaZhengMarks().SingleOrDefault(m => m.MarkId == r.Marks[r.Index]);
        if (mark is null || !_coreMatchParticipantsPlayersAccess._players[mark.TargetSeat].IsAlive)
        {
            NextMouFaZhengMark(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r = r with { MarkId = mark.MarkId, Target = mark.TargetSeat } });
        if (MouFaZhengReceived(mark.MarkId) >= 3)
        {
            RemoveMouFaZhengMark(f);
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                NextMouFaZhengMark(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r with { Stage = MouFaZhengStage.Repay, Remaining = 3 } });
            PublishMouFaZheng(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = r with { Stage = MouFaZhengStage.LosingHp, LossId = 0, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var result = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, mark.TargetSeat, 1);
        if (result == SkillProgramStepOutcome.Continue && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id)
            ContinueMouFaZheng(id);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 31800)]
    internal void ObserveMouFaZhengLoss(ProgramSkillHpLostEvent lost)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouFaZheng: { Stage: MouFaZhengStage.LosingHp } r } f || lost.FrameId != f.Id)
            return;
        var change = _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.SingleOrDefault(c => c.ParentFrameId == f.Id && c.Kind == HpChangeKind.Loss && c.TargetSeat == r.Target);
        if (!ValidMouFaZheng(f) || lost.SkillId != f.SkillId || lost.TargetSeat != r.Target || change is null || change.SourceSeat is not null || change.Amount != lost.Amount || change.HpAfter != lost.RemainingHp)
            throw new InvalidOperationException("Enyuan lost the exact native HP loss invoice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouFaZheng = r with { Stage = MouFaZhengStage.LossChildren, LossId = change.Id, LossHpBefore = change.HpBefore, LossHpAfter = change.HpAfter } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengLossReturnedEvent(f.Id, r.Target, change.Id, change.Amount, change.HpBefore, change.HpAfter));
    }

    internal void FinishMouFaZheng(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouFaZhengCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouFaZheng = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.IssueMouFaZhengXuanhuo, SkillProgramEffectOp.ResolveMouFaZhengEnyuan)]
    internal PromptChoice AiMouFaZheng(PendingDecision p, ProgramSkillFrame f) => f.MouFaZheng!.Stage == MouFaZhengStage.Target ? p.Choices.OrderBy(c => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets[0]]) ? 1 : 0).ThenBy(c => c.Targets[0]).First() : p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
