#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramNativeReturnsKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramNativeReturnsKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }

    [ProgramCapability]
    private sealed class CoreProgramNativeReturnsNativeReturnsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramNativeReturnsNativeReturnsOperations
    {
        public ref global::CardGame.Core.ProgramNativeReturns? _nativeReturns => ref engine._nativeReturns;
    }
}
