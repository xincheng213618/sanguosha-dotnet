#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTurnEndingOrderRuntime
{
    private readonly ICoreTurnEndingOrderProgramPhaseBoundaryOperations _programPhaseBoundaryOperations;
    internal CoreGameEngineTurnEndingOrderRuntime(ICoreTurnEndingOrderProgramPhaseBoundaryOperations programPhaseBoundaryOperations)
    {
        this._programPhaseBoundaryOperations = programPhaseBoundaryOperations;
    }
}
