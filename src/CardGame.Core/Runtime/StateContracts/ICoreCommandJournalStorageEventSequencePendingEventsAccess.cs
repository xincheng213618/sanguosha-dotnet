#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStorageEventSequencePendingEventsAccess : IProgramRuntimeOperations
{
    ref long _eventSequence { get; }

    ref readonly List<IGameEvent> _pendingEvents { get; }
}
