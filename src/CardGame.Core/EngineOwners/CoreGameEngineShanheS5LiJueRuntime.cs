#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5LiJueRuntime : global::CardGame.Core.IShanheS5LiJueProgramHost
{
    private readonly ICoreShanheS5LiJueCharacterLiJueOlOperations _characterLiJueOlOperations;
    private readonly ICoreShanheS5LiJueResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheS5LiJueCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheS5LiJueCoreRuntimeOperations _coreRuntimeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5LiJueRuntime(ICoreShanheS5LiJueCharacterLiJueOlOperations characterLiJueOlOperations, ICoreShanheS5LiJueResolutionStackOperations resolutionStackOperations, ICoreShanheS5LiJueCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheS5LiJueCoreRuntimeOperations coreRuntimeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterLiJueOlOperations = characterLiJueOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
