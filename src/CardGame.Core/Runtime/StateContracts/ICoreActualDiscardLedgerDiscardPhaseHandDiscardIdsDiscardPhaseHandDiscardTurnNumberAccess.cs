#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref int _discardPhaseHandDiscardTurnNumber { get; }

    ref readonly global::System.Collections.Generic.List<int> _discardPhaseHandDiscardIds { get; }
}
