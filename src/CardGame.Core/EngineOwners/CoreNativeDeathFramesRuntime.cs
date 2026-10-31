#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDeathFramesRuntime
{
    private readonly ICoreNativeDeathFramesCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreNativeDeathFramesCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    internal CoreNativeDeathFramesRuntime(ICoreNativeDeathFramesCoreRuntimeOperations coreRuntimeOperations, ICoreNativeDeathFramesCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations)
    {
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
    }
}
