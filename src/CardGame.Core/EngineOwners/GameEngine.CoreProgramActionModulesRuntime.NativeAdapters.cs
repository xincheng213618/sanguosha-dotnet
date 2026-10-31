#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramActionModulesProgramActionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramActionModulesProgramActionsOperations
    {
        public ref global::CardGame.Core.ProgramActionRouter? _programActions => ref engine._programActions;
    }

    [ProgramCapability]
    private sealed class CoreProgramActionModulesKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramActionModulesKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }
}
