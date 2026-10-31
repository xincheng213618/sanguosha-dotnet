#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDeferredTurnEndPreludesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelPendingCardsMovedBatchesAccess _pendingCardsMovedBatchesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries _coreDeferredHandAlignmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateTurnHoldsEntries _corePrivateTurnHoldsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly ICoreDeferredTurnEndPreludesCardsMovedWindowOperations _cardsMovedWindowOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    internal CoreGameEngineDeferredTurnEndPreludesRuntime(global::CardGame.Core.EngineOwners.IKernelPendingCardsMovedBatchesAccess pendingCardsMovedBatchesOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries coreDeferredHandAlignmentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateTurnHoldsEntries corePrivateTurnHoldsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,ICoreDeferredTurnEndPreludesCardsMovedWindowOperations cardsMovedWindowOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess)
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
