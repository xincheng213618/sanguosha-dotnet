#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuHappyGodRuntime
{
    internal bool TryBeginHappyLuBuActualStart(CharacterState owner)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ObtainHappyShenLuBuTurnBasic) || !owner.IsAlive || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLuBuActualStartIssuedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Source.OwnerSeat == owner.Seat))
            return false;
        var candidates = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow).Where(c => c.SkillId == HappyLuBuComposition.Wuqian && c.BindingId == "actual-turn-basic").ToArray();
        if (candidates.Length == 0)
            return false;
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != owner.Seat || _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().LastOrDefault() != new TurnStartedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, owner.Seat))
            throw new InvalidOperationException("Wuqian needs the genuine clean actual-turn start.");
        var c = candidates[0];
        var source = new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId);
        var p = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Seat, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Array.AsReadOnly(new[] { c }), ProgramLifecycleContinuation.NormalTurnStart, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(owner))
        {
            HappyLuBuActualStart = new(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, source, c.GameplayHash)
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLuBuActualStartIssuedEvent(p.Id, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, source, c.GameplayHash));
        _coreRuntimeOperations.PushRuntimeFrame(p);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool IsHappyLuBuActualStart(ProgramLifecycleTriggerWindowFrame p) => p.HappyLuBuActualStart is { } r && p.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && r.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.Source.OwnerSeat == p.OwnerSeat && r.Source.SkillId == HappyLuBuComposition.Wuqian && r.Source.BindingId == "actual-turn-basic" && p.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && p.Continuation == ProgramLifecycleContinuation.NormalTurnStart && p.Candidates is [var c] && new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) == r.Source && c.GameplayHash == r.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLuBuActualStartIssuedEvent>().Count(e => e == new HappyLuBuActualStartIssuedEvent(p.Id, r.ActualTurn, r.Source, r.GameplayHash)) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.LifecycleWindowCompleted, 41200)]
    internal bool CompleteHappyLuBuActualStart(ProgramLifecycleTriggerWindowFrame p)
    {
        if (p.HappyLuBuActualStart is null)
            return false;
        if (!IsHappyLuBuActualStart(p) || p.CandidateIndex != 1 || _resolutionStackOperations._resolutionStack.Any(f => f.Id == p.Id) || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLuBuActualStartReturnedEvent>().Any(e => e.WindowId == p.Id))
            throw new InvalidOperationException("Wuqian lost its original actual-turn completion.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLuBuActualStartReturnedEvent(p.Id, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, p.OwnerSeat));
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            if (_resolutionStackOperations._resolutionStack.Count == 0 && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
                _kernelCoreMainOperations.CompleteGame();
        }
        else if (!_coreMatchParticipantsPlayersAccess._players[p.OwnerSeat].IsAlive)
            _coreActualTurnsOperations.EndTurn();
        else
            _coreActualTurnsOperations.ContinueTurnAfterActualStart(_coreMatchParticipantsPlayersAccess._players[p.OwnerSeat]);
        return true;
    }
}
