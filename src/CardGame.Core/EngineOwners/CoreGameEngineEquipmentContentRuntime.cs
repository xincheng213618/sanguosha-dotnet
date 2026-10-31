#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEquipmentContentRuntime
{
    private readonly ICoreEquipmentContentContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreEquipmentContentEquipmentRulesOperations _equipmentRulesOperations;
    private readonly ICoreEquipmentContentModeRuntimeOperations _modeRuntimeOperations;
    internal CoreGameEngineEquipmentContentRuntime(ICoreEquipmentContentContentRegistryOperations contentRegistryOperations, ICoreEquipmentContentEquipmentRulesOperations equipmentRulesOperations, ICoreEquipmentContentModeRuntimeOperations modeRuntimeOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._equipmentRulesOperations = equipmentRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
