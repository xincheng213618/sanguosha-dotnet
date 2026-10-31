#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterCaoCaoHappyGodRuntime : IHappyGuixinHost
{
    private readonly RuntimeEntries.CharacterCaoCaoMobileGodEntries _characterCaoCaoMobileGodOperations;
    private readonly RuntimeEntries.CharacterCaoCaoOlGodEntries _characterCaoCaoOlGodOperations;
    private readonly IKernelMatchRandomAccess _randomOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries _coreOrderedPrintedSkillLossObserversOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CoreShenwuNpcProgramsEntries _coreShenwuNpcProgramsOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterCaoCaoHappyGodRuntime(RuntimeEntries.CharacterCaoCaoMobileGodEntries characterCaoCaoMobileGodOperations,RuntimeEntries.CharacterCaoCaoOlGodEntries characterCaoCaoOlGodOperations,IKernelMatchRandomAccess randomOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations,RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations,RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries coreOrderedPrintedSkillLossObserversOperations,RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations,RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations,RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,RuntimeEntries.CoreShenwuNpcProgramsEntries coreShenwuNpcProgramsOperations,RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess,EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoCaoMobileGodOperations = characterCaoCaoMobileGodOperations;
        this._characterCaoCaoOlGodOperations = characterCaoCaoOlGodOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreOrderedPrintedSkillLossObserversOperations = coreOrderedPrintedSkillLossObserversOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreShenwuNpcProgramsOperations = coreShenwuNpcProgramsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
