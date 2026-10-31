#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramExpressionBindingsRuntime : global::CardGame.Core.IProgramExpressionCaptureHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramExpressionsEntries _coreProgramExpressionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramExpressionWorldOperations _kernelCoreGameEngineProgramExpressionsOperations;
    internal CoreProgramExpressionBindingsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramExpressionsEntries coreProgramExpressionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.EngineOwners.IKernelCreateProgramExpressionWorldOperations kernelCoreGameEngineProgramExpressionsOperations)
    {
        this._coreProgramExpressionsOperations = coreProgramExpressionsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineProgramExpressionsOperations = kernelCoreGameEngineProgramExpressionsOperations;
    }
}
