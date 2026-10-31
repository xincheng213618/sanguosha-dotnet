#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineStandardBattleHostRuntime
{
    private readonly ICoreStandardBattleHostContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreStandardBattleHostKernelCoreGameEngineStandardBattleHostOperations _kernelCoreGameEngineStandardBattleHostOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    internal CoreGameEngineStandardBattleHostRuntime(ICoreStandardBattleHostContentRegistryOperations contentRegistryOperations, ICoreStandardBattleHostKernelCoreGameEngineStandardBattleHostOperations kernelCoreGameEngineStandardBattleHostOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._kernelCoreGameEngineStandardBattleHostOperations = kernelCoreGameEngineStandardBattleHostOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
    }
}
