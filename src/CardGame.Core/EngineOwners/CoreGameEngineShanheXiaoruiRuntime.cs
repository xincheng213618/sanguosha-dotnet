#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheXiaoruiRuntime : global::CardGame.Core.IShanheXiaoruiProgramHost
{
    private readonly ICoreShanheXiaoruiTurnCardUseEffectsOperations _turnCardUseEffectsOperations;
    private readonly ICoreShanheXiaoruiCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreShanheXiaoruiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheXiaoruiCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheXiaoruiCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly ICoreShanheXiaoruiKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheXiaoruiRuntime(ICoreShanheXiaoruiTurnCardUseEffectsOperations turnCardUseEffectsOperations, ICoreShanheXiaoruiCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreShanheXiaoruiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheXiaoruiCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheXiaoruiCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, ICoreShanheXiaoruiKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
