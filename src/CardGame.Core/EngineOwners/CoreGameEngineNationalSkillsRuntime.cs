#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNationalSkillsRuntime
{
    private readonly ICoreNationalSkillsCharacterCaoPiAnniversaryGodOperations _characterCaoPiAnniversaryGodOperations;
    private readonly ICoreNationalSkillsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreNationalSkillsSkillBindingIndexOperations _skillBindingIndexOperations;
    private readonly ICoreNationalSkillsCoreCurrentTurnSkillSuppressionsOperations _coreCurrentTurnSkillSuppressionsOperations;
    private readonly ICoreNationalSkillsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreNationalSkillsCorePrintedLordSkillQualificationsOperations _corePrintedLordSkillQualificationsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess _coreGameEngineActionQueriesActionQueryAccess;
    internal CoreGameEngineNationalSkillsRuntime(ICoreNationalSkillsCharacterCaoPiAnniversaryGodOperations characterCaoPiAnniversaryGodOperations, ICoreNationalSkillsContentRegistryOperations contentRegistryOperations, ICoreNationalSkillsSkillBindingIndexOperations skillBindingIndexOperations, ICoreNationalSkillsCoreCurrentTurnSkillSuppressionsOperations coreCurrentTurnSkillSuppressionsOperations, ICoreNationalSkillsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreNationalSkillsCorePrintedLordSkillQualificationsOperations corePrintedLordSkillQualificationsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess coreGameEngineActionQueriesActionQueryAccess)
    {
        this._characterCaoPiAnniversaryGodOperations = characterCaoPiAnniversaryGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._skillBindingIndexOperations = skillBindingIndexOperations;
        this._coreCurrentTurnSkillSuppressionsOperations = coreCurrentTurnSkillSuppressionsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._corePrintedLordSkillQualificationsOperations = corePrintedLordSkillQualificationsOperations;
        this._coreMatchDefinitionModeDefinitionAccess = coreMatchDefinitionModeDefinitionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineActionQueriesActionQueryAccess = coreGameEngineActionQueriesActionQueryAccess;
    }
}
