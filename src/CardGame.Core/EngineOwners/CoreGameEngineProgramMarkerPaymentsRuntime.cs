#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramMarkerPaymentsRuntime
{
    private readonly ICoreProgramMarkerPaymentsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreProgramMarkerPaymentsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineProgramMarkerPaymentsRuntime(ICoreProgramMarkerPaymentsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreProgramMarkerPaymentsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
