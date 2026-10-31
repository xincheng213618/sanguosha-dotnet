#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNationalSkillsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries _characterCaoPiAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillBindingIndexAccess _skillBindingIndexOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries _coreCurrentTurnSkillSuppressionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePrintedLordSkillQualificationsEntries _corePrintedLordSkillQualificationsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess _coreGameEngineActionQueriesActionQueryAccess;
    internal CoreGameEngineNationalSkillsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries characterCaoPiAnniversaryGodOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelSkillBindingIndexAccess skillBindingIndexOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries coreCurrentTurnSkillSuppressionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CorePrintedLordSkillQualificationsEntries corePrintedLordSkillQualificationsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess coreGameEngineActionQueriesActionQueryAccess)
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
