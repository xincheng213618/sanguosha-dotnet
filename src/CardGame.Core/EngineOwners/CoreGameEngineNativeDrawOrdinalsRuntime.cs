#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeDrawOrdinalsRuntime
{
    private readonly ICoreNativeDrawOrdinalsNativeDrawOrdinalTriggerOperations _nativeDrawOrdinalTriggerOperations;
    internal CoreGameEngineNativeDrawOrdinalsRuntime(ICoreNativeDrawOrdinalsNativeDrawOrdinalTriggerOperations nativeDrawOrdinalTriggerOperations)
    {
        this._nativeDrawOrdinalTriggerOperations = nativeDrawOrdinalTriggerOperations;
    }
}
