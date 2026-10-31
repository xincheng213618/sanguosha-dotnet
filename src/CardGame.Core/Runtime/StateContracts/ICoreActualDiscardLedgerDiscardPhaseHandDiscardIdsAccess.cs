#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<int> _discardPhaseHandDiscardIds { get; }
}
