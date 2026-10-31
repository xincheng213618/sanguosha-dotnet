#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreActualDiscardLedgerState : Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess, Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess
{
    // Guzheng ledger: hand cards the current turn owner moved into the discard
    // pile during this turn's discard phase. Scoped to its owning turn number so
    // an interrupted turn can never serve a stale pool to the next boundary.
    private List<int> _discardPhaseHandDiscardIds = new();
    internal ref readonly List<int> ReferenceToDiscardPhaseHandDiscardIds => ref _discardPhaseHandDiscardIds;

    private int _discardPhaseHandDiscardTurnNumber = -1;
    internal ref int ReferenceToDiscardPhaseHandDiscardTurnNumber => ref _discardPhaseHandDiscardTurnNumber;

    ref readonly List<int> Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess._discardPhaseHandDiscardIds => ref _discardPhaseHandDiscardIds;

    ref int Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardTurnNumber => ref _discardPhaseHandDiscardTurnNumber;

    ref readonly List<int> Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardIds => ref _discardPhaseHandDiscardIds;
}
