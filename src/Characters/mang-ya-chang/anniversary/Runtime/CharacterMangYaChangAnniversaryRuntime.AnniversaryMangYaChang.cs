#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMangYaChangAnniversaryRuntime
{
    internal const string JiedaoDiscardReason = "skill-program.anniversary:jiedao.discard";
    internal static ProgramTriggerCandidate JiedaoCandidate(ProgramSkillFrame f) => new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext?.OccurrenceIndex ?? 0);
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryJiedaoHost.Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginAnniversaryJiedao(frame, operation);
    internal SkillProgramStepOutcome BeginAnniversaryJiedao(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.AnniversaryJiedao is not null || f.WindowContext is not { } w)
            throw new InvalidOperationException("Jiedao requires its exact unpaid source choice or issued discard obligation.");
        if (operation == SkillProgramEffectOp.ChooseAnniversaryMangYaChangJiedao)
        {
            if (JiedaoSourceWitness(JiedaoCandidate(f), w)is not { } r || !FirstActualJiedaoSourceDamage(f.OwnerSeat))
                throw new InvalidOperationException("Jiedao cannot modify a later, sourceless or already finalized damage instance.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryJiedao = new(AnniversaryJiedaoStage.BonusChoice, r) });
        }
        else
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || w.TargetSeat is not { } target || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || JiedaoCostWitness(JiedaoCandidate(f), w)is not { } paid || paid.Grant is not { } grant || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryJiedaoCostBoundEvent>().Any(e => e.GrantFrameId == grant.FrameId))
                throw new InvalidOperationException("Jiedao can only pay once after its original native damage recipient survived.");
            var required = Math.Min(grant.Bonus, JiedaoDiscardableCards(f).Count);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryJiedao = paid with { Required = required } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryJiedaoCostBoundEvent(f.Id, grant.FrameId, paid.DamageFrameId!.Value));
            if (required == 0)
            {
                FinishAnniversaryJiedao(f);
                return SkillProgramStepOutcome.AwaitChild;
            }
        }

        PublishJiedaoChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<Card> JiedaoDiscardableCards(ProgramSkillFrame f) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(f.OwnerSeat)).Where(c => c.Id > 0 && (_cardZonesOperations._cardZones.Contains(CardLocation.Hand(f.OwnerSeat), c.Id) || _cardZonesOperations._cardZones.Contains(CardLocation.Equipment(f.OwnerSeat), c.Id)) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray());
    internal IReadOnlyList<PromptChoice> JiedaoChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryJiedao!;
        PromptChoice C(string option, string label, int? card = null) => new(new($"jiedao.{(f.Id)}.{(r.Stage)}.{(option)}.{(card)}.{(r.Selected.Count)}"), label, card is { } id ? [id] : [], [], new Dictionary<string, string> { ["program-action"] = "anniversary-jiedao", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
        if (r.Stage == AnniversaryJiedaoStage.BonusChoice)
            return Array.AsReadOnly(Enumerable.Range(0, checked(r.Origin.MaximumBonus + 1)).Select(n => C(n.ToString(CultureInfo.InvariantCulture), $"本次伤害+{(n)}")).ToArray());
        if (r.Stage != AnniversaryJiedaoStage.DiscardChoice)
            return[];
        var choices = JiedaoDiscardableCards(f).Where(c => !r.Selected.Contains(c.Id) && r.Selected.Count < r.Required).Select(c => C("pick", $"弃置【{(_coreSnapshotValueProjectionOperations.ToSnapshot(c).DisplayName)}】", c.Id)).ToList();
        choices.AddRange(r.Selected.Select(id => C("unpick", $"撤回【{(_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id)).DisplayName)}】", id)));
        if (r.Selected.Count == r.Required)
            choices.Add(C("finish", $"弃置选中的{(r.Required)}张牌"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishJiedaoChoice(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, JiedaoChoices(f), f.AnniversaryJiedao!.Stage == AnniversaryJiedaoStage.BonusChoice ? "截刀：选择本次来源伤害的增加值。" : $"截刀：原受伤角色未阵亡，须弃置{(f.AnniversaryJiedao.Required)}张牌。", f.OwnerSeat);
    }

    [ProgramAnswer("anniversary-jiedao")]
    internal void AnswerAnniversaryJiedao(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryJiedao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], JiedaoChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Jiedao lost its original private chooser or current exact choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == AnniversaryJiedaoStage.BonusChoice)
        {
            var bonus = int.Parse(choice.Parameters["option"], CultureInfo.InvariantCulture);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryJiedaoChosenEvent(new(f.Id, r.Origin, bonus)));
            FinishAnniversaryJiedao(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var option = choice.Parameters["option"];
        if (option != "finish")
        {
            var id = choice.Cards.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryJiedao = r with { Selected = option == "pick" ? r.Selected.Append(id).ToArray() : r.Selected.Where(c => c != id).ToArray() } });
            PublishJiedaoChoice(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (r.Selected.Count != r.Required || r.Selected.Any(id => !JiedaoDiscardableCards(f).Any(c => c.Id == id)))
            throw new InvalidOperationException("Jiedao's actual self-discard entities changed before payment.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryJiedao = r with { Stage = AnniversaryJiedaoStage.DiscardChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(JiedaoDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.AnniversaryJiedao!with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryJiedao = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryJiedaoCostPaidEvent(f.Id, r.Grant!.FrameId, r.Before, r.After, r.Selected.Count));
        ContinueAnniversaryJiedao(f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ContinueAnniversaryJiedao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryJiedao: { } r } f || f.Id != id)
            return;
        if (r.Stage != AnniversaryJiedaoStage.DiscardChildren)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishJiedaoChoice(f);
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        FinishAnniversaryJiedao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishAnniversaryJiedao(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryJiedao = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 44400)]
    internal bool ResumeAnniversaryJiedao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryJiedao: not null } f || f.Id != id)
            return false;
        ContinueAnniversaryJiedao(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 44400)]
    internal bool ReturnAnniversaryJiedaoMovement(ProgramSkillFrame f)
    {
        if (f.AnniversaryJiedao is null)
            return false;
        ContinueAnniversaryJiedao(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseAnniversaryMangYaChangJiedao, SkillProgramEffectOp.PayAnniversaryMangYaChangJiedao)]
    internal PromptChoice AiAnniversaryJiedao(PendingDecision prompt, ProgramSkillFrame f) => f.AnniversaryJiedao?.Stage == AnniversaryJiedaoStage.BonusChoice ? prompt.Choices.Last() : prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option") == "finish") ?? prompt.Choices.First();
}
