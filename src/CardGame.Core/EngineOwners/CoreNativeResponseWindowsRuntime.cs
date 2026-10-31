#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeResponseWindowsRuntime
{
    private readonly ICoreNativeResponseWindowsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativeResponseWindowsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreNativeResponseWindowsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    internal CoreNativeResponseWindowsRuntime(ICoreNativeResponseWindowsResolutionStackOperations resolutionStackOperations, ICoreNativeResponseWindowsCoreRuntimeOperations coreRuntimeOperations, ICoreNativeResponseWindowsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
    }
}
