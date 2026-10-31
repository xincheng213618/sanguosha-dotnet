#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreHandCardGuidanceProjectionRuntime
{
    private readonly ICoreHandCardGuidanceProjectionCoreHandGuidanceOperations _coreHandGuidanceOperations;
    private readonly ICoreHandCardGuidanceProjectionCoreNativeLegalActionsOperations _coreNativeLegalActionsOperations;
    internal CoreHandCardGuidanceProjectionRuntime(ICoreHandCardGuidanceProjectionCoreHandGuidanceOperations coreHandGuidanceOperations, ICoreHandCardGuidanceProjectionCoreNativeLegalActionsOperations coreNativeLegalActionsOperations)
    {
        this._coreHandGuidanceOperations = coreHandGuidanceOperations;
        this._coreNativeLegalActionsOperations = coreNativeLegalActionsOperations;
    }
}
