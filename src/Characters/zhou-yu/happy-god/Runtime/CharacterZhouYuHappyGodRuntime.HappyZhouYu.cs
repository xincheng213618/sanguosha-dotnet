#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuHappyGodRuntime
{
    internal const string HappyQinyin = "happy:qinyin";
    internal static CardConversionSource HappyZySource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal (NativeLogicalDiscardInvocationRecordedEvent Invocation, NativeLogicalDiscardMaterialCompletedEvent Material)? HappyZyDiscard(int owner)
    {
        foreach (var invocation in _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActualTurnOwnerSeat == owner && e.ExecutorSeat == owner && e.HandCount > 0))
            foreach (var material in _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == invocation.InvocationId && e.From == CardLocation.Hand(owner) && e.To == CardLocation.DiscardPile))
                if (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == material.CompletionSequence && m.CardId == material.CardId && m.To == material.To && (m.From == material.From || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == material.From)))
                    return (invocation, material);
        return null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 41440, SkillProgramEffectOp.ResolveHappyZhouYuQinyin)]
    internal bool CanOfferHappyZy(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => c.SkillId == HappyQinyin && w.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Discard && HappyZyDiscard(c.OwnerSeat)is not null;
    SkillProgramStepOutcome global::CardGame.Core.IHappyZhouYuHost.Begin(ProgramSkillFrame f) => BeginHappyZy(f);
    internal SkillProgramStepOutcome BeginHappyZy(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyZhouYu is not null || f.InstructionIndex != 1 || f.SkillId != HappyQinyin || f.TriggerId != "whole-turn-discard" || f.WindowContext?.Window != SkillProgramTriggerWindow.DiscardPhaseEnded || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Discard || HappyZyDiscard(f.OwnerSeat)is not { } discard)
            throw new InvalidOperationException("Happy Qinyin requires its real own-discard ending and an actually completed own-hand discard in this turn.");
        var r = new HappyZyReceipt(HappyZySource(f), f.GameplayHash, f.WindowContext.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, discard.Invocation.InvocationId, discard.Material.CardId)
        {
            Stage = HappyZyStage.Choosing,
            Seats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhouYu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZyStartedEvent(f.Id, r.Source, r.Hash, r.Parent, r.Turn, r.DiscardInvocation, r.Seats.Count));
        for (var i = 0; i < r.Seats.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZyParticipantEvent(f.Id, i, r.Seats[i]));
        PublishHappyZy(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HappyZyChoices(ProgramSkillFrame f) => new[]
    {
        ("heal", "令所有角色各回复1点体力"),
        ("lose", "令所有角色各失去1点体力"),
        ("draw", "令所有角色各摸一张牌")
    }.Select(x => new PromptChoice(new("happy-qinyin." + f.Id + "." + x.Item1), x.Item2, [], [], new Dictionary<string, string> { { "program-action", "happy-qinyin" }, { "mode", x.Item1 } })).ToArray();
    internal void PublishHappyZy(ProgramSkillFrame f)
    {
        var choices = HappyZyChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "琴音：选择全场结算方式。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("happy-qinyin")]
    internal void AnswerHappyZy(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyZhouYu: { Stage: HappyZyStage.Choosing } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !HappyZyChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Happy Qinyin requires its exact original three-way choice.");
        RequireHappyZy(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var mode = choice.Parameters["mode"];
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.ResolvingEffect, HappyZhouYu = r with { Mode = mode, Stage = HappyZyStage.Ready } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZyModeEvent(f.Id, mode));
        ResumeHappyZy(f.Id);
    }

    internal void IssueHappyZy(ProgramSkillFrame f, bool skipped, HappyZyStage next)
    {
        var r = f.HappyZhouYu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhouYu = r with { Cursor = r.Cursor + 1, Stage = next, Draw = null } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZyIssuedEvent(f.Id, r.Cursor + 1, r.Target, r.Mode!, skipped));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 41440)]
    internal void CaptureHappyZyHp(ProgramSkillHpLostEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HappyZhouYu: { Stage: HappyZyStage.HpLoss, Mode: "lose" } r } f && fact.FrameId == f.Id && fact.SkillId == f.SkillId && fact.TargetSeat == r.Target && fact.Amount == 1)
            IssueHappyZy(f, false, HappyZyStage.Children);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 41441)]
    internal void CaptureHappyZyPrevented(IdentityHpDeductionPreventedEvent fact)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is IdentityAppearanceFrame appearance && appearance.Id == fact.AppearanceFrameId && fact.Entry == IdentityAppearanceEntry.ProgramHpLoss && fact.RequestedDeduction == 1 && _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == appearance.Receipt.ParentFrameId)is ProgramSkillFrame { HappyZhouYu: { Stage: HappyZyStage.HpLoss, Mode: "lose" } r } f && r.Target == fact.OwnerSeat && appearance.Receipt.OriginalInstructionIndex == f.InstructionIndex)
        {
            var updated = f with
            {
                HappyZhouYu = r with
                {
                    Cursor = r.Cursor + 1,
                    Stage = HappyZyStage.Children
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, updated);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZyIssuedEvent(f.Id, r.Cursor + 1, r.Target, "lose", true));
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 41440)]
    internal bool ResumeHappyZy(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyZhouYu: not null } root || root.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HappyZhouYu: { } r } f && f.Id == id)
        {
            RequireHappyZy(f);
            if (r.Stage == HappyZyStage.Choosing)
            {
                PublishHappyZy(f);
                return true;
            }

            if (r.Stage == HappyZyStage.Drawing)
                return true;
            if (r.Stage == HappyZyStage.Children)
            {
                if (DrainHappyZy(f))
                    return true;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.HappyZhouYu!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, HappyZhouYu = r with { Stage = HappyZyStage.Ready } });
                continue;
            }

            if (r.Stage == HappyZyStage.HpLoss)
            {
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, r.Target, 1);
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || _coreProgramLifecycleOperations.GetActiveProgramFrame(id).HappyZhouYu!.Stage == HappyZyStage.HpLoss)
                    return true;
                continue;
            }

            if (r.Cursor >= r.Seats.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhouYu = r with { Stage = HappyZyStage.Finished } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZyCompletedEvent(f.Id));
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return true;
            }

            var target = r.Seats[r.Cursor];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhouYu = r = r with { Target = target } });
            if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || r.Mode != "draw" && !_coreMatchParticipantsPlayersAccess._players[target].HasHpValue)
            {
                IssueHappyZy(f, true, HappyZyStage.Children);
                continue;
            }

            if (r.Mode == "lose")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhouYu = r with { Stage = HappyZyStage.HpLoss } });
                continue;
            }

            IssueHappyZy(f, false, r.Mode == "draw" ? HappyZyStage.Drawing : HappyZyStage.Children);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            if (r.Mode == "draw")
            {
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, 1, new("skill.happy-qinyin.draw"));
                return true;
            }

            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(id, f.OwnerSeat, target, 1, null, null);
        }

        return true;
    }

    internal bool DrainHappyZy(ProgramSkillFrame f)
    {
        var continuation = f.HappyZhouYu!.Mode == "draw" ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program;
        return _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, continuation) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, continuation) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyZhouYuQinyin)]
    internal void ReturnHappyZyDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.HappyZhouYu is not { Stage: HappyZyStage.Drawing, Mode: "draw", Draw: null } r || outcome.RecipientSeat != r.Target || outcome.RequestedCount != 1 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZyDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Ordinal == r.Cursor))
            throw new InvalidOperationException("Happy Qinyin lost its exact once-issued one-card native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhouYu = r with { Stage = HappyZyStage.Children, Draw = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZyDrawReturnedEvent(f.Id, r.Cursor, r.Target, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ResumeHappyZy(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 41440)]
    internal bool ReturnHappyZyMovement(ProgramSkillFrame f) => f.HappyZhouYu is not null && ResumeHappyZy(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41440)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41440)]
    internal bool RetainIssuedHappyZy(ProgramSkillFrame f) => f.HappyZhouYu is { Mode: not null };
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyZhouYuQinyin)]
    internal PromptChoice ChooseHappyZy(PendingDecision p, ProgramSkillFrame f) => p.Choices.Last();
}
