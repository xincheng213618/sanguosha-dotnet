#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramTargetRulesTargetRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTargetRulesTargetRulesOperations
    {
        public ref global::CardGame.Core.ProgramTargetRules? _targetRules => ref engine._targetRules;
    }

    [ProgramCapability]
    private sealed class CoreProgramTargetRulesKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramTargetRulesKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }
}
