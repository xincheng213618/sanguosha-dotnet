#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeCardUseCompletionRuntime
{
    private readonly RuntimeEntries.CharacterDengAiAnniversaryGodEntries _characterDengAiAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly RuntimeEntries.CharacterWuAnGuoOlEntries _characterWuAnGuoOlOperations;
    private readonly RuntimeEntries.CharacterXinPiOlEntries _characterXinPiOlOperations;
    private readonly RuntimeEntries.CharacterXunXuAnniversaryEntries _characterXunXuAnniversaryOperations;
    private readonly RuntimeEntries.CharacterZhugeGuoLeEntries _characterZhugeGuoLeOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries _coreCurrentUsePhysicalClaimsOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentUseTargetCandidateRefreshEntries _coreCurrentUseTargetCandidateRefreshOperations;
    private readonly RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries _coreExtraCardEffectRoundsOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries _coreHpLossSlashMaterialsOperations;
    private readonly RuntimeEntries.CoreGameEngineMantianGuohaiEntries _coreMantianGuohaiOperations;
    private readonly RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries _coreNextCardTargetAdjustmentsOperations;
    private readonly RuntimeEntries.CoreGameEngineNoDamageSkillDisableEntries _coreNoDamageSkillDisableOperations;
    private readonly RuntimeEntries.CoreGameEngineOriginalUseLightningEntries _coreOriginalUseLightningOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries _coreSoleIncomingRangeDrawOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly RuntimeEntries.CoreNativeFactionSlashEntries _coreNativeFactionSlashOperations;
    private readonly RuntimeEntries.CoreNativeFactionSlashCompletionEntries _coreNativeFactionSlashCompletionOperations;
    private readonly RuntimeEntries.CoreNativeSlashTargetDuelEntries _coreNativeSlashTargetDuelOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly ICoreNativeCardUseCompletionCoreProgramNativeReturnsOperations _coreProgramNativeReturnsOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly IKernelCompleteGameOperations _kernelCoreMainOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    internal CoreNativeCardUseCompletionRuntime(RuntimeEntries.CharacterDengAiAnniversaryGodEntries characterDengAiAnniversaryGodOperations, RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations, RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, RuntimeEntries.CharacterWuAnGuoOlEntries characterWuAnGuoOlOperations, RuntimeEntries.CharacterXinPiOlEntries characterXinPiOlOperations, RuntimeEntries.CharacterXunXuAnniversaryEntries characterXunXuAnniversaryOperations, RuntimeEntries.CharacterZhugeGuoLeEntries characterZhugeGuoLeOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries coreCurrentUsePhysicalClaimsOperations, RuntimeEntries.CoreGameEngineCurrentUseTargetCandidateRefreshEntries coreCurrentUseTargetCandidateRefreshOperations, RuntimeEntries.CoreGameEngineExtraCardEffectRoundsEntries coreExtraCardEffectRoundsOperations, RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries coreHpLossSlashMaterialsOperations, RuntimeEntries.CoreGameEngineMantianGuohaiEntries coreMantianGuohaiOperations, RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries coreNextCardTargetAdjustmentsOperations, RuntimeEntries.CoreGameEngineNoDamageSkillDisableEntries coreNoDamageSkillDisableOperations, RuntimeEntries.CoreGameEngineOriginalUseLightningEntries coreOriginalUseLightningOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations, RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries coreSoleIncomingRangeDrawOperations, RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, RuntimeEntries.CoreNativeFactionSlashEntries coreNativeFactionSlashOperations, RuntimeEntries.CoreNativeFactionSlashCompletionEntries coreNativeFactionSlashCompletionOperations, RuntimeEntries.CoreNativeSlashTargetDuelEntries coreNativeSlashTargetDuelOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, ICoreNativeCardUseCompletionCoreProgramNativeReturnsOperations coreProgramNativeReturnsOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, IKernelCompleteGameOperations kernelCoreMainOperations, EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess)
    {
        this._characterDengAiAnniversaryGodOperations = characterDengAiAnniversaryGodOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterHuaManOlOperations = characterHuaManOlOperations;
        this._characterMaChengOlOperations = characterMaChengOlOperations;
        this._characterWuAnGuoOlOperations = characterWuAnGuoOlOperations;
        this._characterXinPiOlOperations = characterXinPiOlOperations;
        this._characterXunXuAnniversaryOperations = characterXunXuAnniversaryOperations;
        this._characterZhugeGuoLeOperations = characterZhugeGuoLeOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreCurrentUsePhysicalClaimsOperations = coreCurrentUsePhysicalClaimsOperations;
        this._coreCurrentUseTargetCandidateRefreshOperations = coreCurrentUseTargetCandidateRefreshOperations;
        this._coreExtraCardEffectRoundsOperations = coreExtraCardEffectRoundsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreHpLossSlashMaterialsOperations = coreHpLossSlashMaterialsOperations;
        this._coreMantianGuohaiOperations = coreMantianGuohaiOperations;
        this._coreNextCardTargetAdjustmentsOperations = coreNextCardTargetAdjustmentsOperations;
        this._coreNoDamageSkillDisableOperations = coreNoDamageSkillDisableOperations;
        this._coreOriginalUseLightningOperations = coreOriginalUseLightningOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreSoleIncomingRangeDrawOperations = coreSoleIncomingRangeDrawOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeFactionSlashOperations = coreNativeFactionSlashOperations;
        this._coreNativeFactionSlashCompletionOperations = coreNativeFactionSlashCompletionOperations;
        this._coreNativeSlashTargetDuelOperations = coreNativeSlashTargetDuelOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreProgramNativeReturnsOperations = coreProgramNativeReturnsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
    }
}
