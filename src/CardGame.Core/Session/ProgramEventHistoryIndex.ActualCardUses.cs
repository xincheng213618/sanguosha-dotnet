namespace CardGame.Core;

internal sealed partial class ProgramEventHistoryIndex
{
    // A derived journal index, never pending execution state. Accepted order and
    // finalization order differ when a use suspends for another use.
    private readonly Dictionary<long, ActualCardUseHistoryEntry> _actualActions = [];
    private readonly Dictionary<long, CardActionContext> _latestActualActions = [];
    private readonly Dictionary<(int Turn, int Actor), List<long>> _actualTurnActions = [];
    private readonly Dictionary<(long UseId, int Target), int> _finalizedDirectedTargetOrdinals = [];
    private readonly Dictionary<(int Turn, int TurnOwner, int Actor), List<int>> _everDesignatedTargets = [];
    private readonly Dictionary<long, long> _finalizedActions = [];
    private readonly Dictionary<long, long> _previousFinalizedActions = [];
    private readonly Dictionary<long, ActualCardUseHistoryEntry> _completedActualActions = [];
    private readonly Dictionary<long, long> _completedNativeActions = [];
    private readonly Dictionary<long, IReadOnlyList<ActualCardUseHistoryEntry>> _acceptedAtCompletion = [];
    private readonly Dictionary<(int Turn, int Phase, int Actor), long> _lastFinalizedActions = [];
    private readonly Dictionary<(int Turn, int Phase, int Actor), List<long>> _acceptedActualUses = [];
    private readonly Dictionary<long, (List<long> Uses, int Index)> _acceptedActualPositions = [];
    private readonly HashSet<long> _completedNativeUses = [];
    private int _actualCommittedCount;
    private long _actualPendingGeneration = -1;
    private long _actualPendingResetGeneration = -1;
    private readonly List<IGameEvent> _actualPendingWitness = [];

    public ActualCardUseHistoryEntry? FindAction(long actionId)
    {
        SynchronizeActualUses();
        return _actualActions.GetValueOrDefault(actionId);
    }

    public CardActionContext? FindLatestAction(long actionId)
    {
        SynchronizeActualUses();
        return _latestActualActions.GetValueOrDefault(actionId);
    }

    public IReadOnlyList<int> EverDesignatedTargets(int actualTurn, int turnOwnerSeat, int actorSeat)
    {
        SynchronizeActualUses();
        return _everDesignatedTargets.TryGetValue((actualTurn, turnOwnerSeat, actorSeat), out var targets)
            ? Array.AsReadOnly(targets.ToArray()) : [];
    }

    public ActualCardUseHistoryEntry? FindFinalizedUse(long nativeUseId)
    {
        SynchronizeActualUses();
        return _finalizedActions.TryGetValue(nativeUseId, out var actionId) ? _actualActions[actionId] : null;
    }

    public ActualCardUseHistoryEntry? PreviousFinalizedUse(long nativeUseId)
    {
        SynchronizeActualUses();
        return _finalizedActions.TryGetValue(nativeUseId, out var actionId) &&
            _previousFinalizedActions.TryGetValue(actionId, out var previous) ? _actualActions[previous] : null;
    }

    public ActualCardUseHistoryEntry? FindCompletedUse(long nativeUseId)
    {
        SynchronizeActualUses();
        return _completedNativeActions.TryGetValue(nativeUseId, out var actionId) ? _completedActualActions.GetValueOrDefault(actionId) : null;
    }

    public IReadOnlyList<ActualCardUseHistoryEntry> AcceptedPairAtCompletionFor(long actionId)
    {
        SynchronizeActualUses();
        return _acceptedAtCompletion.GetValueOrDefault(actionId) ?? [];
    }

    public IReadOnlyList<ActualCardUseHistoryEntry> AcceptedPairFor(long actionId)
    {
        SynchronizeActualUses();
        if (!_acceptedActualPositions.TryGetValue(actionId, out var position)) return [];
        return position.Index == 0 ? Array.AsReadOnly(new[] { _actualActions[actionId] }) :
            Array.AsReadOnly(new[] { _actualActions[position.Uses[position.Index - 1]], _actualActions[actionId] });
    }

    public int? FinalizedDirectedTargetOrdinal(long nativeUseId, int targetSeat)
    {
        SynchronizeActualUses();
        return _finalizedDirectedTargetOrdinals.TryGetValue((nativeUseId, targetSeat), out var ordinal) ? ordinal : null;
    }

