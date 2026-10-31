#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFaultBoundaryRuntime
{
    private readonly IKernelCommandSessionAccess _commandSessionOperations;
    private readonly IKernelCommandRuntimeAccess _kernelCoreGameEngineCommandSessionOperations;
    internal CoreGameEngineFaultBoundaryRuntime(IKernelCommandSessionAccess commandSessionOperations, IKernelCommandRuntimeAccess kernelCoreGameEngineCommandSessionOperations)
    {
        this._commandSessionOperations = commandSessionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
    }
}
