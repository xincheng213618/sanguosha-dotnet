#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEquipmentSkillsRuntime
{
    private readonly ICoreEquipmentSkillsCharacterWeiYanHappyGodOperations _characterWeiYanHappyGodOperations;
    private readonly ICoreEquipmentSkillsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreEquipmentSkillsCoreAdvancedWeaponsOperations _coreAdvancedWeaponsOperations;
    private readonly ICoreEquipmentSkillsCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineEquipmentSkillsRuntime(ICoreEquipmentSkillsCharacterWeiYanHappyGodOperations characterWeiYanHappyGodOperations, ICoreEquipmentSkillsContentRegistryOperations contentRegistryOperations, ICoreEquipmentSkillsCoreAdvancedWeaponsOperations coreAdvancedWeaponsOperations, ICoreEquipmentSkillsCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAdvancedWeaponsOperations = coreAdvancedWeaponsOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
