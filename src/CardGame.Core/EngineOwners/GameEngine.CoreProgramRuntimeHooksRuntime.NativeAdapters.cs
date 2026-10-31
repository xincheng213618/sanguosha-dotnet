#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramRuntimeHooksModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramRuntimeHooksModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreProgramRuntimeHooksKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramRuntimeHooksKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }

    [ProgramCapability]
    private sealed class CoreProgramRuntimeHooksRuntimeHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramRuntimeHooksRuntimeHooksOperations
    {
        public ref global::CardGame.Core.ProgramRuntimeHooks? _runtimeHooks => ref engine._runtimeHooks;
    }
}
