#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess : IProgramRuntimeOperations
{
    ref readonly List<EventEnvelope> _events { get; }

    ref readonly List<IGameEvent> _pendingEvents { get; }

    ref readonly Queue<EngineNotification> _pendingNotifications { get; }
}
