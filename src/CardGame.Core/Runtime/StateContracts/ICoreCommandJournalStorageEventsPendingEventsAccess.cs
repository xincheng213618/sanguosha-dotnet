#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStorageEventsPendingEventsAccess : IProgramRuntimeOperations
{
    ref readonly List<EventEnvelope> _events { get; }

    ref readonly List<IGameEvent> _pendingEvents { get; }
}
