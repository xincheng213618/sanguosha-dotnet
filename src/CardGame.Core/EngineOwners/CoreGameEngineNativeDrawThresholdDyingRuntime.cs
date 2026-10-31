#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeDrawThresholdDyingRuntime
{
    private readonly ICoreNativeDrawThresholdDyingNativeDrawThresholdOperations _nativeDrawThresholdOperations;
    internal CoreGameEngineNativeDrawThresholdDyingRuntime(ICoreNativeDrawThresholdDyingNativeDrawThresholdOperations nativeDrawThresholdOperations)
    {
        this._nativeDrawThresholdOperations = nativeDrawThresholdOperations;
    }
}
