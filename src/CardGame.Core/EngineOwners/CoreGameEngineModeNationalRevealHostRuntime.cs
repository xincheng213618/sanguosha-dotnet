#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeNationalRevealHostRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateModeNationalRevealHostOperations _kernelCoreGameEngineModeNationalRevealHostOperations;
    internal CoreGameEngineModeNationalRevealHostRuntime(global::CardGame.Core.EngineOwners.IKernelCreateModeNationalRevealHostOperations kernelCoreGameEngineModeNationalRevealHostOperations)
    {
        this._kernelCoreGameEngineModeNationalRevealHostOperations = kernelCoreGameEngineModeNationalRevealHostOperations;
    }
}
