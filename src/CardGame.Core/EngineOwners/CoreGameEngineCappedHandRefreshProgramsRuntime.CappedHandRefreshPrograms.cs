#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCappedHandRefreshProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.ICappedHandRefreshProgramHost.DrawThenDiscardHandToMaximumHp(ProgramSkillFrame frame, int targetSeat, int cap) => BeginCappedHandRefresh(frame, targetSeat, cap);
    internal bool CappedHandRefreshSourceCurrent(ProgramSkillFrame frame)
    {
        if (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            return false;
        if (_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return true;
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } context || _resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.OwnerSeat != frame.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || _coreDeathProgramsOperations.GetCurrentDeathFrame(parent.DeathFrameId).VictimSeat != frame.OwnerSeat)
            return false;
        var candidate = parent.Candidates[parent.CandidateIndex];
        return candidate.OwnerSeat == frame.OwnerSeat && candidate.SkillId == frame.SkillId && candidate.SkillInstanceId == frame.SkillInstanceId && candidate.BindingId == frame.TriggerId;
    }

    internal SkillProgramStepOutcome BeginCappedHandRefresh(ProgramSkillFrame frame, int targetSeat, int cap)
    {
        if (frame.CappedHandRefresh is not null || frame.SelectedTargetSeats is not [var selected] || selected != targetSeat || frame.WindowContext?.Window is not (SkillProgramTriggerWindow.AfterDamageApplied or SkillProgramTriggerWindow.OwnerDied))
            throw new InvalidOperationException("Capped hand refresh lost its single selected target or exact rule window.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || !CappedHandRefreshSourceCurrent(frame))
            return SkillProgramStepOutcome.Continue;
        var maximum = Math.Min(cap, Math.Max(0, _coreMatchParticipantsPlayersAccess._players[targetSeat].MaxHp));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CappedHandRefresh = new(targetSeat, maximum, ProgramCappedHandRefreshStage.Drawing, 0, Array.AsReadOnly(Array.Empty<int>()), Array.AsReadOnly(Array.Empty<int>())) });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[targetSeat], maximum, true, new("program.capped-hand-refresh.draw"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCappedHandRefreshDrawnEvent(frame.Id, frame.OwnerSeat, targetSeat, maximum, drawn.Count));
        BeginCappedHandRefreshMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void BeginCappedHandRefreshMovement(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.CappedHandRefresh!.TargetSeat, 0, null) });
        if (!_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement) && !_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frameId);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 810)]
    internal bool ResumeCappedHandRefresh(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.CappedHandRefresh is not { } draft)
            return false;
        if (frame.PendingMovementContinuation is not null)
            return true;
        if (draft.Stage == ProgramCappedHandRefreshStage.Discarding)
        {
            FinishCappedHandRefresh(frame, completed: true);
            return true;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive || !CappedHandRefreshSourceCurrent(frame))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishCappedHandRefresh(frame, completed: false);
            return true;
        }

        if (draft.Stage == ProgramCappedHandRefreshStage.Selecting)
            return true;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]);
        var count = Math.Max(0, hand.Count - draft.Maximum);
        if (count == 0)
        {
            FinishCappedHandRefresh(frame, completed: true);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CappedHandRefresh = draft with { Stage = ProgramCappedHandRefreshStage.Selecting, DiscardCount = count, CandidateCardIds = Array.AsReadOnly(hand.Select(c => c.Id).ToArray()) } });
        PublishCappedHandRefreshDiscard(frame);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 500)]
    internal bool ReturnCappedHandRefreshMovement(ProgramSkillFrame frame)
    {
        if (frame.CappedHandRefresh is not { } draft)
            return false;
        if (draft.Stage is not (ProgramCappedHandRefreshStage.Drawing or ProgramCappedHandRefreshStage.Discarding) || frame.PendingMovementContinuation is not { } pending || pending.SubjectSeat != draft.TargetSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            throw new InvalidOperationException("Capped hand refresh lost its exact completed movement child.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    internal IReadOnlyList<PromptChoice> CappedHandRefreshChoices(ProgramSkillFrame frame)
    {
        var draft = frame.CappedHandRefresh!;
        return Array.AsReadOnly(draft.CandidateCardIds.Except(draft.SelectedCardIds).Select(id =>
        {
            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Single(c => c.Id == id);
            return new PromptChoice(new ChoiceId($"capped-hand.frame-{(frame.Id)}.pick-{(draft.SelectedCardIds.Count)}.card-{(id)}"), $"弃置【{(card.DisplayName)}】（{(card.RankText)}）", [id], [], new Dictionary<string, string> { ["program-action"] = "capped-hand-discard", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["selection-index"] = draft.SelectedCardIds.Count.ToString(CultureInfo.InvariantCulture) });
        }).ToArray());
    }

    internal void PublishCappedHandRefreshDiscard(ProgramSkillFrame frame)
    {
        var draft = frame.CappedHandRefresh!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.TargetSeat, $"【{(skill.Name)}】将手牌弃至 {(draft.Maximum)} 张，还需选择 {(draft.DiscardCount - draft.SelectedCardIds.Count)} 张。", draft.CandidateCardIds.Except(draft.SelectedCardIds).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.TargetSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name + " · 弃置手牌", skill.Description),
            Choices = CappedHandRefreshChoices(frame)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("capped-hand-discard")]
    internal void ResolveCappedHandRefreshChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Capped hand discard lost its frame.");
        var draft = frame.CappedHandRefresh ?? throw new InvalidOperationException("Capped hand discard lost its paid draw.");
        if (draft.Stage != ProgramCappedHandRefreshStage.Selecting || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != draft.TargetSeat || !prompt.Choices.Any(choice => choice.Id == selected.Id) || selected.Cards is not [var cardId] || selected.Targets.Count != 0 || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("selection-index") != draft.SelectedCardIds.Count.ToString(CultureInfo.InvariantCulture) || !draft.CandidateCardIds.Contains(cardId) || draft.SelectedCardIds.Contains(cardId))
            throw new InvalidOperationException("Capped hand discard changed its exact private selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive || !CappedHandRefreshSourceCurrent(frame) || draft.CandidateCardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(draft.TargetSeat)))
        {
            FinishCappedHandRefresh(frame, completed: false);
            return;
        }

        var ids = Array.AsReadOnly(draft.SelectedCardIds.Append(cardId).ToArray());
        draft = draft with
        {
            SelectedCardIds = ids
        };
        if (ids.Count < draft.DiscardCount)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CappedHandRefresh = draft });
            PublishCappedHandRefreshDiscard(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CappedHandRefresh = draft with { Stage = ProgramCappedHandRefreshStage.Discarding } });
        _corePhysicalMovementFacadesOperations.MoveCards(ids.Select(id => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Single(c => c.Id == id)).ToArray(), CardLocation.Hand(draft.TargetSeat), CardLocation.DiscardPile, new("program.capped-hand-refresh.discard"));
        BeginCappedHandRefreshMovement(frame.Id);
    }

    internal void FinishCappedHandRefresh(ProgramSkillFrame frame, bool completed)
    {
        var draft = frame.CappedHandRefresh!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { CappedHandRefresh = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCappedHandRefreshCompletedEvent(frame.Id, frame.OwnerSeat, draft.TargetSeat, draft.Maximum, draft.Stage == ProgramCappedHandRefreshStage.Discarding ? draft.DiscardCount : 0, completed));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void AssertCappedHandRefresh(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.CappedHandRefresh is not { } draft)
            return;
        if (paused.Op != SkillProgramEffectOp.DrawThenDiscardHandToMaximumHp || frame.SelectedTargetSeats is not [var target] || target != draft.TargetSeat || draft.Maximum < 0 || draft.Maximum > paused.Amount || draft.DiscardCount < 0 || !Enum.IsDefined(draft.Stage) || draft.CandidateCardIds.Distinct().Count() != draft.CandidateCardIds.Count || draft.SelectedCardIds.Distinct().Count() != draft.SelectedCardIds.Count || draft.SelectedCardIds.Any(id => !draft.CandidateCardIds.Contains(id)) || draft.SelectedCardIds.Count > draft.DiscardCount || draft.CandidateCardIds is not System.Collections.IList { IsReadOnly: true } || draft.SelectedCardIds is not System.Collections.IList { IsReadOnly: true })
            throw new InvalidOperationException("Capped hand refresh changed its frozen threshold or private owning draft.");
        if (ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && draft.Stage == ProgramCappedHandRefreshStage.Selecting && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != target || draft.DiscardCount <= draft.SelectedCardIds.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, CappedHandRefreshChoices(frame))))
            throw new InvalidOperationException("Capped hand refresh lost its target-owned private discard prompt.");
    }
}
