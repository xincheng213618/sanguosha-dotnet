#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeAllegianceRuntime
{
    private readonly ICoreModeAllegianceAllegiancePolicyOperations _allegiancePolicyOperations;
    private readonly ICoreModeAllegianceModeRuntimeOperations _modeRuntimeOperations;
    internal CoreGameEngineModeAllegianceRuntime(ICoreModeAllegianceAllegiancePolicyOperations allegiancePolicyOperations, ICoreModeAllegianceModeRuntimeOperations modeRuntimeOperations)
    {
        this._allegiancePolicyOperations = allegiancePolicyOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