    private void SynchronizeActualUses()
    {
        var stamp = CapturePendingProgramHistoryStamp();
        if (_actualCommittedCount == events.Count && _actualPendingGeneration == stamp.Generation) return;
        // The old pending tail normally becomes the committed prefix, retaining
        // its identities. A host replacement/rollback rebuilds the derived view.
        var retained = events.Count >= _actualCommittedCount;
        var knownAppend = _actualCommittedCount == events.Count && _actualPendingResetGeneration == stamp.ResetGeneration;
        for (var index = 0; !knownAppend && retained && index < _actualPendingWitness.Count; index++)
        {
            var offset = _actualCommittedCount + index;
            var current = offset < events.Count ? events[offset].Payload :
                offset - events.Count < pendingEvents.Count ? pendingEvents[offset - events.Count] : null;
            var previous = _actualPendingWitness[index];
            retained = ReferenceEquals(previous, current) || !IsActualUseFact(previous) && current is not null && !IsActualUseFact(current);
        }
        var start = _actualCommittedCount + _actualPendingWitness.Count;
        if (!retained)
        {
            _actualActions.Clear(); _finalizedActions.Clear(); _previousFinalizedActions.Clear();
            _latestActualActions.Clear(); _actualTurnActions.Clear(); _finalizedDirectedTargetOrdinals.Clear(); _everDesignatedTargets.Clear();
            _completedActualActions.Clear(); _completedNativeActions.Clear(); _acceptedAtCompletion.Clear();
            _lastFinalizedActions.Clear(); _acceptedActualUses.Clear(); _acceptedActualPositions.Clear();
            _completedNativeUses.Clear();
            start = 0;
        }
        for (var index = start; index < events.Count + pendingEvents.Count; index++)
            IndexActualUseFact(index < events.Count ? events[index].Payload : pendingEvents[index - events.Count]);
        if (!knownAppend) _actualPendingWitness.Clear();
        for (var index = _actualPendingWitness.Count; index < pendingEvents.Count; index++)
            _actualPendingWitness.Add(pendingEvents[index]);
        _actualCommittedCount = events.Count;
        _actualPendingGeneration = stamp.Generation;
        _actualPendingResetGeneration = stamp.ResetGeneration;
    }

    private static bool IsActualUseFact(IGameEvent payload) => payload is ActualCardActionRecordedEvent or
        ActualCardUseTargetsFinalizedEvent or ActualCardUseTargetsCompletedEvent or ActualCardUseActionUpdatedEvent or
        CardUseFinishedEvent or CardResponseCompletedEvent;

