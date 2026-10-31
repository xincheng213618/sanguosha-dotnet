#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeMaterialSlashRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries _characterHuangfuSongWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries _characterSunQuanOlMagicOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSuppressionsEntries _coreAlcoholSlashSuppressionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries _coreAlternatingSuitAndCompletedTopOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseDebitsEntries _coreCardUseDebitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries _coreCurrentSlashFireOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries _coreFinalTargetSlashOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries _coreJudgedRankSlashPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetPenaltyPaymentsEntries _coreSlashTargetPenaltyPaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries _coreTurnRedSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries _coreNativeAiDecisionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries _coreNativeCardUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries _coreNativeOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries _coreNativeSlashUseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries _coreNativeTargetLegalityOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly ICoreNativeMaterialSlashCoreProgramTargetRulesOperations _coreProgramTargetRulesOperations;
    private readonly ICoreNativeMaterialSlashCoreTrustedMovementDiagnosticsOperations _coreTrustedMovementDiagnosticsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateFangtianHalberdHandleOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeMaterialSlashRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries characterHuangfuSongWumiaoOperations,global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries characterSunQuanOlMagicOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSuppressionsEntries coreAlcoholSlashSuppressionsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries coreAlternatingSuitAndCompletedTopOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseDebitsEntries coreCardUseDebitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries coreCurrentSlashFireOffersOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetSlashEntries coreFinalTargetSlashOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries coreJudgedRankSlashPoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetPenaltyPaymentsEntries coreSlashTargetPenaltyPaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries coreTurnRedSlashBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries coreNativeAiDecisionsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries coreNativeCardUseOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeOwnedCardQueriesEntries coreNativeOwnedCardQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreNativeSlashUseEntries coreNativeSlashUseOperations,global::CardGame.Core.RuntimeEntries.CoreNativeTargetLegalityEntries coreNativeTargetLegalityOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations,ICoreNativeMaterialSlashCoreProgramTargetRulesOperations coreProgramTargetRulesOperations,ICoreNativeMaterialSlashCoreTrustedMovementDiagnosticsOperations coreTrustedMovementDiagnosticsOperations,global::CardGame.Core.EngineOwners.IKernelCreateFangtianHalberdHandleOperations kernelCoreMainOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangfuSongWumiaoOperations = characterHuangfuSongWumiaoOperations;
        this._characterSunQuanOlMagicOperations = characterSunQuanOlMagicOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreAlcoholSlashSuppressionsOperations = coreAlcoholSlashSuppressionsOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreAlternatingSuitAndCompletedTopOperations = coreAlternatingSuitAndCompletedTopOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardUseDebitsOperations = coreCardUseDebitsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCurrentSlashFireOffersOperations = coreCurrentSlashFireOffersOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreFinalTargetSlashOperations = coreFinalTargetSlashOperations;
        this._coreJudgedRankSlashPoliciesOperations = coreJudgedRankSlashPoliciesOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreSlashTargetPenaltyPaymentsOperations = coreSlashTargetPenaltyPaymentsOperations;
        this._coreTurnRedSlashBenefitsOperations = coreTurnRedSlashBenefitsOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreProgramTargetRulesOperations = coreProgramTargetRulesOperations;
        this._coreTrustedMovementDiagnosticsOperations = coreTrustedMovementDiagnosticsOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
