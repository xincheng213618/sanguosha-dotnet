#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries _coreDamageObserverCursorQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries _coreDyingSuitsAndHistoricalUseObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries _corePrivateOfferReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries _coreSameNameHandDiscardOrDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoEntries _coreShanheSourceMouDongZhuoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries _coreShanheSourceMouDongZhuoWineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineShanheSourceMouDongZhuoChildrenRuntime(global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries coreDamageObserverCursorQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries coreDyingSuitsAndHistoricalUseObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries corePrivateOfferReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries coreSameNameHandDiscardOrDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoEntries coreShanheSourceMouDongZhuoOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceMouDongZhuoWineEntries coreShanheSourceMouDongZhuoWineOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreDyingSuitsAndHistoricalUseObserversOperations = coreDyingSuitsAndHistoricalUseObserversOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreSameNameHandDiscardOrDamageObserversOperations = coreSameNameHandDiscardOrDamageObserversOperations;
        this._coreShanheSourceMouDongZhuoOperations = coreShanheSourceMouDongZhuoOperations;
        this._coreShanheSourceMouDongZhuoWineOperations = coreShanheSourceMouDongZhuoWineOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
