#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRuleProgressionRuntime
{
    private readonly IKernelProgramRuleObserversAccess _programRuleObserversOperations;
    private readonly IKernelCreateRuleProgressionOperationsOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess _coreGameEngineRuleProgressionRuleProgressionAccess;
    internal CoreGameEngineRuleProgressionRuntime(IKernelProgramRuleObserversAccess programRuleObserversOperations, IKernelCreateRuleProgressionOperationsOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess coreGameEngineRuleProgressionRuleProgressionAccess)
    {
        this._programRuleObserversOperations = programRuleObserversOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreGameEngineRuleProgressionRuleProgressionAccess = coreGameEngineRuleProgressionRuleProgressionAccess;
    }
}
