#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CharacterMengHuoBoundaryMobileRuntime
{
    private readonly ICharacterMengHuoBoundaryMobilePendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICharacterMengHuoBoundaryMobileResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterMengHuoBoundaryMobileCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterMengHuoBoundaryMobileCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CharacterMengHuoBoundaryMobileRuntime(ICharacterMengHuoBoundaryMobilePendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICharacterMengHuoBoundaryMobileResolutionStackOperations resolutionStackOperations, ICharacterMengHuoBoundaryMobileCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterMengHuoBoundaryMobileCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
