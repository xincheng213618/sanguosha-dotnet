#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeRecoveryDamageRuntime
{
    private readonly ICoreNativeRecoveryDamageNativeHealthRecoveryOperations _nativeHealthRecoveryOperations;
    internal CoreGameEngineNativeRecoveryDamageRuntime(ICoreNativeRecoveryDamageNativeHealthRecoveryOperations nativeHealthRecoveryOperations)
    {
        this._nativeHealthRecoveryOperations = nativeHealthRecoveryOperations;
    }
}
