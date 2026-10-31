#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess : IProgramRuntimeOperations
{
    ref int _discardPhaseHandDiscardTurnNumber { get; }

    ref readonly List<int> _discardPhaseHandDiscardIds { get; }
}
