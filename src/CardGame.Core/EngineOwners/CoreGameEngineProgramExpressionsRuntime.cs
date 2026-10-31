#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramExpressionsRuntime
{
    private readonly ICoreProgramExpressionsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreProgramExpressionsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramExpressionsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramExpressionsKernelCoreGameEngineProgramExpressionsOperations _kernelCoreGameEngineProgramExpressionsOperations;
    private readonly ICoreProgramExpressionsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineProgramExpressionsRuntime(ICoreProgramExpressionsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreProgramExpressionsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramExpressionsCoreRuntimeOperations coreRuntimeOperations, ICoreProgramExpressionsKernelCoreGameEngineProgramExpressionsOperations kernelCoreGameEngineProgramExpressionsOperations, ICoreProgramExpressionsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineProgramExpressionsOperations = kernelCoreGameEngineProgramExpressionsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
