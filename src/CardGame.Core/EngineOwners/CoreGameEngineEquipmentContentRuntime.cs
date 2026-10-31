#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEquipmentContentRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelEquipmentRulesAccess _equipmentRulesOperations;
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    internal CoreGameEngineEquipmentContentRuntime(IKernelContentRegistryAccess contentRegistryOperations, IKernelEquipmentRulesAccess equipmentRulesOperations, IKernelMatchModeRuntimeAccess modeRuntimeOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._equipmentRulesOperations = equipmentRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
