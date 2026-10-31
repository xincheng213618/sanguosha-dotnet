#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFaultBoundaryRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCommandSessionAccess _commandSessionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCommandRuntimeAccess _kernelCoreGameEngineCommandSessionOperations;
    internal CoreGameEngineFaultBoundaryRuntime(global::CardGame.Core.EngineOwners.IKernelCommandSessionAccess commandSessionOperations, global::CardGame.Core.EngineOwners.IKernelCommandRuntimeAccess kernelCoreGameEngineCommandSessionOperations)
    {
        this._commandSessionOperations = commandSessionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
    }
}
