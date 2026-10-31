#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineOwnTrickDrawAlcoholObserversRuntime
{
    private readonly ICoreOwnTrickDrawAlcoholObserversResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreOwnTrickDrawAlcoholObserversCoreHalfHandPhaseDebtDamageObserversOperations _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly ICoreOwnTrickDrawAlcoholObserversCoreLegacyDyingAlcoholReturnsOperations _coreLegacyDyingAlcoholReturnsOperations;
    private readonly ICoreOwnTrickDrawAlcoholObserversCoreOwnTrickDrawTargetsOperations _coreOwnTrickDrawTargetsOperations;
    internal CoreGameEngineOwnTrickDrawAlcoholObserversRuntime(ICoreOwnTrickDrawAlcoholObserversResolutionStackOperations resolutionStackOperations, ICoreOwnTrickDrawAlcoholObserversCoreHalfHandPhaseDebtDamageObserversOperations coreHalfHandPhaseDebtDamageObserversOperations, ICoreOwnTrickDrawAlcoholObserversCoreLegacyDyingAlcoholReturnsOperations coreLegacyDyingAlcoholReturnsOperations, ICoreOwnTrickDrawAlcoholObserversCoreOwnTrickDrawTargetsOperations coreOwnTrickDrawTargetsOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreLegacyDyingAlcoholReturnsOperations = coreLegacyDyingAlcoholReturnsOperations;
        this._coreOwnTrickDrawTargetsOperations = coreOwnTrickDrawTargetsOperations;
    }
}
