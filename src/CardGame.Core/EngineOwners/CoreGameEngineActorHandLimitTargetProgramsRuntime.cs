#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActorHandLimitTargetProgramsRuntime
{
    private readonly ICoreActorHandLimitTargetProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreActorHandLimitTargetProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreActorHandLimitTargetProgramsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreActorHandLimitTargetProgramsCoreCardActionsOperations _coreCardActionsOperations;
    private readonly ICoreActorHandLimitTargetProgramsCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICoreActorHandLimitTargetProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreActorHandLimitTargetProgramsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreActorHandLimitTargetProgramsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreActorHandLimitTargetProgramsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineActorHandLimitTargetProgramsRuntime(ICoreActorHandLimitTargetProgramsContentRegistryOperations contentRegistryOperations, ICoreActorHandLimitTargetProgramsResolutionStackOperations resolutionStackOperations, ICoreActorHandLimitTargetProgramsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreActorHandLimitTargetProgramsCoreCardActionsOperations coreCardActionsOperations, ICoreActorHandLimitTargetProgramsCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICoreActorHandLimitTargetProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreActorHandLimitTargetProgramsCoreTableOrderOperations coreTableOrderOperations, ICoreActorHandLimitTargetProgramsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreActorHandLimitTargetProgramsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
