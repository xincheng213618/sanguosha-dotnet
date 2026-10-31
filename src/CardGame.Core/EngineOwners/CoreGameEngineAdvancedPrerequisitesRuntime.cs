#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAdvancedPrerequisitesRuntime
{
    private readonly ICoreAdvancedPrerequisitesCharacterGuoJiaMobileGodOperations _characterGuoJiaMobileGodOperations;
    private readonly ICoreAdvancedPrerequisitesCardZonesOperations _cardZonesOperations;
    private readonly ICoreAdvancedPrerequisitesSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreAdvancedPrerequisitesCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICoreAdvancedPrerequisitesCoreLordSkillProjectionsOperations _coreLordSkillProjectionsOperations;
    private readonly ICoreAdvancedPrerequisitesCorePlayCardHistoryOperations _corePlayCardHistoryOperations;
    private readonly ICoreAdvancedPrerequisitesCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAdvancedPrerequisitesRuntime(ICoreAdvancedPrerequisitesCharacterGuoJiaMobileGodOperations characterGuoJiaMobileGodOperations, ICoreAdvancedPrerequisitesCardZonesOperations cardZonesOperations, ICoreAdvancedPrerequisitesSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreAdvancedPrerequisitesCoreEquipmentContentOperations coreEquipmentContentOperations, ICoreAdvancedPrerequisitesCoreLordSkillProjectionsOperations coreLordSkillProjectionsOperations, ICoreAdvancedPrerequisitesCorePlayCardHistoryOperations corePlayCardHistoryOperations, ICoreAdvancedPrerequisitesCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
