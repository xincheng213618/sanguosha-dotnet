#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPipelineHooksRuntime
{
    private readonly ICoreProgramPipelineHooksProgramPipelineHooksOperations _programPipelineHooksOperations;
    internal CoreProgramPipelineHooksRuntime(ICoreProgramPipelineHooksProgramPipelineHooksOperations programPipelineHooksOperations)
    {
        this._programPipelineHooksOperations = programPipelineHooksOperations;
    }
}
