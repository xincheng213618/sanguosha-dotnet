#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDyingEntryProgramsRuntime
{
    private readonly ICoreDyingEntryProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDyingEntryProgramsCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreDyingEntryProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreDyingEntryProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreDyingEntryProgramsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreDyingEntryProgramsCoreTreatedWoundedOperations _coreTreatedWoundedOperations;
    private readonly ICoreDyingEntryProgramsCoreNativeDyingOperations _coreNativeDyingOperations;
    private readonly ICoreDyingEntryProgramsCoreNativeDyingFramesOperations _coreNativeDyingFramesOperations;
    private readonly ICoreDyingEntryProgramsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreDyingEntryProgramsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDyingEntryProgramsRuntime(ICoreDyingEntryProgramsResolutionStackOperations resolutionStackOperations, ICoreDyingEntryProgramsCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreDyingEntryProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreDyingEntryProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreDyingEntryProgramsCoreTableOrderOperations coreTableOrderOperations, ICoreDyingEntryProgramsCoreTreatedWoundedOperations coreTreatedWoundedOperations, ICoreDyingEntryProgramsCoreNativeDyingOperations coreNativeDyingOperations, ICoreDyingEntryProgramsCoreNativeDyingFramesOperations coreNativeDyingFramesOperations, ICoreDyingEntryProgramsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreDyingEntryProgramsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreNativeDyingOperations = coreNativeDyingOperations;
        this._coreNativeDyingFramesOperations = coreNativeDyingFramesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
