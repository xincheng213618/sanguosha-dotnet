#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreActualDiscardLedgerState : global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess
{
    // Guzheng ledger: hand cards the current turn owner moved into the discard
    // pile during this turn's discard phase. Scoped to its owning turn number so
    // an interrupted turn can never serve a stale pool to the next boundary.
    private global::System.Collections.Generic.List<int> _discardPhaseHandDiscardIds = new();
    internal ref readonly global::System.Collections.Generic.List<int> ReferenceToDiscardPhaseHandDiscardIds => ref _discardPhaseHandDiscardIds;

    private int _discardPhaseHandDiscardTurnNumber = -1;
    internal ref int ReferenceToDiscardPhaseHandDiscardTurnNumber => ref _discardPhaseHandDiscardTurnNumber;

    ref readonly global::System.Collections.Generic.List<int> global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess._discardPhaseHandDiscardIds => ref _discardPhaseHandDiscardIds;

    ref int global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardTurnNumber => ref _discardPhaseHandDiscardTurnNumber;

    ref readonly global::System.Collections.Generic.List<int> global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess._discardPhaseHandDiscardIds => ref _discardPhaseHandDiscardIds;
}
