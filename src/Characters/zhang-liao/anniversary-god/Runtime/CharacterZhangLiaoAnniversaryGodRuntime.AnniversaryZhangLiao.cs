#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoAnniversaryGodRuntime
{
    internal const string AnnZlDiscard = "skill-program.anniversary-zhang-liao.discard";
    internal const string AnnZlDraw = "skill-program.anniversary-cuixi.draw";
    internal static CardConversionSource AnnZlSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryZhangLiaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginAnnZl(f, op);
    internal int AnnZlAbolished(int seat) => Enum.GetValues<EquipmentSlot>().Count(s => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(s) == 0);
    internal IEnumerable<EquipmentSlot> AnnZlAvailable(int seat) => Enum.GetValues<EquipmentSlot>().Where(s => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(s) > 0);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40750, SkillProgramEffectOp.ResolveAnniversaryZhangLiaoCuixi, SkillProgramEffectOp.ResolveAnniversaryZhangLiaoDuorui, SkillProgramEffectOp.ResolveAnniversaryZhangLiaoZhiti)]
    internal bool AnnZlEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Id switch
    {
        "actual-foreign-turn" => _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(c.OwnerSeat, c.OwnerSeat),
        "damage-abolish" => w.SourceSeat == c.OwnerSeat && w.TargetSeat is { } target && target != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, DamageAmount: > 0 } a && a.SourceSeat == c.OwnerSeat && a.TargetSeat == target && AnnZlAvailable(target).Any(),
        "abolished-draw" => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && AnnZlAbolished(p.Seat) > 0),
        "abolished-discard" => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive && AnnZlAbolished(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat) > 0,
        _ => false
    };
    internal SkillProgramStepOutcome BeginAnnZl(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryZhangLiao is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w)
            throw new InvalidOperationException("Anniversary Zhang Liao needs his original unpaid trigger instruction.");
        if (op == SkillProgramEffectOp.ResolveAnniversaryZhangLiaoZhiti && w.Window == SkillProgramTriggerWindow.DrawPhaseStarting)
        {
            _coreProgramLifecycleOperations.AdjustProgramNormalDraw(f, Math.Min(5, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Sum(p => AnnZlAbolished(p.Seat))));
            return SkillProgramStepOutcome.Continue;
        }

        var target = op == SkillProgramEffectOp.ResolveAnniversaryZhangLiaoDuorui ? w.TargetSeat!.Value : _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;
        if (op == SkillProgramEffectOp.ResolveAnniversaryZhangLiaoCuixi && !_coreForeignTurnContestsOperations.IsForeignActualTurnPindian(f))
            throw new InvalidOperationException("Cuixi requires its exact actual foreign turn parent.");
        var r = new AnniversaryZhangLiaoReceipt(AnnZlSource(f), f.GameplayHash, op, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, target)
        {
            Stage = op == SkillProgramEffectOp.ResolveAnniversaryZhangLiaoCuixi ? AnniversaryZhangLiaoStage.Payment : op == SkillProgramEffectOp.ResolveAnniversaryZhangLiaoDuorui ? AnniversaryZhangLiaoStage.SlotChoice : AnniversaryZhangLiaoStage.DiscardChoice,
            Count = op == SkillProgramEffectOp.ResolveAnniversaryZhangLiaoZhiti ? AnnZlAbolished(target) : 1,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            PhaseToken = _coreProgramEventHistoryOperations.ProgramEventHistory<ActualDiscardRecoveryPhaseStartedEvent>().Select(e => e.Phase.Token).DefaultIfEmpty().Max()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangLiao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangLiaoStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.ActualTurn, target, r.Count));
        ResumeAnnZl(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40750)]
    internal bool ResumeAnnZl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryZhangLiao: not null } f || f.Id != id)
            return false;
        RequireAnnZl(f);
        var r = f.AnniversaryZhangLiao!;
        if (r.Stage == AnniversaryZhangLiaoStage.Drawing)
            return true;
        if (r.Stage is AnniversaryZhangLiaoStage.PaymentChildren or AnniversaryZhangLiaoStage.SlotChildren or AnniversaryZhangLiaoStage.DiscardChildren or AnniversaryZhangLiaoStage.DrawChildren)
        {
            if (DrainAnnZl(f))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.AnniversaryZhangLiao!;
            var stage = r.Stage switch
            {
                AnniversaryZhangLiaoStage.PaymentChildren => AnniversaryZhangLiaoStage.VictimChoice,
                AnniversaryZhangLiaoStage.DiscardChildren => AnniversaryZhangLiaoStage.DiscardChoice,
                _ => AnniversaryZhangLiaoStage.Finished
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, AnniversaryZhangLiao = r = r with { Stage = stage } });
        }

        if (r.Stage == AnniversaryZhangLiaoStage.Damage)
        {
            if (f.AttackAttempt is not null)
                return true;
            FinishAnnZl(f);
            return true;
        }

        if (r.Stage == AnniversaryZhangLiaoStage.Finished || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || r.Stage == AnniversaryZhangLiaoStage.DiscardChoice && r.PaidCount >= r.Count)
        {
            FinishAnnZl(f);
            return true;
        }

        var choices = AnnZlChoices(f);
        if (choices.Count == 0)
        {
            FinishAnnZl(f);
            return true;
        }

        var chooser = r.Stage == AnniversaryZhangLiaoStage.VictimChoice ? r.Target : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name + "：请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [r.Target], chooser)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    internal IReadOnlyList<PromptChoice> AnnZlChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryZhangLiao!;
        PromptChoice C(string option, string label, IReadOnlyList<int>? cards = null, IReadOnlyDictionary<string, string>? extra = null) => new(new("anniversary-zhang-liao." + f.Id + "." + r.PaidCount + "." + option), label, cards ?? [], [r.Target], new Dictionary<string, string>(extra ?? new Dictionary<string, string>()) { ["program-action"] = "anniversary-zhang-liao", ["option"] = option, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == AnniversaryZhangLiaoStage.VictimChoice)
            return[C("draw", "跳过摸牌阶段，令神张辽摸两张牌"), C("damage", "受到神张辽造成的1点伤害")];
        if (r.Stage == AnniversaryZhangLiaoStage.SlotChoice)
            return AnnZlAvailable(r.Target).Select(s => C("slot:" + s, "废除“" + EquipmentCatalog.GetSlotName(s) + "”装备栏")).ToArray();
        if (r.Stage is not (AnniversaryZhangLiaoStage.Payment or AnniversaryZhangLiaoStage.DiscardChoice))
            return[];
        var owner = r.Stage == AnniversaryZhangLiaoStage.Payment ? f.OwnerSeat : r.Target;
        CardZoneKind[] zones = r.Stage == AnniversaryZhangLiaoStage.Payment ? [CardZoneKind.Hand, CardZoneKind.Equipment] : [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment];
        return _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, owner, zones, OwnedCardMoveIntent.Discard).Select((c, i) => C("card:" + i, c.Description, c.Cards, c.Parameters)).ToArray();
    }

    [ProgramAnswer("anniversary-zhang-liao")]
    internal void AnswerAnnZl(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Zhang Liao lost his choice frame.");
        RequireAnnZl(f);
        var r = f.AnniversaryZhangLiao!;
        var exact = AnnZlChoices(f).SingleOrDefault(c => c.Id == chosen.Id && c.Cards.SequenceEqual(chosen.Cards) && c.Targets.SequenceEqual(chosen.Targets) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(chosen.Parameters.OrderBy(p => p.Key))) ?? throw new InvalidOperationException("Zhang Liao requires the exact current choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == AnniversaryZhangLiaoStage.VictimChoice)
        {
            var branch = exact.Parameters["option"] == "draw" ? 1 : 2;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangLiao = r = r with { Branch = branch, Stage = branch == 1 ? AnniversaryZhangLiaoStage.Drawing : AnniversaryZhangLiaoStage.Damage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = [] } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryCuixiBranchEvent(f.Id, r.Source, r.ActualTurn, r.Target, branch));
            if (branch == 1)
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new(AnnZlDraw));
            else
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Target, 1);
            return;
        }

        if (r.Stage == AnniversaryZhangLiaoStage.SlotChoice)
        {
            var slot = Enum.Parse<EquipmentSlot>(exact.Parameters["option"][5..]);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangLiao = r = r with { Stage = AnniversaryZhangLiaoStage.SlotChildren, Slot = slot, CapacityBefore = _coreMatchParticipantsPlayersAccess._players[r.Target].EquipmentSlotCapacity(slot), Before = before, After = before, Paid = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(_coreMatchParticipantsPlayersAccess._players[r.Target], slot, 0);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryDuoruiLeaseEvent(f.Id, r.Source, r.Target, slot, r.CapacityBefore, r.PhaseToken));
            SaveAnnZlPayment(f.Id, before, 1);
        }
        else
        {
            var owner = r.Stage == AnniversaryZhangLiaoStage.Payment ? f.OwnerSeat : r.Target;
            var zone = Enum.Parse<CardZoneKind>(exact.Parameters["source-zone"]);
            var slot = int.Parse(exact.Parameters["slot-index"], CultureInfo.InvariantCulture);
            var cards = zone switch
            {
                CardZoneKind.Hand => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]),
                CardZoneKind.Equipment => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner]),
                CardZoneKind.Judgment => _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[owner]),
                _ => throw new InvalidOperationException("Invalid Zhang Liao card area.")};
            var card = cards[slot];
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangLiao = r with { Stage = r.Stage == AnniversaryZhangLiaoStage.Payment ? AnniversaryZhangLiaoStage.PaymentChildren : AnniversaryZhangLiaoStage.DiscardChildren, Before = before, After = before, Paid = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new(AnnZlDiscard), (_, _) => SaveAnnZlPayment(f.Id, before, r.PaidCount + 1));
        }

        ResumeAnnZl(f.Id);
    }

    internal void SaveAnnZlPayment(long id, long before, int ordinal)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.AnniversaryZhangLiao!;
        var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryZhangLiao = r with { Before = before, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = paid, PaidCount = ordinal, Payments = r.Payments.Append(new(ordinal, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) { Movements = paid }).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangLiaoPaidEvent(id, ordinal, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, paid.Length));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryZhangLiaoCuixi)]
    internal void ReturnAnnZlDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.AnniversaryZhangLiao is not { Stage: AnniversaryZhangLiaoStage.Drawing, Branch: 1, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 2 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Cuixi lost its once-issued native draw outcome.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangLiao = r with { Stage = AnniversaryZhangLiaoStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, Paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.Reason.Value == AnnZlDraw).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ResumeAnnZl(f.Id);
    }

    internal bool DrainAnnZl(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void FinishAnnZl(ProgramSkillFrame f)
    {
        if (f.PendingMovementContinuation is not null || f.AttackAttempt is not null)
            throw new InvalidOperationException("Zhang Liao cannot finish before his native child returns.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangLiao = f.AnniversaryZhangLiao!with { Stage = AnniversaryZhangLiaoStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangLiaoCompletedEvent(f.Id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40750)]
    internal bool ReturnAnnZlMovement(ProgramSkillFrame f) => f.AnniversaryZhangLiao is not null && ResumeAnnZl(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryZhangLiaoCuixi)]
    internal PromptChoice AiAnnZlCuixi(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryZhangLiaoDuorui)]
    internal PromptChoice AiAnnZlDuorui(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryZhangLiaoZhiti)]
    internal PromptChoice AiAnnZlZhiti(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    internal void ApplyAnniversaryCuixiDrawSkip(int currentSeat)
    {
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryCuixiBranchEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Target == currentSeat && e.Branch == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangLiaoStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source == e.Source && s.ActualTurn == e.ActualTurn && s.Target == currentSeat && s.Operation == SkillProgramEffectOp.ResolveAnniversaryZhangLiaoCuixi)))
            _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= global::CardGame.Core.GameEngine.DelayedTurnEffects.SkipDrawPhase;
    }

    internal IEnumerable<AnniversaryDuoruiLeaseEvent> ActiveAnnZlLeases() => _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryDuoruiLeaseEvent>().Where(e => !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryDuoruiExpiredEvent>().Any(x => x.LeaseFrameId == e.FrameId));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 40750)]
    internal void ObserveAnnZlLeases(IGameEvent payload)
    {
        if (payload is not (ActualDiscardRecoveryPhaseStartedEvent { Phase.Kind: ActualDiscardRecoveryPhaseKind.Play } or EquipmentSlotCapacityChangedEvent or PlayerAreasAbolishedEvent { Equipment: true }))
            return;
        foreach (var lease in ActiveAnnZlLeases().ToArray())
        {
            if (payload is ActualDiscardRecoveryPhaseStartedEvent { Phase.Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.Phase.ActorSeat == lease.Source.OwnerSeat && phase.Phase.Token > lease.PhaseToken)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryDuoruiExpiredEvent(lease.FrameId, lease.Target, lease.Slot, phase.Phase.Token, "next-play"));
                if (_coreMatchParticipantsPlayersAccess._players[lease.Target].EquipmentSlotCapacity(lease.Slot) == 0 && !_coreMatchParticipantsPlayersAccess._players[lease.Target].EquipmentAreaAbolished)
                    _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(_coreMatchParticipantsPlayersAccess._players[lease.Target], lease.Slot, lease.CapacityBefore);
            }
            else if (payload is EquipmentSlotCapacityChangedEvent changed && changed.Seat == lease.Target && changed.Slot == lease.Slot || payload is PlayerAreasAbolishedEvent areas && areas.Seat == lease.Target && areas.Equipment)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryDuoruiExpiredEvent(lease.FrameId, lease.Target, lease.Slot, lease.PhaseToken, "intervening-slot-change"));
        }
    }
}
