#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreCommandJournalStorageState : Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess, Contracts.ICoreCommandJournalStoragePendingNotificationsAccess, Contracts.ICoreCommandJournalStorageObserverFailuresAccess, Contracts.ICoreCommandJournalStorageEventSequencePendingEventsAccess, Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess, Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess
{
    private long _eventSequence = default !;
    internal ref long ReferenceToEventSequence => ref _eventSequence;

    private List<EventEnvelope> _events = [];
    internal ref readonly List<EventEnvelope> ReferenceToEvents => ref _events;

    private List<ObserverFailure> _observerFailures = [];
    internal ref readonly List<ObserverFailure> ReferenceToObserverFailures => ref _observerFailures;

    private List<IGameEvent> _pendingEvents = [];
    internal ref readonly List<IGameEvent> ReferenceToPendingEvents => ref _pendingEvents;

    private Queue<EngineNotification> _pendingNotifications = [];
    internal ref readonly Queue<EngineNotification> ReferenceToPendingNotifications => ref _pendingNotifications;

    private bool _pendingStatePublication = default !;
    internal ref bool ReferenceToPendingStatePublication => ref _pendingStatePublication;

    ref readonly List<EventEnvelope> Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess._events => ref _events;

    ref readonly List<IGameEvent> Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess._pendingEvents => ref _pendingEvents;

    ref readonly Queue<EngineNotification> Contracts.ICoreCommandJournalStoragePendingNotificationsAccess._pendingNotifications => ref _pendingNotifications;

    ref readonly List<ObserverFailure> Contracts.ICoreCommandJournalStorageObserverFailuresAccess._observerFailures => ref _observerFailures;

    ref long Contracts.ICoreCommandJournalStorageEventSequencePendingEventsAccess._eventSequence => ref _eventSequence;

    ref readonly List<IGameEvent> Contracts.ICoreCommandJournalStorageEventSequencePendingEventsAccess._pendingEvents => ref _pendingEvents;

    ref readonly List<EventEnvelope> Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._events => ref _events;

    ref readonly List<IGameEvent> Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._pendingEvents => ref _pendingEvents;

    ref readonly Queue<EngineNotification> Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._pendingNotifications => ref _pendingNotifications;

    ref readonly List<IGameEvent> Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess._pendingEvents => ref _pendingEvents;

    ref long Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess._eventSequence => ref _eventSequence;

    ref readonly List<EventEnvelope> Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess._events => ref _events;
}
