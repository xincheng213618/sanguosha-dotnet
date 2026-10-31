#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenhePoisonRuntime
{
    private readonly RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly RuntimeEntries.CharacterZhouYuMouEntries _characterZhouYuMouOperations;
    private readonly RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelPendingCardsMovedBatchesAccess _pendingCardsMovedBatchesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineActivityBattleRulesEntries _coreActivityBattleRulesOperations;
    private readonly RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly RuntimeEntries.CoreGameEngineDeferredTurnEndPreludesEntries _coreDeferredTurnEndPreludesOperations;
    private readonly RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries _coreDyingEntryProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly RuntimeEntries.CoreNativeDyingEntries _coreNativeDyingOperations;
    private readonly RuntimeEntries.CoreNativeFireAttackEntries _coreNativeFireAttackOperations;
    private readonly RuntimeEntries.CoreNativeTrickUseEntries _coreNativeTrickUseOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelCardAttackHandleFactory _kernelCoreMainOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineWenhePoisonRuntime(RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations,RuntimeEntries.CharacterZhouYuMouEntries characterZhouYuMouOperations,RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations,IKernelCardZoneStoreAccess cardZonesOperations,IKernelPendingCardsMovedBatchesAccess pendingCardsMovedBatchesOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,RuntimeEntries.CoreGameEngineActivityBattleRulesEntries coreActivityBattleRulesOperations,RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,RuntimeEntries.CoreGameEngineDeferredTurnEndPreludesEntries coreDeferredTurnEndPreludesOperations,RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries coreDyingEntryProgramsOperations,RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations,RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations,RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations,RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations,RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations,RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations,RuntimeEntries.CoreNativeDyingEntries coreNativeDyingOperations,RuntimeEntries.CoreNativeFireAttackEntries coreNativeFireAttackOperations,RuntimeEntries.CoreNativeTrickUseEntries coreNativeTrickUseOperations,RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations,RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,IKernelCardAttackHandleFactory kernelCoreMainOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess,EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess,EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterQinLangOlOperations = characterQinLangOlOperations;
        this._characterZhouYuMouOperations = characterZhouYuMouOperations;
        this._characterZhugeLiangMouOperations = characterZhugeLiangMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreActivityBattleRulesOperations = coreActivityBattleRulesOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreDeferredTurnEndPreludesOperations = coreDeferredTurnEndPreludesOperations;
        this._coreDyingEntryProgramsOperations = coreDyingEntryProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreTieredRoundZeroReturnsOperations = coreTieredRoundZeroReturnsOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeDyingOperations = coreNativeDyingOperations;
        this._coreNativeFireAttackOperations = coreNativeFireAttackOperations;
        this._coreNativeTrickUseOperations = coreNativeTrickUseOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreMatchDefinitionModeDefinitionAccess = coreMatchDefinitionModeDefinitionAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
