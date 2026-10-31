#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramTargetResolversRuntime
{
    internal ProgramTargetResolverRegistry ProgramTargetResolvers => _programTargetResolversOperations._programTargetResolvers ??= new(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetResolversDefinitions.ProgramTargetResolverMetadata.Value, _coreProgramSelectionModulesOperations.ProgramSelectionQueries);
}
