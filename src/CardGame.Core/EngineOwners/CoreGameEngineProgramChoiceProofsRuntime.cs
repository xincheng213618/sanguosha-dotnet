#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramChoiceProofsRuntime
{
    private readonly ICoreProgramChoiceProofsKernelCoreGameEngineAdvancedLifecycleHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    internal CoreGameEngineProgramChoiceProofsRuntime(ICoreProgramChoiceProofsKernelCoreGameEngineAdvancedLifecycleHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations)
    {
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
    }
}
