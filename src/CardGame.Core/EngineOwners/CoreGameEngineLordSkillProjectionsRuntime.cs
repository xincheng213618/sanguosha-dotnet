#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineLordSkillProjectionsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries _coreCurrentTurnSkillSuppressionsOperations;
    private readonly ICoreLordSkillProjectionsCoreModeAllegianceOperations _coreModeAllegianceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries _corePrivateGeneralLibrariesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePrintedLordSkillQualificationsEntries _corePrintedLordSkillQualificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreLordSkillProjectionsBoundaryYuanShuOperations _boundaryYuanShuOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess _coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess;
    internal CoreGameEngineLordSkillProjectionsRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries coreCurrentTurnSkillSuppressionsOperations, ICoreLordSkillProjectionsCoreModeAllegianceOperations coreModeAllegianceOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries corePrivateGeneralLibrariesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CorePrintedLordSkillQualificationsEntries corePrintedLordSkillQualificationsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, ICoreLordSkillProjectionsBoundaryYuanShuOperations boundaryYuanShuOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreCurrentTurnSkillSuppressionsOperations = coreCurrentTurnSkillSuppressionsOperations;
        this._coreModeAllegianceOperations = coreModeAllegianceOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePrivateGeneralLibrariesOperations = corePrivateGeneralLibrariesOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._corePrintedLordSkillQualificationsOperations = corePrintedLordSkillQualificationsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._boundaryYuanShuOperations = boundaryYuanShuOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess = coreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess;
    }
}
