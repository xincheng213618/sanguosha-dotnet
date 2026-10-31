#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineOverflowTargetCancellationRuntime : global::CardGame.Core.IOverflowTargetCancellationHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries _characterCuiLieAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries _characterXunYuMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries _characterYuanTanYuanShangOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries _coreCompletedCategoryUseProofOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries _coreCurrentUsePhysicalClaimsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries _coreDesignatedExtraTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries _coreOverflowTargetCancellationObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries _coreProgramAtomicCardMovesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseRolesEntries _coreProgramCardUseRolesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries _coreResponseEntityExchangeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedTrickTargetsEntries _coreRoundGainedTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries _coreSkillOwnershipWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries _coreHappyWeiYanGainDestinationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeRuleQueriesEntries _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineOverflowTargetCancellationRuntime(global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries characterCuiLieAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries characterXunYuMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries characterYuanTanYuanShangOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedCategoryUseProofEntries coreCompletedCategoryUseProofOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries coreCurrentUsePhysicalClaimsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries coreDesignatedExtraTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries coreOverflowTargetCancellationObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries coreProgramAtomicCardMovesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseRolesEntries coreProgramCardUseRolesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries coreResponseEntityExchangeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedTrickTargetsEntries coreRoundGainedTrickTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries coreSkillOwnershipWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries coreHappyWeiYanGainDestinationsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeRuleQueriesEntries coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterBeiMiHuOlOperations = characterBeiMiHuOlOperations;
        this._characterCuiLieAnniversaryOperations = characterCuiLieAnniversaryOperations;
        this._characterHuaManOlOperations = characterHuaManOlOperations;
        this._characterXunYuMobileGodOperations = characterXunYuMobileGodOperations;
        this._characterYuanTanYuanShangOlOperations = characterYuanTanYuanShangOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreCompletedCategoryUseProofOperations = coreCompletedCategoryUseProofOperations;
        this._coreCurrentUsePhysicalClaimsOperations = coreCurrentUsePhysicalClaimsOperations;
        this._coreDesignatedExtraTargetsOperations = coreDesignatedExtraTargetsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreOverflowTargetCancellationObserversOperations = coreOverflowTargetCancellationObserversOperations;
        this._coreProgramAtomicCardMovesOperations = coreProgramAtomicCardMovesOperations;
        this._coreProgramCardUseRolesOperations = coreProgramCardUseRolesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreResponseEntityExchangeOperations = coreResponseEntityExchangeOperations;
        this._coreRoundGainedTrickTargetsOperations = coreRoundGainedTrickTargetsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillOwnershipWindowsOperations = coreSkillOwnershipWindowsOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
