#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramSelectionModulesRuntime
{
    private readonly ICoreProgramSelectionModulesProgramSelectionQueriesOperations _programSelectionQueriesOperations;
    private readonly ICoreProgramSelectionModulesProgramSelectionsOperations _programSelectionsOperations;
    private readonly ICoreProgramSelectionModulesKernelCoreProgramSelectionModulesOperations _kernelCoreProgramSelectionModulesOperations;
    internal CoreProgramSelectionModulesRuntime(ICoreProgramSelectionModulesProgramSelectionQueriesOperations programSelectionQueriesOperations, ICoreProgramSelectionModulesProgramSelectionsOperations programSelectionsOperations, ICoreProgramSelectionModulesKernelCoreProgramSelectionModulesOperations kernelCoreProgramSelectionModulesOperations)
    {
        this._programSelectionQueriesOperations = programSelectionQueriesOperations;
        this._programSelectionsOperations = programSelectionsOperations;
        this._kernelCoreProgramSelectionModulesOperations = kernelCoreProgramSelectionModulesOperations;
    }
}
