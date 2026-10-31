#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreDrawPhaseObligationsRuntime
{
    private readonly ICoreDrawPhaseObligationsOwnedDrawPhaseOperations _ownedDrawPhaseOperations;
    internal CoreDrawPhaseObligationsRuntime(ICoreDrawPhaseObligationsOwnedDrawPhaseOperations ownedDrawPhaseOperations)
    {
        this._ownedDrawPhaseOperations = ownedDrawPhaseOperations;
    }
}
