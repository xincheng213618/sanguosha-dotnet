#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineResponseUseCompletionRuntime
{
    private readonly RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly RuntimeEntries.CharacterCuiYanOlEntries _characterCuiYanOlOperations;
    private readonly RuntimeEntries.CharacterHuangGaiMouMobileEntries _characterHuangGaiMouMobileOperations;
    private readonly RuntimeEntries.CharacterLiFengOlEntries _characterLiFengOlOperations;
    private readonly RuntimeEntries.CharacterLiuChengMouMobileEntries _characterLiuChengMouMobileOperations;
    private readonly RuntimeEntries.CharacterLuSuMobileShiEntries _characterLuSuMobileShiOperations;
    private readonly RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterMaMiDiOlEntries _characterMaMiDiOlOperations;
    private readonly RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly RuntimeEntries.CharacterWuAnGuoOlEntries _characterWuAnGuoOlOperations;
    private readonly RuntimeEntries.CharacterZouShiLeEntries _characterZouShiLeOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries _coreCurrentTurnUseKindsOperations;
    private readonly RuntimeEntries.CoreGameEngineDelayedNullificationEntries _coreDelayedNullificationOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineResponseCompletionEntries _coreResponseCompletionOperations;
    private readonly RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries _coreTieredRoundZeroResponsesOperations;
    private readonly RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries _coreWholeActualTurnUsesOperations;
    private readonly RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeDodgeEntries _coreNativeDodgeOperations;
    private readonly RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineResponseUseCompletionRuntime(RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations,RuntimeEntries.CharacterCuiYanOlEntries characterCuiYanOlOperations,RuntimeEntries.CharacterHuangGaiMouMobileEntries characterHuangGaiMouMobileOperations,RuntimeEntries.CharacterLiFengOlEntries characterLiFengOlOperations,RuntimeEntries.CharacterLiuChengMouMobileEntries characterLiuChengMouMobileOperations,RuntimeEntries.CharacterLuSuMobileShiEntries characterLuSuMobileShiOperations,RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations,RuntimeEntries.CharacterMaMiDiOlEntries characterMaMiDiOlOperations,RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations,RuntimeEntries.CharacterWuAnGuoOlEntries characterWuAnGuoOlOperations,RuntimeEntries.CharacterZouShiLeEntries characterZouShiLeOperations,IKernelContentRegistryAccess contentRegistryOperations,IKernelProgramPipelineHooksAccess programPipelineHooksOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations,RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries coreCurrentTurnUseKindsOperations,RuntimeEntries.CoreGameEngineDelayedNullificationEntries coreDelayedNullificationOperations,RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,RuntimeEntries.CoreGameEngineResponseCompletionEntries coreResponseCompletionOperations,RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries coreTieredRoundZeroResponsesOperations,RuntimeEntries.CoreGameEngineWholeActualTurnUsesEntries coreWholeActualTurnUsesOperations,RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations,RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations,RuntimeEntries.CoreNativeDodgeEntries coreNativeDodgeOperations,RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations,EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoXiOlOperations = characterCaoXiOlOperations;
        this._characterCuiYanOlOperations = characterCuiYanOlOperations;
        this._characterHuangGaiMouMobileOperations = characterHuangGaiMouMobileOperations;
        this._characterLiFengOlOperations = characterLiFengOlOperations;
        this._characterLiuChengMouMobileOperations = characterLiuChengMouMobileOperations;
        this._characterLuSuMobileShiOperations = characterLuSuMobileShiOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._characterMaMiDiOlOperations = characterMaMiDiOlOperations;
        this._characterMiHengLeOperations = characterMiHengLeOperations;
        this._characterWuAnGuoOlOperations = characterWuAnGuoOlOperations;
        this._characterZouShiLeOperations = characterZouShiLeOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCurrentTurnUseKindsOperations = coreCurrentTurnUseKindsOperations;
        this._coreDelayedNullificationOperations = coreDelayedNullificationOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreResponseCompletionOperations = coreResponseCompletionOperations;
        this._coreTieredRoundZeroResponsesOperations = coreTieredRoundZeroResponsesOperations;
        this._coreWholeActualTurnUsesOperations = coreWholeActualTurnUsesOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDodgeOperations = coreNativeDodgeOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
