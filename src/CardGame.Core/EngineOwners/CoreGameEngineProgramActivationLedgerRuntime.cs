#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramActivationLedgerRuntime
{
    private readonly ICoreProgramActivationLedgerSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreProgramActivationLedgerCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramActivationLedgerCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramActivationLedgerKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineProgramActivationLedgerRuntime(ICoreProgramActivationLedgerSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreProgramActivationLedgerCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramActivationLedgerCoreRuntimeOperations coreRuntimeOperations, ICoreProgramActivationLedgerKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
