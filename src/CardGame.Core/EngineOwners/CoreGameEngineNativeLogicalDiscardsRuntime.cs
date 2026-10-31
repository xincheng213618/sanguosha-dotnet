#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeLogicalDiscardsRuntime
{
    private readonly ICoreNativeLogicalDiscardsNativeLogicalDiscardOperations _nativeLogicalDiscardOperations;
    internal CoreGameEngineNativeLogicalDiscardsRuntime(ICoreNativeLogicalDiscardsNativeLogicalDiscardOperations nativeLogicalDiscardOperations)
    {
        this._nativeLogicalDiscardOperations = nativeLogicalDiscardOperations;
    }
}
