#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreResolutionIdentityBoundaryRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelResolutionSchedulerAccess _resolutionSchedulerOperations;
    internal CoreResolutionIdentityBoundaryRuntime(global::CardGame.Core.EngineOwners.IKernelResolutionSchedulerAccess resolutionSchedulerOperations)
    {
        this._resolutionSchedulerOperations = resolutionSchedulerOperations;
    }
}
