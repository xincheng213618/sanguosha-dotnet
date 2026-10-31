#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAdvancedPrerequisitesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoJiaMobileGodEntries _characterGuoJiaMobileGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly ICoreAdvancedPrerequisitesCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries _coreLordSkillProjectionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAdvancedPrerequisitesRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuoJiaMobileGodEntries characterGuoJiaMobileGodOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, ICoreAdvancedPrerequisitesCoreEquipmentContentOperations coreEquipmentContentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries coreLordSkillProjectionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuoJiaMobileGodOperations = characterGuoJiaMobileGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
