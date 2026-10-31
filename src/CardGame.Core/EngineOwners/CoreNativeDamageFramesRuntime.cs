#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDamageFramesRuntime
{
    private readonly ICoreNativeDamageFramesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativeDamageFramesCoreDynamicDiscardDamagePaymentsOperations _coreDynamicDiscardDamagePaymentsOperations;
    private readonly ICoreNativeDamageFramesCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreNativeDamageFramesCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    internal CoreNativeDamageFramesRuntime(ICoreNativeDamageFramesResolutionStackOperations resolutionStackOperations, ICoreNativeDamageFramesCoreDynamicDiscardDamagePaymentsOperations coreDynamicDiscardDamagePaymentsOperations, ICoreNativeDamageFramesCoreRuntimeOperations coreRuntimeOperations, ICoreNativeDamageFramesCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDynamicDiscardDamagePaymentsOperations = coreDynamicDiscardDamagePaymentsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
    }
}
