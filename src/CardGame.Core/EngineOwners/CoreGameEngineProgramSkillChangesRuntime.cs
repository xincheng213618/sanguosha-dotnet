#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramSkillChangesRuntime : global::CardGame.Core.IProgramSkillChangeFacts, global::CardGame.Core.IProgramSkillChangeOperations, global::CardGame.Core.IProgramSkillGrantQueries
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillBindingIndexAccess _skillBindingIndexOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineConversionStatesEntries _coreConversionStatesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingOwnerSkillReplacementEntries _coreDyingOwnerSkillReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreProgramSkillChangesProgramSkillChangeOperations _programSkillChangeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess _coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess;
    internal CoreGameEngineProgramSkillChangesRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelSkillBindingIndexAccess skillBindingIndexOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineConversionStatesEntries coreConversionStatesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingOwnerSkillReplacementEntries coreDyingOwnerSkillReplacementOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, ICoreProgramSkillChangesProgramSkillChangeOperations programSkillChangeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._skillBindingIndexOperations = skillBindingIndexOperations;
        this._coreConversionStatesOperations = coreConversionStatesOperations;
        this._coreDyingOwnerSkillReplacementOperations = coreDyingOwnerSkillReplacementOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._programSkillChangeOperations = programSkillChangeOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess = coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess;
    }
}
