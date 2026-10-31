#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePendingDecisionPolicyPipelineRuntime
{
    private readonly RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly RuntimeEntries.CharacterLiuFengBoundaryEntries _characterLiuFengBoundaryOperations;
    private readonly RuntimeEntries.CharacterLiuZhangOlEntries _characterLiuZhangOlOperations;
    private readonly RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries _coreChainedStateBasicConversionsOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries _coreDrawFundedDistinctBasicOffersOperations;
    private readonly RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly EngineStates.Contracts.ICorePlayerDecisionPendingDecisionBackingAccess _corePlayerDecisionPendingDecisionBackingAccess;
    internal CorePendingDecisionPolicyPipelineRuntime(RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, RuntimeEntries.CharacterLiuFengBoundaryEntries characterLiuFengBoundaryOperations, RuntimeEntries.CharacterLiuZhangOlEntries characterLiuZhangOlOperations, RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries coreChainedStateBasicConversionsOperations, RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicOffersEntries coreDrawFundedDistinctBasicOffersOperations, RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, EngineStates.Contracts.ICorePlayerDecisionPendingDecisionBackingAccess corePlayerDecisionPendingDecisionBackingAccess)
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
