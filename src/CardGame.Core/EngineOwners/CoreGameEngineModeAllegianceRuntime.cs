#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeAllegianceRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelAllegiancePolicyAccess _allegiancePolicyOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    internal CoreGameEngineModeAllegianceRuntime(global::CardGame.Core.EngineOwners.IKernelAllegiancePolicyAccess allegiancePolicyOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations)
    {
        this._allegiancePolicyOperations = allegiancePolicyOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
