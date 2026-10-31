#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanduChariotResponsesRuntime
{
    private readonly ICoreGuanduChariotResponsesCardZonesOperations _cardZonesOperations;
    private readonly ICoreGuanduChariotResponsesPendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICoreGuanduChariotResponsesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreGuanduChariotResponsesCoreDelayedNullificationOperations _coreDelayedNullificationOperations;
    private readonly ICoreGuanduChariotResponsesCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreGuanduChariotResponsesCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreGuanduChariotResponsesKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineGuanduChariotResponsesRuntime(ICoreGuanduChariotResponsesCardZonesOperations cardZonesOperations, ICoreGuanduChariotResponsesPendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICoreGuanduChariotResponsesResolutionStackOperations resolutionStackOperations, ICoreGuanduChariotResponsesCoreDelayedNullificationOperations coreDelayedNullificationOperations, ICoreGuanduChariotResponsesCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreGuanduChariotResponsesCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreGuanduChariotResponsesKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDelayedNullificationOperations = coreDelayedNullificationOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
