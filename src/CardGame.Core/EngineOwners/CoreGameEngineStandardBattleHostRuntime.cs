#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineStandardBattleHostRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelCreateStandardBattleHostOperations _kernelCoreGameEngineStandardBattleHostOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    internal CoreGameEngineStandardBattleHostRuntime(IKernelContentRegistryAccess contentRegistryOperations, IKernelCreateStandardBattleHostOperations kernelCoreGameEngineStandardBattleHostOperations, EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._kernelCoreGameEngineStandardBattleHostOperations = kernelCoreGameEngineStandardBattleHostOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
    }
}
