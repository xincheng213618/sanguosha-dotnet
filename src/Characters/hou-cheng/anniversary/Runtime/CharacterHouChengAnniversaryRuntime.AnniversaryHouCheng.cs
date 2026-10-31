#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHouChengAnniversaryRuntime
{
    internal const string HouChengAction = "anniversary-hou-cheng", HouChengObtainReason = "skill-program.anniversary:xianniang.obtain", HouChengGiveReason = "skill-program.anniversary:xianniang.give";
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryHouChengHost.Begin(ProgramSkillFrame f) => BeginHouCheng(f);
    internal static CardConversionSource HouChengSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasHouChengContent => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryHouChengXianniang);

    internal bool HouChengForeignDiscard(int owner, ProgramSkillWindowContext c)
    {
        if (c is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch })
            return false;
        return batch.Movements.Any(move => move.To == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(m => m.CardId == move.CardId && m.CompletionSequence == move.Sequence && m.From.OwnerSeat == owner && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(i => i.InvocationId == m.InvocationId && i.ExecutorSeat != owner && i.TurnNumber == batch.TurnNumber && m.CompletionSequence > i.SequenceBefore && m.CompletionSequence <= i.SequenceAfter)));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 48700)]
    internal bool CanOfferHouCheng(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c) => candidate.SkillId != AnniversaryHouChengComposition.Skill || t.Id == "foreign-discard" && HouChengForeignDiscard(candidate.OwnerSeat, c) || t.Id == "after-damage" && c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } target, DamageFrameId: { } id } && target == candidate.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(d => d.Id == id && d.TargetSeat == target && d.Amount == c.Amount);
    internal bool HouChengParent(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } c || f.SkillId != AnniversaryHouChengComposition.Skill || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            return false;
        if (f.TriggerId == "foreign-discard")
            return HouChengForeignDiscard(f.OwnerSeat, c) && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } w && c.MovementBatch is { } batch && w.Batch.Id == batch.Id && w.Batch.TurnNumber == batch.TurnNumber && w.Batch.Movements.SequenceEqual(batch.Movements) && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]);
        return f.TriggerId == "after-damage" && c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } target, DamageFrameId: { } id } && target == f.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } d && d.ParentFrameId == id && d.TargetSeat == target && d.TriggerWindow == c.Window && d.CandidateIndex >= 0 && d.CandidateIndex < d.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, d.Candidates[d.CandidateIndex].ToProgramCandidate()) && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(a => a.Id == id && a.TargetSeat == target && a.Amount == c.Amount);
    }

    internal SkillProgramStepOutcome BeginHouCheng(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryHouCheng is not null || !HouChengParent(f))
            throw new InvalidOperationException("Xianniang needs its original actual damage or foreign logical discard candidate.");
        var r = new AnniversaryHouChengReceipt(HouChengSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, AnniversaryHouChengStage.Source);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHouChengStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Round, r.Turn, r.TurnOwner));
        PublishHouCheng(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IEnumerable<int> HouChengSources(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count >= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count).Select(p => p.Seat);
    internal IEnumerable<int> HouChengRecipients(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && p.Seat != f.AnniversaryHouCheng!.Victim).Select(p => p.Seat);
    internal Card HouChengSlot(ProgramSkillFrame f, PromptChoice c)
    {
        var r = f.AnniversaryHouCheng!;
        var owner = r.Stage == AnniversaryHouChengStage.Obtain ? r.Victim : f.OwnerSeat;
        return _cardZonesOperations._cardZones.CardsAt(new(Enum.Parse<CardZoneKind>(c.Parameters["source-zone"]), owner))[int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture)];
    }

    internal IReadOnlyList<PromptChoice> HouChengChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryHouCheng!;
        PromptChoice Choice(string verb, string text, IReadOnlyList<int>? targets = null) => new(new($"hou-cheng.{(f.Id)}.{(r.Stage)}.{(r.Selected.Count)}.{(verb)}.{(targets?.FirstOrDefault(-1))}"), text, [], targets ?? [], new Dictionary<string, string> { ["program-action"] = HouChengAction, ["verb"] = verb });
        if (r.Stage == AnniversaryHouChengStage.Source)
            return Array.AsReadOnly(HouChengSources(f).Select(s => Choice("source", $"从 {(_coreMatchParticipantsPlayersAccess._players[s].Name)} 处获得至多两张牌", [s])).Append(Choice("stop", "不获得牌，结束献酿")).ToArray());
        if (r.Stage == AnniversaryHouChengStage.Recipient)
            return Array.AsReadOnly(HouChengRecipients(f).Select(s => Choice("recipient", $"交给 {(_coreMatchParticipantsPlayersAccess._players[s].Name)} 至多 {(r.Obtained)} 张牌", [s])).Append(Choice("stop", "不交牌，结算献酿")).ToArray());
        if (r.Stage is not (AnniversaryHouChengStage.Obtain or AnniversaryHouChengStage.Give))
            return[];
        var owner = r.Stage == AnniversaryHouChengStage.Obtain ? r.Victim : f.OwnerSeat;
        var cards = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, owner, [CardZoneKind.Hand, CardZoneKind.Equipment], r.Stage == AnniversaryHouChengStage.Obtain ? OwnedCardMoveIntent.Obtain : OwnedCardMoveIntent.Transfer, canSelect: (zone, _) => !(r.Stage == AnniversaryHouChengStage.Obtain && owner == f.OwnerSeat && zone == CardZoneKind.Hand)).Where(c => !r.Selected.Contains(HouChengSlot(f, c).Id)).Select(c => c with { Id = new($"hou-cheng.{(f.Id)}.{(r.Stage)}.{(r.Selected.Count)}.{(c.Id)}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = HouChengAction, ["verb"] = "card" } });
        return Array.AsReadOnly(cards.Append(Choice("finish", r.Selected.Count == 0 ? "不选牌，继续结算" : $"确定已选 {(r.Selected.Count)} 张牌")).ToArray());
    }

    internal void PublishHouCheng(ProgramSkillFrame f)
    {
        var r = f.AnniversaryHouCheng!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            SettleHouCheng(f);
            return;
        }

        if (r.Stage is AnniversaryHouChengStage.Obtain or AnniversaryHouChengStage.Give && (r.Selected.Count >= (r.Stage == AnniversaryHouChengStage.Obtain ? 2 : r.Obtained) || HouChengChoices(f).Count == 1))
        {
            MoveHouCheng(f);
            return;
        }

        var choices = HouChengChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage switch
        {
            AnniversaryHouChengStage.Source => "献酿：选择手牌数不小于你的角色。",
            AnniversaryHouChengStage.Obtain => "献酿：选择至多两张牌获得。",
            AnniversaryHouChengStage.Recipient => "献酿：可交给另一名其他角色至多等量的牌。",
            _ => "献酿：选择至多等量的牌交出。"
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

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryHouChengXianniang)]
    internal PromptChoice HouChengAi(PendingDecision p) => p.Choices.First();
    [ProgramAnswer(HouChengAction)]
    internal void AnswerHouCheng(PromptChoice c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHouCheng: { } r } f || !ValidHouCheng(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !HouChengChoices(f).Any(a => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([a], [c])))
            throw new InvalidOperationException("Xianniang requires its exact current private selection and original frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (c.Parameters["verb"])
        {
            case "stop":
                SettleHouCheng(f);
                return;
            case "source":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = r with { Victim = c.Targets.Single(), Stage = AnniversaryHouChengStage.Obtain, Selected = [] } });
                break;
            case "recipient":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = r with { Recipient = c.Targets.Single(), Stage = AnniversaryHouChengStage.Give, Selected = [] } });
                break;
            case "card":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = r with { Selected = r.Selected.Append(HouChengSlot(f, c).Id).ToArray() } });
                break;
            case "finish":
                MoveHouCheng(f);
                return;
            default:
                throw new InvalidOperationException("Unknown Xianniang choice.");
        }

        PublishHouCheng(f);
    }

    internal void MoveHouCheng(ProgramSkillFrame f)
    {
        var r = f.AnniversaryHouCheng!;
        var giving = r.Stage == AnniversaryHouChengStage.Give;
        if (!ValidHouCheng(f) || r.Stage is not (AnniversaryHouChengStage.Obtain or AnniversaryHouChengStage.Give) || r.Selected.Count > (giving ? r.Obtained : 2))
            throw new InvalidOperationException("Xianniang cannot replace or repay its selected native entities.");
        if (giving ? !HouChengRecipients(f).Contains(r.Recipient) : !HouChengSources(f).Contains(r.Victim))
        {
            SettleHouCheng(f);
            return;
        }

        var cardOwner = giving ? f.OwnerSeat : r.Victim;
        var recipient = giving ? r.Recipient : f.OwnerSeat;
        if (r.Selected.Any(id => _cardZonesOperations._cardZones.GetLocation(id)is not { Zone: CardZoneKind.Hand or CardZoneKind.Equipment, OwnerSeat: { } owner } || owner != cardOwner))
            throw new InvalidOperationException("Xianniang lost its unpaid selected cards.");
        if (r.Selected.Count == 0)
        {
            if (giving)
                SettleHouCheng(f);
            else
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = r with { Stage = AnniversaryHouChengStage.Recipient, Obtained = 0 } });
                SettleHouCheng(f);
            }

            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var reason = new CardMoveReason(giving ? HouChengGiveReason : HouChengObtainReason);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = r with { Stage = giving ? AnniversaryHouChengStage.GiveChildren : AnniversaryHouChengStage.ObtainChildren, Before = before, After = before, Moves = [], BatchId = 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        void Paid(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var current = active.AnniversaryHouCheng!;
            var count = moves.Count(m => m.To == CardLocation.Hand(recipient) && m.From != m.To);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { AnniversaryHouCheng = current with { BatchId = batch, Moves = moves, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Obtained = giving ? current.Obtained : count } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHouChengMovementEvent(f.Id, r.Source, r.Hash, r.Round, r.Turn, r.TurnOwner, giving, cardOwner, recipient, count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, batch));
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Selected, CardLocation.Hand(recipient), reason, giving ? _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, cardOwner, recipient, r.Selected, before, Paid) : Paid, disclosure: CardMovementDisclosure.FaceDown);
        ContinueHouCheng(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal bool DrainHouCheng(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void SettleHouCheng(ProgramSkillFrame f)
    {
        var r = f.AnniversaryHouCheng!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = r with { Stage = AnniversaryHouChengStage.HpChildren, Selected = [] }, PendingMovementContinuation = null });
        ContinueHouCheng(f.Id);
    }

    internal int HouChengRoundGain(int owner, int round) => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHouChengMovementEvent>().Where(e => !e.Giving && e.Source.OwnerSeat == owner && e.Round == round && ValidHouChengMovement(e)).Sum(e => e.ActualCount);
    internal void ContinueHouCheng(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (!ValidHouCheng(f))
            throw new InvalidOperationException("Xianniang lost its original movement or damage receipt.");
        var r = f.AnniversaryHouCheng!;
        if (DrainHouCheng(f))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        r = f.AnniversaryHouCheng!;
        if (r.Stage == AnniversaryHouChengStage.ObtainChildren)
        {
            if (r.Obtained == 0)
            {
                SettleHouCheng(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = r with { Stage = AnniversaryHouChengStage.Recipient, Selected = [], Moves = [], Before = 0, After = 0, BatchId = 0 } });
            PublishHouCheng(f);
            return;
        }

        if (r.Stage == AnniversaryHouChengStage.GiveChildren)
        {
            SettleHouCheng(f);
            return;
        }

        if (r.Stage != AnniversaryHouChengStage.HpChildren)
        {
            PublishHouCheng(f);
            return;
        }

        if (!r.HpPaid && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && HouChengRoundGain(f.OwnerSeat, r.Round) > 2)
        {
            var result = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, 1, (before, after, actual) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeFrame(active.Id, active with { AnniversaryHouCheng = active.AnniversaryHouCheng!with { HpPaid = true, HpBefore = before, HpAfter = after } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHouChengHpPaidEvent(f.Id, before, after, actual));
            });
            if (result == SkillProgramStepOutcome.AwaitChild || DrainHouCheng(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
                return;
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryHouChengCompletedEvent(id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryHouCheng = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 48700)]
    internal bool ResumeHouCheng(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryHouCheng: not null } f || f.Id != id)
            return false;
        ContinueHouCheng(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 48700)]
    internal bool ReturnHouChengMovement(ProgramSkillFrame f) => f.AnniversaryHouCheng is not null && ResumeHouCheng(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 48700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 48700)]
    internal bool RetainHouCheng(ProgramSkillFrame f) => f.AnniversaryHouCheng is not null && ValidHouCheng(f);
}
