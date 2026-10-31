#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWenhePoisonRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouYuMouEntries _characterZhouYuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelPendingCardsMovedBatchesAccess _pendingCardsMovedBatchesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries _coreActivityBattleRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardSupplyCompletionEntries _coreCardSupplyCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries _coreCardUsePaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredTurnEndPreludesEntries _coreDeferredTurnEndPreludesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries _coreDyingEntryProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries _coreResponseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDyingEntries _coreNativeDyingOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeFireAttackEntries _coreNativeFireAttackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries _coreNativeTrickUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardAttackHandleFactory _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineWenhePoisonRuntime(global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouYuMouEntries characterZhouYuMouOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelPendingCardsMovedBatchesAccess pendingCardsMovedBatchesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries coreActivityBattleRulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardSupplyCompletionEntries coreCardSupplyCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries coreCardUsePaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredTurnEndPreludesEntries coreDeferredTurnEndPreludesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries coreDyingEntryProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries coreResponseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDyingEntries coreNativeDyingOperations, global::CardGame.Core.RuntimeEntries.CoreNativeFireAttackEntries coreNativeFireAttackOperations, global::CardGame.Core.RuntimeEntries.CoreNativeTrickUseEntries coreNativeTrickUseOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelCardAttackHandleFactory kernelCoreMainOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
        this._coreCardSupplyCompletionOperations = coreCardSupplyCompletionOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
        this._coreDeferredTurnEndPreludesOperations = coreDeferredTurnEndPreludesOperations;
        this._coreDyingEntryProgramsOperations = coreDyingEntryProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreResponseCompletionOperations = coreResponseCompletionOperations;
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
