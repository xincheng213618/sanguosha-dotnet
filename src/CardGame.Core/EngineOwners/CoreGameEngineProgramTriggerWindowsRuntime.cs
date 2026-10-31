#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramTriggerWindowsRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly IKernelCreateWuLuKangWindowHostOperations _kernelCharacterLuKangWumiaoOperations;
    private readonly IKernelCreateDouyuTriggerWindowHostOperations _kernelCharacterPangJiOlOperations;
    private readonly IKernelTriggerWindowHostFactories _kernelCoreGameEngineProgramTriggerWindowsOperations;
    private readonly ICoreProgramTriggerWindowsProgramLifecycleWindowOperations _programLifecycleWindowOperations;
    private readonly ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations _programPhaseBoundaryOperations;
    internal CoreGameEngineProgramTriggerWindowsRuntime(IKernelFrameStoreAccess resolutionStackOperations, IKernelCreateWuLuKangWindowHostOperations kernelCharacterLuKangWumiaoOperations, IKernelCreateDouyuTriggerWindowHostOperations kernelCharacterPangJiOlOperations, IKernelTriggerWindowHostFactories kernelCoreGameEngineProgramTriggerWindowsOperations, ICoreProgramTriggerWindowsProgramLifecycleWindowOperations programLifecycleWindowOperations, ICoreProgramTriggerWindowsProgramPhaseBoundaryOperations programPhaseBoundaryOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._kernelCharacterLuKangWumiaoOperations = kernelCharacterLuKangWumiaoOperations;
        this._kernelCharacterPangJiOlOperations = kernelCharacterPangJiOlOperations;
        this._kernelCoreGameEngineProgramTriggerWindowsOperations = kernelCoreGameEngineProgramTriggerWindowsOperations;
        this._programLifecycleWindowOperations = programLifecycleWindowOperations;
        this._programPhaseBoundaryOperations = programPhaseBoundaryOperations;
    }
}
