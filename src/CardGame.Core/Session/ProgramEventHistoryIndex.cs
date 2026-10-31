namespace CardGame.Core;

/// <summary>Incremental typed reads over this match's committed and pending facts.</summary>
internal sealed class ProgramEventHistoryIndex(IReadOnlyList<EventEnvelope> events, List<IGameEvent> pendingEvents)
    : IProgramEventHistoryQueries, IProgramEventBoundaryQueries, IProgramEventJournalQueries
{
    public IEnumerable<IGameEvent> Complete() => events.Select(item => item.Payload).Concat(pendingEvents);

    public IGameEvent? LastMatching(Func<IGameEvent, bool> predicate)
    {
        var pendingIndex = pendingEvents.FindLastIndex(item => predicate(item));
        if (pendingIndex >= 0) return pendingEvents[pendingIndex];
        if (events is List<EventEnvelope> committed)
        {
            var index = committed.FindLastIndex(item => predicate(item.Payload));
            return index < 0 ? null : committed[index].Payload;
        }
        for (var index = events.Count - 1; index >= 0; index--)
            if (predicate(events[index].Payload)) return events[index].Payload;
        return null;
    }

    public IEnumerable<IGameEvent> SinceLastBoundary(Func<IGameEvent, bool> boundary)
    {
        var pendingBoundary = pendingEvents.FindLastIndex(item => boundary(item));
        if (pendingBoundary >= 0) return pendingEvents.Skip(pendingBoundary + 1);
        // Production owns a List. Keep its original search rather than paying
        // an interface indexer call for every historical fact on hot queries.
        var flushedBoundary = events is List<EventEnvelope> committed
            ? committed.FindLastIndex(item => boundary(item.Payload))
            : FindLastBoundary(boundary);
        return events.Skip(flushedBoundary + 1).Select(item => item.Payload).Concat(pendingEvents);
    }

    private int FindLastBoundary(Func<IGameEvent, bool> boundary)
    {
        for (var index = events.Count - 1; index >= 0; index--)
            if (boundary(events[index].Payload)) return index;
        return -1;
    }

    private readonly Dictionary<Type, object> _typedProgramEventHistory = [];
    private long _pendingProgramHistoryGeneration;
    private long _pendingProgramHistoryResetGeneration;
    private int _pendingProgramHistoryObservedCount;
    private bool _pendingProgramHistoryWitnessInitialized;
    private List<IGameEvent>.Enumerator _pendingProgramHistoryWitness;

    private readonly record struct PendingProgramHistoryStamp(long Generation, long ResetGeneration, int Count);

    private void ResetPendingProgramHistoryWitness()
    {
        _pendingProgramHistoryObservedCount = pendingEvents.Count;
        _pendingProgramHistoryWitness = pendingEvents.GetEnumerator();
        _pendingProgramHistoryWitnessInitialized = true;
    }

    private PendingProgramHistoryStamp CapturePendingProgramHistoryStamp()
    {
        var reset = !_pendingProgramHistoryWitnessInitialized ||
            _pendingProgramHistoryObservedCount != pendingEvents.Count;
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
        return new(_pendingProgramHistoryGeneration, _pendingProgramHistoryResetGeneration, pendingEvents.Count);
    }

    internal void Append(IGameEvent payload)
    {
        // Check before appending so an untracked replacement cannot be hidden
        // by a later tracked append. Known appends preserve every old identity.
        _ = CapturePendingProgramHistoryStamp();
        pendingEvents.Add(payload);
        _pendingProgramHistoryGeneration++;
        ResetPendingProgramHistoryWitness();
    }

    internal void ClearPending()
    {
        pendingEvents.Clear();
        _pendingProgramHistoryGeneration++;
        _pendingProgramHistoryResetGeneration++;
        ResetPendingProgramHistoryWitness();
    }

    // This is an index of journal facts, not another active-policy store. Pending
    // facts participate immediately; committed payloads retain their frozen form.
    public IReadOnlyList<T> Read<T>() where T : IGameEvent
    {
        if (!_typedProgramEventHistory.TryGetValue(typeof(T), out var value))
            _typedProgramEventHistory[typeof(T)] = value = new TypedProgramEventHistory<T>();
        return ((TypedProgramEventHistory<T>)value).Read(events, pendingEvents, CapturePendingProgramHistoryStamp());
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
