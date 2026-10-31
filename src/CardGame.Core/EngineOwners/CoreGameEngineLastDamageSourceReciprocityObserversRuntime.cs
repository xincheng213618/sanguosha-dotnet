#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLastDamageSourceReciprocityEntries _coreLastDamageSourceReciprocityOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery _resolutionStackOperations2;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries _coreDyingSuitsAndHistoricalUseObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramAttackHandleOperations _kernelCoreGameEngineProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardAttackHandleLookup _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineLastDamageSourceReciprocityObserversRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineLastDamageSourceReciprocityEntries coreLastDamageSourceReciprocityOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery resolutionStackOperations2,global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries coreDyingSuitsAndHistoricalUseObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.EngineOwners.IKernelCreateProgramAttackHandleOperations kernelCoreGameEngineProgramAttackAttemptOperations,global::CardGame.Core.EngineOwners.IKernelCardAttackHandleLookup kernelCoreMainOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreLastDamageSourceReciprocityOperations = coreLastDamageSourceReciprocityOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._resolutionStackOperations2 = resolutionStackOperations2;
        this._coreDyingSuitsAndHistoricalUseObserversOperations = coreDyingSuitsAndHistoricalUseObserversOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._kernelCoreGameEngineProgramAttackAttemptOperations = kernelCoreGameEngineProgramAttackAttemptOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
