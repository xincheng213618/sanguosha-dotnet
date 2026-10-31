#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheDoubleStealRuntime : global::CardGame.Core.IShanheDoubleStealHost
{
    private readonly ICoreShanheDoubleStealResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheDoubleStealCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreShanheDoubleStealCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheDoubleStealCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheDoubleStealKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineShanheDoubleStealRuntime(ICoreShanheDoubleStealResolutionStackOperations resolutionStackOperations, ICoreShanheDoubleStealCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreShanheDoubleStealCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheDoubleStealCoreRuntimeOperations coreRuntimeOperations, ICoreShanheDoubleStealKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
