#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNamedAmbushInvalidationRuntime
{
    private readonly RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly RuntimeEntries.CharacterChengYuMouEntries _characterChengYuMouOperations;
    private readonly RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries _characterHuangZhongAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterLiJueOlEntries _characterLiJueOlOperations;
    private readonly RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries _coreChainedStateBasicReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries _coreCompletedCategoryUseProofOperations;
    private readonly RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicResponsesEntries _coreDrawFundedDistinctBasicResponsesOperations;
    private readonly RuntimeEntries.CoreGameEngineNamedAmbushEntries _coreNamedAmbushOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries _coreRoundOnceCardlessNativeResponsesOperations;
    private readonly RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries _coreTieredRoundZeroResponsesOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly RuntimeEntries.CoreNativeNullificationFramesEntries _coreNativeNullificationFramesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineNamedAmbushInvalidationRuntime(RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations, RuntimeEntries.CharacterChengYuMouEntries characterChengYuMouOperations, RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries characterHuangZhongAnniversaryGodOperations, RuntimeEntries.CharacterLiJueOlEntries characterLiJueOlOperations, RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries coreChainedStateBasicReturnsOperations, RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries coreCompletedCategoryUseProofOperations, RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicResponsesEntries coreDrawFundedDistinctBasicResponsesOperations, RuntimeEntries.CoreGameEngineNamedAmbushEntries coreNamedAmbushOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries coreRoundOnceCardlessNativeResponsesOperations, RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries coreTieredRoundZeroResponsesOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, RuntimeEntries.CoreNativeNullificationFramesEntries coreNativeNullificationFramesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoXiOlOperations = characterCaoXiOlOperations;
        this._characterChengYuMouOperations = characterChengYuMouOperations;
        this._characterHuangZhongAnniversaryGodOperations = characterHuangZhongAnniversaryGodOperations;
        this._characterLiJueOlOperations = characterLiJueOlOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreChainedStateBasicReturnsOperations = coreChainedStateBasicReturnsOperations;
        this._coreCompletedCategoryUseProofOperations = coreCompletedCategoryUseProofOperations;
        this._coreDrawFundedDistinctBasicResponsesOperations = coreDrawFundedDistinctBasicResponsesOperations;
        this._coreNamedAmbushOperations = coreNamedAmbushOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRoundOnceCardlessNativeResponsesOperations = coreRoundOnceCardlessNativeResponsesOperations;
        this._coreTieredRoundZeroResponsesOperations = coreTieredRoundZeroResponsesOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativeNullificationFramesOperations = coreNativeNullificationFramesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
