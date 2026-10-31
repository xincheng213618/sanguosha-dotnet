#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineDamagePreventionDrawRuntime : global::CardGame.Core.IDamagePreventionDrawProgramHost
{
    private readonly ICoreDamagePreventionDrawResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDamagePreventionDrawCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreDamagePreventionDrawCoreDamagePreventionProgramsOperations _coreDamagePreventionProgramsOperations;
    private readonly ICoreDamagePreventionDrawCorePreventionDrawObserversOperations _corePreventionDrawObserversOperations;
    private readonly ICoreDamagePreventionDrawCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreDamagePreventionDrawCoreRuntimeOperations _coreRuntimeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineDamagePreventionDrawRuntime(ICoreDamagePreventionDrawResolutionStackOperations resolutionStackOperations, ICoreDamagePreventionDrawCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreDamagePreventionDrawCoreDamagePreventionProgramsOperations coreDamagePreventionProgramsOperations, ICoreDamagePreventionDrawCorePreventionDrawObserversOperations corePreventionDrawObserversOperations, ICoreDamagePreventionDrawCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreDamagePreventionDrawCoreRuntimeOperations coreRuntimeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreDamagePreventionProgramsOperations = coreDamagePreventionProgramsOperations;
        this._corePreventionDrawObserversOperations = corePreventionDrawObserversOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
