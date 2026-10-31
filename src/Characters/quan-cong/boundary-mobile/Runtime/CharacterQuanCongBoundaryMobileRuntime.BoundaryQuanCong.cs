#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQuanCongBoundaryMobileRuntime
{
    internal const string BoundaryYaomingAction = "boundary-quan-cong-yaoming";
    internal const string BoundaryYaomingDiscardReason = "skill-program.boundary:yaoming-mobile.discard";
    SkillProgramStepOutcome global::CardGame.Core.IBoundaryQuanCongProgramHost.Resolve(ProgramSkillFrame frame) => BeginBoundaryYaoming(frame);
    internal static CardConversionSource BoundaryYaomingSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal (int Remaining, string? Pending) BoundaryYaomingState(int owner, string instance)
    {
        var last = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryQuanCongYaomingStateEvent>().LastOrDefault(e => e.OwnerSeat == owner && e.SkillInstanceId == instance);
        return last is null ? (2, null) : (last.Remaining, last.PendingOption);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 51200, SkillProgramEffectOp.ResolveBoundaryQuanCongYaoming)]
    internal bool AllowsBoundaryYaoming(ProgramActivationLegalityContext gate) => _coreNationalSkillsOperations.GetSkillBindingShard(gate.Owner).ProgramInstances.Any(i => i.SkillId == gate.SkillId && BoundaryYaomingState(gate.OwnerSeat, i.SkillInstanceId).Remaining > 0);
    internal bool OriginalBoundaryYaoming(ProgramSkillFrame frame)
    {
        if (frame.SkillId != BoundaryQuanCongComposition.Skill || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0)
            return false;
        if (frame.TriggerId is null)
            return frame.ActivationId == BoundaryQuanCongComposition.Activation;
        return frame.TriggerId == BoundaryQuanCongComposition.Trigger && frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } window && window.TargetSeat == frame.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == window.ParentFrameId)is { } damage && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(frame, damage);
    }

    internal SkillProgramStepOutcome BeginBoundaryYaoming(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.BoundaryQuanCongYaoming is not null || !OriginalBoundaryYaoming(frame))
            throw new InvalidOperationException("Yaoming requires its original phase or actual received-damage frame.");
        var receipt = new BoundaryQuanCongYaomingReceipt(BoundaryYaomingSource(frame), frame.GameplayHash, frame.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressTurnNumberAccess._turnNumber, BoundaryQuanCongYaomingStage.Option);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryQuanCongYaoming = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryQuanCongYaomingStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.ParentWindowId, receipt.Turn));
        if (frame.TriggerId is not null)
        {
            var state = BoundaryYaomingState(frame.OwnerSeat, frame.SkillInstanceId);
            var amount = frame.WindowContext!.Amount;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryQuanCongYaomingStateEvent(frame.OwnerSeat, frame.SkillInstanceId, frame.Id, "damage", Math.Min(4, state.Remaining + amount), state.Pending, amount));
        }

        PublishBoundaryYaoming(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice BoundaryYaomingChoice(ProgramSkillFrame frame, string option, int target, string label) => new(new($"boundary-yaoming.{(frame.Id)}.{(option)}.{(target)}"), label, [], target < 0 ? [] : [target], new Dictionary<string, string> { ["program-action"] = BoundaryYaomingAction, ["option"] = option, ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> BoundaryYaomingChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryQuanCongYaoming!;
        if (receipt.Stage == BoundaryQuanCongYaomingStage.Card)
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, receipt.TargetSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(choice => choice with { Parameters = new Dictionary<string, string>(choice.Parameters) { ["program-action"] = BoundaryYaomingAction, ["option"] = "card" } }).ToArray());
        if (receipt.Stage != BoundaryQuanCongYaomingStage.Option)
            return[];
        var result = new List<PromptChoice>();
        if (BoundaryYaomingState(frame.OwnerSeat, frame.SkillInstanceId).Remaining > 0 && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            var own = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count;
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            {
                if (_corePhysicalOwnedCardQueriesOperations.GetHand(player).Count <= own)
                    result.Add(BoundaryYaomingChoice(frame, "draw", player.Seat, $"令 {(player.Name)} 摸一张牌"));
                if (player.Seat != frame.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count >= own && _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, player.Seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Count > 0)
                    result.Add(BoundaryYaomingChoice(frame, "discard", player.Seat, $"弃置 {(player.Name)} 的一张牌"));
            }
        }

        result.Add(BoundaryYaomingChoice(frame, "skip", -1, "不发动邀名"));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishBoundaryYaoming(ProgramSkillFrame frame)
    {
        var choices = BoundaryYaomingChoices(frame);
        if (choices.Count == 0 || choices.All(c => c.Parameters["option"] == "skip"))
        {
            CompleteBoundaryYaoming(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        var state = BoundaryYaomingState(frame.OwnerSeat, frame.SkillInstanceId);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var text = frame.BoundaryQuanCongYaoming!.Stage == BoundaryQuanCongYaomingStage.Card ? "邀名：弃置所选角色的一张手牌或装备牌。" : $"邀名：剩余 {(state.Remaining)}/4 次，上次记录：{((state.Pending == "draw" ? "摸牌" : state.Pending == "discard" ? "弃牌" : "无"))}。选择效果和角色。";
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, text, Array.AsReadOnly(choices.SelectMany(c => c.Cards).Distinct().ToArray()), Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryYaomingAction)]
    internal void AnswerBoundaryYaoming(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryQuanCongYaoming: { } receipt } frame || !ValidBoundaryYaoming(frame) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], BoundaryYaomingChoices(frame).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Yaoming requires its owning participant's current option, target or anonymous card slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        var option = answer.Parameters["option"];
        if (option == "skip")
        {
            CompleteBoundaryYaoming(frame);
            return;
        }

        if (receipt.Stage == BoundaryQuanCongYaomingStage.Option)
        {
            var state = BoundaryYaomingState(frame.OwnerSeat, frame.SkillInstanceId);
            if (state.Remaining <= 0)
                throw new InvalidOperationException("Yaoming has no remaining charge.");
            var switched = state.Pending is not null && state.Pending != option;
            var target = answer.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryQuanCongYaoming = receipt = receipt with { Option = option, TargetSeat = target, Stage = option == "draw" ? BoundaryQuanCongYaomingStage.Drawing : BoundaryQuanCongYaomingStage.Card, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryQuanCongYaomingSelectedEvent(frame.Id, option, target));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryQuanCongYaomingStateEvent(frame.OwnerSeat, frame.SkillInstanceId, frame.Id, "use", Math.Min(4, state.Remaining - 1 + (switched ? 1 : 0)), switched ? null : option));
            if (option == "draw")
            {
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, target, 1, new("skill-program.boundary:yaoming-mobile.draw"));
                return;
            }

            PublishBoundaryYaoming(frame);
            return;
        }

        var zone = Enum.Parse<CardZoneKind>(answer.Parameters["source-zone"]);
        var slot = int.Parse(answer.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = (zone == CardZoneKind.Hand ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat]) : _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat]))[slot];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryQuanCongYaoming = receipt with { Stage = BoundaryQuanCongYaomingStage.Children, DiscardedCardId = card.Id, Before = before, After = before }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, [card.Id], new(BoundaryYaomingDiscardReason), (_, _) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { BoundaryQuanCongYaoming = active.BoundaryQuanCongYaoming!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryQuanCongYaomingDiscardedEvent(frame.Id, receipt.TargetSeat, card.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        ContinueBoundaryYaoming(frame.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBoundaryQuanCongYaoming)]
    internal void ReturnBoundaryYaomingDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!ValidBoundaryYaoming(frame) || frame.BoundaryQuanCongYaoming is not { Stage: BoundaryQuanCongYaomingStage.Drawing } receipt || outcome.RecipientSeat != receipt.TargetSeat || outcome.RequestedCount != 1 || outcome.SequenceBefore < receipt.Before || outcome.SequenceAfter < outcome.SequenceBefore)
            throw new InvalidOperationException("Yaoming draw must return once to the original charged action and selected recipient.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryQuanCongYaoming = receipt with { Stage = BoundaryQuanCongYaomingStage.Children, DrawAttemptId = outcome.AttemptFrameId, After = outcome.SequenceAfter }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryQuanCongYaomingDrawReturnedEvent(frame.Id, receipt.TargetSeat, outcome.AttemptFrameId, outcome.ActualDrawIds.Count));
        ContinueBoundaryYaoming(frame.Id);
    }

    internal bool ValidBoundaryYaoming(ProgramSkillFrame frame) => frame.BoundaryQuanCongYaoming is { } receipt && OriginalBoundaryYaoming(frame) && receipt.Source == BoundaryYaomingSource(frame) && receipt.GameplayHash == frame.GameplayHash && receipt.ParentWindowId == (frame.WindowContext?.ParentFrameId ?? 0) && receipt.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryQuanCongYaomingStartedEvent>().Count(e => e.FrameId == frame.Id && e.Source == receipt.Source && e.GameplayHash == receipt.GameplayHash && e.ParentWindowId == receipt.ParentWindowId && e.Turn == receipt.Turn) == 1;
    [ProgramContinuation(ProgramContinuationKind.Resume, 51200)]
    internal bool ContinueBoundaryYaoming(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryQuanCongYaoming: { } receipt } frame || frame.Id != id || receipt.Stage == BoundaryQuanCongYaomingStage.Complete)
            return false;
        if (!ValidBoundaryYaoming(frame))
            throw new InvalidOperationException("Yaoming lost its owning charged action or native return.");
        if (receipt.Stage is BoundaryQuanCongYaomingStage.Option or BoundaryQuanCongYaomingStage.Card)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBoundaryYaoming(frame);
            return true;
        }

        if (receipt.Stage == BoundaryQuanCongYaomingStage.Drawing)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        CompleteBoundaryYaoming(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 51200)]
    internal bool ReturnBoundaryYaomingMovement(ProgramSkillFrame frame) => frame.BoundaryQuanCongYaoming is not null && ContinueBoundaryYaoming(frame.Id);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 51200)]
    internal bool IsBoundaryYaomingPaidMovement(ProgramSkillFrame frame, SkillProgramEffect? paidEffect, ProgramMovementContinuation pending)
    {
        if (paidEffect?.Op != SkillProgramEffectOp.ResolveBoundaryQuanCongYaoming || frame.PendingMovementContinuation != pending || pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null || frame.BoundaryQuanCongYaoming is not { Stage: BoundaryQuanCongYaomingStage.Children } receipt || !ValidBoundaryYaoming(frame) || receipt.Before > receipt.After || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryQuanCongYaomingSelectedEvent>().Count(e => e.FrameId == frame.Id && e.Option == receipt.Option && e.TargetSeat == receipt.TargetSeat) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryQuanCongYaomingStateEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat && e.SkillInstanceId == frame.SkillInstanceId && e.Cause == "use") != 1)
            return false;
        if (receipt.Option == "discard" && receipt.DiscardedCardId is { } cardId)
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryQuanCongYaomingDiscardedEvent>().Count(e => e.FrameId == frame.Id && e.TargetSeat == receipt.TargetSeat && e.CardId == cardId && e.Before == receipt.Before && e.After == receipt.After) != 1)
                return false;
            var invoice = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().SingleOrDefault(e => e.ParentFrameId == frame.Id && e.Reason.Value == BoundaryYaomingDiscardReason && e.InitiatorSeat == frame.OwnerSeat && e.ExecutorSeat == frame.OwnerSeat && e.SubjectSeat == receipt.TargetSeat && e.ActualCount == 1 && e.SequenceBefore == receipt.Before && e.SequenceAfter == receipt.After);
            return invoice is not null && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(material => material.InvocationId == invoice.InvocationId && material.CardId == cardId && material.From.OwnerSeat == receipt.TargetSeat && material.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == material.CompletionSequence && move.CardId == cardId && move.Sequence > receipt.Before && move.Sequence <= receipt.After && move.From == material.From && move.To == material.To && move.Reason.Value == BoundaryYaomingDiscardReason));
        }

        if (receipt.Option != "draw" || receipt.DiscardedCardId is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryQuanCongYaomingDrawReturnedEvent>().Count(e => e.FrameId == frame.Id && e.RecipientSeat == receipt.TargetSeat && e.AttemptId == receipt.DrawAttemptId) != 1)
            return false;
        return receipt.DrawAttemptId is not { } attempt || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptStartedEvent>().Count(e => e.Context.AttemptFrameId == attempt && e.Context.ParentFrameId == frame.Id && e.Context.RecipientSeat == receipt.TargetSeat && e.Context.RequestedCount == 1 && e.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation && e.Return.InstructionIndex == frame.InstructionIndex) == 1;
    }

    internal void CompleteBoundaryYaoming(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryQuanCongYaoming = frame.BoundaryQuanCongYaoming!with { Stage = BoundaryQuanCongYaomingStage.Complete }, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 51200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 51200)]
    internal bool KeepPaidBoundaryYaoming(ProgramSkillFrame frame) => frame.BoundaryQuanCongYaoming is { Option: not null } && ValidBoundaryYaoming(frame);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 51200)]
    internal void AssertBoundaryYaomingFrame(ProgramSkillFrame frame)
    {
        if (frame.BoundaryQuanCongYaoming is not null && !ValidBoundaryYaoming(frame))
            throw new InvalidOperationException("Yaoming changed its original charge, damage parent or frame identity.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryQuanCongYaoming)]
    internal PromptChoice BoundaryYaomingAi(PendingDecision pending, ProgramSkillFrame frame)
    {
        var state = BoundaryYaomingState(frame.OwnerSeat, frame.SkillInstanceId);
        return pending.Choices.OrderByDescending(choice =>
        {
            var option = choice.Parameters["option"];
            if (option == "skip")
                return -0.5;
            if (option == "card")
                return choice.Cards.Count == 1 ? CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards[0]).Kind).HandKeepValue : 5;
            var score = _coreAiBrainsAiBrainsAccess._aiBrains[frame.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(frame.OwnerSeat), choice.Targets.Single(), option == "draw" ? new(0, 0, 0, 1, 0, 0, false, false) : new(1, 0, 0, 0, 0, 0, false, false));
            return score + (state.Pending is not null && state.Pending != option ? 0.2 : 0);
        }).First();
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> BoundaryQuanCongPublicStates(CharacterState owner, string skillId)
    {
        if (skillId != BoundaryQuanCongComposition.Skill)
            return[];
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId).SelectMany(instance =>
        {
            var state = BoundaryYaomingState(owner.Seat, instance.SkillInstanceId);
            return new[]
            {
                new ProgramPublicRuleStateSnapshot(instance.SkillInstanceId, "yaoming-remaining", ProgramPublicRuleStateKind.GameActivationProgress, state.Remaining),
                new ProgramPublicRuleStateSnapshot(instance.SkillInstanceId, "yaoming-pending-option", ProgramPublicRuleStateKind.GameActivationProgress, state.Pending == "discard" ? 1 : state.Pending == "draw" ? 2 : 0)
            };
        }).ToArray();
    }
}
