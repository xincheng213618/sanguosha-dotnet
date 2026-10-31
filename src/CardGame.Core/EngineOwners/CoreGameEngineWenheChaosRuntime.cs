#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenheChaosRuntime
{
    private readonly ICoreWenheChaosContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreWenheChaosRandomOperations _randomOperations;
    private readonly ICoreWenheChaosResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreWenheChaosCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreWenheChaosCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreWenheChaosCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreWenheChaosCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreWenheChaosCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreWenheChaosCoreWenheCardsOperations _coreWenheCardsOperations;
    private readonly ICoreWenheChaosCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreWenheChaosCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreWenheChaosKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineWenheChaosRuntime(ICoreWenheChaosContentRegistryOperations contentRegistryOperations, ICoreWenheChaosRandomOperations randomOperations, ICoreWenheChaosResolutionStackOperations resolutionStackOperations, ICoreWenheChaosCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreWenheChaosCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreWenheChaosCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreWenheChaosCoreRuntimeOperations coreRuntimeOperations, ICoreWenheChaosCoreTableOrderOperations coreTableOrderOperations, ICoreWenheChaosCoreWenheCardsOperations coreWenheCardsOperations, ICoreWenheChaosCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreWenheChaosCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreWenheChaosKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreWenheCardsOperations = coreWenheCardsOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
