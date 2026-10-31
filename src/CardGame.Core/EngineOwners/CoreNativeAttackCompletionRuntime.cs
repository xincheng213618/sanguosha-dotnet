#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeAttackCompletionRuntime
{
    private readonly RuntimeEntries.CharacterDongChengMouEntries _characterDongChengMouOperations;
    private readonly RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly RuntimeEntries.CharacterLuXunWumiaoEntries _characterLuXunWumiaoOperations;
    private readonly RuntimeEntries.CharacterSimaYiMobileGodEntries _characterSimaYiMobileGodOperations;
    private readonly RuntimeEntries.CharacterXunYuMobileGodEntries _characterXunYuMobileGodOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentCardEnhancementsEntries _coreCurrentCardEnhancementsOperations;
    private readonly RuntimeEntries.CoreGameEngineDragonBoatCardsEntries _coreDragonBoatCardsOperations;
    private readonly RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries _coreOriginalTargetAdditionsOperations;
    private readonly RuntimeEntries.CoreGameEngineOriginalUseLightningEntries _coreOriginalUseLightningOperations;
    private readonly RuntimeEntries.CoreGameEnginePaidVirtualSlashTargetsEntries _corePaidVirtualSlashTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries _coreProgramVirtualDuelOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundOnceCardlessProgramRequestsEntries _coreRoundOnceCardlessProgramRequestsOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundOnceCardlessReturnsEntries _coreRoundOnceCardlessReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations;
    private readonly RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly RuntimeEntries.CoreNativeGroupAttackEntries _coreNativeGroupAttackOperations;
    private readonly RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    private readonly RuntimeEntries.CoreNativeNullificationFramesEntries _coreNativeNullificationFramesOperations;
    private readonly RuntimeEntries.CoreNativeSlashCompletionEntries _coreNativeSlashCompletionOperations;
    private readonly RuntimeEntries.CoreNativeSlashUseEntries _coreNativeSlashUseOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly ICoreNativeAttackCompletionCoreProgramNativeReturnsOperations _coreProgramNativeReturnsOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeAttackCompletionRuntime(RuntimeEntries.CharacterDongChengMouEntries characterDongChengMouOperations, RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, RuntimeEntries.CharacterLuXunWumiaoEntries characterLuXunWumiaoOperations, RuntimeEntries.CharacterSimaYiMobileGodEntries characterSimaYiMobileGodOperations, RuntimeEntries.CharacterXunYuMobileGodEntries characterXunYuMobileGodOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineCurrentCardEnhancementsEntries coreCurrentCardEnhancementsOperations, RuntimeEntries.CoreGameEngineDragonBoatCardsEntries coreDragonBoatCardsOperations, RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries coreOriginalTargetAdditionsOperations, RuntimeEntries.CoreGameEngineOriginalUseLightningEntries coreOriginalUseLightningOperations, RuntimeEntries.CoreGameEnginePaidVirtualSlashTargetsEntries corePaidVirtualSlashTargetsOperations, RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries coreProgramVirtualDuelOperations, RuntimeEntries.CoreGameEngineRoundOnceCardlessProgramRequestsEntries coreRoundOnceCardlessProgramRequestsOperations, RuntimeEntries.CoreGameEngineRoundOnceCardlessReturnsEntries coreRoundOnceCardlessReturnsOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeBorrowedSwordEntries coreNativeBorrowedSwordOperations, RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, RuntimeEntries.CoreNativeGroupAttackEntries coreNativeGroupAttackOperations, RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations, RuntimeEntries.CoreNativeNullificationFramesEntries coreNativeNullificationFramesOperations, RuntimeEntries.CoreNativeSlashCompletionEntries coreNativeSlashCompletionOperations, RuntimeEntries.CoreNativeSlashUseEntries coreNativeSlashUseOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, ICoreNativeAttackCompletionCoreProgramNativeReturnsOperations coreProgramNativeReturnsOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDongChengMouOperations = characterDongChengMouOperations;
        this._characterGuanYuWumiaoOperations = characterGuanYuWumiaoOperations;
        this._characterLuXunWumiaoOperations = characterLuXunWumiaoOperations;
        this._characterSimaYiMobileGodOperations = characterSimaYiMobileGodOperations;
        this._characterXunYuMobileGodOperations = characterXunYuMobileGodOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCurrentCardEnhancementsOperations = coreCurrentCardEnhancementsOperations;
        this._coreDragonBoatCardsOperations = coreDragonBoatCardsOperations;
        this._coreOriginalTargetAdditionsOperations = coreOriginalTargetAdditionsOperations;
        this._coreOriginalUseLightningOperations = coreOriginalUseLightningOperations;
        this._corePaidVirtualSlashTargetsOperations = corePaidVirtualSlashTargetsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramVirtualDuelOperations = coreProgramVirtualDuelOperations;
        this._coreRoundOnceCardlessProgramRequestsOperations = coreRoundOnceCardlessProgramRequestsOperations;
        this._coreRoundOnceCardlessReturnsOperations = coreRoundOnceCardlessReturnsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeBorrowedSwordOperations = coreNativeBorrowedSwordOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeGroupAttackOperations = coreNativeGroupAttackOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreNativeNullificationFramesOperations = coreNativeNullificationFramesOperations;
        this._coreNativeSlashCompletionOperations = coreNativeSlashCompletionOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreProgramNativeReturnsOperations = coreProgramNativeReturnsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
