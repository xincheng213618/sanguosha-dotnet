#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativePublicPileEquipmentChildrenRuntime
{
    private readonly ICoreNativePublicPileEquipmentChildrenPendingCardsMovedBatchesOperations _pendingCardsMovedBatchesOperations;
    private readonly ICoreNativePublicPileEquipmentChildrenResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativePublicPileEquipmentChildrenCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICoreNativePublicPileEquipmentChildrenCoreNativePublicPileOriginalCardsOperations _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineNativePublicPileEquipmentChildrenRuntime(ICoreNativePublicPileEquipmentChildrenPendingCardsMovedBatchesOperations pendingCardsMovedBatchesOperations, ICoreNativePublicPileEquipmentChildrenResolutionStackOperations resolutionStackOperations, ICoreNativePublicPileEquipmentChildrenCoreEquipmentContentOperations coreEquipmentContentOperations, ICoreNativePublicPileEquipmentChildrenCoreNativePublicPileOriginalCardsOperations coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
