#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNationalGeneralRevealRuntime
{
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreNationalGeneralRevealRuntime(RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
