#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramMovementSourceCountsRuntime
{
    private readonly ICoreProgramMovementSourceCountsCoreProgramDiscardOriginsOperations _coreProgramDiscardOriginsOperations;
    internal CoreGameEngineProgramMovementSourceCountsRuntime(ICoreProgramMovementSourceCountsCoreProgramDiscardOriginsOperations coreProgramDiscardOriginsOperations)
    {
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
    }
}
