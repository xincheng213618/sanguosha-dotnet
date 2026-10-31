#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramExpressionBindingsRuntime : IProgramExpressionCaptureHost
{
    private readonly RuntimeEntries.CoreGameEngineProgramExpressionsEntries _coreProgramExpressionsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly IKernelCreateProgramExpressionWorldOperations _kernelCoreGameEngineProgramExpressionsOperations;
    internal CoreProgramExpressionBindingsRuntime(RuntimeEntries.CoreGameEngineProgramExpressionsEntries coreProgramExpressionsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, IKernelCreateProgramExpressionWorldOperations kernelCoreGameEngineProgramExpressionsOperations)
    {
        this._coreProgramExpressionsOperations = coreProgramExpressionsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineProgramExpressionsOperations = kernelCoreGameEngineProgramExpressionsOperations;
    }
}
