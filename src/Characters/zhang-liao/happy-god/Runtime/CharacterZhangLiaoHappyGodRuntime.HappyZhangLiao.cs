#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLiaoHappyGodRuntime
{
    internal const string HappyZlTake = "skill-program.happy-duorui.take";
    internal const string HappyZlDiscard = "skill-program.happy-zhiti.discard";
    internal const string HappyZlDraw = "skill-program.happy-duorui.draw";
    internal bool HasHappyZl => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveHappyZhangLiaoDuorui);

    internal static CardConversionSource HappyZlSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IHappyZhangLiaoHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHappyZl(f, op);
    internal int HappyZlScore() => _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && p.Hp < p.MaxHp) + _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Sum(p => _characterZhangLiaoAnniversaryGodOperations.AnnZlAbolished(p.Seat));
    internal IEnumerable<RuleQueryContribution> HappyZlNumericContributions(CharacterState player, SkillRuleQuery query) => query == SkillRuleQuery.SlashLimit && player.IsAlive && _coreNativeSkillContextQueriesOperations.HasProgramSkill(player, "happy:zhiti") && HappyZlScore() > 2 ? [new FiniteRuleQueryContribution("happy:zhiti", SkillRuleOperation.Add, 1)] : [];
    internal IEnumerable<HappyZlLeaseEvent> ActiveHappyZlLeases(int owner, int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZlLeaseEvent>().Where(e => e.Source.OwnerSeat == owner && e.Target == target && !_coreProgramEventHistoryOperations.ProgramEventHistory<HappyZlLeaseExpiredEvent>().Any(x => x.LeaseFrameId == e.FrameId));
    internal bool HasHappyZlSourceRestoration(int source, int target) => HasHappyZl && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreNativeSkillContextQueriesOperations.HasProgramSkill(_coreMatchParticipantsPlayersAccess._players[target], "happy:duorui") && ActiveHappyZlLeases(target, source).Any();
    internal HappyZlColorEvent? HappyZlColor(int owner, int target, bool? color) => color is null || owner == target ? null : _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZlColorEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner && e.Target == target && e.Turn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Color == color && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZlStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source == e.Source && s.Turn == e.Turn && s.Op == SkillProgramEffectOp.ResolveHappyZhangLiaoDuorui));
    internal CardUseFrame? HappyZlDamageUse(int owner, ProgramSkillWindowContext w)
    {
        if (w is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source, TargetSeat: { } target } || source != owner || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } damage || damage.Id != w.ParentFrameId || damage.ParentFrameId != w.DamageFrameId)
            return null;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(damage);
        if (attack.IsSourceLess || !attack.DamageWasApplied || attack.SourceSeat != owner || attack.TargetSeat != target || attack.CardUserSeat != owner)
            return null;
        return _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { Action: { } a } use && a.ActorSeat == owner && HappyZlColor(owner, target, a.EffectiveIsRed ?? global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(a.EffectiveSuit))is not null ? use : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40980, SkillProgramEffectOp.ResolveHappyZhangLiaoDuorui, SkillProgramEffectOp.ResolveHappyZhangLiaoDuoruiDamage, SkillProgramEffectOp.ResolveHappyZhangLiaoZhiti)]
    internal bool HappyZlEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Id switch
    {
        "take-hand" => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0),
        "colored-damage" => w.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _characterZhangLiaoAnniversaryGodOperations.AnnZlAvailable(target).Any() && HappyZlDamageUse(c.OwnerSeat, w)is not null,
        "restore-source" => w.Window == SkillProgramTriggerWindow.BeforeDamageApplied && w.TargetSeat == c.OwnerSeat && w.SourceSeat is { } source && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, DamageAmount: > 0 } attack && attack.SourceSeat == source && attack.TargetSeat == c.OwnerSeat && ActiveHappyZlLeases(c.OwnerSeat, source).Any(),
        "wounded-slots-draw" => HappyZlScore() > 1,
        "wounded-slots-discard" => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive && HappyZlScore() > 3 && _characterZhangLiaoAnniversaryGodOperations.AnnZlAbolished(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat) > 0,
        _ => false
    };
    internal SkillProgramStepOutcome BeginHappyZl(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyZhangLiao is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w)
            throw new InvalidOperationException("Happy Zhang Liao requires his original trigger frame.");
        if (op == SkillProgramEffectOp.ResolveHappyZhangLiaoZhiti && w.Window == SkillProgramTriggerWindow.DrawPhaseStarting)
        {
            _coreProgramLifecycleOperations.AdjustProgramNormalDraw(f, 1);
            return SkillProgramStepOutcome.Continue;
        }

        var r = new HappyZlReceipt(HappyZlSource(f), f.GameplayHash, op, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        if (op == SkillProgramEffectOp.ResolveHappyZhangLiaoDuorui)
            r = r with
            {
                Stage = HappyZlStage.Target
            };
        else if (op == SkillProgramEffectOp.ResolveHappyZhangLiaoZhiti)
            r = r with
            {
                Stage = HappyZlStage.Discard,
                Target = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
                Count = _characterZhangLiaoAnniversaryGodOperations.AnnZlAbolished(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            };
        else if (f.TriggerId == "restore-source")
        {
            if (w.Window != SkillProgramTriggerWindow.BeforeDamageApplied || w.TargetSeat != f.OwnerSeat || w.SourceSeat is not { } source || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, DamageAmount: > 0 } attack || attack.SourceSeat != source || attack.TargetSeat != f.OwnerSeat)
                throw new InvalidOperationException("Happy Duorui restoration requires the exact positive sourced damage attempt before HP changes.");
            r = r with
            {
                Stage = HappyZlStage.Restoring,
                Target = source,
                Restore = ActiveHappyZlLeases(f.OwnerSeat, source).ToArray()
            };
        }
        else
        {
            var target = w.TargetSeat!.Value;
            var slots = _characterZhangLiaoAnniversaryGodOperations.AnnZlAvailable(target).ToArray();
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || slots.Length == 0)
                return SkillProgramStepOutcome.Continue;
            var before = _randomOperations._random.State;
            var slot = slots[_randomOperations._random.Next(slots.Length)];
            r = r with
            {
                Stage = HappyZlStage.SlotChildren,
                Target = target,
                Count = 1,
                Slot = slot,
                Capacity = _coreMatchParticipantsPlayersAccess._players[target].EquipmentSlotCapacity(slot),
                RandomSlots = slots,
                RandomBefore = before,
                RandomAfter = _randomOperations._random.State
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhangLiao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlStartedEvent(f.Id, r.Source, r.Hash, op, r.Parent, r.Turn));
        if (r.Stage == HappyZlStage.SlotChildren)
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(_coreMatchParticipantsPlayersAccess._players[r.Target], r.Slot!.Value, 0);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlLeaseEvent(f.Id, r.Source, r.Target, r.Slot.Value, r.Capacity));
            SaveHappyZlPayment(f.Id, before);
        }

        ResumeHappyZl(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40980)]
    internal bool ResumeHappyZl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyZhangLiao: not null } f || f.Id != id)
            return false;
        var r = f.HappyZhangLiao!;
        if (r.Stage == HappyZlStage.Drawing)
            return true;
        if (r.Stage is HappyZlStage.GainChildren or HappyZlStage.SlotChildren or HappyZlStage.RestoreChildren or HappyZlStage.DrawChildren or HappyZlStage.DiscardChildren)
        {
            if (_characterZhangLiaoAnniversaryGodOperations.DrainAnnZl(f))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.HappyZhangLiao!;
            var next = r.Stage switch
            {
                HappyZlStage.RestoreChildren => HappyZlStage.Restoring,
                HappyZlStage.DiscardChildren => HappyZlStage.Discard,
                _ => HappyZlStage.Finished
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, HappyZhangLiao = r = r with { Stage = next } });
        }

        if (r.Stage == HappyZlStage.Restoring)
        {
            while (r.Cursor < r.Restore.Count)
            {
                var lease = r.Restore[r.Cursor];
                var active = ActiveHappyZlLeases(f.OwnerSeat, r.Target).Contains(lease) && _coreMatchParticipantsPlayersAccess._players[r.Target].EquipmentSlotCapacity(lease.Slot) == 0 && !_coreMatchParticipantsPlayersAccess._players[r.Target].EquipmentAreaAbolished;
                r = r with
                {
                    Cursor = r.Cursor + 1,
                    Restored = r.Restored + (active ? 1 : 0)
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhangLiao = r });
                if (!active)
                    continue;
                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhangLiao = r with { Stage = HappyZlStage.RestoreChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlLeaseExpiredEvent(lease.FrameId, lease.Target, lease.Slot, "damage-source-restored"));
                _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(_coreMatchParticipantsPlayersAccess._players[r.Target], lease.Slot, lease.Capacity);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlRestoredEvent(f.Id, lease.FrameId, lease.Target, lease.Slot));
                SaveHappyZlPayment(id, before);
                return ResumeHappyZl(id);
            }

            if (r.Restored > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhangLiao = r with { Stage = HappyZlStage.Drawing, Count = r.Restored } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.Restored, new(HappyZlDraw));
                return true;
            }

            FinishHappyZl(f);
            return true;
        }

        if (r.Stage == HappyZlStage.Finished || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Target >= 0 && !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || r.Stage == HappyZlStage.Discard && r.Cursor >= r.Count)
        {
            FinishHappyZl(f);
            return true;
        }

        var choices = HappyZlChoices(f);
        if (choices.Count == 0)
        {
            FinishHappyZl(f);
            return true;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage == HappyZlStage.Hand,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    internal IReadOnlyList<PromptChoice> HappyZlChoices(ProgramSkillFrame f)
    {
        var r = f.HappyZhangLiao!;
        PromptChoice C(string option, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets, IReadOnlyDictionary<string, string>? extra = null) => new(new("happy-zhang-liao." + f.Id + "." + r.Stage + "." + r.Cursor + "." + option), label, cards, targets, new Dictionary<string, string>(extra ?? new Dictionary<string, string>()) { ["program-action"] = "happy-zhang-liao", ["option"] = option, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == HappyZlStage.Target)
            return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).Select(p => C("target:" + p.Seat, "观看并获得" + p.Name + "一张手牌", [], [p.Seat])).ToArray();
        if (r.Stage == HappyZlStage.Hand)
            return _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Target, [CardZoneKind.Hand], OwnedCardMoveIntent.Obtain).Select(c =>
            {
                var slot = int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture);
                var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target])[slot];
                return r.View.Contains(card.Id) ? C("take:" + slot, "获得【" + card.DisplayName + "】", [card.Id], [r.Target], c.Parameters) : null;
            }).OfType<PromptChoice>().ToArray();
        if (r.Stage == HappyZlStage.Discard)
            return _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Target, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard).Select((c, i) => C("discard:" + i, c.Description, c.Cards, [r.Target], c.Parameters)).ToArray();
        return[];
    }

    [ProgramAnswer("happy-zhang-liao")]
    internal void AnswerHappyZl(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Happy Zhang Liao lost choice owner.");
        var r = f.HappyZhangLiao!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([chosen], HappyZlChoices(f).Where(c => c.Id == chosen.Id).ToArray()))
            throw new InvalidOperationException("Happy Zhang Liao requires his exact current dynamic card choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == HappyZlStage.Target)
        {
            var target = chosen.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhangLiao = r with { Target = target, Stage = HappyZlStage.Hand, View = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Select(c => c.Id).ToArray() } });
            ResumeHappyZl(f.Id);
            return;
        }

        var zone = Enum.Parse<CardZoneKind>(chosen.Parameters["source-zone"]);
        var slot = int.Parse(chosen.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = _cardZonesOperations._cardZones.CardsAt(new(zone, r.Target))[slot];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (r.Stage == HappyZlStage.Hand)
        {
            var color = global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.Target], card));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhangLiao = r with { Stage = HappyZlStage.GainChildren, Card = card.Id, Color = color }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(HappyZlTake), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, r.Target, f.OwnerSeat, [card.Id], before, (_, _) =>
            {
                SaveHappyZlPayment(f.Id, before);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlColorEvent(f.Id, r.Source, r.Turn, r.Target, color));
            }));
        }
        else
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhangLiao = r with { Stage = HappyZlStage.DiscardChildren, Cursor = r.Cursor + 1 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new(HappyZlDiscard), (_, _) => SaveHappyZlPayment(f.Id, before));
        }

        ResumeHappyZl(f.Id);
    }

    internal void SaveHappyZlPayment(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.HappyZhangLiao!;
        var ordinal = r.Payments.Count + 1;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhangLiao = r with { Payments = r.Payments.Append(new(ordinal, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) { Moves = moves }).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlPaidEvent(id, ordinal, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyZhangLiaoDuoruiDamage)]
    internal void ReturnHappyZlDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.HappyZhangLiao is not { Stage: HappyZlStage.Drawing, Draw: null } r || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.Restored || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Happy Duorui lost its exact once-paid native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhangLiao = r with { Stage = HappyZlStage.DrawChildren, Draw = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        SaveHappyZlPayment(f.Id, outcome.SequenceBefore);
        ResumeHappyZl(f.Id);
    }

    internal void FinishHappyZl(ProgramSkillFrame f)
    {
        if (f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Happy Zhang Liao cannot finish before his typed movement returns.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhangLiao = f.HappyZhangLiao!with { Stage = HappyZlStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlCompletedEvent(f.Id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40980)]
    internal bool ReturnHappyZlMovement(ProgramSkillFrame f) => f.HappyZhangLiao is not null && ResumeHappyZl(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyZhangLiaoDuorui)]
    internal PromptChoice AiHappyZlTake(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyZhangLiaoDuoruiDamage)]
    internal PromptChoice AiHappyZlDamage(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyZhangLiaoZhiti)]
    internal PromptChoice AiHappyZlZhiti(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 40980)]
    internal CardSnapshot[] HappyZlPrivateHand(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.HappyZhangLiao is { Stage: HappyZlStage.Hand }).SelectMany(f => f.HappyZhangLiao!.View).Distinct().Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray();
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 40980)]
    internal void ObserveHappyZlSlots(IGameEvent payload)
    {
        if (!HasHappyZl || payload is not (EquipmentSlotCapacityChangedEvent or PlayerAreasAbolishedEvent { Equipment: true }))
            return;
        foreach (var lease in _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZlLeaseEvent>().Where(e => !_coreProgramEventHistoryOperations.ProgramEventHistory<HappyZlLeaseExpiredEvent>().Any(x => x.LeaseFrameId == e.FrameId)).ToArray())
            if (payload is EquipmentSlotCapacityChangedEvent changed && changed.Seat == lease.Target && changed.Slot == lease.Slot || payload is PlayerAreasAbolishedEvent areas && areas.Seat == lease.Target && areas.Equipment)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZlLeaseExpiredEvent(lease.FrameId, lease.Target, lease.Slot, "intervening-slot-change"));
    }
}
