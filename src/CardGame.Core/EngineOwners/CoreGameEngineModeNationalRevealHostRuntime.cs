#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeNationalRevealHostRuntime
{
    private readonly ICoreModeNationalRevealHostKernelCoreGameEngineModeNationalRevealHostOperations _kernelCoreGameEngineModeNationalRevealHostOperations;
    internal CoreGameEngineModeNationalRevealHostRuntime(ICoreModeNationalRevealHostKernelCoreGameEngineModeNationalRevealHostOperations kernelCoreGameEngineModeNationalRevealHostOperations)
    {
        this._kernelCoreGameEngineModeNationalRevealHostOperations = kernelCoreGameEngineModeNationalRevealHostOperations;
    }
}
