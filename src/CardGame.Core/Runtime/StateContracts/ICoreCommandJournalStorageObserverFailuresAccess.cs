#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStorageObserverFailuresAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.ObserverFailure> _observerFailures { get; }
}
