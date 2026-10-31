#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeHarvestRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuXunMouMobileEntries _characterLuXunMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTianChouOlEntries _characterTianChouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunXuAnniversaryEntries _characterXunXuAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYangBiaoAnniversaryEntries _characterYangBiaoAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries _characterZhugeGuoLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries _coreCurrentUsePhysicalClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries _coreExtraCardEffectRoundsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGroupTargetNullificationEntries _coreGroupTargetNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheAlliedTricksEntries _coreShanheAlliedTricksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries _coreVirtualOrdinaryTrickUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries _coreWushuangTacticRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries _coreNativeNullificationFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCompleteGameOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeHarvestRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterLuXunMouMobileEntries characterLuXunMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations, global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterTianChouOlEntries characterTianChouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXunXuAnniversaryEntries characterXunXuAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterYangBiaoAnniversaryEntries characterYangBiaoAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoLeEntries characterZhugeGuoLeOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries coreCurrentUsePhysicalClaimsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries coreExtraCardEffectRoundsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGroupTargetNullificationEntries coreGroupTargetNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheAlliedTricksEntries coreShanheAlliedTricksOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineVirtualOrdinaryTrickUsesEntries coreVirtualOrdinaryTrickUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangTacticRulesEntries coreWushuangTacticRulesOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries coreNativeNullificationFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineOwners.IKernelCompleteGameOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterLuXunMouMobileOperations = characterLuXunMouMobileOperations;
        this._characterMiHengLeOperations = characterMiHengLeOperations;
        this._characterQinLangOlOperations = characterQinLangOlOperations;
        this._characterTianChouOlOperations = characterTianChouOlOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._characterXunXuAnniversaryOperations = characterXunXuAnniversaryOperations;
        this._characterYangBiaoAnniversaryOperations = characterYangBiaoAnniversaryOperations;
        this._characterZhugeGuoLeOperations = characterZhugeGuoLeOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCurrentUsePhysicalClaimsOperations = coreCurrentUsePhysicalClaimsOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreExtraCardEffectRoundsOperations = coreExtraCardEffectRoundsOperations;
        this._coreGroupTargetNullificationOperations = coreGroupTargetNullificationOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreShanheAlliedTricksOperations = coreShanheAlliedTricksOperations;
        this._coreTieredRoundZeroReturnsOperations = coreTieredRoundZeroReturnsOperations;
        this._coreVirtualOrdinaryTrickUsesOperations = coreVirtualOrdinaryTrickUsesOperations;
        this._coreWushuangTacticRulesOperations = coreWushuangTacticRulesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeNullificationFramesOperations = coreNativeNullificationFramesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
