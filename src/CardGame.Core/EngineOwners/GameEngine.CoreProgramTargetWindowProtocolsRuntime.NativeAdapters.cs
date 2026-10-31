#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramTargetWindowProtocolsKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTargetWindowProtocolsKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }

    [ProgramCapability]
    private sealed class CoreProgramTargetWindowProtocolsTargetWindowProtocolsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTargetWindowProtocolsTargetWindowProtocolsOperations
    {
        public ref global::CardGame.Core.ProgramTargetWindowProtocols? _targetWindowProtocols => ref engine._targetWindowProtocols;
    }
}
