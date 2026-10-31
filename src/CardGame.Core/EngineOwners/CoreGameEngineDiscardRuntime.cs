#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDiscardRuntime
{
    private readonly ICoreDiscardManualDiscardOperations _manualDiscardOperations;
    internal CoreGameEngineDiscardRuntime(ICoreDiscardManualDiscardOperations manualDiscardOperations)
    {
        this._manualDiscardOperations = manualDiscardOperations;
    }
}
