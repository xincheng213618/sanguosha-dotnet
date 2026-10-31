#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreResponseCompletionRuntimeServicesRuntime : global::CardGame.Core.IResponseCompletionFacts, global::CardGame.Core.IResponseCompletionNativeProtocols
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries _characterCaoZhiLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFanJiangZhangDaOlEntries _characterFanJiangZhangDaOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries _characterLiFengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaMiDiOlEntries _characterMaMiDiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries _characterMiHengLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouCangBoundaryMobileEntries _characterZhouCangBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries _characterZhugeLiangWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries _characterZuoCiHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelPendingCardsMovedBatchesAccess _pendingCardsMovedBatchesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredTurnEndPreludesEntries _coreDeferredTurnEndPreludesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDelayedNullificationEntries _coreDelayedNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduChariotResponsesEntries _coreGuanduChariotResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePairedColorResponsesEntries _corePairedColorResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries _coreResponseEntityExchangeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseUseCompletionEntries _coreResponseUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries _coreShanheStartingResourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries _coreTieredRoundZeroResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries _coreWushuangJinzhiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries _coreWushuangShopCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries _coreNativeDodgeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateCardAttackHandleAndCreateDuelHandleAndCreateGroupCardHandleOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess _coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    internal CoreResponseCompletionRuntimeServicesRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoZhiLeEntries characterCaoZhiLeOperations, global::CardGame.Core.RuntimeEntries.CharacterFanJiangZhangDaOlEntries characterFanJiangZhangDaOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiFengOlEntries characterLiFengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaMiDiOlEntries characterMaMiDiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMiHengLeEntries characterMiHengLeOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouCangBoundaryMobileEntries characterZhouCangBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries characterZhugeLiangWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries characterZuoCiHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelPendingCardsMovedBatchesAccess pendingCardsMovedBatchesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredTurnEndPreludesEntries coreDeferredTurnEndPreludesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDelayedNullificationEntries coreDelayedNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduChariotResponsesEntries coreGuanduChariotResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePairedColorResponsesEntries corePairedColorResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries coreResponseEntityExchangeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseUseCompletionEntries coreResponseUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries coreShanheStartingResourcesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries coreTieredRoundZeroResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangJinzhiEntries coreWushuangJinzhiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries coreWushuangShopCardsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries coreNativeDodgeOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelCreateCardAttackHandleAndCreateDuelHandleAndCreateGroupCardHandleOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess coreGameEngineHpChangeProgramsPendingHpChangesAccess)
    {
        this._characterCaoZhiLeOperations = characterCaoZhiLeOperations;
        this._characterFanJiangZhangDaOlOperations = characterFanJiangZhangDaOlOperations;
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._characterLiFengOlOperations = characterLiFengOlOperations;
        this._characterMaChengOlOperations = characterMaChengOlOperations;
        this._characterMaMiDiOlOperations = characterMaMiDiOlOperations;
        this._characterMiHengLeOperations = characterMiHengLeOperations;
        this._characterZhouCangBoundaryMobileOperations = characterZhouCangBoundaryMobileOperations;
        this._characterZhugeLiangWumiaoOperations = characterZhugeLiangWumiaoOperations;
        this._characterZuoCiHappyGodOperations = characterZuoCiHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._pendingCardsMovedBatchesOperations = pendingCardsMovedBatchesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreDeferredTurnEndPreludesOperations = coreDeferredTurnEndPreludesOperations;
        this._coreDelayedNullificationOperations = coreDelayedNullificationOperations;
        this._coreGuanduChariotResponsesOperations = coreGuanduChariotResponsesOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._corePairedColorResponsesOperations = corePairedColorResponsesOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreResponseEntityExchangeOperations = coreResponseEntityExchangeOperations;
        this._coreResponseUseCompletionOperations = coreResponseUseCompletionOperations;
        this._coreShanheStartingResourcesOperations = coreShanheStartingResourcesOperations;
        this._coreTieredRoundZeroResponsesOperations = coreTieredRoundZeroResponsesOperations;
        this._coreWushuangJinzhiOperations = coreWushuangJinzhiOperations;
        this._coreWushuangShopCardsOperations = coreWushuangShopCardsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDodgeOperations = coreNativeDodgeOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineHpChangeProgramsPendingHpChangesAccess = coreGameEngineHpChangeProgramsPendingHpChangesAccess;
    }
}
