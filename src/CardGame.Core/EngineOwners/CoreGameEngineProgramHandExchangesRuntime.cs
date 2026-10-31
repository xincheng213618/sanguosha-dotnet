#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramHandExchangesRuntime
{
    private readonly ICoreProgramHandExchangesCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreProgramHandExchangesCoreHalfHandPhaseDebtProgramsOperations _coreHalfHandPhaseDebtProgramsOperations;
    private readonly ICoreProgramHandExchangesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramHandExchangesCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramHandExchangesCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreProgramHandExchangesCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreProgramHandExchangesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramHandExchangesRuntime(ICoreProgramHandExchangesCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreProgramHandExchangesCoreHalfHandPhaseDebtProgramsOperations coreHalfHandPhaseDebtProgramsOperations, ICoreProgramHandExchangesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramHandExchangesCoreRuntimeOperations coreRuntimeOperations, ICoreProgramHandExchangesCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreProgramHandExchangesCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreProgramHandExchangesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreHalfHandPhaseDebtProgramsOperations = coreHalfHandPhaseDebtProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
