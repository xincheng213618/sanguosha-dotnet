#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSchoolTrickRuntime : IShanheSchoolTrickHost
{
    private readonly RuntimeEntries.CharacterDaQiaoLeEntries _characterDaQiaoLeOperations;
    private readonly RuntimeEntries.CharacterFeiYiOlEntries _characterFeiYiOlOperations;
    private readonly RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly RuntimeEntries.CharacterHuangYueyingOlMouEntries _characterHuangYueyingOlMouOperations;
    private readonly RuntimeEntries.CharacterJiangJiMouEntries _characterJiangJiMouOperations;
    private readonly RuntimeEntries.CharacterSunQuanOlMagicEntries _characterSunQuanOlMagicOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries _coreNamedAmbushInvalidationOperations;
    private readonly RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries _coreNextCardTargetAdjustmentsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _coreSequentialTrickTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheSchoolsEntries _coreShanheSchoolsOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries _coreSoleIncomingEffectRepeatOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSchoolTrickRuntime(RuntimeEntries.CharacterDaQiaoLeEntries characterDaQiaoLeOperations,RuntimeEntries.CharacterFeiYiOlEntries characterFeiYiOlOperations,RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations,RuntimeEntries.CharacterHuangYueyingOlMouEntries characterHuangYueyingOlMouOperations,RuntimeEntries.CharacterJiangJiMouEntries characterJiangJiMouOperations,RuntimeEntries.CharacterSunQuanOlMagicEntries characterSunQuanOlMagicOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations,RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries coreNamedAmbushInvalidationOperations,RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries coreNextCardTargetAdjustmentsOperations,RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries coreSequentialTrickTargetsOperations,RuntimeEntries.CoreGameEngineShanheSchoolsEntries coreShanheSchoolsOperations,RuntimeEntries.CoreGameEngineSoleIncomingEffectRepeatEntries coreSoleIncomingEffectRepeatOperations,RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations,RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoLeOperations = characterDaQiaoLeOperations;
        this._characterFeiYiOlOperations = characterFeiYiOlOperations;
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterHuangYueyingOlMouOperations = characterHuangYueyingOlMouOperations;
        this._characterJiangJiMouOperations = characterJiangJiMouOperations;
        this._characterSunQuanOlMagicOperations = characterSunQuanOlMagicOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreNamedAmbushInvalidationOperations = coreNamedAmbushInvalidationOperations;
        this._coreNextCardTargetAdjustmentsOperations = coreNextCardTargetAdjustmentsOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSequentialTrickTargetsOperations = coreSequentialTrickTargetsOperations;
        this._coreShanheSchoolsOperations = coreShanheSchoolsOperations;
        this._coreSoleIncomingEffectRepeatOperations = coreSoleIncomingEffectRepeatOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