    private void IndexActualUseFact(IGameEvent payload)
    {
        switch (payload)
        {
            case ActualCardActionRecordedEvent fact when !_actualActions.ContainsKey(fact.Action.ActionId):
            {
                var action = fact.Action;
                _actualActions.Add(action.ActionId, new(action, fact.ActualTurn, fact.TurnOwner,
                    fact.Phase, fact.PlayPhaseInstanceId, fact.IsSelfNativeResponseUse));
                _latestActualActions.Add(action.ActionId, action);
                if (action.Type == CardActionType.Use || fact.IsSelfNativeResponseUse)
                {
                    var turnKey = (fact.ActualTurn, action.ActorSeat);
                    if (!_actualTurnActions.TryGetValue(turnKey, out var turnUses)) _actualTurnActions[turnKey] = turnUses = [];
                    turnUses.Add(action.ActionId);
                }
                if (fact.Phase != TurnPhase.Play || action.ActorSeat != fact.TurnOwner ||
                    action.Type != CardActionType.Use && !fact.IsSelfNativeResponseUse) break;
                var key = (fact.ActualTurn, fact.PlayPhaseInstanceId, action.ActorSeat);
                if (!_acceptedActualUses.TryGetValue(key, out var uses)) _acceptedActualUses[key] = uses = [];
                _acceptedActualPositions.Add(action.ActionId, (uses, uses.Count));
                uses.Add(action.ActionId);
                break;
            }
            case ActualCardUseTargetsFinalizedEvent fact when !_finalizedActions.ContainsKey(fact.NativeUseId) &&
                _actualActions.TryGetValue(fact.Action.ActionId, out var accepted):
            {
                var actionId = fact.Action.ActionId;
                _actualActions[actionId] = accepted with { Action = fact.Action, NativeUseId = fact.NativeUseId,
                    PrimaryTargets = fact.PrimaryTargets, Completed = _completedNativeUses.Contains(fact.NativeUseId) };
                _latestActualActions[actionId] = fact.Action;
                _finalizedActions.Add(fact.NativeUseId, actionId);
                ObserveDesignatedTargets(fact.Action, fact.ActualTurn, fact.TurnOwner);
                if (accepted.AcceptedActorSeat == fact.Action.ActorSeat &&
                    _actualTurnActions.TryGetValue((accepted.ActualTurn, accepted.AcceptedActorSeat), out var turnUses))
                    foreach (var target in fact.Action.EffectiveDesignatedTargetSeats.Distinct())
                        _finalizedDirectedTargetOrdinals[(fact.NativeUseId, target)] = turnUses.Count(id =>
                            id <= actionId && _latestActualActions[id].EffectiveDesignatedTargetSeats.Contains(target));
                if (accepted.Phase != TurnPhase.Play || fact.Action.ActorSeat != accepted.TurnOwner) break;
                var key = (accepted.ActualTurn, accepted.PlayPhaseInstanceId, fact.Action.ActorSeat);
                if (_lastFinalizedActions.TryGetValue(key, out var previous)) _previousFinalizedActions[actionId] = previous;
                _lastFinalizedActions[key] = actionId;
                break;
            }
            case CardUseFinishedEvent fact:
                _completedNativeUses.Add(fact.ResolutionId);
                if (_finalizedActions.TryGetValue(fact.ResolutionId, out var finished))
                    _actualActions[finished] = _actualActions[finished] with { Completed = true };
                break;
            case ActualCardUseTargetsCompletedEvent fact when !_completedActualActions.ContainsKey(fact.Action.ActionId) &&
                _actualActions.TryGetValue(fact.Action.ActionId, out var completedAccepted):
            {
                var actionId = fact.Action.ActionId;
                _completedActualActions[actionId] = completedAccepted with { Action = fact.Action, NativeUseId = fact.NativeUseId,
                    PrimaryTargets = fact.PrimaryTargets, Completed = _completedNativeUses.Contains(fact.NativeUseId) };
                _latestActualActions[actionId] = fact.Action;
                ObserveDesignatedTargets(fact.Action, fact.ActualTurn, fact.TurnOwner);
                _completedNativeActions.Add(fact.NativeUseId, actionId);
                if (_acceptedActualPositions.TryGetValue(actionId, out var position))
                {
                    // This is the completion-window prefix, which can include
                    // nested uses accepted after the completing outer action.
                    var count = position.Uses.Count;
                    var pair = new ActualCardUseHistoryEntry[Math.Min(2, count)];
                    for (var index = 0; index < pair.Length; index++)
                    {
                        var id = position.Uses[count - pair.Length + index];
                        pair[index] = _completedActualActions.GetValueOrDefault(id) ?? _actualActions[id] with { Completed = false };
                    }
                    _acceptedAtCompletion[actionId] = Array.AsReadOnly(pair);
                }
                break;
            }
            case ActualCardUseActionUpdatedEvent fact when _actualActions.ContainsKey(fact.Action.ActionId):
                _latestActualActions[fact.Action.ActionId] = fact.Action;
                if (_finalizedActions.ContainsKey(fact.NativeUseId))
                    ObserveDesignatedTargets(fact.Action, fact.ActualTurn, fact.TurnOwner);
                break;
            case CardResponseCompletedEvent fact when _actualActions.TryGetValue(fact.ActionId, out var response):
                _actualActions[fact.ActionId] = response with { PrimaryTargets = Array.Empty<int>(), Completed = true };
                _completedActualActions[fact.ActionId] = _actualActions[fact.ActionId];
                break;
        }
    }

    private void ObserveDesignatedTargets(CardActionContext action, int actualTurn, int turnOwnerSeat)
    {
        if (action.Type != CardActionType.Use) return;
        var key = (actualTurn, turnOwnerSeat, action.ActorSeat);
        if (!_everDesignatedTargets.TryGetValue(key, out var targets)) _everDesignatedTargets[key] = targets = [];
        foreach (var target in action.EffectiveDesignatedTargetSeats)
            if (target != action.ActorSeat && !targets.Contains(target)) targets.Add(target);
    }
}
