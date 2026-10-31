#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreAvailableBoundSubsetProgramsRuntime : IAvailableBoundSubsetProgramEffectHost
{
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    internal CoreAvailableBoundSubsetProgramsRuntime(RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations)
    {
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
    }
}
