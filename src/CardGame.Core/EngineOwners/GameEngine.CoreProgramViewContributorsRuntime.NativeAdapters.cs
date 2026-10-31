#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class CoreProgramViewContributorsViewContributorsAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramViewContributorsViewContributorsOperations
    {
        public ref global::CardGame.Core.ProgramViewContributors? _viewContributors => ref engine._viewContributors;
    }

    [ProgramCapability]
    private sealed class CoreProgramViewContributorsKernelCoreProgramRuntimeModulesAdapter(GameEngine engine) : global::CardGame.Core.EngineOwners.ICoreProgramViewContributorsKernelCoreProgramRuntimeModulesOperations
    {
        public global::System.Delegate BindRuntimeHandler(global::System.Reflection.MethodInfo method, global::System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }
}
