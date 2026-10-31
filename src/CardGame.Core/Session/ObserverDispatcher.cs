namespace CardGame.Core;

internal interface IObserverSource
{
    Action<GameSnapshot>? StateChanged { get; }
    Action<GameLogEntry>? LogAdded { get; }
    Action<AiThoughtRecord>? AiThoughtAdded { get; }
    Action<AiGeneralThought>? AiGeneralThoughtAdded { get; }
    Action<CardMovementRecord>? CardMoved { get; }
    Action<EventEnvelope>? EventCommitted { get; }
    IReadOnlySet<long>? CaptureHiddenLogSequences();
    bool StatePublicationPending { get; set; }
}

/// <summary>Delivers one prepared commit and isolates each synchronous observer failure.</summary>
internal sealed class ObserverDispatcher(Queue<EngineNotification> notifications,
    List<ObserverFailure> failures, IObserverSource source)
{
    private int _failureSequence;

    internal void Flush(GameSnapshot preparedSnapshot)
    {
        var hiddenLogs = source.CaptureHiddenLogSequences();
        while (notifications.TryDequeue(out var notification))
        {
            switch (notification)
            {
                case LogNotification log:
                    if (hiddenLogs is null || !hiddenLogs.Contains(log.Entry.Sequence))
                        Invoke(source.LogAdded, log.Entry, nameof(source.LogAdded));
                    break;
                case AiThoughtNotification thought:
                    Invoke(source.AiThoughtAdded, thought.Thought, nameof(source.AiThoughtAdded));
                    break;
                case AiGeneralThoughtNotification thought:
                    Invoke(source.AiGeneralThoughtAdded, thought.Thought, nameof(source.AiGeneralThoughtAdded));
                    break;
                case CardMovedNotification movement:
                    Invoke(source.CardMoved, movement.Movement, nameof(source.CardMoved));
                    break;
                case EventNotification typedEvent:
                    Invoke(source.EventCommitted, typedEvent.Event, nameof(source.EventCommitted));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown engine notification {notification.GetType().Name}.");
            }
        }
        if (source.StatePublicationPending)
        {
            source.StatePublicationPending = false;
            Invoke(source.StateChanged, preparedSnapshot, nameof(source.StateChanged));
        }
    }

    private void Invoke<T>(Action<T>? observers, T payload, string notificationType)
    {
        if (observers is null) return;
        foreach (Action<T> observer in observers.GetInvocationList())
        {
            try { observer(payload); }
            catch (Exception exception) { RecordFailure(notificationType, exception); }
        }
    }

    private void RecordFailure(string notificationType, Exception exception)
    {
        const int failureLimit = 128;
        if (failures.Count == failureLimit) failures.RemoveAt(0);
        failures.Add(new ObserverFailure(++_failureSequence, notificationType,
            exception.GetType().FullName ?? exception.GetType().Name, exception.Message));
    }
}
