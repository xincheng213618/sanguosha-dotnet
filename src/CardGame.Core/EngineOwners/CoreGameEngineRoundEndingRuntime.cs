#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRoundEndingRuntime
{
    private readonly ICoreRoundEndingCharacterGuanYuRuntimeOperations _characterGuanYuRuntimeOperations;
    private readonly ICoreRoundEndingContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreRoundEndingModeRulesOperations _modeRulesOperations;
    private readonly ICoreRoundEndingPendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICoreRoundEndingResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRoundEndingCoreActualTurnsOperations _coreActualTurnsOperations;
    private readonly ICoreRoundEndingCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreRoundEndingCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreRoundEndingCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreRoundEndingCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreRoundEndingCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreRoundEndingCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreRoundEndingCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreRoundEndingCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreRoundEndingKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRoundEndingRuntime(ICoreRoundEndingCharacterGuanYuRuntimeOperations characterGuanYuRuntimeOperations, ICoreRoundEndingContentRegistryOperations contentRegistryOperations, ICoreRoundEndingModeRulesOperations modeRulesOperations, ICoreRoundEndingPendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICoreRoundEndingResolutionStackOperations resolutionStackOperations, ICoreRoundEndingCoreActualTurnsOperations coreActualTurnsOperations, ICoreRoundEndingCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreRoundEndingCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreRoundEndingCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreRoundEndingCoreRuntimeOperations coreRuntimeOperations, ICoreRoundEndingCoreTableOrderOperations coreTableOrderOperations, ICoreRoundEndingCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreRoundEndingCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreRoundEndingCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreRoundEndingKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess coreActualRoundProgressRoundNumberRoundTurnSeatsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterGuanYuRuntimeOperations = characterGuanYuRuntimeOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberRoundTurnSeatsAccess = coreActualRoundProgressRoundNumberRoundTurnSeatsAccess;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
