#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramIssuedTriggerBridgesRuntime
{
    private readonly ICoreProgramIssuedTriggerBridgesProgramIssuedTriggersOperations _programIssuedTriggersOperations;
    private readonly ICoreProgramIssuedTriggerBridgesProgramPipelineHooksOperations _programPipelineHooksOperations;
    private readonly ICoreProgramIssuedTriggerBridgesProgramTriggerProofsOperations _programTriggerProofsOperations;
    private readonly ICoreProgramIssuedTriggerBridgesKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreGameEngineProgramIssuedTriggerBridgesRuntime(ICoreProgramIssuedTriggerBridgesProgramIssuedTriggersOperations programIssuedTriggersOperations, ICoreProgramIssuedTriggerBridgesProgramPipelineHooksOperations programPipelineHooksOperations, ICoreProgramIssuedTriggerBridgesProgramTriggerProofsOperations programTriggerProofsOperations, ICoreProgramIssuedTriggerBridgesKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._programIssuedTriggersOperations = programIssuedTriggersOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._programTriggerProofsOperations = programTriggerProofsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
