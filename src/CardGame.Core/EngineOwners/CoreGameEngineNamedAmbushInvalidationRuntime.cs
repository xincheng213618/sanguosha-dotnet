#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNamedAmbushInvalidationRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengYuMouEntries _characterChengYuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries _characterHuangZhongAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiJueOlEntries _characterLiJueOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries _coreChainedStateBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries _coreCompletedCategoryUseProofOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicResponsesEntries _coreDrawFundedDistinctBasicResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushEntries _coreNamedAmbushOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries _coreRoundOnceCardlessNativeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries _coreTieredRoundZeroResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries _coreNativeNullificationFramesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineNamedAmbushInvalidationRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterChengYuMouEntries characterChengYuMouOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries characterHuangZhongAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterLiJueOlEntries characterLiJueOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries coreChainedStateBasicReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries coreCompletedCategoryUseProofOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicResponsesEntries coreDrawFundedDistinctBasicResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushEntries coreNamedAmbushOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries coreRoundOnceCardlessNativeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries coreTieredRoundZeroResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries coreNativeNullificationFramesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
