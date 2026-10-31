#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreCommandJournalStorageObserverFailuresAccess : IProgramRuntimeOperations
{
    ref readonly List<ObserverFailure> _observerFailures { get; }
}
