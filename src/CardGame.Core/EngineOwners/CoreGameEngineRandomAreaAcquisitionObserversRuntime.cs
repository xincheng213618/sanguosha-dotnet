#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRandomAreaAcquisitionObserversRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries _coreDamageObserverCursorQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries _coreOrderedPrintedSkillLossObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRandomAreaAcquisitionProofsEntries _coreRandomAreaAcquisitionProofsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries _coreTypedRecoveryDamageCursorOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRandomAreaAcquisitionObserversEntries _coreRandomAreaAcquisitionObserversOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineRandomAreaAcquisitionObserversRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries coreDamageObserverCursorQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries coreOrderedPrintedSkillLossObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRandomAreaAcquisitionProofsEntries coreRandomAreaAcquisitionProofsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries coreTypedRecoveryDamageCursorOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRandomAreaAcquisitionObserversEntries coreRandomAreaAcquisitionObserversOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreOrderedPrintedSkillLossObserversOperations = coreOrderedPrintedSkillLossObserversOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRandomAreaAcquisitionProofsOperations = coreRandomAreaAcquisitionProofsOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreTypedRecoveryDamageCursorOperations = coreTypedRecoveryDamageCursorOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreRandomAreaAcquisitionObserversOperations = coreRandomAreaAcquisitionObserversOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
