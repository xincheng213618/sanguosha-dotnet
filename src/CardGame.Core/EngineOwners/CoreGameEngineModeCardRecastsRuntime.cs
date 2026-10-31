#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeCardRecastsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly ICoreModeCardRecastsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    internal CoreGameEngineModeCardRecastsRuntime(global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, ICoreModeCardRecastsCoreEquipmentContentOperations coreEquipmentContentOperations)
    {
        this._modeRulesOperations = modeRulesOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
    }
}
