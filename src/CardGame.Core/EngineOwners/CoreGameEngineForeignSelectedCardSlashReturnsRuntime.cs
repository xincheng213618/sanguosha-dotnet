#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineForeignSelectedCardSlashReturnsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries _coreCompletedCategoryUseProofOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries _coreCurrentSlashFireOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries _coreForeignSelectedCardSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries _coreShownEntityTurnPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashReturnsEntries _coreForeignSelectedCardSlashReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries _coreSameNameHandDiscardOrDamageObserversOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CoreGameEngineForeignSelectedCardSlashReturnsRuntime(global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries coreCompletedCategoryUseProofOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries coreCurrentSlashFireOffersOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashEntries coreForeignSelectedCardSlashOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries coreShownEntityTurnPoliciesOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignSelectedCardSlashReturnsEntries coreForeignSelectedCardSlashReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries coreSameNameHandDiscardOrDamageObserversOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess,global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCompletedCategoryUseProofOperations = coreCompletedCategoryUseProofOperations;
        this._coreCurrentSlashFireOffersOperations = coreCurrentSlashFireOffersOperations;
        this._coreForeignSelectedCardSlashOperations = coreForeignSelectedCardSlashOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShownEntityTurnPoliciesOperations = coreShownEntityTurnPoliciesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreForeignSelectedCardSlashReturnsOperations = coreForeignSelectedCardSlashReturnsOperations;
        this._coreSameNameHandDiscardOrDamageObserversOperations = coreSameNameHandDiscardOrDamageObserversOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
