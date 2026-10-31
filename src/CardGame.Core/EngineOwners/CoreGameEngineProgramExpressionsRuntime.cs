#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramExpressionsRuntime
{
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly IKernelCreateProgramExpressionWorldOperations _kernelCoreGameEngineProgramExpressionsOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineProgramExpressionsRuntime(RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,IKernelCreateProgramExpressionWorldOperations kernelCoreGameEngineProgramExpressionsOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineProgramExpressionsOperations = kernelCoreGameEngineProgramExpressionsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
