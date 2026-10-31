#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramTargetResolversRuntime
{
    private readonly ICoreProgramTargetResolversProgramTargetResolversOperations _programTargetResolversOperations;
    private readonly ICoreProgramTargetResolversCoreProgramSelectionModulesOperations _coreProgramSelectionModulesOperations;
    internal CoreGameEngineProgramTargetResolversRuntime(ICoreProgramTargetResolversProgramTargetResolversOperations programTargetResolversOperations, ICoreProgramTargetResolversCoreProgramSelectionModulesOperations coreProgramSelectionModulesOperations)
    {
        this._programTargetResolversOperations = programTargetResolversOperations;
        this._coreProgramSelectionModulesOperations = coreProgramSelectionModulesOperations;
    }
}
