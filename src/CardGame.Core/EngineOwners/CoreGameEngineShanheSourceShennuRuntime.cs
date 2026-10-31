#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSourceShennuRuntime : global::CardGame.Core.IShanheSourceShennuHost
{
    private readonly ICoreShanheSourceShennuContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheSourceShennuResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheSourceShennuCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreShanheSourceShennuCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheSourceShennuCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheSourceShennuCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheSourceShennuCoreStrategicProgramsOperations _coreStrategicProgramsOperations;
    private readonly ICoreShanheSourceShennuCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreShanheSourceShennuCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheSourceShennuKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess _coreMatchDefinitionModeDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceShennuRuntime(ICoreShanheSourceShennuContentRegistryOperations contentRegistryOperations, ICoreShanheSourceShennuResolutionStackOperations resolutionStackOperations, ICoreShanheSourceShennuCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreShanheSourceShennuCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheSourceShennuCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheSourceShennuCoreRuntimeOperations coreRuntimeOperations, ICoreShanheSourceShennuCoreStrategicProgramsOperations coreStrategicProgramsOperations, ICoreShanheSourceShennuCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreShanheSourceShennuCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheSourceShennuKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess coreMatchDefinitionModeDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreStrategicProgramsOperations = coreStrategicProgramsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchDefinitionModeDefinitionOptionsAccess = coreMatchDefinitionModeDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
