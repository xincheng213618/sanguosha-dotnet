#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStorageEventSequencePendingEventsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref long _eventSequence { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IGameEvent> _pendingEvents { get; }
}
