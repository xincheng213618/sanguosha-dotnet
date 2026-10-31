#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRuleProgressionRuntime
{
    private readonly ICoreRuleProgressionProgramRuleObserversOperations _programRuleObserversOperations;
    private readonly ICoreRuleProgressionKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess _coreGameEngineRuleProgressionRuleProgressionAccess;
    internal CoreGameEngineRuleProgressionRuntime(ICoreRuleProgressionProgramRuleObserversOperations programRuleObserversOperations, ICoreRuleProgressionKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess coreGameEngineRuleProgressionRuleProgressionAccess)
    {
        this._programRuleObserversOperations = programRuleObserversOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreGameEngineRuleProgressionRuleProgressionAccess = coreGameEngineRuleProgressionRuleProgressionAccess;
    }
}
