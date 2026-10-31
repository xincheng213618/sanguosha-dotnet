#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramSkillChangesRuntime : IProgramSkillChangeFacts, IProgramSkillChangeOperations, IProgramSkillGrantQueries
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelSkillBindingIndexAccess _skillBindingIndexOperations;
    private readonly RuntimeEntries.CoreGameEngineConversionStatesEntries _coreConversionStatesOperations;
    private readonly RuntimeEntries.CoreGameEngineDyingOwnerSkillReplacementEntries _coreDyingOwnerSkillReplacementOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreProgramSkillChangesProgramSkillChangeOperations _programSkillChangeOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess _coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess;
    internal CoreGameEngineProgramSkillChangesRuntime(IKernelContentRegistryAccess contentRegistryOperations, IKernelSkillBindingIndexAccess skillBindingIndexOperations, RuntimeEntries.CoreGameEngineConversionStatesEntries coreConversionStatesOperations, RuntimeEntries.CoreGameEngineDyingOwnerSkillReplacementEntries coreDyingOwnerSkillReplacementOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, ICoreProgramSkillChangesProgramSkillChangeOperations programSkillChangeOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess)
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
