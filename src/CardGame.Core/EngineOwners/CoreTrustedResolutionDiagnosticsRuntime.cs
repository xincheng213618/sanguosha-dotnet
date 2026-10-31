#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreTrustedResolutionDiagnosticsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    internal CoreTrustedResolutionDiagnosticsRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
