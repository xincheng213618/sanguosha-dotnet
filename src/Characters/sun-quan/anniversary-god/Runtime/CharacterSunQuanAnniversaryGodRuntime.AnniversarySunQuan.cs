#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanAnniversaryGodRuntime
{
    internal const string AnniversaryMingReason = "skill-program.anniversary-sun-quan.ming";
    internal const string AnniversaryMingDraw = "skill-program.anniversary-sun-quan.colors";
    internal static CardConversionSource AnniversarySunQuanSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, f.SkillInstanceId);
    internal bool HasAnniversaryMing => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(AnniversarySunQuanComposition.Cangming);

    internal IEnumerable<AnniversaryMingPileCreatedEvent> AnniversaryMingPiles() => !HasAnniversaryMing ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMingPileCreatedEvent>().Where(e => _coreTableSeatQueriesOperations.IsValidPlayerSeat(e.HolderSeat) && e.Location == new CardLocation(CardZoneKind.PrivatePersistentPile, e.HolderSeat, $"anniversary-ming:{(e.FrameId)}:{(e.HolderSeat)}") && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunQuanStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source == e.Source && s.GameplayHash == e.GameplayHash && s.Operation is SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingSetup or SkillProgramEffectOp.ResolveAnniversarySunQuanJichao));
    internal IReadOnlyList<Card> AnniversaryMingCards(int holder) => AnniversaryMingPiles().Where(p => p.HolderSeat == holder).SelectMany(p => _cardZonesOperations._cardZones.CardsAt(p.Location)).ToArray();
    internal int[] AnniversaryJichaoPeers(int owner) => _coreTableOrderOperations.CurrentTableOrder().Where(s => s != owner && _coreMatchParticipantsPlayersAccess._players[s].IsAlive && (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count > 0 || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[s]).Count > 0)).ToArray();
    internal int AnniversaryDamageTotal(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Where(e => !e.SourceLess && e.SourceSeat == owner && e.Amount > 0).Sum(e => e.Amount);
    internal bool AnniversaryJichaoUnlocked(int owner, string instance)
    {
        var last = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryJichaoLockedEvent>().LastOrDefault(e => e.OwnerSeat == owner && e.SkillInstanceId == instance);
        return last is null || AnniversaryDamageTotal(owner) - last.DamageTotal >= 3;
    }

    SkillProgramStepOutcome global::CardGame.Core.IAnniversarySunQuanHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp operation) => BeginAnniversarySunQuan(f, operation);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 40500, SkillProgramEffectOp.ResolveAnniversarySunQuanJichao)]
    internal bool CanActivateAnniversaryJichao(ProgramActivationLegalityContext q) => AnniversaryJichaoPeers(q.OwnerSeat).Length > 0 || AnniversaryJichaoUnlocked(q.OwnerSeat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(q.Owner, q.SkillId)) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != q.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetJudgment(p).Count > 0);
    internal SkillProgramStepOutcome BeginAnniversarySunQuan(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversarySunQuan is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("Anniversary Sun Quan requires the original single unpaid instruction.");
        if (operation == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingReturn && (f.WindowContext is not { } context || !IsIssuedAnniversaryMingReturn(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context)))
            throw new InvalidOperationException("Cangming return requires its exact living holder and original native window.");
        var r = new AnniversarySunQuanReceipt(AnniversarySunQuanSource(f), f.GameplayHash, operation, f.WindowContext?.ParentFrameId ?? 0)
        {
            Stage = operation == SkillProgramEffectOp.ResolveAnniversarySunQuanJichao ? AnniversarySunQuanStage.ChoosingBranch : AnniversarySunQuanStage.MovementChildren,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversarySunQuan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunQuanStartedEvent(f.Id, r.Source, r.GameplayHash, operation, r.ParentId));
        if (operation == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingSetup)
            MoveAnniversaryMing(f, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(_corePhysicalOwnedCardQueriesOperations.GetHand).Select(c => c.Id).ToArray(), true);
        else if (operation == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingReturn)
            MoveAnniversaryMing(f, AnniversaryMingCards(f.OwnerSeat).Select(c => c.Id).ToArray(), false);
        else if (operation == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingColors)
        {
            var count = AnniversaryMingBatchColors(f.WindowContext!.MovementBatch!);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversarySunQuan = r with { ColorCount = count, Stage = AnniversarySunQuanStage.Drawing } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new(AnniversaryMingDraw));
        }
        else
            PublishAnniversarySunQuan(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void MoveAnniversaryMing(ProgramSkillFrame f, IReadOnlyList<int> cards, bool deposit)
    {
        if (cards.Count == 0)
        {
            FinishAnniversarySunQuan(f);
            return;
        }

        var locations = new Dictionary<int, CardLocation>();
        if (deposit)
            foreach (var holder in cards.Select(id => _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat!.Value).Distinct())
            {
                var location = new CardLocation(CardZoneKind.PrivatePersistentPile, holder, $"anniversary-ming:{(f.Id)}:{(holder)}");
                _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
                locations.Add(holder, location);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMingPileCreatedEvent(f.Id, AnniversarySunQuanSource(f), f.GameplayHash, holder, location));
            }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversarySunQuan = f.AnniversarySunQuan!with { Stage = AnniversarySunQuanStage.MovementChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(f.OwnerSeat), new(AnniversaryMingReason), (batch, movements) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversarySunQuan = current.AnniversarySunQuan!with { BatchId = batch, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Movements = movements } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunQuanMovementPaidEvent(f.Id, batch, movements.Count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }, deposit ? (_, from) => locations[from.OwnerSeat!.Value] : null, CardMovementDisclosure.FaceDown);
        ResumeAnniversarySunQuan(f.Id);
    }

    internal IReadOnlyList<PromptChoice> AnniversarySunQuanChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversarySunQuan!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return[];
        PromptChoice Choice(int branch, int? target, string label) => new(new($"anniversary-surge.{(f.Id)}.{(branch)}.{(target ?? -1)}"), label, [], target is { } s ? [s] : [], new Dictionary<string, string> { ["program-action"] = "anniversary-surge", ["branch"] = branch.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == AnniversarySunQuanStage.ChoosingBranch)
        {
            var choices = new List<PromptChoice>();
            if (AnniversaryJichaoPeers(f.OwnerSeat).Length > 0)
                choices.Add(Choice(1, null, "令一名其他角色随机将半数手牌及装备牌置入溟"));
            if (AnniversaryJichaoUnlocked(f.OwnerSeat, f.SkillInstanceId) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != f.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(p).Count > 0))
                choices.Add(Choice(2, null, "令所有其他角色将所有牌置入溟，然后此项失效直到你累计造成三点伤害"));
            return choices;
        }

        return r.Stage != AnniversarySunQuanStage.ChoosingTarget ? [] : AnniversaryJichaoPeers(f.OwnerSeat).Select(s => Choice(1, s, $"激潮：选择{(_coreMatchParticipantsPlayersAccess._players[s].Name)}")).ToArray();
    }

    internal void PublishAnniversarySunQuan(ProgramSkillFrame f)
    {
        var choices = AnniversarySunQuanChoices(f);
        if (choices.Count == 0)
        {
            FinishAnniversarySunQuan(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "激潮：选择一项。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-surge")]
    internal void AnswerAnniversarySunQuan(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversarySunQuan: { } r } f || !AnniversarySunQuanChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Jichao requires its exact current branch or other living participant.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = int.Parse(selected.Parameters["branch"], CultureInfo.InvariantCulture);
        if (r.Stage == AnniversarySunQuanStage.ChoosingBranch && branch == 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversarySunQuan = r with { Stage = AnniversarySunQuanStage.ChoosingTarget, Branch = 1 } });
            PublishAnniversarySunQuan(f);
            return;
        }

        var target = selected.Targets.Count == 1 ? selected.Targets[0] : -1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversarySunQuan = r with { Branch = branch, Target = target } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunQuanBranchChosenEvent(f.Id, branch, target));
        int[] ids;
        if (branch == 2)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryJichaoLockedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, f.GameplayHash, AnniversaryDamageTotal(f.OwnerSeat)));
            ids = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(p)).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(p))).Select(c => c.Id).ToArray();
        }
        else
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id).ToArray();
            for (var i = hand.Length - 1; i > 0; i--)
            {
                var j = _randomOperations._random.Next(i + 1);
                (hand[i], hand[j]) = (hand[j], hand[i]);
            }

            ids = hand.Take((hand.Length + 1) / 2).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id)).ToArray();
        }

        MoveAnniversaryMing(f, ids, true);
    }

    internal bool DrainAnniversarySunQuan(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingColors)]
    internal void ReturnAnniversarySunQuanColors(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.AnniversarySunQuan is not { Stage: AnniversarySunQuanStage.Drawing, DrawOutcome: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.ColorCount || outcome.SequenceBefore < r.Before || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunQuanMovementPaidEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Cangming colors lost its exact native draw producer and unique typed outcome.");
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.Reason.Value == AnniversaryMingDraw && m.To == CardLocation.Hand(f.OwnerSeat)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversarySunQuan = r with { Stage = AnniversarySunQuanStage.MovementChildren, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, Movements = moves }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunQuanMovementPaidEvent(f.Id, 0, moves.Length, outcome.SequenceBefore, outcome.SequenceAfter));
        ResumeAnniversarySunQuan(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40500)]
    internal bool ResumeAnniversarySunQuan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversarySunQuan: not null } f || f.Id != id)
            return false;
        if (f.AnniversarySunQuan.Stage == AnniversarySunQuanStage.Drawing)
            return true;
        if (f.AnniversarySunQuan.Stage is AnniversarySunQuanStage.ChoosingBranch or AnniversarySunQuanStage.ChoosingTarget)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishAnniversarySunQuan(f);
            return true;
        }

        if (DrainAnniversarySunQuan(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        FinishAnniversarySunQuan(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40500)]
    internal bool ReturnAnniversarySunQuanMovement(ProgramSkillFrame f) => f.AnniversarySunQuan is not null && ResumeAnniversarySunQuan(f.Id);
    internal void FinishAnniversarySunQuan(ProgramSkillFrame f)
    {
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunQuanMovementPaidEvent>().Any(e => e.FrameId == f.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunQuanMovementPaidEvent(f.Id, 0, 0, f.AnniversarySunQuan!.Before, f.AnniversarySunQuan.After));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, AnniversarySunQuan = f.AnniversarySunQuan!with { Stage = AnniversarySunQuanStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunQuanCompletedEvent(f.Id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversarySunQuanJichao)]
    internal PromptChoice ChooseAnniversarySunQuan(PendingDecision p, ProgramSkillFrame f) => p.Choices.Last();
}
