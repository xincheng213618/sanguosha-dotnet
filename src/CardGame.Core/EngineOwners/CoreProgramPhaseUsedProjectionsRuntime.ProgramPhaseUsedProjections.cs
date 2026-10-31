#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPhaseUsedProjectionsRuntime
{
    internal ProgramPhaseUsedProjections PhaseUsedProjections => _phaseUsedProjectionsOperations._phaseUsedProjections ??= new(CoreProgramPhaseUsedProjectionsDefinitions.PhaseUsedProjectionMetadata.Value, method => (Func<int, string, int, int>)_kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler(method, typeof(Func<int, string, int, int>)));
}
