#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStorageEventsPendingEventsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.EventEnvelope> _events { get; }

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IGameEvent> _pendingEvents { get; }
}
