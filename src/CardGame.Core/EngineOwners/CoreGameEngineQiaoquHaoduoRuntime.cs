#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineQiaoquHaoduoRuntime
{
    private readonly ICoreQiaoquHaoduoResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreQiaoquHaoduoCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreQiaoquHaoduoCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    internal CoreGameEngineQiaoquHaoduoRuntime(ICoreQiaoquHaoduoResolutionStackOperations resolutionStackOperations, ICoreQiaoquHaoduoCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreQiaoquHaoduoCoreTableSeatQueriesOperations coreTableSeatQueriesOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
    }
}
