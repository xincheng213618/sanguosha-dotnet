#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTieredRoundActualTargetWindowsRuntime
{
    private readonly ICoreTieredRoundActualTargetWindowsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreTieredRoundActualTargetWindowsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreTieredRoundActualTargetWindowsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreTieredRoundActualTargetWindowsCoreTieredRoundZeroUsesOperations _coreTieredRoundZeroUsesOperations;
    internal CoreGameEngineTieredRoundActualTargetWindowsRuntime(ICoreTieredRoundActualTargetWindowsResolutionStackOperations resolutionStackOperations, ICoreTieredRoundActualTargetWindowsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreTieredRoundActualTargetWindowsCoreRuntimeOperations coreRuntimeOperations, ICoreTieredRoundActualTargetWindowsCoreTieredRoundZeroUsesOperations coreTieredRoundZeroUsesOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTieredRoundZeroUsesOperations = coreTieredRoundZeroUsesOperations;
    }
}
