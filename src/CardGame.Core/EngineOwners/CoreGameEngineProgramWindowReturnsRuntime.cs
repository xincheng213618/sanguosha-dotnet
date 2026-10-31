#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramWindowReturnsRuntime
{
    private readonly ICoreProgramWindowReturnsProgramWindowReturnOperations _programWindowReturnOperations;
    internal CoreGameEngineProgramWindowReturnsRuntime(ICoreProgramWindowReturnsProgramWindowReturnOperations programWindowReturnOperations)
    {
        this._programWindowReturnOperations = programWindowReturnOperations;
    }
}
