#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDamagePreventionProgramsRuntime
{
    private readonly RuntimeEntries.CharacterCaiYangAnniversaryEntries _characterCaiYangAnniversaryOperations;
    private readonly RuntimeEntries.CharacterCaoSongOlEntries _characterCaoSongOlOperations;
    private readonly RuntimeEntries.CharacterChenGongMouMobileEntries _characterChenGongMouMobileOperations;
    private readonly RuntimeEntries.CharacterDongChengMouEntries _characterDongChengMouOperations;
    private readonly RuntimeEntries.CharacterFuRongFuQianAnniversaryEntries _characterFuRongFuQianAnniversaryOperations;
    private readonly RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly RuntimeEntries.CharacterMangYaChangAnniversaryEntries _characterMangYaChangAnniversaryOperations;
    private readonly RuntimeEntries.CharacterXinPiOlEntries _characterXinPiOlOperations;
    private readonly RuntimeEntries.CharacterZhangLiaoHappyGodEntries _characterZhangLiaoHappyGodOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries _coreAttributedMarkerEventsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageCardColorHealthEntries _coreDamageCardColorHealthOperations;
    private readonly RuntimeEntries.CoreGameEngineHealthQueriesEntries _coreHealthQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramJudgmentOutcomesEntries _coreProgramJudgmentOutcomesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly RuntimeEntries.CoreNativeDamageCompletionEntries _coreNativeDamageCompletionOperations;
    private readonly RuntimeEntries.CoreProgramPrivateDamageOfferOperationsEntries _coreProgramPrivateDamageOfferOperationsOperations;
    private readonly ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations _coreProgramRuntimeHooksOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly ICoreDamagePreventionProgramsProgramWindowCursorOperations _programWindowCursorOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDamagePreventionProgramsRuntime(RuntimeEntries.CharacterCaiYangAnniversaryEntries characterCaiYangAnniversaryOperations, RuntimeEntries.CharacterCaoSongOlEntries characterCaoSongOlOperations, RuntimeEntries.CharacterChenGongMouMobileEntries characterChenGongMouMobileOperations, RuntimeEntries.CharacterDongChengMouEntries characterDongChengMouOperations, RuntimeEntries.CharacterFuRongFuQianAnniversaryEntries characterFuRongFuQianAnniversaryOperations, RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, RuntimeEntries.CharacterMangYaChangAnniversaryEntries characterMangYaChangAnniversaryOperations, RuntimeEntries.CharacterXinPiOlEntries characterXinPiOlOperations, RuntimeEntries.CharacterZhangLiaoHappyGodEntries characterZhangLiaoHappyGodOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries coreAttributedMarkerEventsOperations, RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineDamageCardColorHealthEntries coreDamageCardColorHealthOperations, RuntimeEntries.CoreGameEngineHealthQueriesEntries coreHealthQueriesOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, RuntimeEntries.CoreGameEngineProgramJudgmentOutcomesEntries coreProgramJudgmentOutcomesOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, RuntimeEntries.CoreNativeDamageCompletionEntries coreNativeDamageCompletionOperations, RuntimeEntries.CoreProgramPrivateDamageOfferOperationsEntries coreProgramPrivateDamageOfferOperationsOperations, ICoreDamagePreventionProgramsCoreProgramRuntimeHooksOperations coreProgramRuntimeHooksOperations, RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, ICoreDamagePreventionProgramsProgramWindowCursorOperations programWindowCursorOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
