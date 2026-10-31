#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHpConditionalSkillGrantsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTangZiOlEntries _characterTangZiOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries _coreCurrentTurnSkillSuppressionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries _coreLordSkillProjectionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries _corePrivateGeneralLibrariesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess _coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess;
    internal CoreGameEngineHpConditionalSkillGrantsRuntime(global::CardGame.Core.RuntimeEntries.CharacterTangZiOlEntries characterTangZiOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries coreCurrentTurnSkillSuppressionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries coreLordSkillProjectionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries corePrivateGeneralLibrariesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess)
    {
        this._characterTangZiOlOperations = characterTangZiOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreCurrentTurnSkillSuppressionsOperations = coreCurrentTurnSkillSuppressionsOperations;
        this._coreLordSkillProjectionsOperations = coreLordSkillProjectionsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePrivateGeneralLibrariesOperations = corePrivateGeneralLibrariesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess = coreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess;
    }
}
