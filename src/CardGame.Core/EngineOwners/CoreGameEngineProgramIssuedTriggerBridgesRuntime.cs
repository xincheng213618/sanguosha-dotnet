#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramIssuedTriggerBridgesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramIssuedTriggersAccess _programIssuedTriggersOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramTriggerProofsAccess _programTriggerProofsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelResolveRuntimeHandlerTargetOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreGameEngineProgramIssuedTriggerBridgesRuntime(global::CardGame.Core.EngineOwners.IKernelProgramIssuedTriggersAccess programIssuedTriggersOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.EngineOwners.IKernelProgramTriggerProofsAccess programTriggerProofsOperations, global::CardGame.Core.EngineOwners.IKernelResolveRuntimeHandlerTargetOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._programIssuedTriggersOperations = programIssuedTriggersOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._programTriggerProofsOperations = programTriggerProofsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
