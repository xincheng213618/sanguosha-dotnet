#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNationalGeneralRevealRuntime
{
    private readonly ICoreNationalGeneralRevealCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreNationalGeneralRevealCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreNationalGeneralRevealKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreNationalGeneralRevealRuntime(ICoreNationalGeneralRevealCoreMatchLogOperations coreMatchLogOperations, ICoreNationalGeneralRevealCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreNationalGeneralRevealKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
