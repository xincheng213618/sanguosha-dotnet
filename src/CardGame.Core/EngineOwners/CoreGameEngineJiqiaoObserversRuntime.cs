#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineJiqiaoObserversRuntime : global::CardGame.Core.IJiqiaoProgramHost
{
    private readonly ICoreJiqiaoObserversResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreJiqiaoObserversCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    private readonly ICoreJiqiaoObserversCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreJiqiaoObserversCoreJiqiaoProgramsOperations _coreJiqiaoProgramsOperations;
    private readonly ICoreJiqiaoObserversCoreObserverSubtreesOperations _coreObserverSubtreesOperations;
    private readonly ICoreJiqiaoObserversCorePrivateOfferReturnsOperations _corePrivateOfferReturnsOperations;
    private readonly ICoreJiqiaoObserversCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreJiqiaoObserversCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreJiqiaoObserversCoreWushuangShopCardsOperations _coreWushuangShopCardsOperations;
    private readonly ICoreJiqiaoObserversCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreJiqiaoObserversCoreJiqiaoObserversOperations _coreJiqiaoObserversOperations;
    private readonly ICoreJiqiaoObserversCoreObserverSubtreesOperations2 _coreObserverSubtreesOperations2;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    internal CoreGameEngineJiqiaoObserversRuntime(ICoreJiqiaoObserversResolutionStackOperations resolutionStackOperations, ICoreJiqiaoObserversCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations, ICoreJiqiaoObserversCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreJiqiaoObserversCoreJiqiaoProgramsOperations coreJiqiaoProgramsOperations, ICoreJiqiaoObserversCoreObserverSubtreesOperations coreObserverSubtreesOperations, ICoreJiqiaoObserversCorePrivateOfferReturnsOperations corePrivateOfferReturnsOperations, ICoreJiqiaoObserversCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreJiqiaoObserversCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreJiqiaoObserversCoreWushuangShopCardsOperations coreWushuangShopCardsOperations, ICoreJiqiaoObserversCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreJiqiaoObserversCoreJiqiaoObserversOperations coreJiqiaoObserversOperations, ICoreJiqiaoObserversCoreObserverSubtreesOperations2 coreObserverSubtreesOperations2, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreJiqiaoProgramsOperations = coreJiqiaoProgramsOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._corePrivateOfferReturnsOperations = corePrivateOfferReturnsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreWushuangShopCardsOperations = coreWushuangShopCardsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreJiqiaoObserversOperations = coreJiqiaoObserversOperations;
        this._coreObserverSubtreesOperations2 = coreObserverSubtreesOperations2;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
    }
}
