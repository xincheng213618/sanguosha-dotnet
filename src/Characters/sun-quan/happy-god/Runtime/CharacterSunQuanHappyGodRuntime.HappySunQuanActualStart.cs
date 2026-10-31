#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanHappyGodRuntime
{
    internal bool HasHappySunQuanContent => _contentRegistryOperations._contentRegistry.Skills.ContainsKey(HappySunQuanComposition.Learning);

    internal int HappyXue(int holder) => _coreMatchParticipantsPlayersAccess._players[holder].Markers.GetValueOrDefault(PlayerMarkerKind.HappyXue);
    internal static string HappyLearningInstance(int holder, int turn) => $"issued:happy-learning:{(holder)}:{(turn)}";
    internal ProgramTriggerCandidate HappyLearningCandidate(int holder, int turn)
    {
        var p = _contentRegistryOperations._contentRegistry.GetSkill(HappySunQuanComposition.Learning).Program!;
        return new(holder, p.Id, "resolve-study", HappyLearningInstance(holder, turn), p.GameplayHash, p.Triggers.Single().Priority);
    }

    internal bool TryBeginHappyLearningActualStart(CharacterState current)
    {
        if (!HasHappySunQuanContent || !current.IsAlive || HappyXue(current.Seat) == 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLearningIssuedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.HolderSeat == current.Seat))
            return false;
        if (_resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || current.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber < 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().LastOrDefault()is not { } start || start.ActorSeat != current.Seat || start.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
            throw new InvalidOperationException("Learning requires the real clean actual turn start, including a face-down turn.");
        var grants = _coreMatchParticipantsPlayersAccess._players[current.Seat].MarkerSourceCounts.Where(p => p.Key.Marker == PlayerMarkerKind.HappyXue && p.Value > 0).Select(p => _coreProgramEventHistoryOperations.ProgramEventHistory<HappyXueGrantEvent>().LastOrDefault(e => e.Target == current.Seat && e.Source.OwnerSeat == p.Key.SkillOwnerSeat && e.After == p.Value) ?? throw new InvalidOperationException("Learning lost its original printed Quanxue grant.")).ToArray();
        if (grants.Sum(g => g.After) != HappyXue(current.Seat))
            throw new InvalidOperationException("Learning lost its exact attributed source counts.");
        var candidates = new List<ProgramTriggerCandidate>
        {
            HappyLearningCandidate(current.Seat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
        };
        candidates.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != current.Seat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(current.Seat, p.Seat)).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)).Where(c => c.SkillId == HappySunQuanComposition.Dingli));
        var parent = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, current.Seat, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Array.AsReadOnly(candidates.ToArray()), ProgramLifecycleContinuation.NormalTurnStart, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(current))
        {
            HappyLearningStart = new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, current.Seat)
            {
                Grants = grants
            },
            ParticipantFacts = new System.Collections.ObjectModel.ReadOnlyDictionary<int, SkillProgramTriggerFacts>(candidates.Select(c => c.OwnerSeat).Distinct().ToDictionary(s => s, s => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[s])))
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLearningIssuedEvent(parent.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, current.Seat, grants.Length, grants.Sum(g => g.After)));
        _coreRuntimeOperations.PushRuntimeFrame(parent);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool IsHappyLearningStart(ProgramLifecycleTriggerWindowFrame p) => p.HappyLearningStart is { } r && p.OwnerSeat == r.HolderSeat && p.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && r.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && p.Continuation == ProgramLifecycleContinuation.NormalTurnStart && p.Candidates.Count > 0 && p.Candidates[0] == HappyLearningCandidate(p.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber) && r.Grants.Count > 0 && r.Grants.Select(g => g.Source.OwnerSeat).Distinct().Count() == r.Grants.Count && r.Grants.All(g => g.Target == p.OwnerSeat && g.After > 0 && g.After == g.Before + 1 && g.Source.SkillId == HappySunQuanComposition.Quanxue && g.Source.BindingId == "grant-study" && _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program!.GameplayHash == g.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyXueGrantEvent>().Count(e => e == g) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == g.FrameId && e.OwnerSeat == g.Source.OwnerSeat && e.SkillId == g.Source.SkillId && e.SkillInstanceId == g.Source.SkillInstanceId && e.BindingId == g.Source.BindingId) == 1) && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLearningIssuedEvent>().Count(e => e == new HappyLearningIssuedEvent(p.Id, r.ActualTurn, r.HolderSeat, r.Grants.Count, r.Grants.Sum(g => g.After))) == 1;
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 40940)]
    internal bool IsIssuedHappyLearning(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == HappySunQuanComposition.Learning && c == HappyLearningCandidate(c.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber) && w.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && w.OwnerSeat == c.OwnerSeat && w.SourceSeat == c.OwnerSeat && w.TargetSeat == c.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } parent && IsHappyLearningStart(parent) && parent.CandidateIndex == 0 && parent.Candidates[0] == c && (_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.HappySunQuan is { Operation: SkillProgramEffectOp.ResolveHappySunQuanLearning, Branch: 2 } && f.WindowContext == w && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c)));
    [ProgramPipelineHook(ProgramPipelinePhase.LifecycleWindowCompleted, 40940)]
    internal bool CompleteHappyLearningStart(ProgramLifecycleTriggerWindowFrame parent)
    {
        if (parent.HappyLearningStart is null)
            return false;
        if (!IsHappyLearningStart(parent) || parent.CandidateIndex < 0 || parent.CandidateIndex > parent.Candidates.Count || _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && parent.CandidateIndex != parent.Candidates.Count || _resolutionStackOperations._resolutionStack.Any(p => p.Id == parent.Id) || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLearningRemovedEvent>().Count(e => e.WindowId == parent.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLearningReturnedEvent>().Any(e => e.WindowId == parent.Id))
            throw new InvalidOperationException("Learning lost its exact actual-start typed return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyLearningReturnedEvent(parent.Id, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, parent.OwnerSeat));
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            if (_resolutionStackOperations._resolutionStack.Count == 0 && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed)
                _kernelCoreMainOperations.CompleteGame();
            return true;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat].IsAlive)
        {
            _coreActualTurnsOperations.EndTurn();
            return true;
        }

        _coreActualTurnsOperations.ContinueTurnAfterActualStart(_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat]);
        return true;
    }

    internal bool HasHappyLearningPlaySelfOnly(int actor) => HasHappySunQuanContent && actor == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLearningChoiceEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.HolderSeat == actor && e.Branch == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<HappyLearningRemovedEvent>().Count(m => m.FrameId == e.FrameId && m.WindowId == e.WindowId && m.ActualTurn == e.ActualTurn && m.HolderSeat == actor && m.Total > 0) == 1);
}
