#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePersistentPrivateWhirlRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    internal CoreGameEnginePersistentPrivateWhirlRuntime(IKernelContentRegistryAccess contentRegistryOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
    }
}
