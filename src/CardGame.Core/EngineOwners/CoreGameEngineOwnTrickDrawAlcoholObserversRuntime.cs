#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineOwnTrickDrawAlcoholObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLegacyDyingAlcoholReturnsEntries _coreLegacyDyingAlcoholReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnTrickDrawTargetsEntries _coreOwnTrickDrawTargetsOperations;
    internal CoreGameEngineOwnTrickDrawAlcoholObserversRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLegacyDyingAlcoholReturnsEntries coreLegacyDyingAlcoholReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnTrickDrawTargetsEntries coreOwnTrickDrawTargetsOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreLegacyDyingAlcoholReturnsOperations = coreLegacyDyingAlcoholReturnsOperations;
        this._coreOwnTrickDrawTargetsOperations = coreOwnTrickDrawTargetsOperations;
    }
}
