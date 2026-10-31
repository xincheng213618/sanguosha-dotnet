#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActualPhaseEndsRuntime
{
    private readonly ICoreActualPhaseEndsActualPhaseEndOperations _actualPhaseEndOperations;
    internal CoreGameEngineActualPhaseEndsRuntime(ICoreActualPhaseEndsActualPhaseEndOperations actualPhaseEndOperations)
    {
        this._actualPhaseEndOperations = actualPhaseEndOperations;
    }
}
