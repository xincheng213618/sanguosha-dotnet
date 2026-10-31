#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunRuOlRuntime
{
    internal const string SunRuOlRefreshDiscardReason = "skill-program.sun-ru.ol.weimian.discard";
    internal const string SunRuOlRefreshDrawReason = "skill-program.sun-ru.ol.weimian.draw";
    internal static int SunRuOlBitCount(int mask) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_ru_ol_SunRuOlWeimian.SunRuOlBitCount(mask);
    internal static EquipmentSlot[] SunRuOlSlots => Enum.GetValues<EquipmentSlot>();

    internal static string SunRuOlSlotName(EquipmentSlot s) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_ru_ol_SunRuOlWeimian.SunRuOlSlotName(s);
    internal static string SunRuOlBenefitName(int b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_ru_ol_SunRuOlWeimian.SunRuOlBenefitName(b);
    internal int SunRuOlAvailableSlotMask(int seat) => SunRuOlSlots.Select((slot, index) => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot) > 0 ? 1 << index : 0).Aggregate(0, (a, b) => a | b);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 26110, SkillProgramEffectOp.AbolishEquipmentSlotsForDistinctBenefits)]
    internal bool CanActivateSunRuOlWeimian(int ownerSeat) => SunRuOlAvailableSlotMask(ownerSeat) != 0;
    internal SkillProgramStepOutcome BeginSunRuOlWeimian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var turn = new SunRuOlTurnToken(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
        if (f.SunRuOlWeimian is not null || f.InstructionIndex != 1 || f.TriggerId is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || !SunRuOlActualTurn(turn) || SunRuOlAvailableSlotMask(f.OwnerSeat) == 0)
            throw new InvalidOperationException("Weimian requires its accepted living recipient and one to three currently available native equipment slots.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunRuOlWeimian = new(SunRuOlSource(f), f.GameplayHash, f.InstructionIndex, turn, target, SunRuOlWeimianStage.Slots) });
        PublishSunRuOlChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> SunRuOlChoices(ProgramSkillFrame f)
    {
        var r = f.SunRuOlWeimian!;
        PromptChoice Choice(string branch, int value, string label) => new(new($"sunru-ol.{(f.Id)}.{(r.Stage)}.{(branch)}.{(value)}"), label, [], [r.RecipientSeat], new Dictionary<string, string> { ["program-action"] = "sunru-ol-weimian", ["branch"] = branch, ["value"] = value.ToString(CultureInfo.InvariantCulture) });
        var choices = new List<PromptChoice>();
        if (r.Stage == SunRuOlWeimianStage.Slots)
        {
            var available = SunRuOlAvailableSlotMask(f.OwnerSeat);
            for (var mask = 1; mask < 32; mask++)
                if ((mask & available) == mask && SunRuOlBitCount(mask) <= 3)
                    choices.Add(Choice("slots", mask, "废除：" + string.Join("、", SunRuOlSlots.Where((_, i) => (mask & (1 << i)) != 0).Select(SunRuOlSlotName))));
        }
        else if (r.Stage == SunRuOlWeimianStage.Benefits)
        {
            for (var mask = 1; mask < 8; mask++)
                if (SunRuOlBitCount(mask) == SunRuOlBitCount(r.SlotsMask))
                    choices.Add(Choice("benefits", mask, string.Join("；", Enumerable.Range(1, 3).Where(i => (mask & (1 << (i - 1))) != 0).Select(SunRuOlBenefitName))));
        }
        else if (r.Stage == SunRuOlWeimianStage.RestoreSlot)
            foreach (var(slot, index)in SunRuOlSlots.Select((s, i) => (s, i)))
                if (_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].EquipmentSlotCapacity(slot) == 0)
                    choices.Add(Choice("restore", index, "恢复" + SunRuOlSlotName(slot)));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishSunRuOlChoice(ProgramSkillFrame f)
    {
        var r = f.SunRuOlWeimian!;
        var choices = SunRuOlChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("A live Weimian choice lost every valid native option.");
        var chooser = r.Stage == SunRuOlWeimianStage.Slots ? f.OwnerSeat : r.RecipientSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == SunRuOlWeimianStage.Slots ? "慰勉：选择废除一至三个装备栏" : r.Stage == SunRuOlWeimianStage.Benefits ? $"慰勉：选择{(SunRuOlBitCount(r.SlotsMask))}个不同项目" : "慰勉：选择恢复一个被废除的装备栏", [], [r.RecipientSeat], r.RecipientSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("sunru-ol-weimian")]
    internal void AnswerSunRuOlChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SunRuOlWeimian: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == SunRuOlWeimianStage.Slots ? f.OwnerSeat : r.RecipientSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], SunRuOlChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Weimian lost the current private chooser or exact distinct native choices.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var value = int.Parse(choice.Parameters["value"], CultureInfo.InvariantCulture);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (choice.Parameters["branch"] == "slots")
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                CompleteSunRuOl(f, false);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.CostChildren, SlotsMask = value, Paid = true, SequenceBefore = before, SequenceAfter = before } });
            foreach (var(slot, index)in SunRuOlSlots.Select((s, i) => (s, i)))
                if ((value & (1 << index)) != 0)
                    _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], slot, 0);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.SunRuOlWeimian!with
            {
                SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunRuOlWeimian = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunRuOlWeimianPaidEvent(f.Id, r.Source, r.RecipientSeat, r.Turn, value, before, r.SequenceAfter));
        }
        else if (choice.Parameters["branch"] == "benefits")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.Applying, BenefitsMask = value } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunRuOlWeimianBenefitsChosenEvent(f.Id, r.RecipientSeat, value));
        }
        else if (choice.Parameters["branch"] == "restore")
        {
            var slot = SunRuOlSlots[value];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.RestoreChildren, NextBenefit = 2, SequenceBefore = before, SequenceAfter = before } });
            if (!_coreAdvancedLifecycleOperations.RestoreEquipmentSlot(_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat], slot))
                throw new InvalidOperationException("A selected abolished equipment slot was not restored.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunRuOlWeimianBenefitIssuedEvent(f.Id, r.RecipientSeat, 1, slot, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }
        else
            throw new InvalidOperationException("Unknown Weimian choice.");
        ContinueSunRuOl(f.Id);
    }

    internal bool DrainSunRuOlChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueSunRuOl(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && f.Id == id && (f.SunRuOlChishi is not null || f.SunRuOlWeimian is not null))
        {
            if (DrainSunRuOlChildren(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            if (f.SunRuOlChishi is not null)
            {
                CompleteSunRuOl(f, true);
                return;
            }

            var r = f.SunRuOlWeimian!;
            if (!_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                CompleteSunRuOl(f, r.Paid);
                return;
            }

            if (!r.Paid && (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId)))
            {
                CompleteSunRuOl(f, false);
                return;
            }

            switch (r.Stage)
            {
                case SunRuOlWeimianStage.Slots:
                case SunRuOlWeimianStage.Benefits:
                case SunRuOlWeimianStage.RestoreSlot:
                    PublishSunRuOlChoice(f);
                    return;
                case SunRuOlWeimianStage.CostChildren:
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.Benefits } });
                    continue;
                case SunRuOlWeimianStage.DiscardChildren:
                    IssueSunRuOlRefreshDraw(f);
                    continue;
                case SunRuOlWeimianStage.RestoreChildren:
                case SunRuOlWeimianStage.RecoveryChildren:
                case SunRuOlWeimianStage.DrawChildren:
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.Applying } });
                    continue;
                case SunRuOlWeimianStage.Applying:
                    var next = Enumerable.Range(r.NextBenefit, Math.Max(0, 4 - r.NextBenefit)).FirstOrDefault(i => (r.BenefitsMask & (1 << (i - 1))) != 0);
                    if (next == 0)
                    {
                        CompleteSunRuOl(f, true);
                        return;
                    }

                    var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                    if (next == 1)
                    {
                        if (SunRuOlSlots.Any(s => _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].EquipmentSlotCapacity(s) == 0))
                        {
                            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.RestoreSlot } });
                            PublishSunRuOlChoice(f);
                            return;
                        }

                        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunRuOlWeimian = r with { NextBenefit = 2 } });
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunRuOlWeimianBenefitIssuedEvent(id, r.RecipientSeat, 1, null, before, before));
                    }
                    else if (next == 2)
                    {
                        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.RecoveryChildren, NextBenefit = 3, SequenceBefore = before, SequenceAfter = before } });
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunRuOlWeimianBenefitIssuedEvent(id, r.RecipientSeat, 2, null, before, before));
                        if (_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat] is { HasHpValue: true } target && target.Hp < target.MaxHp)
                            _coreProgramLifecycleOperations.RecoverProgramTargetTo(id, f.OwnerSeat, r.RecipientSeat, SkillProgramNumberExpression.IntegerConstant, checked(target.Hp + 1), true);
                    }
                    else
                    {
                        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.DiscardChildren, NextBenefit = 4, SequenceBefore = before, SequenceAfter = before } });
                        var ids = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat]).Select(c => c.Id).ToArray();
                        if (ids.Length > 0)
                            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.RecipientSeat, ids, new(SunRuOlRefreshDiscardReason));
                        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunRuOlWeimian = f.SunRuOlWeimian!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunRuOlWeimianBenefitIssuedEvent(id, r.RecipientSeat, 3, null, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
                    }

                    continue;
                default:
                    throw new InvalidOperationException("Unknown Weimian stage.");
            }
        }
    }

    internal void IssueSunRuOlRefreshDraw(ProgramSkillFrame f)
    {
        var r = f.SunRuOlWeimian!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunRuOlWeimian = r with { Stage = SunRuOlWeimianStage.DrawChildren, SequenceBefore = before, SequenceAfter = before } });
        _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat], 4, true, new(SunRuOlRefreshDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunRuOlWeimian = f.SunRuOlWeimian!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal void CompleteSunRuOl(ProgramSkillFrame f, bool completed)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunRuOlChishi = null, SunRuOlWeimian = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26110)]
    internal bool ResumeSunRuOl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.SunRuOlChishi is null && f.SunRuOlWeimian is null)
            return false;
        ContinueSunRuOl(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26110)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26110)]
    internal bool CanContinuePaidSunRuOl(ProgramSkillFrame f) => f.SunRuOlChishi is { } c && _coreProgramEventHistoryOperations.ProgramEventHistory<SunRuOlChishiAppliedEvent>().Any(e => e.FrameId == f.Id && e.Source == c.Source) || f.SunRuOlWeimian is { Paid: true } r && _coreProgramEventHistoryOperations.ProgramEventHistory<SunRuOlWeimianPaidEvent>().Any(e => e.FrameId == f.Id && e.Source == r.Source);
    [ProgramAiChoice(SkillProgramEffectOp.AbolishEquipmentSlotsForDistinctBenefits)]
    internal PromptChoice ChooseAiSunRuOl(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => c.Parameters["branch"] == "slots" ? SunRuOlBitCount(int.Parse(c.Parameters["value"], CultureInfo.InvariantCulture)) : c.Parameters["branch"] == "restore" ? 10 : SunRuOlBenefitAiValue(f, int.Parse(c.Parameters["value"], CultureInfo.InvariantCulture))).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    internal int SunRuOlBenefitAiValue(ProgramSkillFrame f, int mask)
    {
        var t = _coreMatchParticipantsPlayersAccess._players[f.SunRuOlWeimian!.RecipientSeat];
        return ((mask & 1) != 0 && SunRuOlSlots.Any(s => t.EquipmentSlotCapacity(s) == 0) ? 3 : 0) + ((mask & 2) != 0 && t.HasHpValue && t.Hp < t.MaxHp ? 8 : 0) + ((mask & 4) != 0 ? 7 - _corePhysicalOwnedCardQueriesOperations.GetHand(t).Count : 0);
    }
}
