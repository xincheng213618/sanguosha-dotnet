#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramDamageHooksRuntime
{
    private readonly ICoreProgramDamageHooksDamageHooksOperations _damageHooksOperations;
    private readonly ICoreProgramDamageHooksModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreProgramDamageHooksCoreNativeCardClassificationsOperations _coreNativeCardClassificationsOperations;
    private readonly ICoreProgramDamageHooksKernelCoreProgramRuntimeModulesOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramDamageHooksRuntime(ICoreProgramDamageHooksDamageHooksOperations damageHooksOperations, ICoreProgramDamageHooksModeRuntimeOperations modeRuntimeOperations, ICoreProgramDamageHooksCoreNativeCardClassificationsOperations coreNativeCardClassificationsOperations, ICoreProgramDamageHooksKernelCoreProgramRuntimeModulesOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._damageHooksOperations = damageHooksOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
