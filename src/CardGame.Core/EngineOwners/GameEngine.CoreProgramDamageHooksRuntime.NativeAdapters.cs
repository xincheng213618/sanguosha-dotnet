#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramDamageHooksKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDamageHooksKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }

    [ProgramCapability]
    private sealed class CoreProgramDamageHooksModeRuntimeAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDamageHooksModeRuntimeOperations
    {
        public ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class CoreProgramDamageHooksDamageHooksAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDamageHooksDamageHooksOperations
    {
        public ref global::CardGame.Core.ProgramDamageHooks? _damageHooks => ref engine._damageHooks;
    }
}
