#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRicePileObserversRuntime
{
    private readonly ICoreRicePileObserversResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreRicePileObserversCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreRicePileObserversCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreRicePileObserversCoreObserverSubtreesOperations _coreObserverSubtreesOperations;
    private readonly ICoreRicePileObserversCoreRecoveryReplacementDamageRidesOperations _coreRecoveryReplacementDamageRidesOperations;
    private readonly ICoreRicePileObserversCoreRicePileReceiptsOperations _coreRicePileReceiptsOperations;
    private readonly ICoreRicePileObserversCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreRicePileObserversCoreObserverSubtreesOperations2 _coreObserverSubtreesOperations2;
    private readonly ICoreRicePileObserversCoreRicePileObserversOperations _coreRicePileObserversOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CoreGameEngineRicePileObserversRuntime(ICoreRicePileObserversResolutionStackOperations resolutionStackOperations, ICoreRicePileObserversCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreRicePileObserversCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreRicePileObserversCoreObserverSubtreesOperations coreObserverSubtreesOperations, ICoreRicePileObserversCoreRecoveryReplacementDamageRidesOperations coreRecoveryReplacementDamageRidesOperations, ICoreRicePileObserversCoreRicePileReceiptsOperations coreRicePileReceiptsOperations, ICoreRicePileObserversCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreRicePileObserversCoreObserverSubtreesOperations2 coreObserverSubtreesOperations2, ICoreRicePileObserversCoreRicePileObserversOperations coreRicePileObserversOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreRicePileReceiptsOperations = coreRicePileReceiptsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreObserverSubtreesOperations2 = coreObserverSubtreesOperations2;
        this._coreRicePileObserversOperations = coreRicePileObserversOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
