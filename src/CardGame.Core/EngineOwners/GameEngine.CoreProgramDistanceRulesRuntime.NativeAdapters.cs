#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramDistanceRulesDistanceRulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDistanceRulesDistanceRulesOperations
    {
        public ref global::CardGame.Core.ProgramDistanceRules? _distanceRules => ref engine._distanceRules;
    }

    [ProgramCapability]
    private sealed class CoreProgramDistanceRulesKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramDistanceRulesKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }
}
