namespace CardGame.Core;

internal sealed partial class ProgramEventHistoryIndex
{
    // A derived journal index, never pending execution state. Accepted order and
    // finalization order differ when a use suspends for another use.
    private readonly Dictionary<long, ActualCardUseHistoryEntry> _actualActions = [];
    private readonly Dictionary<long, CardActionContext> _latestActualActions = [];
    private readonly Dictionary<long, NativeCardUseHistorySnapshot> _nativeUses = [];
    private readonly Dictionary<long, long> _finalizedActions = [];
    private readonly Dictionary<long, ActualCardUseHistoryEntry> _completedActualActions = [];
    private readonly Dictionary<long, long> _completedNativeActions = [];
    private readonly HashSet<long> _completedNativeUses = [];
    private int _actualCommittedCount;
    private long _actualPendingGeneration = -1;
    private long _actualPendingResetGeneration = -1;
    private readonly List<IGameEvent> _actualPendingWitness = [];

    public ActualCardUseHistoryEntry? FindAction(long actionId)
    {
        SynchronizeNativeHistory();
        return _actualActions.GetValueOrDefault(actionId);
    }

    public NativeCardUseHistorySnapshot? FindNativeUse(long nativeUseId)
    {
        SynchronizeNativeHistory();
        return _nativeUses.GetValueOrDefault(nativeUseId);
    }

    public CardActionContext? FindLatestAction(long actionId)
    {
        SynchronizeNativeHistory();
        return _latestActualActions.GetValueOrDefault(actionId);
    }

    public ActualCardUseHistoryEntry? FindFinalizedUse(long nativeUseId)
    {
        SynchronizeNativeHistory();
        return _finalizedActions.TryGetValue(nativeUseId, out var actionId) ? _actualActions[actionId] : null;
    }

    public ActualCardUseHistoryEntry? FindCompletedUse(long nativeUseId)
    {
        SynchronizeNativeHistory();
        return _completedNativeActions.TryGetValue(nativeUseId, out var actionId) ? _completedActualActions.GetValueOrDefault(actionId) : null;
    }

    private void SynchronizeNativeHistory()
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
            retained = ReferenceEquals(previous, current) || !IsNativeHistoryFact(previous) && current is not null && !IsNativeHistoryFact(current);
        }
        var start = _actualCommittedCount + _actualPendingWitness.Count;
        if (!retained)
        {
            _actualActions.Clear(); _finalizedActions.Clear();
            _latestActualActions.Clear(); ClearCardUseFacts(); _actualPhaseHistory.Clear();
            _nativeUses.Clear();
            _completedActualActions.Clear(); _completedNativeActions.Clear();
            _completedNativeUses.Clear();
            start = 0;
        }
        for (var index = start; index < events.Count + pendingEvents.Count; index++)
            IndexNativeHistoryFact(index < events.Count ? events[index].Payload : pendingEvents[index - events.Count], index + 1L);
        if (!knownAppend) _actualPendingWitness.Clear();
        for (var index = _actualPendingWitness.Count; index < pendingEvents.Count; index++)
            _actualPendingWitness.Add(pendingEvents[index]);
        _actualCommittedCount = events.Count;
        _actualPendingGeneration = stamp.Generation;
        _actualPendingResetGeneration = stamp.ResetGeneration;
    }

    private static bool IsNativeHistoryFact(IGameEvent payload) => payload is ActualCardActionRecordedEvent or
        ActualCardUseTargetsFinalizedEvent or ActualCardUseTargetsCompletedEvent or ActualCardUseActionUpdatedEvent or
        CardUseFinishedEvent or CardResponseCompletedEvent or NativeCardUseHistoryRecordedEvent or
        ActualDiscardRecoveryPhaseStartedEvent or ActualPhaseSkippedEvent or TurnStartedEvent or TurnEndedEvent;

    private void IndexNativeHistoryFact(IGameEvent payload, long cursor)
    {
        IndexCardUseFacts(payload, cursor);
        IndexActualPhaseFact(payload, cursor);
        switch (payload)
        {
            case NativeCardUseHistoryRecordedEvent { Snapshot: var snapshot }:
                _nativeUses[snapshot.NativeUseId] = snapshot;
                break;
            case ActualCardActionRecordedEvent fact when !_actualActions.ContainsKey(fact.Action.ActionId):
            {
                var action = fact.Action;
                _actualActions.Add(action.ActionId, new(action, fact.ActualTurn, fact.TurnOwner,
                    fact.Phase, fact.PlayPhaseInstanceId, fact.IsSelfNativeResponseUse));
                _latestActualActions.Add(action.ActionId, action);
                break;
            }
            case ActualCardUseTargetsFinalizedEvent fact when !_finalizedActions.ContainsKey(fact.NativeUseId) &&
                _actualActions.TryGetValue(fact.Action.ActionId, out var accepted):
            {
                var actionId = fact.Action.ActionId;
                _actualActions[actionId] = accepted with { Action = fact.Action, NativeUseId = fact.NativeUseId,
                    PrimaryTargets = fact.PrimaryTargets, Completed = _completedNativeUses.Contains(fact.NativeUseId),
                    HistoryPoint = new(accepted.ActualTurn, accepted.TurnOwner, cursor, fact.NativeUseId, actionId) };
                _latestActualActions[actionId] = fact.Action;
                _finalizedActions.Add(fact.NativeUseId, actionId);
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
                var point = new CardUseHistoryPoint(completedAccepted.ActualTurn, completedAccepted.TurnOwner, cursor, fact.NativeUseId, actionId);
                _actualActions[actionId] = completedAccepted with { CompletionPoint = point };
                _completedActualActions[actionId] = completedAccepted with { Action = fact.Action, NativeUseId = fact.NativeUseId,
                    PrimaryTargets = fact.PrimaryTargets, Completed = _completedNativeUses.Contains(fact.NativeUseId),
                    HistoryPoint = point, CompletionPoint = point };
                _latestActualActions[actionId] = fact.Action;
                _completedNativeActions.Add(fact.NativeUseId, actionId);
                break;
            }
            case ActualCardUseActionUpdatedEvent fact when _actualActions.ContainsKey(fact.Action.ActionId):
                _latestActualActions[fact.Action.ActionId] = fact.Action;
                break;
            case CardResponseCompletedEvent fact when _actualActions.TryGetValue(fact.ActionId, out var response):
            {
                var point = new CardUseHistoryPoint(response.ActualTurn, response.TurnOwner, cursor, CurrentActionId: fact.ActionId);
                _actualActions[fact.ActionId] = response with { PrimaryTargets = Array.Empty<int>(), Completed = true, CompletionPoint = point };
                _completedActualActions[fact.ActionId] = _actualActions[fact.ActionId] with { HistoryPoint = point };
                break;
            }
        }
    }

}
