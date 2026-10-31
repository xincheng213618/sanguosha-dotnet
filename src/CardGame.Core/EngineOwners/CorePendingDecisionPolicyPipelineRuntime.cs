#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePendingDecisionPolicyPipelineRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuFengBoundaryEntries _characterLiuFengBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries _characterLiuZhangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries _coreChainedStateBasicConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries _coreDrawFundedDistinctBasicOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionPendingDecisionBackingAccess _corePlayerDecisionPendingDecisionBackingAccess;
    internal CorePendingDecisionPolicyPipelineRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuFengBoundaryEntries characterLiuFengBoundaryOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries characterLiuZhangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries coreChainedStateBasicConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries coreDrawFundedDistinctBasicOffersOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionPendingDecisionBackingAccess corePlayerDecisionPendingDecisionBackingAccess)
    {
        this._characterGuoTuOlOperations = characterGuoTuOlOperations;
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._characterLiuFengBoundaryOperations = characterLiuFengBoundaryOperations;
        this._characterLiuZhangOlOperations = characterLiuZhangOlOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._coreChainedStateBasicConversionsOperations = coreChainedStateBasicConversionsOperations;
        this._coreDrawFundedDistinctBasicOffersOperations = coreDrawFundedDistinctBasicOffersOperations;
        this._coreRequestedDeckBasicsOperations = coreRequestedDeckBasicsOperations;
        this._corePlayerDecisionPendingDecisionBackingAccess = corePlayerDecisionPendingDecisionBackingAccess;
    }
}
