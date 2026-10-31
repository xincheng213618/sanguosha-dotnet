#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanAnniversaryGodRuntime
{
    internal bool TryBeginAnniversaryCangmingActualTurnStart(CharacterState current)
    {
        if (!AnniversaryMingHolderHasCards(current.Seat))
            return false;
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || current.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().LastOrDefault()is not { } started || started.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || started.ActorSeat != current.Seat)
            throw new InvalidOperationException("Cangming requires the exact clean actual-turn-start boundary.");
        var candidate = AnniversaryMingReturnCandidate(current.Seat, "return-start");
        var frame = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, current.Seat, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, [candidate], ProgramLifecycleContinuation.NormalTurnStart, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(current))
        {
            AnniversaryCangmingActualStart = true
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMingActualStartIssuedEvent(frame.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, current.Seat, candidate.SkillInstanceId, candidate.GameplayHash));
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.LifecycleWindowCompleted, 40500)]
    internal bool CompleteAnniversaryCangmingActualTurnStart(ProgramLifecycleTriggerWindowFrame frame)
    {
        if (frame.AnniversaryCangmingActualStart is null)
            return false;
        if (frame.AnniversaryCangmingActualStart != true || frame.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || frame.Continuation != ProgramLifecycleContinuation.NormalTurnStart || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || frame.CandidateIndex != 1 || frame.Candidates is not [var candidate] || candidate != AnniversaryMingReturnCandidate(frame.OwnerSeat, "return-start") || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMingActualStartIssuedEvent>().Count(e => e == new AnniversaryMingActualStartIssuedEvent(frame.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, frame.OwnerSeat, candidate.SkillInstanceId, candidate.GameplayHash)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryMingActualStartReturnedEvent>().Any(e => e.WindowId == frame.Id) || _resolutionStackOperations._resolutionStack.Any(p => p.Id == frame.Id))
            throw new InvalidOperationException("Cangming actual turn start lost its one issued holder, native lifecycle return or completed cursor.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryMingActualStartReturnedEvent(frame.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, frame.OwnerSeat));
        _coreActualTurnsOperations.ContinueTurnAfterActualStart(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        return true;
    }
}
