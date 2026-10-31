#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5XuchuRuntime : global::CardGame.Core.IShanheS5LuoyiProgramHost
{
    private readonly ICoreShanheS5XuchuResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheS5XuchuCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreShanheS5XuchuCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheS5XuchuCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheS5XuchuCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheS5XuchuKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5XuchuRuntime(ICoreShanheS5XuchuResolutionStackOperations resolutionStackOperations, ICoreShanheS5XuchuCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreShanheS5XuchuCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheS5XuchuCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheS5XuchuCoreRuntimeOperations coreRuntimeOperations, ICoreShanheS5XuchuKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
