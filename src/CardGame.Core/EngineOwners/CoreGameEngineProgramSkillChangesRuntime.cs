#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramSkillChangesRuntime : global::CardGame.Core.IProgramSkillChangeFacts, global::CardGame.Core.IProgramSkillChangeOperations, global::CardGame.Core.IProgramSkillGrantQueries
{
    private readonly ICoreProgramSkillChangesContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramSkillChangesSkillBindingIndexOperations _skillBindingIndexOperations;
    private readonly ICoreProgramSkillChangesCoreConversionStatesOperations _coreConversionStatesOperations;
    private readonly ICoreProgramSkillChangesCoreDyingOwnerSkillReplacementOperations _coreDyingOwnerSkillReplacementOperations;
    private readonly ICoreProgramSkillChangesCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramSkillChangesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramSkillChangesCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICoreProgramSkillChangesCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreProgramSkillChangesCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramSkillChangesCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreProgramSkillChangesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreProgramSkillChangesProgramSkillChangeOperations _programSkillChangeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess _coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess;
    internal CoreGameEngineProgramSkillChangesRuntime(ICoreProgramSkillChangesContentRegistryOperations contentRegistryOperations, ICoreProgramSkillChangesSkillBindingIndexOperations skillBindingIndexOperations, ICoreProgramSkillChangesCoreConversionStatesOperations coreConversionStatesOperations, ICoreProgramSkillChangesCoreDyingOwnerSkillReplacementOperations coreDyingOwnerSkillReplacementOperations, ICoreProgramSkillChangesCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramSkillChangesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramSkillChangesCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICoreProgramSkillChangesCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreProgramSkillChangesCoreMatchLogOperations coreMatchLogOperations, ICoreProgramSkillChangesCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreProgramSkillChangesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, ICoreProgramSkillChangesProgramSkillChangeOperations programSkillChangeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess)
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
