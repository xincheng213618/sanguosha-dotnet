#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramExpressionBindingsRuntime : global::CardGame.Core.IProgramExpressionCaptureHost
{
    private readonly ICoreProgramExpressionBindingsCoreProgramExpressionsOperations _coreProgramExpressionsOperations;
    private readonly ICoreProgramExpressionBindingsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramExpressionBindingsKernelCoreGameEngineProgramExpressionsOperations _kernelCoreGameEngineProgramExpressionsOperations;
    internal CoreProgramExpressionBindingsRuntime(ICoreProgramExpressionBindingsCoreProgramExpressionsOperations coreProgramExpressionsOperations, ICoreProgramExpressionBindingsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramExpressionBindingsKernelCoreGameEngineProgramExpressionsOperations kernelCoreGameEngineProgramExpressionsOperations)
    {
        this._coreProgramExpressionsOperations = coreProgramExpressionsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineProgramExpressionsOperations = kernelCoreGameEngineProgramExpressionsOperations;
    }
}
