#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreResolutionIdentityBoundaryRuntime
{
    private readonly ICoreResolutionIdentityBoundaryResolutionSchedulerOperations _resolutionSchedulerOperations;
    internal CoreResolutionIdentityBoundaryRuntime(ICoreResolutionIdentityBoundaryResolutionSchedulerOperations resolutionSchedulerOperations)
    {
        this._resolutionSchedulerOperations = resolutionSchedulerOperations;
    }
}
