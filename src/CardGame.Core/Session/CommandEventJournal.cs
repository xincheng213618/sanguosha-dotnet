namespace CardGame.Core;

/// <summary>
/// Prepares immutable event envelopes and commits a reserved batch. The supplied
/// stores are shared with existing rule readers; this object holds no engine.
/// </summary>
internal sealed class CommandEventJournal(List<EventEnvelope> events,
    List<IGameEvent> pendingEvents, Queue<EngineNotification> notifications)
{
    internal readonly record struct PreparedBatch(EventNotification[] Events,
        EventNotification?[] Notifications);

    internal PreparedBatch Prepare(long revision, long previousSequence,
        Func<EventNotification[], EventNotification?[]> projectPublicEvents)
    {
        var prepared = new EventNotification[pendingEvents.Count];
        for (var index = 0; index < prepared.Length; index++)
        {
            var sequence = checked(previousSequence + index + 1L);
            prepared[index] = new EventNotification(new EventEnvelope(
                new EventId(sequence), ParentId: null, sequence, revision,
                $"revision-{revision}", CommittedEventProjection.Freeze(pendingEvents[index])));
        }
        var published = projectPublicEvents(prepared);
        if (published.Length != prepared.Length)
            throw new InvalidOperationException("Public event projection must retain a slot for each committed event.");
        return new(prepared, published);
    }

    internal void Reserve(PreparedBatch prepared)
    {
        events.EnsureCapacity(checked(events.Count + prepared.Events.Length));
        notifications.EnsureCapacity(checked(notifications.Count + prepared.Events.Length));
    }

    internal long? Commit(PreparedBatch prepared)
    {
        for (var index = 0; index < prepared.Events.Length; index++)
        {
            events.Add(prepared.Events[index].Event);
            if (prepared.Notifications[index] is { } notification) notifications.Enqueue(notification);
        }
        return prepared.Events.Length == 0 ? null : prepared.Events[^1].Event.Sequence;
    }

    internal void ClearNotifications() => notifications.Clear();
}
