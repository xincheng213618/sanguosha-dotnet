#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardSupplyCompletionEntries _coreCardSupplyCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUndamagedTargetRevealEntries _coreCompletedUndamagedTargetRevealOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries _coreDyingSuitsAndHistoricalUseObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOffTurnUsedCardObserversEntries _coreOffTurnUsedCardObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntityObserversEntries _coreOriginalHandEntityObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardObserversEntries _coreOutsidePhaseDrawDiscardObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries _coreRecipientCategoryMarkObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries _coreResponseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries _coreSameNameHandDiscardOrDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUndamagedTargetRevealObserversEntries _coreCompletedUndamagedTargetRevealObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries _coreDrawFundedDistinctBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime(global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardSupplyCompletionEntries coreCardSupplyCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUndamagedTargetRevealEntries coreCompletedUndamagedTargetRevealOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries coreDyingSuitsAndHistoricalUseObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOffTurnUsedCardObserversEntries coreOffTurnUsedCardObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntityObserversEntries coreOriginalHandEntityObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardObserversEntries coreOutsidePhaseDrawDiscardObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries coreRecipientCategoryMarkObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries coreResponseCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries coreSameNameHandDiscardOrDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUndamagedTargetRevealObserversEntries coreCompletedUndamagedTargetRevealObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries coreDrawFundedDistinctBasicReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardSupplyCompletionOperations = coreCardSupplyCompletionOperations;
        this._coreCompletedUndamagedTargetRevealOperations = coreCompletedUndamagedTargetRevealOperations;
        this._coreDyingSuitsAndHistoricalUseObserversOperations = coreDyingSuitsAndHistoricalUseObserversOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreOffTurnUsedCardObserversOperations = coreOffTurnUsedCardObserversOperations;
        this._coreOriginalHandEntityObserversOperations = coreOriginalHandEntityObserversOperations;
        this._coreOutsidePhaseDrawDiscardObserversOperations = coreOutsidePhaseDrawDiscardObserversOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreRecipientCategoryMarkObserversOperations = coreRecipientCategoryMarkObserversOperations;
        this._coreResponseCompletionOperations = coreResponseCompletionOperations;
        this._coreSameNameHandDiscardOrDamageObserversOperations = coreSameNameHandDiscardOrDamageObserversOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._coreCompletedUndamagedTargetRevealObserversOperations = coreCompletedUndamagedTargetRevealObserversOperations;
        this._coreDrawFundedDistinctBasicReturnsOperations = coreDrawFundedDistinctBasicReturnsOperations;
        this._coreTieredRoundZeroReturnsOperations = coreTieredRoundZeroReturnsOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
