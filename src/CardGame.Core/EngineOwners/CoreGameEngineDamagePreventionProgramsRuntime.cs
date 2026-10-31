#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDamagePreventionProgramsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaiYangAnniversaryEntries _characterCaiYangAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoSongOlEntries _characterCaoSongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChenGongMouMobileEntries _characterChenGongMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongChengMouEntries _characterDongChengMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFuRongFuQianAnniversaryEntries _characterFuRongFuQianAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMangYaChangAnniversaryEntries _characterMangYaChangAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries _characterXinPiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoHappyGodEntries _characterZhangLiaoHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries _coreAttributedMarkerEventsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageCardColorHealthEntries _coreDamageCardColorHealthOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHealthQueriesEntries _coreHealthQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramJudgmentOutcomesEntries _coreProgramJudgmentOutcomesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageCompletionEntries _coreNativeDamageCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramPrivateDamageOfferOperationsEntries _coreProgramPrivateDamageOfferOperationsOperations;
    private readonly ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations _coreProgramRuntimeHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreDamagePreventionProgramsProgramWindowCursorOperations _programWindowCursorOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDamagePreventionProgramsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaiYangAnniversaryEntries characterCaiYangAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoSongOlEntries characterCaoSongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterChenGongMouMobileEntries characterChenGongMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterDongChengMouEntries characterDongChengMouOperations, global::CardGame.Core.RuntimeEntries.CharacterFuRongFuQianAnniversaryEntries characterFuRongFuQianAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMangYaChangAnniversaryEntries characterMangYaChangAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterXinPiOlEntries characterXinPiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoHappyGodEntries characterZhangLiaoHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries coreAttributedMarkerEventsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageCardColorHealthEntries coreDamageCardColorHealthOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHealthQueriesEntries coreHealthQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramJudgmentOutcomesEntries coreProgramJudgmentOutcomesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageCompletionEntries coreNativeDamageCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreProgramPrivateDamageOfferOperationsEntries coreProgramPrivateDamageOfferOperationsOperations, ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations coreProgramRuntimeHooksOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, ICoreDamagePreventionProgramsProgramWindowCursorOperations programWindowCursorOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaiYangAnniversaryOperations = characterCaiYangAnniversaryOperations;
        this._characterCaoSongOlOperations = characterCaoSongOlOperations;
        this._characterChenGongMouMobileOperations = characterChenGongMouMobileOperations;
        this._characterDongChengMouOperations = characterDongChengMouOperations;
        this._characterFuRongFuQianAnniversaryOperations = characterFuRongFuQianAnniversaryOperations;
        this._characterLvQianOlOperations = characterLvQianOlOperations;
        this._characterMangYaChangAnniversaryOperations = characterMangYaChangAnniversaryOperations;
        this._characterXinPiOlOperations = characterXinPiOlOperations;
        this._characterZhangLiaoHappyGodOperations = characterZhangLiaoHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAttributedMarkerEventsOperations = coreAttributedMarkerEventsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDamageCardColorHealthOperations = coreDamageCardColorHealthOperations;
        this._coreHealthQueriesOperations = coreHealthQueriesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramJudgmentOutcomesOperations = coreProgramJudgmentOutcomesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeDamageCompletionOperations = coreNativeDamageCompletionOperations;
        this._coreProgramPrivateDamageOfferOperationsOperations = coreProgramPrivateDamageOfferOperationsOperations;
        this._coreProgramRuntimeHooksOperations = coreProgramRuntimeHooksOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._programWindowCursorOperations = programWindowCursorOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
