#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativePublicPileEquipmentChildrenRuntime
{
    private readonly IKernelPendingCardsMovedBatchesAccess _pendingCardsMovedBatchesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly ICoreNativePublicPileEquipmentChildrenCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineNativePublicPileEquipmentChildrenRuntime(IKernelPendingCardsMovedBatchesAccess pendingCardsMovedBatchesOperations, IKernelFrameStoreAccess resolutionStackOperations, ICoreNativePublicPileEquipmentChildrenCoreEquipmentContentOperations coreEquipmentContentOperations, RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
