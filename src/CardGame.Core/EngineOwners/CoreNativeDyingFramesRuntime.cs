#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDyingFramesRuntime
{
    private readonly ICoreNativeDyingFramesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativeDyingFramesCoreRuntimeOperations _coreRuntimeOperations;
    internal CoreNativeDyingFramesRuntime(ICoreNativeDyingFramesResolutionStackOperations resolutionStackOperations, ICoreNativeDyingFramesCoreRuntimeOperations coreRuntimeOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
    }
}
