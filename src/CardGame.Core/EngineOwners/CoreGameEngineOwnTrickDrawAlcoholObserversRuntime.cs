#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineOwnTrickDrawAlcoholObserversRuntime
{
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly RuntimeEntries.CoreGameEngineLegacyDyingAlcoholReturnsEntries _coreLegacyDyingAlcoholReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineOwnTrickDrawTargetsEntries _coreOwnTrickDrawTargetsOperations;
    internal CoreGameEngineOwnTrickDrawAlcoholObserversRuntime(IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations, RuntimeEntries.CoreGameEngineLegacyDyingAlcoholReturnsEntries coreLegacyDyingAlcoholReturnsOperations, RuntimeEntries.CoreGameEngineOwnTrickDrawTargetsEntries coreOwnTrickDrawTargetsOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreLegacyDyingAlcoholReturnsOperations = coreLegacyDyingAlcoholReturnsOperations;
        this._coreOwnTrickDrawTargetsOperations = coreOwnTrickDrawTargetsOperations;
    }
}
