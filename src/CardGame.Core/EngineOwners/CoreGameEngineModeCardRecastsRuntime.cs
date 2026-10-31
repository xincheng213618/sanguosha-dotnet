#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineModeCardRecastsRuntime
{
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly ICoreModeCardRecastsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    internal CoreGameEngineModeCardRecastsRuntime(IKernelMatchModeRulesAccess modeRulesOperations, ICoreModeCardRecastsCoreEquipmentContentOperations coreEquipmentContentOperations)
    {
        this._modeRulesOperations = modeRulesOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
    }
}
