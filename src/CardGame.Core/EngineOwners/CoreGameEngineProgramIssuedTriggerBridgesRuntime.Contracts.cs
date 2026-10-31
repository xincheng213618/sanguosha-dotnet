#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramIssuedTriggerBridgesKernelCoreProgramRuntimeModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    object? ResolveRuntimeHandlerTarget(global::System.Reflection.MethodInfo method);
}

internal interface ICoreProgramIssuedTriggerBridgesProgramIssuedTriggersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramIssuedTriggerRuntime? _programIssuedTriggers { get; }
}

internal interface ICoreProgramIssuedTriggerBridgesProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreProgramIssuedTriggerBridgesProgramTriggerProofsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramTriggerProofRuntime? _programTriggerProofs { get; }
}
