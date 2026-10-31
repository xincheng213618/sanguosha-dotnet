#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramContinuationPermissionsRuntime
{
    private readonly ICoreProgramContinuationPermissionsCoreProgramRuntimeHooksOperations _coreProgramRuntimeHooksOperations;
    internal CoreGameEngineProgramContinuationPermissionsRuntime(ICoreProgramContinuationPermissionsCoreProgramRuntimeHooksOperations coreProgramRuntimeHooksOperations)
    {
        this._coreProgramRuntimeHooksOperations = coreProgramRuntimeHooksOperations;
    }
}
