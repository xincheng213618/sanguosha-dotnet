#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeAttackCompletionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongChengMouEntries _characterDongChengMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries _characterGuanYuWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuXunWumiaoEntries _characterLuXunWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiMobileGodEntries _characterSimaYiMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries _characterXunYuMobileGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentCardEnhancementsEntries _coreCurrentCardEnhancementsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatCardsEntries _coreDragonBoatCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries _coreOriginalTargetAdditionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries _coreOriginalUseLightningOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePaidVirtualSlashTargetsEntries _corePaidVirtualSlashTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries _coreProgramVirtualDuelOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessProgramRequestsEntries _coreRoundOnceCardlessProgramRequestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessReturnsEntries _coreRoundOnceCardlessReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeGroupAttackEntries _coreNativeGroupAttackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries _coreNativeNullificationFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashCompletionEntries _coreNativeSlashCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries _coreNativeSlashUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly ICoreNativeAttackCompletionCoreProgramNativeReturnsOperations _coreProgramNativeReturnsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeAttackCompletionRuntime(global::CardGame.Core.RuntimeEntries.CharacterDongChengMouEntries characterDongChengMouOperations, global::CardGame.Core.RuntimeEntries.CharacterGuanYuWumiaoEntries characterGuanYuWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterLuXunWumiaoEntries characterLuXunWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaYiMobileGodEntries characterSimaYiMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries characterXunYuMobileGodOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentCardEnhancementsEntries coreCurrentCardEnhancementsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatCardsEntries coreDragonBoatCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries coreOriginalTargetAdditionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries coreOriginalUseLightningOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePaidVirtualSlashTargetsEntries corePaidVirtualSlashTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries coreProgramVirtualDuelOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessProgramRequestsEntries coreRoundOnceCardlessProgramRequestsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessReturnsEntries coreRoundOnceCardlessReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries coreNativeBorrowedSwordOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeGroupAttackEntries coreNativeGroupAttackOperations, global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries coreNativeNullificationFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashCompletionEntries coreNativeSlashCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries coreNativeSlashUseOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, ICoreNativeAttackCompletionCoreProgramNativeReturnsOperations coreProgramNativeReturnsOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
