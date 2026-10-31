#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess : IProgramRuntimeOperations
{
    ref readonly List<int> _discardPhaseHandDiscardIds { get; }
}
