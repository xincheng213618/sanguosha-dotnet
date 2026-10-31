#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCompletedUseChildReturnsRuntime
{
    private readonly ICoreCompletedUseChildReturnsCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICoreCompletedUseChildReturnsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreCompletedUseChildReturnsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreCompletedUseChildReturnsKernelCoreMainOperations _kernelCoreMainOperations;
    internal CoreGameEngineCompletedUseChildReturnsRuntime(ICoreCompletedUseChildReturnsCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICoreCompletedUseChildReturnsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreCompletedUseChildReturnsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreCompletedUseChildReturnsKernelCoreMainOperations kernelCoreMainOperations)
    {
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
    }
}
