#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramTriggerWindowsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateWuLuKangWindowHostOperations _kernelCharacterLuKangWumiaoOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateDouyuTriggerWindowHostOperations _kernelCharacterPangJiOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTriggerWindowHostFactories _kernelCoreGameEngineProgramTriggerWindowsOperations;
    private readonly ICoreProgramTriggerWindowsProgramLifecycleWindowOperations _programLifecycleWindowOperations;
    private readonly ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations _programPhaseBoundaryOperations;
    internal CoreGameEngineProgramTriggerWindowsRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.EngineOwners.IKernelCreateWuLuKangWindowHostOperations kernelCharacterLuKangWumiaoOperations, global::CardGame.Core.EngineOwners.IKernelCreateDouyuTriggerWindowHostOperations kernelCharacterPangJiOlOperations, global::CardGame.Core.EngineOwners.IKernelTriggerWindowHostFactories kernelCoreGameEngineProgramTriggerWindowsOperations, ICoreProgramTriggerWindowsProgramLifecycleWindowOperations programLifecycleWindowOperations, ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations programPhaseBoundaryOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._kernelCharacterLuKangWumiaoOperations = kernelCharacterLuKangWumiaoOperations;
        this._kernelCharacterPangJiOlOperations = kernelCharacterPangJiOlOperations;
        this._kernelCoreGameEngineProgramTriggerWindowsOperations = kernelCoreGameEngineProgramTriggerWindowsOperations;
        this._programLifecycleWindowOperations = programLifecycleWindowOperations;
        this._programPhaseBoundaryOperations = programPhaseBoundaryOperations;
    }
}
