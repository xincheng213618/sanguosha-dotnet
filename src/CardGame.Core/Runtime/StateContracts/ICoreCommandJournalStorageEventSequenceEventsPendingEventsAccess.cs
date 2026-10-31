#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess : IProgramRuntimeOperations
{
    ref readonly List<IGameEvent> _pendingEvents { get; }

    ref long _eventSequence { get; }

    ref readonly List<EventEnvelope> _events { get; }
}
