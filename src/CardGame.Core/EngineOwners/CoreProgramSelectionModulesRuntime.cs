#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramSelectionModulesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramSelectionQueriesAccess _programSelectionQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramSelectionsAccess _programSelectionsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateSelectionQueriesOperations _kernelCoreProgramSelectionModulesOperations;
    internal CoreProgramSelectionModulesRuntime(global::CardGame.Core.EngineOwners.IKernelProgramSelectionQueriesAccess programSelectionQueriesOperations, global::CardGame.Core.EngineOwners.IKernelProgramSelectionsAccess programSelectionsOperations, global::CardGame.Core.EngineOwners.IKernelCreateSelectionQueriesOperations kernelCoreProgramSelectionModulesOperations)
    {
        this._programSelectionQueriesOperations = programSelectionQueriesOperations;
        this._programSelectionsOperations = programSelectionsOperations;
        this._kernelCoreProgramSelectionModulesOperations = kernelCoreProgramSelectionModulesOperations;
    }
}
