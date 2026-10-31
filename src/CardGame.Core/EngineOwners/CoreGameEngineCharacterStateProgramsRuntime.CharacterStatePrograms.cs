#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCharacterStateProgramsRuntime
{
    internal void RecordCharacterStateChange(int targetSeat, SkillProgramTriggerWindow window, long? parentFrameId = null, CharacterTurnedOverState? turnedOver = null)
    {
        // Opt-in event boundaries leave command journals for older content unchanged.
        if (!_coreMatchSetupSetupCompleteAccess._setupComplete || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(window))
            return;
        var change = new CharacterStateChangeContext(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parentFrameId ?? _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id, targetSeat, window)
        {
            TurnedOver = turnedOver
        };
        _coreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess._pendingCharacterStateChanges.Add(change);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CharacterStateChangedEvent(change));
    }

    internal bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = null, CharacterStateContinuation continuation = CharacterStateContinuation.Boundary, int? cardId = null, CardKind? cardKind = null)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed || (resumeFrameId is null ? _resolutionStackOperations._resolutionStack.Count != 0 : _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != resumeFrameId))
            return false;
        while (_coreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess._pendingCharacterStateChanges.FirstOrDefault(change => resumeFrameId is null || change.ParentFrameId == resumeFrameId)is { } change)
        {
            _coreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess._pendingCharacterStateChanges.Remove(change);
            if (!_coreMatchParticipantsPlayersAccess._players[change.TargetSeat].IsAlive)
                continue;
            var facts = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).ToDictionary(player => player.Seat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts);
            if (change.TurnedOver is { } turnedOver)
                facts[change.TargetSeat] = facts[change.TargetSeat] with
                {
                    OwnerIsFaceDown = turnedOver.IsFaceDown
                };
            var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).SelectMany(player => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(player, change.Window, facts[player.Seat]).Where(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Subject == SkillProgramTriggerSubject.Any || player.Seat == change.TargetSeat)).Where(candidate => !_characterZhaoZhongOlOperations.IsIssuedDiciChainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate))).OrderBy(candidate => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, candidate.OwnerSeat)).ThenByDescending(candidate => candidate.Priority).ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal).ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ThenBy(candidate => candidate.OccurrenceIndex).Concat(_characterZhaoZhongOlOperations.CollectIssuedDiciChainCandidates(change).OrderBy(candidate => candidate.OccurrenceIndex)).ToArray();
            if (candidates.Length == 0)
                continue;
            if (continuation == CharacterStateContinuation.ChainedStateBasic)
                _coreChainedStateBasicConversionsOperations.BeginChainedStateBasicChild(resumeFrameId!.Value, change.Id);
            _coreRuntimeOperations.PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(change.Id, change.TargetSeat, change.Window, candidates, ProgramLifecycleContinuation.ResumeCharacterStateChange, facts[change.TargetSeat]) { ParticipantFacts = facts, ResumeProgramFrameId = resumeFrameId, CharacterStateContinuation = continuation, ResumeCardId = cardId, ResumeCardKind = cardKind });
            _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
            return true;
        }

        return false;
    }

    internal void ResumeCharacterStateChange(ProgramLifecycleTriggerWindowFrame frame)
    {
        var continuation = frame.CharacterStateContinuation ?? throw new InvalidOperationException("A character-state window lost its continuation.");
        if (continuation == CharacterStateContinuation.ChainedStateBasic)
        {
            _coreChainedStateBasicConversionsOperations.ResumeChainedStateBasicState(frame);
            return;
        }

        if (TryBeginCharacterStateProgramWindow(frame.ResumeProgramFrameId, continuation, frame.ResumeCardId, frame.ResumeCardKind))
            return;
        switch (continuation)
        {
            case CharacterStateContinuation.Program:
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.ResumeProgramFrameId!.Value);
                break;
            case CharacterStateContinuation.KemingEcho:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not KemingSkillEchoFrame echo || echo.Id != frame.ResumeProgramFrameId || !_characterCaoPiAnniversaryGodOperations.KemingSkillEchoFrameRidesOn(frame, echo))
                    throw new InvalidOperationException("Keming echo lost its exact character-state return.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(echo.Id);
                break;
            case CharacterStateContinuation.SkillInvocation:
                if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not SkillInvocationFrame invocation || invocation.Id != frame.ResumeProgramFrameId)
                    throw new InvalidOperationException("A skill invocation lost its exact character-state return.");
                _coreRuntimeOperations.AdvanceRuntimeFrame(invocation.Id);
                break;
            case CharacterStateContinuation.SoleIncomingRangeDraw:
                _coreSoleIncomingRangeDrawOperations.ContinueSoleIncomingRangeDraw(frame.ResumeProgramFrameId!.Value);
                break;
            case CharacterStateContinuation.NativeCardEffectDraw:
                _coreNativeCardEffectDrawsOperations.ContinueNativeDrawTwo(frame.ResumeProgramFrameId!.Value);
                break;
            case CharacterStateContinuation.WenheCardEffect:
                _coreWenheCardsOperations.ContinueWenheCardEffect(frame.ResumeProgramFrameId!.Value);
                break;
            case CharacterStateContinuation.GuanduCardEffect:
                _coreGuanduCardsOperations.ContinueGuanduCardEffect(frame.ResumeProgramFrameId!.Value);
                break;
            case CharacterStateContinuation.WumiaoFlood:
                _characterGuanYuWumiaoOperations.ContinueWumiaoFlood(frame.ResumeProgramFrameId!.Value);
                break;
            case CharacterStateContinuation.DragonBoatCardEffect:
                _coreDragonBoatCardsOperations.ContinueDragonBoatCardEffect(frame.ResumeProgramFrameId!.Value);
                break;
            case CharacterStateContinuation.CardUse:
                var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(frame.ResumeProgramFrameId!.Value, frame.ResumeCardId!.Value);
                _coreNativeCardUseCompletionOperations.FinishCardUse(frame.ResumeProgramFrameId!.Value, card, frame.ResumeCardKind);
                break;
            case CharacterStateContinuation.VirtualBasicCardUse:
                _coreVirtualBasicOffersOperations.FinishVirtualBasicUse(frame.ResumeProgramFrameId!.Value);
                break;
            case CharacterStateContinuation.SkippedTurn:
                CompleteFaceUpSkippedTurn(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
                break;
            case CharacterStateContinuation.Boundary:
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                break;
        }
    }

    internal bool TryBeginJudgmentPhaseStartingPrograms(CharacterState turnOwner)
    {
        var facts = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).ToDictionary(player => player.Seat, player => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(player)with { OwnerEventTargetDistance = _coreNativeRuleQueriesOperations.GetCombatDistance(player.Seat, turnOwner.Seat) });
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).SelectMany(player => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(player, SkillProgramTriggerWindow.JudgmentPhaseStarting, facts[player.Seat]).Where(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).TurnOwnerScope == (player.Seat == turnOwner.Seat ? SkillProgramTurnOwnerScope.Own : SkillProgramTurnOwnerScope.OtherLiving))).OrderBy(candidate => _coreTableOrderOperations.RelativeTableOrder(turnOwner.Seat, candidate.OwnerSeat)).ThenByDescending(candidate => candidate.Priority).ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal).ThenBy(candidate => candidate.BindingId, StringComparer.Ordinal).ToArray();
        if (candidates.Length == 0)
            return false;
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, turnOwner.Seat, SkillProgramTriggerWindow.JudgmentPhaseStarting, candidates, ProgramLifecycleContinuation.CompleteJudgmentPhaseStarting, facts[turnOwner.Seat]) { ParticipantFacts = facts });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal void CompleteFaceUpSkippedTurn(CharacterState current)
    {
        _coreConvertingGiftsOperations.ExpireGiftRetentionObligations();
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase = TurnPhase.Finished;
        _coreMatchLogOperations.AddLog("TurnEnded", $"{(current.Name)} 的翻面回合结束。", current.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnEndedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, current.Seat));
        if (_coreDeferredHandAlignmentsOperations.TryBeginDeferredTurnEnd(current, skipped: true))
            return;
        _coreActualTurnsOperations.AdvanceAfterDeferredTurnEnd(current, skipped: true);
    }
}
