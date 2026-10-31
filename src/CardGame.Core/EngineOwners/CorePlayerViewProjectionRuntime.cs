#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePlayerViewProjectionRuntime
{
    private readonly ICorePlayerViewProjectionCorePlayerViewProjectionQueriesOperations _corePlayerViewProjectionQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    internal CorePlayerViewProjectionRuntime(ICorePlayerViewProjectionCorePlayerViewProjectionQueriesOperations corePlayerViewProjectionQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess)
    {
        this._corePlayerViewProjectionQueriesOperations = corePlayerViewProjectionQueriesOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
    }
}
