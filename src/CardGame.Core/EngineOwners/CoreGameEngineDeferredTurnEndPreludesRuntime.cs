#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDeferredTurnEndPreludesRuntime
{
    private readonly IKernelPendingCardsMovedBatchesAccess _pendingCardsMovedBatchesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries _coreDeferredHandAlignmentsOperations;
    private readonly RuntimeEntries.CoreGameEnginePrivateTurnHoldsEntries _corePrivateTurnHoldsOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly ICoreDeferredTurnEndPreludesCardsMovedWindowOperations _cardsMovedWindowOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    internal CoreGameEngineDeferredTurnEndPreludesRuntime(IKernelPendingCardsMovedBatchesAccess pendingCardsMovedBatchesOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries coreDeferredHandAlignmentsOperations,RuntimeEntries.CoreGameEnginePrivateTurnHoldsEntries corePrivateTurnHoldsOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,ICoreDeferredTurnEndPreludesCardsMovedWindowOperations cardsMovedWindowOperations,EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess)
    {
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreDeferredHandAlignmentsOperations = coreDeferredHandAlignmentsOperations;
        this._corePrivateTurnHoldsOperations = corePrivateTurnHoldsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._cardsMovedWindowOperations = cardsMovedWindowOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    }
}
