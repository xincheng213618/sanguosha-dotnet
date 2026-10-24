namespace CardGame.Core;

public sealed partial class GameEngine
{
    private readonly Dictionary<Type, object> _typedProgramEventHistory = [];
    private long _pendingProgramHistoryGeneration;
    private long _pendingProgramHistoryResetGeneration;
    private int _pendingProgramHistoryObservedCount;
    private bool _pendingProgramHistoryWitnessInitialized;
    private List<IGameEvent>.Enumerator _pendingProgramHistoryWitness;

    private readonly record struct PendingProgramHistoryStamp(long Generation, long ResetGeneration, int Count);

    private void ResetPendingProgramHistoryWitness()
    {
        _pendingProgramHistoryObservedCount = _pendingEvents.Count;
        _pendingProgramHistoryWitness = _pendingEvents.GetEnumerator();
        _pendingProgramHistoryWitnessInitialized = true;
    }

    private PendingProgramHistoryStamp CapturePendingProgramHistoryStamp()
    {
        var reset = !_pendingProgramHistoryWitnessInitialized ||
            _pendingProgramHistoryObservedCount != _pendingEvents.Count;
        if (!reset)
        {
            try
            {
                // List's public fail-fast enumerator detects same-count replace,
                // reorder and remove/add too. It allocates nothing on warm reads.
                // Production writes reset this witness explicitly; this fallback
                // retains exact host-fixture List mutation compatibility.
                _pendingProgramHistoryWitness.MoveNext();
            }
            catch (InvalidOperationException) { reset = true; }
        }
        if (reset)
        {
            _pendingProgramHistoryGeneration++;
            _pendingProgramHistoryResetGeneration++;
            ResetPendingProgramHistoryWitness();
        }
        return new(_pendingProgramHistoryGeneration, _pendingProgramHistoryResetGeneration, _pendingEvents.Count);
    }

    private void AppendPendingProgramEvent(IGameEvent payload)
    {
        // Check before appending so an untracked replacement cannot be hidden
        // by a later tracked append. Known appends preserve every old identity.
        _ = CapturePendingProgramHistoryStamp();
        _pendingEvents.Add(payload);
        _pendingProgramHistoryGeneration++;
        ResetPendingProgramHistoryWitness();
    }

    private void ClearPendingProgramEvents()
    {
        _pendingEvents.Clear();
        _pendingProgramHistoryGeneration++;
        _pendingProgramHistoryResetGeneration++;
        ResetPendingProgramHistoryWitness();
    }

    // This is an index of journal facts, not another active-policy store. Pending
    // facts participate immediately; committed payloads retain their frozen form.
    private IReadOnlyList<T> ProgramEventHistory<T>() where T : IGameEvent
    {
        if (!_typedProgramEventHistory.TryGetValue(typeof(T), out var value))
            _typedProgramEventHistory[typeof(T)] = value = new TypedProgramEventHistory<T>();
        return ((TypedProgramEventHistory<T>)value).Read(_events, _pendingEvents, CapturePendingProgramHistoryStamp());
    }

    private sealed class TypedProgramEventHistory<T> where T : IGameEvent
    {
        private readonly List<T> _committed = [];
        private int _committedCount;
        private readonly List<T> _pending = [];
        private int _pendingScanned;
        private long _pendingGeneration = -1;
        private long _pendingResetGeneration = -1;
        private long _pendingScanOperations;
        private int _snapshotCommittedCount;
        private IReadOnlyList<T> _snapshot = Array.Empty<T>();

        internal IReadOnlyList<T> Read(IReadOnlyList<EventEnvelope> events, IReadOnlyList<IGameEvent> pending,
            PendingProgramHistoryStamp stamp)
        {
            if (_committedCount == events.Count && _pendingGeneration == stamp.Generation) return _snapshot;
            var changed = false;
            for (; _committedCount < events.Count; _committedCount++)
                if (events[_committedCount].Payload is T fact)
                {
                    _committed.Add(fact);
                    changed = true;
                }
            var reset = _pendingResetGeneration != stamp.ResetGeneration;
            if (reset) { _pending.Clear(); _pendingScanned = 0; }
            for (; _pendingScanned < stamp.Count; _pendingScanned++)
            {
                _pendingScanOperations++;
                if (pending[_pendingScanned] is T typed) _pending.Add(typed);
            }
            var previousPendingCount = _snapshot.Count - _snapshotCommittedCount;
            if (_pending.Count != previousPendingCount) changed = true;
            // A reset can retain the exact same typed identities even when
            // unrelated facts changed; preserve that existing frozen snapshot.
            if (reset && !changed)
                for (var index = 0; index < _pending.Count; index++)
                {
                    if (ReferenceEquals(_snapshot[_snapshotCommittedCount + index], _pending[index])) continue;
                    changed = true;
                    break;
                }
            _pendingGeneration = stamp.Generation;
            _pendingResetGeneration = stamp.ResetGeneration;
            if (!changed) return _snapshot;

            var length = _committed.Count + _pending.Count;
            if (length == 0) _snapshot = Array.Empty<T>();
            else
            {
                var selected = new T[length];
                _committed.CopyTo(selected);
                _pending.CopyTo(selected, _committed.Count);
                _snapshot = Array.AsReadOnly(selected);
            }
            // Commit preparation may replace a pending payload with a frozen
            // copy. Rebuilt snapshots always take that new committed identity.
            _snapshotCommittedCount = _committed.Count;
            return _snapshot;
        }
    }
}
