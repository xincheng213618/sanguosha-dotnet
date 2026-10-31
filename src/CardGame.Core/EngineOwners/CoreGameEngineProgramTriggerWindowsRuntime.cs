#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramTriggerWindowsRuntime
{
    private readonly ICoreProgramTriggerWindowsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramTriggerWindowsKernelCharacterLuKangWumiaoOperations _kernelCharacterLuKangWumiaoOperations;
    private readonly ICoreProgramTriggerWindowsKernelCharacterPangJiOlOperations _kernelCharacterPangJiOlOperations;
    private readonly ICoreProgramTriggerWindowsKernelCoreGameEngineProgramTriggerWindowsOperations _kernelCoreGameEngineProgramTriggerWindowsOperations;
    private readonly ICoreProgramTriggerWindowsProgramLifecycleWindowOperations _programLifecycleWindowOperations;
    private readonly ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations _programPhaseBoundaryOperations;
    internal CoreGameEngineProgramTriggerWindowsRuntime(ICoreProgramTriggerWindowsResolutionStackOperations resolutionStackOperations, ICoreProgramTriggerWindowsKernelCharacterLuKangWumiaoOperations kernelCharacterLuKangWumiaoOperations, ICoreProgramTriggerWindowsKernelCharacterPangJiOlOperations kernelCharacterPangJiOlOperations, ICoreProgramTriggerWindowsKernelCoreGameEngineProgramTriggerWindowsOperations kernelCoreGameEngineProgramTriggerWindowsOperations, ICoreProgramTriggerWindowsProgramLifecycleWindowOperations programLifecycleWindowOperations, ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations programPhaseBoundaryOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._kernelCharacterLuKangWumiaoOperations = kernelCharacterLuKangWumiaoOperations;
        this._kernelCharacterPangJiOlOperations = kernelCharacterPangJiOlOperations;
        this._kernelCoreGameEngineProgramTriggerWindowsOperations = kernelCoreGameEngineProgramTriggerWindowsOperations;
        this._programLifecycleWindowOperations = programLifecycleWindowOperations;
        this._programPhaseBoundaryOperations = programPhaseBoundaryOperations;
    }
}
