#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDrawPhaseAttemptsRuntime
{
    private readonly ICoreDrawPhaseAttemptsOwnedDrawPhaseOperations _ownedDrawPhaseOperations;
    internal CoreGameEngineDrawPhaseAttemptsRuntime(ICoreDrawPhaseAttemptsOwnedDrawPhaseOperations ownedDrawPhaseOperations)
    {
        this._ownedDrawPhaseOperations = ownedDrawPhaseOperations;
    }
}
