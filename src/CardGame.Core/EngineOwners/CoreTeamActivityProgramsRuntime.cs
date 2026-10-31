#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreTeamActivityProgramsRuntime : global::CardGame.Core.ITeamActivityProgramHost
{
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess _randomOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraEffectsAndRankGuessingEntries _coreExtraEffectsAndRankGuessingOperations;
    private readonly ICoreTeamActivityProgramsCoreModeAllegianceOperations _coreModeAllegianceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTeamActivitiesEntries _coreTeamActivitiesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreTeamActivityProgramsRuntime(global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess randomOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraEffectsAndRankGuessingEntries coreExtraEffectsAndRankGuessingOperations, ICoreTeamActivityProgramsCoreModeAllegianceOperations coreModeAllegianceOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTeamActivitiesEntries coreTeamActivitiesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
