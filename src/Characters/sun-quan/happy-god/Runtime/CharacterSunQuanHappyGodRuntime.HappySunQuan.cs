#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanHappyGodRuntime
{
    internal const string HappyShehuReason = "skill-program.happy-shehu.discard", HappyDingliDraw = "skill-program.happy-dingli.draw";
    internal static CardConversionSource HappySunQuanSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IHappySunQuanHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHappySunQuan(f, op);
    internal ProgramLifecycleTriggerWindowFrame? HappyLearningParent(long id) => _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == id && p.HappyLearningStart is not null);
    internal ProgramCardTriggerWindowFrame? HappyShehuParent(long id) => _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == id);
    internal bool HappyDingliQualified(int owner, int holder) => owner != holder && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[holder].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].HasHpValue && _coreMatchParticipantsPlayersAccess._players[holder].HasHpValue && (_coreMatchParticipantsPlayersAccess._players[holder].Hp < _coreMatchParticipantsPlayersAccess._players[owner].Hp || _coreMatchParticipantsPlayersAccess._players[owner].Hp < _coreMatchParticipantsPlayersAccess._players[owner].MaxHp);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40940, SkillProgramEffectOp.GrantHappySunQuanLearning, SkillProgramEffectOp.ResolveHappySunQuanLearning, SkillProgramEffectOp.ResolveHappySunQuanShehu, SkillProgramEffectOp.ResolveHappySunQuanDingli)]
    internal bool HappySunQuanEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (c.SkillId == HappySunQuanComposition.Quanxue)
            return _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat);
        if (c.SkillId == HappySunQuanComposition.Learning)
            return IsIssuedHappyLearning(c, w);
        if (c.SkillId == HappySunQuanComposition.Shehu)
            return HappyShehuParent(w.ParentFrameId)is { } p && p.Action.Type == CardActionType.Use && p.Action.ActorSeat == c.OwnerSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.Action.EffectiveKind) && p.Action.EffectiveDesignatedTargetSeats.Any(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && HappyXue(s) > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count > 0) && !_coreProgramEventHistoryOperations.ProgramEventHistory<HappySunQuanStartedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.Operation == SkillProgramEffectOp.ResolveHappySunQuanShehu && e.ActionId == p.Action.ActionId);
        return HappyLearningParent(w.ParentFrameId)is { } start && IsHappyLearningStart(start) && start.CandidateIndex > 0 && start.CandidateIndex < start.Candidates.Count && start.Candidates[start.CandidateIndex] == c && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLearningChoiceEvent>().Count(e => e.WindowId == start.Id) == 1 && HappyDingliQualified(c.OwnerSeat, start.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginHappySunQuan(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.HappySunQuan is not null || f.WindowContext is not { } w)
            throw new InvalidOperationException("Happy Sun Quan requires his original unpaid trigger.");
        var card = op == SkillProgramEffectOp.ResolveHappySunQuanShehu ? HappyShehuParent(w.ParentFrameId) : null;
        var start = op is SkillProgramEffectOp.ResolveHappySunQuanLearning or SkillProgramEffectOp.ResolveHappySunQuanDingli ? HappyLearningParent(w.ParentFrameId) : null;
        var target = start?.OwnerSeat ?? f.OwnerSeat;
        var amount = op == SkillProgramEffectOp.ResolveHappySunQuanLearning ? start!.HappyLearningStart!.Grants.Sum(g => g.After) : 0;
        var branch = op == SkillProgramEffectOp.ResolveHappySunQuanDingli ? (_coreMatchParticipantsPlayersAccess._players[target].Hp >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp ? 1 : 2) : 0;
        if (op == SkillProgramEffectOp.ResolveHappySunQuanDingli)
            amount = branch == 1 ? 1 : Math.Min(2, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp - _coreMatchParticipantsPlayersAccess._players[target].Hp);
        int[] targets = card is null ? [] : card.Action.EffectiveDesignatedTargetSeats.Distinct().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && HappyXue(s) > 0).OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, s)).ToArray();
        var r = new HappySunQuanReceipt(HappySunQuanSource(f), f.GameplayHash, op, w.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
        {
            Stage = op switch
            {
                SkillProgramEffectOp.GrantHappySunQuanLearning => HappySunQuanStage.Targets,
                SkillProgramEffectOp.ResolveHappySunQuanLearning => HappySunQuanStage.LearningChoice,
                SkillProgramEffectOp.ResolveHappySunQuanShehu => HappySunQuanStage.Discard,
                _ => branch == 1 ? HappySunQuanStage.RecoveryChildren : HappySunQuanStage.Drawing
            },
            Target = target,
            Amount = amount,
            Branch = branch,
            Targets = targets,
            ActionId = card?.Action.ActionId ?? 0,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappySunQuan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappySunQuanStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.ActualTurn, r.ActionId, target, amount));
        if (op == SkillProgramEffectOp.ResolveHappySunQuanLearning)
        {
            if (start is null || !IsHappyLearningStart(start) || start.CandidateIndex != 0 || f.OwnerSeat != target || HappyXue(target) != amount)
                throw new InvalidOperationException("Learning lost the exact holder's frozen source obligations.");
            foreach (var grant in start.HappyLearningStart!.Grants)
                _coreStrategicProgramsOperations.SetStrategicMarker(target, PlayerMarkerKind.HappyXue, grant.Source.OwnerSeat, 0, f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLearningRemovedEvent(f.Id, start.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, target, amount));
        }

        if (op == SkillProgramEffectOp.ResolveHappySunQuanShehu)
            for (var i = 0; i < targets.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyShehuTargetEvent(f.Id, i, r.ActionId, targets[i], HappyXue(targets[i])));
        if (op == SkillProgramEffectOp.ResolveHappySunQuanDingli)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyDingliBenefitEvent(f.Id, r.ParentId, target, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp, _coreMatchParticipantsPlayersAccess._players[target].Hp, branch, amount));
            if (branch == 2)
            {
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, amount, new(HappyDingliDraw));
                return SkillProgramStepOutcome.AwaitChild;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
            RecordHappySunQuanMovement(f.Id, r.Before);
        }

        ContinueHappySunQuan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> HappySunQuanChoices(ProgramSkillFrame f)
    {
        var r = f.HappySunQuan!;
        PromptChoice C(string option, string label, int target = -1, IReadOnlyList<int>? cards = null, IReadOnlyDictionary<string, string>? parameters = null) => new(new($"happy-sun-quan.{(f.Id)}.{(r.Index)}.{(option)}"), label, cards ?? [], target < 0 ? [] : [target], new Dictionary<string, string>(parameters ?? new Dictionary<string, string>()) { ["program-action"] = "happy-sun-quan", ["option"] = option, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == HappySunQuanStage.Targets)
        {
            var result = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && !r.Targets.Contains(p.Seat)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => C("target:" + p.Seat, "令“" + p.Name + "”获得一个学", p.Seat)).ToList();
            result.Add(C("finish", "完成选择"));
            return result;
        }

        if (r.Stage == HappySunQuanStage.LearningChoice)
            return[C("restricted", "本回合出牌阶段只能对自己使用牌", f.OwnerSeat), C("lose-hp", "失去1点体力", f.OwnerSeat)];
        if (r.Stage != HappySunQuanStage.Discard || r.Index >= r.Targets.Count)
            return[];
        return _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Targets[r.Index], [CardZoneKind.Hand], OwnedCardMoveIntent.Discard).Select((c, i) => C("discard:" + i, c.Description, r.Targets[r.Index], c.Cards, c.Parameters)).ToArray();
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40940)]
    internal bool ContinueHappySunQuan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappySunQuan: not null } f || f.Id != id)
            return false;
        var r = f.HappySunQuan!;
        if (r.Stage == HappySunQuanStage.Drawing)
            return true;
        if (r.Stage is HappySunQuanStage.MovementChildren or HappySunQuanStage.RecoveryChildren or HappySunQuanStage.HpLoss)
        {
            if (DrainHappySunQuan(f))
                return true;
            var next = r.Operation == SkillProgramEffectOp.ResolveHappySunQuanShehu ? HappySunQuanStage.Discard : HappySunQuanStage.Finished;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, HappySunQuan = r = r with { Stage = next, Index = r.Operation == SkillProgramEffectOp.ResolveHappySunQuanShehu ? r.Index + 1 : r.Index, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = [], ChosenCard = 0 } });
        }

        if (r.Stage == HappySunQuanStage.Finished || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishHappySunQuan(f);
            return true;
        }

        if (r.Stage == HappySunQuanStage.Targets && r.Targets.Count == 2)
        {
            FinishHappySunQuan(f);
            return true;
        }

        if (r.Stage == HappySunQuanStage.Discard)
        {
            while (r.Index < r.Targets.Count && (!_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Index]].IsAlive || HappySunQuanChoices(f).Count == 0))
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyShehuSkippedEvent(f.Id, r.Index, r.Targets[r.Index]));
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappySunQuan = r = r with { Index = r.Index + 1 } });
            }

            if (r.Index >= r.Targets.Count)
            {
                FinishHappySunQuan(f);
                return true;
            }
        }

        var choices = HappySunQuanChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
        {
            FinishHappySunQuan(f);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.Stage == HappySunQuanStage.Discard ? r.Targets[r.Index] : f.OwnerSeat,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    [ProgramAnswer("happy-sun-quan")]
    internal void AnswerHappySunQuan(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Happy Sun Quan lost his choice parent.");
        if (!HappySunQuanChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [chosen])))
            throw new InvalidOperationException("Happy Sun Quan requires his exact current choice.");
        var r = f.HappySunQuan!;
        var option = chosen.Parameters["option"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == HappySunQuanStage.Targets)
        {
            if (option == "finish")
            {
                FinishHappySunQuan(f);
                return;
            }

            var target = chosen.Targets.Single();
            var before = _coreMatchParticipantsPlayersAccess._players[target].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.HappyXue, f.OwnerSeat));
            _coreStrategicProgramsOperations.SetStrategicMarker(target, PlayerMarkerKind.HappyXue, f.OwnerSeat, before + 1, f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyXueGrantEvent(f.Id, r.Source, r.GameplayHash, target, before, before + 1));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappySunQuan = r with { Targets = r.Targets.Append(target).ToArray() } });
            ContinueHappySunQuan(f.Id);
            return;
        }

        if (r.Stage == HappySunQuanStage.LearningChoice)
        {
            var branch = option == "restricted" ? 1 : 2;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappySunQuan = r with { Branch = branch, Stage = branch == 1 ? HappySunQuanStage.Finished : HappySunQuanStage.HpLoss } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLearningChoiceEvent(f.Id, r.ParentId, r.ActualTurn, f.OwnerSeat, branch));
            if (branch == 2 && _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, 1) == SkillProgramStepOutcome.AwaitChild)
                return;
            ContinueHappySunQuan(f.Id);
            return;
        }

        var holder = r.Targets[r.Index];
        var slot = int.Parse(chosen.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder])[slot];
        var sequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappySunQuan = r with { Stage = HappySunQuanStage.MovementChildren, Target = holder, ChosenCard = card.Id, Before = sequence, After = sequence, Paid = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new(HappyShehuReason), (_, records) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.HappySunQuan!;
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { HappySunQuan = paid with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = records, Payments = paid.Payments.Append(new(r.Index, holder, card.Id, sequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) { Movements = records }).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappySunQuanMovementEvent(f.Id, r.Index, sequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, records.Count));
        });
        ContinueHappySunQuan(f.Id);
    }

    internal void RecordHappySunQuanMovement(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappySunQuan = f.HappySunQuan!with { Before = before, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = paid } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappySunQuanMovementEvent(id, f.HappySunQuan.Index, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, paid.Length));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappySunQuanDingli)]
    internal void ReturnHappyDingliDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.HappySunQuan is not { Stage: HappySunQuanStage.Drawing, Branch: 2, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.Amount || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Dingli lost its once-issued native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappySunQuan = r with { Stage = HappySunQuanStage.MovementChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, Paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.Reason.Value == HappyDingliDraw).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappySunQuanMovementEvent(f.Id, 0, outcome.SequenceBefore, outcome.SequenceAfter, f.HappySunQuan!.Paid.Count));
        ContinueHappySunQuan(f.Id);
    }

    internal bool DrainHappySunQuan(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void FinishHappySunQuan(ProgramSkillFrame f)
    {
        if (f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Happy Sun Quan cannot finish with an unreturned paid child.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappySunQuan = f.HappySunQuan!with { Stage = HappySunQuanStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappySunQuanCompletedEvent(f.Id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40940)]
    internal bool ReturnHappySunQuanMovement(ProgramSkillFrame f) => f.HappySunQuan is not null && ContinueHappySunQuan(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.GrantHappySunQuanLearning, SkillProgramEffectOp.ResolveHappySunQuanLearning, SkillProgramEffectOp.ResolveHappySunQuanShehu, SkillProgramEffectOp.ResolveHappySunQuanDingli)]
    internal PromptChoice AiHappySunQuan(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
