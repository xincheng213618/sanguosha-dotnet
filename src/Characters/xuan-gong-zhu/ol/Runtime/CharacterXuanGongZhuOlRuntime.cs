#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterXuanGongZhuOlRuntime : IXuanGongZhuProgramHost
{
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly RuntimeEntries.CoreGameEngineQimeiProgramsEntries _coreQimeiProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries _coreTypedRecoveryDamageCursorOperations;
    private readonly RuntimeEntries.CoreGameEngineZhuijiProgramsEntries _coreZhuijiProgramsOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    internal CharacterXuanGongZhuOlRuntime(RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations,RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,RuntimeEntries.CoreGameEngineQimeiProgramsEntries coreQimeiProgramsOperations,RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries coreTypedRecoveryDamageCursorOperations,RuntimeEntries.CoreGameEngineZhuijiProgramsEntries coreZhuijiProgramsOperations,RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreQimeiProgramsOperations = coreQimeiProgramsOperations;
        this._coreTypedRecoveryDamageCursorOperations = coreTypedRecoveryDamageCursorOperations;
        this._coreZhuijiProgramsOperations = coreZhuijiProgramsOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
    }
}
