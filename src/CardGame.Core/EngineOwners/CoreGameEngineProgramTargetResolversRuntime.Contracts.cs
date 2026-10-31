#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramTargetResolversCoreProgramSelectionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IProgramSelectionQueries ProgramSelectionQueries { get; }
}

internal interface ICoreProgramTargetResolversProgramTargetResolversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramTargetResolverRegistry? _programTargetResolvers { get; }
}
