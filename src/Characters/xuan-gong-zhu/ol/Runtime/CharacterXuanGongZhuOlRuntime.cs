#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterXuanGongZhuOlRuntime : global::CardGame.Core.IXuanGongZhuProgramHost
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineQimeiProgramsEntries _coreQimeiProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries _coreTypedRecoveryDamageCursorOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineZhuijiProgramsEntries _coreZhuijiProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXuanGongZhuOlEntries _characterXuanGongZhuOlOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CharacterXuanGongZhuOlRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineQimeiProgramsEntries coreQimeiProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries coreTypedRecoveryDamageCursorOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineZhuijiProgramsEntries coreZhuijiProgramsOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CharacterXuanGongZhuOlEntries characterXuanGongZhuOlOperations,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreQimeiProgramsOperations = coreQimeiProgramsOperations;
        this._coreTypedRecoveryDamageCursorOperations = coreTypedRecoveryDamageCursorOperations;
        this._coreZhuijiProgramsOperations = coreZhuijiProgramsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._characterXuanGongZhuOlOperations = characterXuanGongZhuOlOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
