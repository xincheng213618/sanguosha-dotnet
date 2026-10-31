#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeDrawAttemptsRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly ICoreNativeDrawAttemptsNativeDrawAttemptOperations _nativeDrawAttemptOperations;
    internal CoreGameEngineNativeDrawAttemptsRuntime(IKernelContentRegistryAccess contentRegistryOperations, ICoreNativeDrawAttemptsNativeDrawAttemptOperations nativeDrawAttemptOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._nativeDrawAttemptOperations = nativeDrawAttemptOperations;
    }
}
