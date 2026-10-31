#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFaultBoundaryRuntime
{
    private readonly ICoreFaultBoundaryCommandSessionOperations _commandSessionOperations;
    private readonly ICoreFaultBoundaryKernelCoreGameEngineCommandSessionOperations _kernelCoreGameEngineCommandSessionOperations;
    internal CoreGameEngineFaultBoundaryRuntime(ICoreFaultBoundaryCommandSessionOperations commandSessionOperations, ICoreFaultBoundaryKernelCoreGameEngineCommandSessionOperations kernelCoreGameEngineCommandSessionOperations)
    {
        this._commandSessionOperations = commandSessionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
    }
}
