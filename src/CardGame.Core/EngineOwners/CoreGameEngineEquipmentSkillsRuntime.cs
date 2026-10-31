#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEquipmentSkillsRuntime
{
    private readonly RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries _coreAdvancedWeaponsOperations;
    private readonly ICoreEquipmentSkillsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineEquipmentSkillsRuntime(RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries coreAdvancedWeaponsOperations, ICoreEquipmentSkillsCoreEquipmentContentOperations coreEquipmentContentOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
