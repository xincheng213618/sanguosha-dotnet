#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchDefinitionQueriesRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
    internal CoreMatchDefinitionQueriesRuntime(IKernelContentRegistryAccess contentRegistryOperations, EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess = coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
    }
}
