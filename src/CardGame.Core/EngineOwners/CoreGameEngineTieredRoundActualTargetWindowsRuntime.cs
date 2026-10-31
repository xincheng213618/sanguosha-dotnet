#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTieredRoundActualTargetWindowsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries _coreTieredRoundZeroUsesOperations;
    internal CoreGameEngineTieredRoundActualTargetWindowsRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries coreTieredRoundZeroUsesOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTieredRoundZeroUsesOperations = coreTieredRoundZeroUsesOperations;
    }
}
