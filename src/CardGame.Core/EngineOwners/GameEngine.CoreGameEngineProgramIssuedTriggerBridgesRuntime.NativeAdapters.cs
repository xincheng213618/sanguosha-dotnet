#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramIssuedTriggerBridgesKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramIssuedTriggerBridgesKernelCoreProgramRuntimeModulesOperations
    {
        public object? ResolveRuntimeHandlerTarget(global::System.Reflection.MethodInfo method) => engine.ResolveRuntimeHandlerTarget(method);
    }

    [ProgramCapability]
    private sealed class CoreProgramIssuedTriggerBridgesProgramIssuedTriggersAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramIssuedTriggerBridgesProgramIssuedTriggersOperations
    {
        public ref global::CardGame.Core.ProgramIssuedTriggerRuntime? _programIssuedTriggers => ref engine._programIssuedTriggers;
    }

    [ProgramCapability]
    private sealed class CoreProgramIssuedTriggerBridgesProgramPipelineHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramIssuedTriggerBridgesProgramPipelineHooksOperations
    {
        public ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }

    [ProgramCapability]
    private sealed class CoreProgramIssuedTriggerBridgesProgramTriggerProofsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramIssuedTriggerBridgesProgramTriggerProofsOperations
    {
        public ref global::CardGame.Core.ProgramTriggerProofRuntime? _programTriggerProofs => ref engine._programTriggerProofs;
    }
}
