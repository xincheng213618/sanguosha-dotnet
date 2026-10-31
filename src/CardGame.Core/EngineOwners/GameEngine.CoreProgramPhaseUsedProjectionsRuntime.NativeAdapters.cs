#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramPhaseUsedProjectionsKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseUsedProjectionsKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }

    [ProgramCapability]
    private sealed class CoreProgramPhaseUsedProjectionsPhaseUsedProjectionsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseUsedProjectionsPhaseUsedProjectionsOperations
    {
        public ref global::CardGame.Core.ProgramPhaseUsedProjections? _phaseUsedProjections => ref engine._phaseUsedProjections;
    }
}
