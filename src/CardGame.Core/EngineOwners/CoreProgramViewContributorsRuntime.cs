#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramViewContributorsRuntime
{
    private readonly ICoreProgramViewContributorsViewContributorsOperations _viewContributorsOperations;
    private readonly ICoreProgramViewContributorsKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramViewContributorsRuntime(ICoreProgramViewContributorsViewContributorsOperations viewContributorsOperations, ICoreProgramViewContributorsKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._viewContributorsOperations = viewContributorsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
