#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHealthReplacementProgramsRuntime
{
    private readonly RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly RuntimeEntries.CharacterDongBaiOlEntries _characterDongBaiOlOperations;
    private readonly RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly RuntimeEntries.CharacterSunJianMouOlEntries _characterSunJianMouOlOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineCardDamageMaximumHpReplacementEntries _coreCardDamageMaximumHpReplacementOperations;
    private readonly RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries _coreDyingEntryProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries _coreGrantedEntityPhasesOperations;
    private readonly RuntimeEntries.CoreGameEngineHealthQueriesEntries _coreHealthQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineQimeiProgramsEntries _coreQimeiProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeDyingEntries _coreNativeDyingOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _coreMatchSetupSetupCompleteAccess;
    private readonly EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreGameEngineHealthReplacementProgramsRuntime(RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, RuntimeEntries.CharacterDongBaiOlEntries characterDongBaiOlOperations, RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations, RuntimeEntries.CharacterSunJianMouOlEntries characterSunJianMouOlOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, RuntimeEntries.CoreGameEngineCardDamageMaximumHpReplacementEntries coreCardDamageMaximumHpReplacementOperations, RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries coreDyingEntryProgramsOperations, RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries coreGrantedEntityPhasesOperations, RuntimeEntries.CoreGameEngineHealthQueriesEntries coreHealthQueriesOperations, RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineQimeiProgramsEntries coreQimeiProgramsOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeDyingEntries coreNativeDyingOperations, RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess coreMatchSetupSetupCompleteAccess, EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterDongBaiOlOperations = characterDongBaiOlOperations;
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._characterSunJianMouOlOperations = characterSunJianMouOlOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreCardDamageMaximumHpReplacementOperations = coreCardDamageMaximumHpReplacementOperations;
        this._coreDyingEntryProgramsOperations = coreDyingEntryProgramsOperations;
        this._coreGrantedEntityPhasesOperations = coreGrantedEntityPhasesOperations;
        this._coreHealthQueriesOperations = coreHealthQueriesOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreQimeiProgramsOperations = coreQimeiProgramsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeDyingOperations = coreNativeDyingOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupSetupCompleteAccess = coreMatchSetupSetupCompleteAccess;
        this._coreGameEngineHpChangeProgramsPendingHpChangesAccess = coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    }
}
