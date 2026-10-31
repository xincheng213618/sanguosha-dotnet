#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramRuntimeHooksRuntime
{
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly IKernelRuntimeHooksAccess _runtimeHooksOperations;
    private readonly IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations;
    internal CoreProgramRuntimeHooksRuntime(IKernelMatchModeRuntimeAccess modeRuntimeOperations, IKernelRuntimeHooksAccess runtimeHooksOperations, IKernelBindRuntimeHandlerOperations kernelCoreProgramRuntimeModulesOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._runtimeHooksOperations = runtimeHooksOperations;
        this._kernelCoreProgramRuntimeModulesOperations = kernelCoreProgramRuntimeModulesOperations;
    }
}
