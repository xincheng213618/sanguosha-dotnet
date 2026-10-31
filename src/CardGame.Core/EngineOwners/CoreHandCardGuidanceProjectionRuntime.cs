#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreHandCardGuidanceProjectionRuntime
{
    private readonly RuntimeEntries.CoreGameEngineHandGuidanceEntries _coreHandGuidanceOperations;
    private readonly ICoreHandCardGuidanceProjectionCoreNativeLegalActionsOperations _coreNativeLegalActionsOperations;
    internal CoreHandCardGuidanceProjectionRuntime(RuntimeEntries.CoreGameEngineHandGuidanceEntries coreHandGuidanceOperations, ICoreHandCardGuidanceProjectionCoreNativeLegalActionsOperations coreNativeLegalActionsOperations)
    {
        this._coreHandGuidanceOperations = coreHandGuidanceOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
    }
}
