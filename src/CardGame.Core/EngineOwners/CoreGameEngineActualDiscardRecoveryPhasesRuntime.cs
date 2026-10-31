#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActualDiscardRecoveryPhasesRuntime
{
    private readonly ICoreActualDiscardRecoveryPhasesActualDiscardRecoveryPhaseOperations _actualDiscardRecoveryPhaseOperations;
    internal CoreGameEngineActualDiscardRecoveryPhasesRuntime(ICoreActualDiscardRecoveryPhasesActualDiscardRecoveryPhaseOperations actualDiscardRecoveryPhaseOperations)
    {
        this._actualDiscardRecoveryPhaseOperations = actualDiscardRecoveryPhaseOperations;
    }
}
