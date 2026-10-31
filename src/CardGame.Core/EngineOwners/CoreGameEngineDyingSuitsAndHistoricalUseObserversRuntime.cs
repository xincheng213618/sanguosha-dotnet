#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries _characterCaoPiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries _characterHuaTuoAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiMobileGodEntries _characterSimaYiMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries _characterZhangLiaoMobileGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDelayedPlacementTargetProgramsEntries _coreDelayedPlacementTargetProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingOwnedCardSelectionsEntries _coreDyingOwnedCardSelectionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries _coreObserverSubtreesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries _coreOrderedPrintedSkillLossObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries _coreOverflowTargetCancellationObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries _coreProgramDiscardOriginsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundPricedPileAlcoholEntries _coreRoundPricedPileAlcoholOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries _coreSoleTargetQuarantineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries _coreUnrespondableCounterspellsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries _coreWenhePoisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineZhenyiJudgmentWindowsEntries _coreZhenyiJudgmentWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries _coreNativeJudgmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries _coreParticipantHandProgramRescueRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoPiAnniversaryGodEntries characterCaoPiAnniversaryGodOperations,global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations,global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries characterHuaTuoAnniversaryGodOperations,global::CardGame.Core.RuntimeEntries.CharacterSimaYiMobileGodEntries characterSimaYiMobileGodOperations,global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoMobileGodEntries characterZhangLiaoMobileGodOperations,global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDelayedPlacementTargetProgramsEntries coreDelayedPlacementTargetProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingOwnedCardSelectionsEntries coreDyingOwnedCardSelectionsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries coreObserverSubtreesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossObserversEntries coreOrderedPrintedSkillLossObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries coreOverflowTargetCancellationObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries coreProgramDiscardOriginsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundPricedPileAlcoholEntries coreRoundPricedPileAlcoholOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries coreSoleTargetQuarantineOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries coreUnrespondableCounterspellsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries coreWenhePoisonOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineZhenyiJudgmentWindowsEntries coreZhenyiJudgmentWindowsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries coreNativeJudgmentOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries coreParticipantHandProgramRescueRidesOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess,global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoPiAnniversaryGodOperations = characterCaoPiAnniversaryGodOperations;
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterHuaTuoAnniversaryGodOperations = characterHuaTuoAnniversaryGodOperations;
        this._characterSimaYiMobileGodOperations = characterSimaYiMobileGodOperations;
        this._characterZhangLiaoMobileGodOperations = characterZhangLiaoMobileGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreDelayedPlacementTargetProgramsOperations = coreDelayedPlacementTargetProgramsOperations;
        this._coreDyingOwnedCardSelectionsOperations = coreDyingOwnedCardSelectionsOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreObserverSubtreesOperations = coreObserverSubtreesOperations;
        this._coreOrderedPrintedSkillLossObserversOperations = coreOrderedPrintedSkillLossObserversOperations;
        this._coreOverflowTargetCancellationObserversOperations = coreOverflowTargetCancellationObserversOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreProgramDiscardOriginsOperations = coreProgramDiscardOriginsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreRoundPricedPileAlcoholOperations = coreRoundPricedPileAlcoholOperations;
        this._coreSoleTargetQuarantineOperations = coreSoleTargetQuarantineOperations;
        this._coreUnrespondableCounterspellsOperations = coreUnrespondableCounterspellsOperations;
        this._coreWenhePoisonOperations = coreWenhePoisonOperations;
        this._coreZhenyiJudgmentWindowsOperations = coreZhenyiJudgmentWindowsOperations;
        this._coreNativeJudgmentOperations = coreNativeJudgmentOperations;
        this._coreParticipantHandDamageRidesOperations = coreParticipantHandDamageRidesOperations;
        this._coreParticipantHandProgramRescueRidesOperations = coreParticipantHandProgramRescueRidesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
