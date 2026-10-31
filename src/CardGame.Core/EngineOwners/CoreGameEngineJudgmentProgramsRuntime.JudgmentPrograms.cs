#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJudgmentProgramsRuntime
{
    internal bool TryBeginProgramJudgmentWindow(JudgmentFrame pending, Card judgmentCard, Suit effectiveSuit, bool succeeded)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat].IsAlive)
            return false;
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).OrderBy(player => _coreTableOrderOperations.RelativeTableOrder(0, player.Seat)).SelectMany(owner => _coreSkillProgramsOperations.EnabledUniqueProgramTriggers(owner, SkillProgramTriggerWindow.JudgmentFinalized).OrderBy(binding => binding.SkillId, StringComparer.Ordinal).ThenBy(binding => binding.Trigger.Id, StringComparer.Ordinal).Where(binding => MatchesFinalJudgment(binding.Trigger, owner, pending, judgmentCard, effectiveSuit)).Select(binding => new ProgramJudgmentTriggerCandidate(owner.Seat, binding.Program.Id, binding.Trigger.Id, binding.SkillInstanceId, binding.Program.GameplayHash))).ToArray();
        candidates = _characterZhangFeiAnniversaryGodOperations.PrependShenZfJudgmentGain(pending, candidates);
        if (candidates.Length == 0)
            return false;
        var context = new JudgmentFinalizedContext(pending.Id, pending.TargetSeat, pending.Reason, judgmentCard.Id, judgmentCard.Kind, effectiveSuit, _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentRank(pending, judgmentCard.Rank), succeeded, pending.SourceSeat);
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramJudgmentTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, pending.Id, context, Array.AsReadOnly(candidates)));
        return true;
    }

    internal bool MatchesFinalJudgment(SkillProgramTrigger trigger, CharacterState owner, JudgmentFrame pending, Card judgmentCard, Suit effectiveSuit) => trigger.Window == SkillProgramTriggerWindow.JudgmentFinalized && (trigger.Subject == SkillProgramTriggerSubject.Any || pending.TargetSeat == owner.Seat) && (trigger.JudgmentSource is null || trigger.JudgmentSource == SkillProgramTriggerSubject.Any || pending.SourceSeat == owner.Seat) && (trigger.JudgmentReasons.Count == 0 || trigger.JudgmentReasons.Contains(pending.Reason, StringComparer.Ordinal)) && trigger.Suits.Contains(effectiveSuit) && _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentRank(pending, judgmentCard.Rank) >= trigger.MinimumRank && _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentRank(pending, judgmentCard.Rank) <= trigger.MaximumRank && !trigger.ExcludedReasons.Contains(pending.Reason, StringComparer.Ordinal) && CanStartFinalJudgmentEffects(owner, trigger) && (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ClaimJudgmentCard) || _cardZonesOperations._cardZones.GetLocation(judgmentCard.Id) == CardLocation.Judgment(pending.TargetSeat));
    internal bool CanRunProgramJudgmentTrigger(ProgramJudgmentTriggerCandidate candidate, SkillProgramTrigger trigger, JudgmentFinalizedContext judgment)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        return owner.IsAlive && trigger.Window == _coreZhenyiJudgmentWindowsOperations.CurrentZhenyiJudgmentWindowKind(judgment.JudgmentFrameId) && (trigger.Subject == SkillProgramTriggerSubject.Any || judgment.SubjectSeat == owner.Seat) && (trigger.JudgmentSource is null || trigger.JudgmentSource == SkillProgramTriggerSubject.Any || judgment.SourceSeat == owner.Seat) && (trigger.JudgmentReasons.Count == 0 || trigger.JudgmentReasons.Contains(judgment.Reason, StringComparer.Ordinal)) && trigger.Suits.Contains(judgment.Suit) && judgment.Rank >= trigger.MinimumRank && judgment.Rank <= trigger.MaximumRank && !trigger.ExcludedReasons.Contains(judgment.Reason, StringComparer.Ordinal) && CanStartFinalJudgmentEffects(owner, trigger) && (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ClaimJudgmentCard) || _cardZonesOperations._cardZones.GetLocation(judgment.CardId) == CardLocation.Judgment(judgment.SubjectSeat));
    }

    internal bool CanStartFinalJudgmentEffects(CharacterState owner, SkillProgramTrigger trigger)
    {
        var first = trigger.Effects.FirstOrDefault(effect => effect.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner)));
        if (first is null)
            return false;
        return first.Op != SkillProgramEffectOp.SelectTarget || first.TargetKind is { } kind && _coreProgramLifecycleOperations.GetProgramTargetSeats(owner.Seat, kind).Count > 0;
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.BeforeJudgmentDecision, 0, NativeFrameAutomaticPending.RequireNoneAfterFrame, NativeFrameAutomaticPublish.None)]
    [NativeFrameRoute(typeof(ProgramJudgmentTriggerWindowFrame), 440)]
    internal void ContinueProgramJudgmentWindowCore()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramJudgmentTriggerWindowFrame frame)
        {
            var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment ?? throw new InvalidOperationException("Missing judgment trigger continuation.");
            if (frame.ZhenyiBeforeResult == true)
            {
                frame = _coreZhenyiJudgmentWindowsOperations.RefreshZhenyiBeforeResultWindow(frame, pending);
                if (frame.CandidateIndex == frame.Candidates.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    _coreZhenyiJudgmentWindowsOperations.CompleteZhenyiBeforeResultWindow(frame, pending);
                    return;
                }
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                ShortCircuitProgramJudgmentWindow(frame, pending);
                return;
            }

            if (frame.CandidateIndex == frame.Candidates.Count)
            {
                var succeeded = pending.Succeeded ?? throw new InvalidOperationException("A final judgment trigger lost its result.");
                var judgmentCard = _coreNativeDamageHandlesOperations.GetJudgmentCard(pending) ?? throw new InvalidOperationException("A final judgment trigger lost its card.");
                _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramJudgmentTriggerWindow);
                var completed = _coreNativeJudgmentOperations.CompleteFinalizedJudgment(pending, judgmentCard, succeeded);
                if (completed is { } result)
                    _coreNativeJudgmentOperations.ResumeCompletedJudgment(new global::CardGame.Core.GameEngine.JudgmentCompletionReceipt(pending, result));
                return;
            }

            var candidate = frame.Candidates[frame.CandidateIndex];
            var program = _contentRegistryOperations._contentRegistry!.Skills[candidate.SkillId].Program!;
            if (program.GameplayHash != candidate.GameplayHash)
                throw new InvalidOperationException("A running judgment trigger definition changed.");
            var trigger = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Trigger, candidate.TriggerId).Trigger!;
            var shared = new ProgramTriggerCandidate(candidate.OwnerSeat, candidate.SkillId, candidate.TriggerId, candidate.SkillInstanceId, candidate.GameplayHash, trigger.Priority);
            var context = CreateFinalizedJudgmentProgramContext(frame, candidate);
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(shared, context))
            {
                // This opportunity belongs to the frozen grant. Another grant of the
                // same skill cannot inherit it after the original instance expires.
                AdvanceProgramJudgmentCandidate(frame with { Activated = false });
                continue;
            }

            if (!frame.Activated)
            {
                if (trigger.Optional)
                {
                    ExposeProgramJudgmentPrompt(frame, candidate);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Activated = true });
                continue;
            }

            _coreProgramLifecycleOperations.BeginProgramBinding(shared, context);
            return;
        }
    }

    internal ProgramSkillWindowContext CreateFinalizedJudgmentProgramContext(ProgramJudgmentTriggerWindowFrame frame, ProgramJudgmentTriggerCandidate candidate) => new(frame.ZhenyiBeforeResult == true ? SkillProgramTriggerWindow.JudgmentBeforeResult : SkillProgramTriggerWindow.JudgmentFinalized, frame.Id, candidate.OwnerSeat, SourceSeat: frame.Judgment.SourceSeat, TargetSeat: frame.Judgment.SubjectSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), Judgment: frame.Judgment);
    internal void ShortCircuitProgramJudgmentWindow(ProgramJudgmentTriggerWindowFrame frame, JudgmentFrame pending)
    {
        if (frame.CandidateIndex < frame.Candidates.Count && frame.Activated)
        {
            var candidate = frame.Candidates[frame.CandidateIndex];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentTriggerResolvedEvent(frame.Id, frame.Judgment.JudgmentFrameId, candidate.SkillId, candidate.TriggerId, candidate.OwnerSeat, Activated: true));
        }

        var succeeded = pending.Succeeded ?? throw new InvalidOperationException("A terminal judgment trigger lost its result.");
        var judgmentCard = _coreNativeDamageHandlesOperations.GetJudgmentCard(pending) ?? throw new InvalidOperationException("A terminal judgment trigger lost its card.");
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramJudgmentTriggerWindow);
        var completed = _coreNativeJudgmentOperations.CompleteFinalizedJudgment(pending, judgmentCard, succeeded);
        if (completed is { } result)
        {
            _coreNativeJudgmentOperations.ResumeCompletedJudgment(new global::CardGame.Core.GameEngine.JudgmentCompletionReceipt(pending, result));
        }
    }

    internal void AdvanceProgramJudgmentCandidate(ProgramJudgmentTriggerWindowFrame frame) => _programWindowCursorOperations.Get().AdvanceProgramJudgmentCandidate(frame);
    internal void ExposeProgramJudgmentPrompt(ProgramJudgmentTriggerWindowFrame frame, ProgramJudgmentTriggerCandidate candidate)
    {
        var skill = _contentRegistryOperations._contentRegistry!.Skills[candidate.SkillId];
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramJudgmentTrigger, candidate.OwnerSeat, frame.ZhenyiBeforeResult == true ? $"【{(skill.Name)}】：可在{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(frame.Judgment.Suit))} {(frame.Judgment.Rank)} 的判定结果生效前修改。" : $"【{(skill.Name)}】：{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(frame.Judgment.Suit))} {(frame.Judgment.Rank)} 的判定结果已生效。", [], [], SourceSeat: candidate.OwnerSeat, IncomingCard: frame.Judgment.CardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.Judgment.SubjectSeat,
            Choices = [ProgramJudgmentChoice("activate", "发动技能"), ProgramJudgmentChoice("skip", "不发动")]
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal static PromptChoice ProgramJudgmentChoice(string action, string label) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_JudgmentPrograms.ProgramJudgmentChoice(action, label);
    internal CommandResult SubmitProgramJudgmentTriggerAnswer(int actorSeat, PromptId prompt, ChoiceId choice)
    {
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(actorSeat, DecisionKind.ProgramJudgmentTrigger, prompt, CommandErrorCode.IllegalAction);
        if (error is not null)
            return _kernelCoreMainOperations.Reject(error.Code, error.Message);
        var selected = _corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.SingleOrDefault(item => item.Id == choice);
        if (selected is null)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The judgment trigger choice is not available.");
        return _kernelCoreMainOperations.Accept(() =>
        {
            ResolveProgramJudgmentChoice(selected);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void ResolveProgramJudgmentChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramJudgmentTriggerWindowFrame ?? throw new InvalidOperationException("Missing judgment trigger frame.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (selected.Parameters["action"])
        {
            case "program-judgment-trigger-skip":
                AdvanceProgramJudgmentCandidate(frame);
                break;
            case "program-judgment-trigger-activate":
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Activated = true });
                break;
            default:
                throw new InvalidOperationException("Unsupported judgment trigger choice.");
        }

        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramJudgmentTriggerWindowFrame>();
    }

    internal bool IsAiProgramJudgmentPending() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramJudgmentTrigger, PlayerSeat: var playerSeat } && !_coreMatchParticipantsPlayersAccess._players[playerSeat].IsHuman;
    internal void ResolvePendingAiProgramJudgment()
    {
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("AI judgment program prompt is missing.");
        if (_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].IsHuman)
            throw new InvalidOperationException("A human judgment program prompt cannot use the AI route.");
        if (decision.Kind != DecisionKind.ProgramJudgmentTrigger)
            throw new InvalidOperationException("Unsupported AI judgment program prompt.");
        ResolveProgramJudgmentChoice(decision.Choices[0]);
    }

    internal bool IsProgramJudgmentPromptValid(ProgramJudgmentTriggerWindowFrame frame)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } decision || frame.CandidateIndex < 0 || frame.CandidateIndex >= frame.Candidates.Count || decision.PlayerSeat != frame.Candidates[frame.CandidateIndex].OwnerSeat || decision.Choices.Any(choice => choice.Cards.Count != 0))
            return false;
        return decision.Kind == DecisionKind.ProgramJudgmentTrigger && decision.Choices.Count == 2 && decision.ValidTargetSeats.Count == 0;
    }

    internal void AssertProgramJudgmentWindowState()
    {
        var frames = _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().ToArray();
        if (frames.Length == 0)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind is DecisionKind.ProgramJudgmentTrigger)
                throw new InvalidOperationException("A judgment trigger prompt lost its frame.");
            return;
        }

        var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment;
        var frame = pending is null ? null : frames.LastOrDefault(candidate => candidate.Judgment.JudgmentFrameId == pending.Id);
        var suspendedFrames = frames.Where(candidate => !ReferenceEquals(candidate, frame)).ToArray();
        if (suspendedFrames.Any(suspended => !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(child => child.WindowContext is { Window: var suspendedWindow, ParentFrameId: var parentId } && suspendedWindow == (suspended.ZhenyiBeforeResult == true ? SkillProgramTriggerWindow.JudgmentBeforeResult : SkillProgramTriggerWindow.JudgmentFinalized) && parentId == suspended.Id)))
            throw new InvalidOperationException("A suspended judgment trigger window lost its program child.");
        if (frame is null)
        {
            if (pending is not null && _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind is DecisionKind.ProgramJudgmentTrigger)
                throw new InvalidOperationException("A judgment trigger prompt belongs to no active judgment window.");
            return;
        }

        var frameIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(item => ReferenceEquals(item, frame));
        var judgmentFrame = frameIndex > 0 ? _resolutionStackOperations._resolutionStack[frameIndex - 1] as JudgmentFrame : null;
        var prompt = _corePendingDecisionPolicyPipelineOperations._pendingDecision;
        var resolvingDamage = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(child => child.AttackAttempt?.ProgramJudgmentWindowId == frame.Id);
        var resolvingProgram = _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame skillFrame && skillFrame.WindowContext is { Window: var activeWindow, ParentFrameId: var parentId } && activeWindow == (frame.ZhenyiBeforeResult == true ? SkillProgramTriggerWindow.JudgmentBeforeResult : SkillProgramTriggerWindow.JudgmentFinalized) && parentId == frame.Id;
        // The new suit-cost instruction owns its judgment and may wait on a finalized-program child subtree.
        resolvingProgram |= judgmentFrame is not null && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(owner => owner.Id == judgmentFrame.ParentFrameId && owner.SlashSuitDiscard is { Stage: ProgramSlashSuitDiscardStage.Judging } draft && draft.JudgmentFrameId == judgmentFrame.Id && draft.CardUseFrameId == judgmentFrame.ParentAttackId) && frameIndex + 1 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[frameIndex + 1] is ProgramSkillFrame ownedChild && ownedChild.WindowContext is { Window: SkillProgramTriggerWindow.JudgmentFinalized, ParentFrameId: var ownedParentId } && ownedParentId == frame.Id;
        resolvingProgram |= _coreExactRepeatedJudgmentOperations.IsExactRepeatedJudgmentFinalizedChildSubtree(frame, judgmentFrame, frameIndex);
        resolvingProgram |= _coreOwnedDamagePointJudgmentsOperations.IsExactOwnedDamagePointJudgmentFinalizedChildSubtree(frame, judgmentFrame, frameIndex);
        resolvingProgram |= _characterXiZhiCaiOlOperations.IsExactChouceFinalizedChildSubtree(frame, judgmentFrame, frameIndex);
        resolvingProgram |= _coreDamageJudgmentSuitPaymentObserversOperations.IsExactDamageJudgmentSuitPaymentFinalizedSubtree(frame, judgmentFrame, frameIndex);
        resolvingProgram |= _coreJudgmentOutcomeDamageEdgesOperations.IsExactJudgmentOutcomeFinalizedSubtree(frame, judgmentFrame, frameIndex);
        resolvingProgram |= _coreYaodaoChitongOperations.IsExactYaodaoChitongJudgmentFinalizedSubtree(frame, judgmentFrame, frameIndex);
        resolvingProgram |= _characterMaYuanYiOlOperations.IsExactBinghuoFinalizedChildSubtree(frame, judgmentFrame, frameIndex);
        resolvingProgram |= _characterZhouQunOlOperations.IsExactNingwuFinalizedChildSubtree(frame, judgmentFrame, frameIndex);
        resolvingProgram |= _characterZhangFeiAnniversaryGodOperations.IsExactShenZfJudgmentGainSubtree(frame, judgmentFrame, frameIndex);
        if (pending is null || (frame.ZhenyiBeforeResult == true ? pending.ZhenyiBeforeResultCompleted == true || pending.Succeeded is not null : pending.Succeeded is null) || (!ReferenceEquals(_resolutionStackOperations._resolutionStack.Last(), frame) && !resolvingDamage && !resolvingProgram) || judgmentFrame is null || judgmentFrame.Id != frame.ParentFrameId || pending.Id != frame.Judgment.JudgmentFrameId || pending.TargetSeat != frame.Judgment.SubjectSeat || _coreNativeDamageHandlesOperations.GetJudgmentCard(pending)?.Id != frame.Judgment.CardId || frame.CandidateIndex < 0 || frame.CandidateIndex >= frame.Candidates.Count || (!resolvingDamage && !resolvingProgram && prompt is not null && !IsProgramJudgmentPromptValid(frame)))
            throw new InvalidOperationException("A final judgment trigger window has an invalid cursor or prompt.");
    }
}
