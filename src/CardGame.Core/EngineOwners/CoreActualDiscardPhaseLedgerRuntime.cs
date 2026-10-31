#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreActualDiscardPhaseLedgerRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess _coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    internal CoreActualDiscardPhaseLedgerRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess = coreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    }
}
