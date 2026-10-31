#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineBoundRankBonusProgramsRuntime : global::CardGame.Core.IBoundRankBonusProgramHost
{
    private readonly ICoreBoundRankBonusProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreBoundRankBonusProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreBoundRankBonusProgramsCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreBoundRankBonusProgramsCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreBoundRankBonusProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreBoundRankBonusProgramsCoreProgramPersistentStateOperations _coreProgramPersistentStateOperations;
    private readonly ICoreBoundRankBonusProgramsCoreProgramSkillChangesOperations _coreProgramSkillChangesOperations;
    private readonly ICoreBoundRankBonusProgramsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineBoundRankBonusProgramsRuntime(ICoreBoundRankBonusProgramsCardZonesOperations cardZonesOperations, ICoreBoundRankBonusProgramsResolutionStackOperations resolutionStackOperations, ICoreBoundRankBonusProgramsCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreBoundRankBonusProgramsCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreBoundRankBonusProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreBoundRankBonusProgramsCoreProgramPersistentStateOperations coreProgramPersistentStateOperations, ICoreBoundRankBonusProgramsCoreProgramSkillChangesOperations coreProgramSkillChangesOperations, ICoreBoundRankBonusProgramsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreProgramSkillChangesOperations = coreProgramSkillChangesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
