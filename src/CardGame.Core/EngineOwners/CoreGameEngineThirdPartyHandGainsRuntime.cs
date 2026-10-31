#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineThirdPartyHandGainsRuntime
{
    private readonly ICoreThirdPartyHandGainsCharacterTangJiOlOperations _characterTangJiOlOperations;
    private readonly ICoreThirdPartyHandGainsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreThirdPartyHandGainsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreThirdPartyHandGainsCoreProgramMovementSourceCountsOperations _coreProgramMovementSourceCountsOperations;
    private readonly ICoreThirdPartyHandGainsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineThirdPartyHandGainsRuntime(ICoreThirdPartyHandGainsCharacterTangJiOlOperations characterTangJiOlOperations, ICoreThirdPartyHandGainsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreThirdPartyHandGainsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreThirdPartyHandGainsCoreProgramMovementSourceCountsOperations coreProgramMovementSourceCountsOperations, ICoreThirdPartyHandGainsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterTangJiOlOperations = characterTangJiOlOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramMovementSourceCountsOperations = coreProgramMovementSourceCountsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
