#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuJinDingOlRuntime
{
    internal const string QingyuanTakeReason = "skill-program.hu-jin-ding.qingyuan.take";
    SkillProgramStepOutcome global::CardGame.Core.IHuJinDingProgramHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginQingyuan(f, op);
    internal IReadOnlyList<int> QingyuanNewPartnerChoices(int owner, string skill, string instance) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && !QingyuanPartners(owner, skill, instance).Any(mark => mark.PartnerSeat == p.Seat)).Select(p => p.Seat).ToArray());
    internal int[] QingyuanTakeHolders(int owner, string skill, string instance) => QingyuanPartners(owner, skill, instance).Where(IsLiveQingyuanPartner).Select(p => p.PartnerSeat).Distinct().Where(s => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count > 0).Order().ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26820)]
    internal bool CanOfferHuJinDing(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [{ } e] || !HuJinDingComposition.Handles(e.Op))
            return true;
        if (e.Op == SkillProgramEffectOp.TakeRandomQingyuanHandAfterGain)
            return QingyuanGainQualification(c, context)is not null && QingyuanTakeHolders(c.OwnerSeat, c.SkillId, c.SkillInstanceId).Length > 0;
        if (QingyuanNewPartnerChoices(c.OwnerSeat, c.SkillId, c.SkillInstanceId).Count == 0)
            return false;
        var second = e.Op == SkillProgramEffectOp.SelectQingyuanFirstDamagePartner;
        if (QingyuanPartners(c.OwnerSeat, c.SkillId, c.SkillInstanceId).Any(p => p.FirstDamage == second))
            return false;
        return second ? context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == c.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanFirstDamageEvent>().Any(d => d.OwnerSeat == c.OwnerSeat && d.DamageFrameId == context.DamageFrameId && d.Amount == context.Amount) : context.Window == SkillProgramTriggerWindow.GameStarting;
    }

    internal SkillProgramStepOutcome BeginQingyuan(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Qingyuan is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanOfferHuJinDing(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Qingyuan requires its exact unissued lifecycle or qualified hand-gain candidate.");
        var receipt = new QingyuanReceipt(HuJinDingSource(f), f.GameplayHash, op, f.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressTurnNumberAccess._turnNumber, QingyuanStage.ChoosingPartner, Damage: op == SkillProgramEffectOp.SelectQingyuanFirstDamagePartner ? _coreProgramEventHistoryOperations.ProgramEventHistory<QingyuanFirstDamageEvent>().Single(d => d.OwnerSeat == f.OwnerSeat) : null);
        if (op == SkillProgramEffectOp.TakeRandomQingyuanHandAfterGain)
            receipt = receipt with
            {
                BatchId = context.MovementBatch!.Id,
                MovementIndex = context.MovementIndex!.Value
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingyuan = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingyuanStartedEvent(f.Id, receipt.Source, receipt.GameplayHash, op, receipt.ParentId, receipt.ActualTurn, receipt.BatchId, receipt.MovementIndex));
        if (op == SkillProgramEffectOp.TakeRandomQingyuanHandAfterGain)
        {
            PayQingyuanTake(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishQingyuanPartner(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> QingyuanPartnerChoices(ProgramSkillFrame f) => Array.AsReadOnly(QingyuanNewPartnerChoices(f.OwnerSeat, f.SkillId, f.SkillInstanceId).Select(s => new PromptChoice(new($"qingyuan.{(f.Id)}.partner.{(s)}"), $"选择{(_coreMatchParticipantsPlayersAccess._players[s].Name)}为轻缘角色", [], [s], new Dictionary<string, string> { ["program-action"] = "qingyuan-partner" })).ToArray());
    internal void PublishQingyuanPartner(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = QingyuanPartnerChoices(f);
        if (choices.Count == 0)
        {
            FinishQingyuan(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.Qingyuan!.Damage is null ? "轻缘：选择一名其他角色。" : "轻缘：首次受到真实伤害后，选择另一名未被选择的其他角色。", [], choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("qingyuan-partner")]
    internal void AnswerQingyuanPartner(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Qingyuan: { Stage: QingyuanStage.ChoosingPartner } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !QingyuanPartnerChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Qingyuan lost its exact mandatory public partner choice.");
        AssertHuJinDing(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var partner = choice.Targets.Single();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingyuanPartnerSelectedEvent(f.Id, r.Source, r.GameplayHash, partner, r.Damage is not null, r.ParentId, r.Damage?.DamageFrameId, r.Damage?.Amount ?? 0, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        SynchronizeQingyuanPublicMarkers();
        FinishQingyuan(f);
    }

    internal void PayQingyuanTake(ProgramSkillFrame f)
    {
        var r = f.Qingyuan!;
        var candidates = QingyuanTakeHolders(f.OwnerSeat, f.SkillId, f.SkillInstanceId);
        if (candidates.Length == 0)
            throw new InvalidOperationException("A Qingyuan take lost all actual nonempty marked hands before its random payment.");
        var holderIndex = _randomOperations._random.Next(candidates.Length);
        var holder = candidates[holderIndex];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[holder]).ToArray();
        var cardIndex = _randomOperations._random.Next(hand.Length);
        var card = hand[cardIndex];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingyuan = r with { Stage = QingyuanStage.TakeChildren, HolderCandidates = candidates, HandCandidates = hand.Select(c => c.Id).ToArray(), HolderRandomIndex = holderIndex, CardRandomIndex = cardIndex, HolderSeat = holder, CardId = card.Id, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(holder), CardLocation.Hand(f.OwnerSeat), new(QingyuanTakeReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingyuan = f.Qingyuan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingyuanTakePaidEvent(f.Id, holder, card.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, r.ActualTurn));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainHuJinDingChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 26820)]
    internal bool ResumeQingyuan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Qingyuan: { } r } f || f.Id != id)
            return false;
        AssertHuJinDing(f);
        if (r.Stage == QingyuanStage.ChoosingPartner)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishQingyuanPartner(f);
            return true;
        }

        if (!DrainHuJinDingChildren(id))
            FinishQingyuan(f);
        return true;
    }

    internal void FinishQingyuan(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QingyuanCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Qingyuan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26820)]
    internal bool ReturnHuJinDingMovement(ProgramSkillFrame f) => f.Qingyuan is not null && ResumeQingyuan(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.SelectQingyuanOpeningPartner, SkillProgramEffectOp.SelectQingyuanFirstDamagePartner)]
    internal PromptChoice AiQingyuan(PendingDecision prompt, ProgramSkillFrame f) => prompt.Choices.OrderByDescending(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets[0]]).Count).First();
}
