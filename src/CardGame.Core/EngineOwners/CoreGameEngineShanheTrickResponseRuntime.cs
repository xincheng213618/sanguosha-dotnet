#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheTrickResponseRuntime
{
    private readonly ICoreShanheTrickResponseResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheTrickResponseCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreShanheTrickResponseCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreShanheTrickResponseCoreNativeCardContinuationCursorsOperations _coreNativeCardContinuationCursorsOperations;
    private readonly ICoreShanheTrickResponseCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheTrickResponseCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheTrickResponseRuntime(ICoreShanheTrickResponseResolutionStackOperations resolutionStackOperations, ICoreShanheTrickResponseCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreShanheTrickResponseCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreShanheTrickResponseCoreNativeCardContinuationCursorsOperations coreNativeCardContinuationCursorsOperations, ICoreShanheTrickResponseCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheTrickResponseCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
