#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramDamageReductionRuntime
{
    private readonly ICoreProgramDamageReductionResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramDamageReductionCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    private readonly ICoreProgramDamageReductionCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramDamageReductionKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineProgramDamageReductionRuntime(ICoreProgramDamageReductionResolutionStackOperations resolutionStackOperations, ICoreProgramDamageReductionCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations, ICoreProgramDamageReductionCoreRuntimeOperations coreRuntimeOperations, ICoreProgramDamageReductionKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
