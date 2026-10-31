#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineAppliedDamageReflectionRuntime : global::CardGame.Core.IAppliedDamageReflectionHost
{
    private readonly ICoreAppliedDamageReflectionResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreAppliedDamageReflectionCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreAppliedDamageReflectionCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreAppliedDamageReflectionCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreAppliedDamageReflectionCoreNativeCardClassificationsOperations _coreNativeCardClassificationsOperations;
    private readonly ICoreAppliedDamageReflectionCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreAppliedDamageReflectionCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAppliedDamageReflectionRuntime(ICoreAppliedDamageReflectionResolutionStackOperations resolutionStackOperations, ICoreAppliedDamageReflectionCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreAppliedDamageReflectionCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreAppliedDamageReflectionCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreAppliedDamageReflectionCoreNativeCardClassificationsOperations coreNativeCardClassificationsOperations, ICoreAppliedDamageReflectionCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreAppliedDamageReflectionCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
