#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineStandardBattleHostRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateStandardBattleHostOperations _kernelCoreGameEngineStandardBattleHostOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    internal CoreGameEngineStandardBattleHostRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelCreateStandardBattleHostOperations kernelCoreGameEngineStandardBattleHostOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._kernelCoreGameEngineStandardBattleHostOperations = kernelCoreGameEngineStandardBattleHostOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
    }
}
