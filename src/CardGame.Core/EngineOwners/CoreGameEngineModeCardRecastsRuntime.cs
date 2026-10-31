#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeCardRecastsRuntime
{
    private readonly ICoreModeCardRecastsModeRulesOperations _modeRulesOperations;
    private readonly ICoreModeCardRecastsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    internal CoreGameEngineModeCardRecastsRuntime(ICoreModeCardRecastsModeRulesOperations modeRulesOperations, ICoreModeCardRecastsCoreEquipmentContentOperations coreEquipmentContentOperations)
    {
        this._modeRulesOperations = modeRulesOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
    }
}
