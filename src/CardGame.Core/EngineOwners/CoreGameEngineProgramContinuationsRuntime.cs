#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramContinuationsRuntime
{
    private readonly ICoreProgramContinuationsProgramChildWindowOperations _programChildWindowOperations;
    internal CoreGameEngineProgramContinuationsRuntime(ICoreProgramContinuationsProgramChildWindowOperations programChildWindowOperations)
    {
        this._programChildWindowOperations = programChildWindowOperations;
    }
}
