#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreTeamActivityProgramsRuntime : global::CardGame.Core.ITeamActivityProgramHost
{
    private readonly ICoreTeamActivityProgramsRandomOperations _randomOperations;
    private readonly ICoreTeamActivityProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreTeamActivityProgramsCoreExtraEffectsAndRankGuessingOperations _coreExtraEffectsAndRankGuessingOperations;
    private readonly ICoreTeamActivityProgramsCoreModeAllegianceOperations _coreModeAllegianceOperations;
    private readonly ICoreTeamActivityProgramsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreTeamActivityProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreTeamActivityProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreTeamActivityProgramsCoreTeamActivitiesOperations _coreTeamActivitiesOperations;
    private readonly ICoreTeamActivityProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreTeamActivityProgramsRuntime(ICoreTeamActivityProgramsRandomOperations randomOperations, ICoreTeamActivityProgramsResolutionStackOperations resolutionStackOperations, ICoreTeamActivityProgramsCoreExtraEffectsAndRankGuessingOperations coreExtraEffectsAndRankGuessingOperations, ICoreTeamActivityProgramsCoreModeAllegianceOperations coreModeAllegianceOperations, ICoreTeamActivityProgramsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreTeamActivityProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreTeamActivityProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreTeamActivityProgramsCoreTeamActivitiesOperations coreTeamActivitiesOperations, ICoreTeamActivityProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreExtraEffectsAndRankGuessingOperations = coreExtraEffectsAndRankGuessingOperations;
        this._coreModeAllegianceOperations = coreModeAllegianceOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTeamActivitiesOperations = coreTeamActivitiesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
