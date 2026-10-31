#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchDefinitionQueriesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
    internal CoreMatchDefinitionQueriesRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess = coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
    }
}
