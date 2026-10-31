#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActivityOrdinaryTricksRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess _coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess;
    internal CoreGameEngineActivityOrdinaryTricksRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess = coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess;
    }
}
