#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRecoveryDamageEdgesRuntime
{
    private readonly RuntimeEntries.CoreGameEngineNativeRecoveryDamageEntries _coreNativeRecoveryDamageOperations;
    internal CoreGameEngineRecoveryDamageEdgesRuntime(RuntimeEntries.CoreGameEngineNativeRecoveryDamageEntries coreNativeRecoveryDamageOperations)
    {
        this._coreNativeRecoveryDamageOperations = coreNativeRecoveryDamageOperations;
    }
}
