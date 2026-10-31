#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCardUseModulesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiHappyGodEntries _characterSimaYiHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries _characterSimaYiMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunQuanHappyGodEntries _characterSunQuanHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries _characterYuanTanYuanShangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangLingOlEntries _characterZhangLingOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess _turnCardUseEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _coreAlcoholIdentityPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSuppressionsEntries _coreAlcoholSlashSuppressionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries _coreBoundDiscardSlashBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries _coreCurrentTurnSkillSuppressionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries _coreDyingWindowSkillPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries _coreFixedDistanceOneDebtOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries _coreHandComparisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries _coreIssuedPlayUsePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries _coreNextActualUseTargetAdjustmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNextSlashProgramsEntries _coreNextSlashProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries _coreOriginalTargetAdditionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineParticipantAndReserveProgramsEntries _coreParticipantAndReserveProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseStartControlPoliciesEntries _corePhaseStartControlPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramBooleanStateExpirationEntries _coreProgramBooleanStateExpirationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries _coreProgramPhaseColorRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries _coreResponseEntityExchangeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries _coreTurnHandCategoryRestrictionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreCardUseModulesCoreProgramCardLegalityOperations _coreProgramCardLegalityOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess _coreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess;
    internal CoreGameEngineCardUseModulesRuntime(global::CardGame.Core.RuntimeEntries.CharacterSimaYiHappyGodEntries characterSimaYiHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries characterSimaYiMouOperations, global::CardGame.Core.RuntimeEntries.CharacterSunQuanHappyGodEntries characterSunQuanHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries characterYuanTanYuanShangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangLingOlEntries characterZhangLingOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.EngineOwners.IKernelTurnCardUseEffectsAccess turnCardUseEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries coreAlcoholIdentityPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSuppressionsEntries coreAlcoholSlashSuppressionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries coreBoundDiscardSlashBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnSkillSuppressionsEntries coreCurrentTurnSkillSuppressionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingWindowSkillPoliciesEntries coreDyingWindowSkillPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries coreFixedDistanceOneDebtOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries coreHandComparisonOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries coreIssuedPlayUsePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries coreNextActualUseTargetAdjustmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNextSlashProgramsEntries coreNextSlashProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries coreOriginalTargetAdditionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineParticipantAndReserveProgramsEntries coreParticipantAndReserveProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseStartControlPoliciesEntries corePhaseStartControlPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramBooleanStateExpirationEntries coreProgramBooleanStateExpirationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries coreProgramPhaseColorRestrictionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries coreResponseEntityExchangeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries coreTurnHandCategoryRestrictionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, ICoreCardUseModulesCoreProgramCardLegalityOperations coreProgramCardLegalityOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess coreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess)
    {
        this._characterSimaYiHappyGodOperations = characterSimaYiHappyGodOperations;
        this._characterSimaYiMouOperations = characterSimaYiMouOperations;
        this._characterSunQuanHappyGodOperations = characterSunQuanHappyGodOperations;
        this._characterYuanTanYuanShangOlOperations = characterYuanTanYuanShangOlOperations;
        this._characterZhangLingOlOperations = characterZhangLingOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._turnCardUseEffectsOperations = turnCardUseEffectsOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreAlcoholIdentityPoliciesOperations = coreAlcoholIdentityPoliciesOperations;
        this._coreAlcoholSlashSuppressionsOperations = coreAlcoholSlashSuppressionsOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreBoundDiscardSlashBenefitsOperations = coreBoundDiscardSlashBenefitsOperations;
        this._coreCurrentTurnSkillSuppressionsOperations = coreCurrentTurnSkillSuppressionsOperations;
        this._coreDyingWindowSkillPoliciesOperations = coreDyingWindowSkillPoliciesOperations;
        this._coreFixedDistanceOneDebtOperations = coreFixedDistanceOneDebtOperations;
        this._coreHandComparisonOperations = coreHandComparisonOperations;
        this._coreIssuedPlayUsePoliciesOperations = coreIssuedPlayUsePoliciesOperations;
        this._coreNextActualUseTargetAdjustmentsOperations = coreNextActualUseTargetAdjustmentsOperations;
        this._coreNextSlashProgramsOperations = coreNextSlashProgramsOperations;
        this._coreOriginalTargetAdditionsOperations = coreOriginalTargetAdditionsOperations;
        this._coreParticipantAndReserveProgramsOperations = coreParticipantAndReserveProgramsOperations;
        this._corePhaseStartControlPoliciesOperations = corePhaseStartControlPoliciesOperations;
        this._coreProgramBooleanStateExpirationOperations = coreProgramBooleanStateExpirationOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPhaseColorRestrictionsOperations = coreProgramPhaseColorRestrictionsOperations;
        this._coreResponseEntityExchangeOperations = coreResponseEntityExchangeOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramCardLegalityOperations = coreProgramCardLegalityOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess = coreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess;
    }
}
