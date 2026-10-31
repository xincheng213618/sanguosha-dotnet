#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineBoundCardMovementContinuationRuntime : global::CardGame.Core.IBoundCardMovementContinuationHost
{
    private readonly ICoreBoundCardMovementContinuationCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreBoundCardMovementContinuationCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreBoundCardMovementContinuationCoreRuntimeOperations _coreRuntimeOperations;
    internal CoreGameEngineBoundCardMovementContinuationRuntime(ICoreBoundCardMovementContinuationCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreBoundCardMovementContinuationCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreBoundCardMovementContinuationCoreRuntimeOperations coreRuntimeOperations)
    {
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
    }
}
