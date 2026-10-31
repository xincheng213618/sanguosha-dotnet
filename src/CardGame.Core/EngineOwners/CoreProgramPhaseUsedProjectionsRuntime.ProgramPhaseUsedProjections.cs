#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPhaseUsedProjectionsRuntime
{
    internal ProgramPhaseUsedProjections PhaseUsedProjections => _phaseUsedProjectionsOperations._phaseUsedProjections ??= new(global::CardGame.Core.EngineOwners.CoreProgramPhaseUsedProjectionsDefinitions.PhaseUsedProjectionMetadata.Value, method => (Func<int, string, int, int>)_kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler(method, typeof(Func<int, string, int, int>)));
}
