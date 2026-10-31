#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramIssuedTriggerBridgesRuntime
{
    private readonly IKernelProgramIssuedTriggersAccess _programIssuedTriggersOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly IKernelProgramTriggerProofsAccess _programTriggerProofsOperations;
    private readonly IKernelResolveRuntimeHandlerTargetOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreGameEngineProgramIssuedTriggerBridgesRuntime(IKernelProgramIssuedTriggersAccess programIssuedTriggersOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, IKernelProgramTriggerProofsAccess programTriggerProofsOperations, IKernelResolveRuntimeHandlerTargetOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._programIssuedTriggersOperations = programIssuedTriggersOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._programTriggerProofsOperations = programTriggerProofsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
