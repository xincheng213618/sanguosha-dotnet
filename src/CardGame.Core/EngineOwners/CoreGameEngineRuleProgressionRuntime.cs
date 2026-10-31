#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRuleProgressionRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramRuleObserversAccess _programRuleObserversOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateRuleProgressionOperationsOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess _coreGameEngineRuleProgressionRuleProgressionAccess;
    internal CoreGameEngineRuleProgressionRuntime(global::CardGame.Core.EngineOwners.IKernelProgramRuleObserversAccess programRuleObserversOperations, global::CardGame.Core.EngineOwners.IKernelCreateRuleProgressionOperationsOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess coreGameEngineRuleProgressionRuleProgressionAccess)
    {
        this._programRuleObserversOperations = programRuleObserversOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreGameEngineRuleProgressionRuleProgressionAccess = coreGameEngineRuleProgressionRuleProgressionAccess;
    }
}
