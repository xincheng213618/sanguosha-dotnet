#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreCommandJournalStorageState : global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageObserverFailuresAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequencePendingEventsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess
{
    private long _eventSequence = default !;
    internal ref long ReferenceToEventSequence => ref _eventSequence;

    private global::System.Collections.Generic.List<global::CardGame.Core.EventEnvelope> _events = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.EventEnvelope> ReferenceToEvents => ref _events;

    private global::System.Collections.Generic.List<global::CardGame.Core.ObserverFailure> _observerFailures = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ObserverFailure> ReferenceToObserverFailures => ref _observerFailures;

    private global::System.Collections.Generic.List<global::CardGame.Core.IGameEvent> _pendingEvents = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IGameEvent> ReferenceToPendingEvents => ref _pendingEvents;

    private global::System.Collections.Generic.Queue<global::CardGame.Core.EngineNotification> _pendingNotifications = [];
    internal ref readonly global::System.Collections.Generic.Queue<global::CardGame.Core.EngineNotification> ReferenceToPendingNotifications => ref _pendingNotifications;

    private bool _pendingStatePublication = default !;
    internal ref bool ReferenceToPendingStatePublication => ref _pendingStatePublication;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.EventEnvelope> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess._events => ref _events;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IGameEvent> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess._pendingEvents => ref _pendingEvents;

    ref readonly global::System.Collections.Generic.Queue<global::CardGame.Core.EngineNotification> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess._pendingNotifications => ref _pendingNotifications;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ObserverFailure> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageObserverFailuresAccess._observerFailures => ref _observerFailures;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequencePendingEventsAccess._eventSequence => ref _eventSequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IGameEvent> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequencePendingEventsAccess._pendingEvents => ref _pendingEvents;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.EventEnvelope> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._events => ref _events;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IGameEvent> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._pendingEvents => ref _pendingEvents;

    ref readonly global::System.Collections.Generic.Queue<global::CardGame.Core.EngineNotification> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._pendingNotifications => ref _pendingNotifications;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IGameEvent> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess._pendingEvents => ref _pendingEvents;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess._eventSequence => ref _eventSequence;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.EventEnvelope> global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess._events => ref _events;
}
