#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreDamageAdjacencyServicesRuntime : global::CardGame.Core.IDamageAdjacencyFacts, global::CardGame.Core.IDamageAdjacencyProtocols, global::CardGame.Core.IDamageWindowOperations
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoSongOlEntries _characterCaoSongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries _characterHuangYueyingOlMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSkillReplacementsEntries _coreAlcoholSlashSkillReplacementsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries _coreCardUsePaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries _coreChainedStateBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterTurnedOverProgramsEntries _coreCharacterTurnedOverProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetSkillRoundDrawEntries _coreDamageTargetSkillRoundDrawOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicObserversEntries _coreDrawFundedDistinctBasicObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFactionRequestCostRecoveriesEntries _coreFactionRequestCostRecoveriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries _coreHellSoulCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineMantianGuohaiEntries _coreMantianGuohaiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProvenanceClaimObserversEntries _coreProvenanceClaimObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileEquipmentSequencesEntries _corePublicPileEquipmentSequencesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRandomDamageTargetSkillsEntries _coreRandomDamageTargetSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicOwnershipEntries _coreRequestedDeckBasicOwnershipOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShenwuCardsEntries _coreShenwuCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries _coreTypedRecoveryDamageCursorOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheChaosEntries _coreWenheChaosOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries _coreWenhePoisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenyiDamageHealthEntries _coreWenyiDamageHealthOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHulaoActivityProgramsEntries _coreHulaoActivityProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageFramesEntries _coreNativeDamageFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly ICoreDamageAdjacencyServicesCoreProgramRuntimeHooksOperations _coreProgramRuntimeHooksOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateDamageTriggerCapturedAttackOperations _kernelCoreDamageAdjacencyServicesOperations;
    private readonly ICoreDamageAdjacencyServicesDamageAdjacencyOperations _damageAdjacencyOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    internal CoreDamageAdjacencyServicesRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoSongOlEntries characterCaoSongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries characterHuangYueyingOlMouOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlcoholSlashSkillReplacementsEntries coreAlcoholSlashSkillReplacementsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUsePaymentsEntries coreCardUsePaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicReturnsEntries coreChainedStateBasicReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterTurnedOverProgramsEntries coreCharacterTurnedOverProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetSkillRoundDrawEntries coreDamageTargetSkillRoundDrawOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries coreDeckEquipmentProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicObserversEntries coreDrawFundedDistinctBasicObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFactionRequestCostRecoveriesEntries coreFactionRequestCostRecoveriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries coreHellSoulCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineMantianGuohaiEntries coreMantianGuohaiOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProvenanceClaimObserversEntries coreProvenanceClaimObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileEquipmentSequencesEntries corePublicPileEquipmentSequencesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRandomDamageTargetSkillsEntries coreRandomDamageTargetSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicOwnershipEntries coreRequestedDeckBasicOwnershipOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShenwuCardsEntries coreShenwuCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries coreTypedRecoveryDamageCursorOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenheChaosEntries coreWenheChaosOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries coreWenhePoisonOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenyiDamageHealthEntries coreWenyiDamageHealthOperations, global::CardGame.Core.RuntimeEntries.CoreHulaoActivityProgramsEntries coreHulaoActivityProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageFramesEntries coreNativeDamageFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, ICoreDamageAdjacencyServicesCoreProgramRuntimeHooksOperations coreProgramRuntimeHooksOperations, global::CardGame.Core.EngineOwners.IKernelCreateDamageTriggerCapturedAttackOperations kernelCoreDamageAdjacencyServicesOperations, ICoreDamageAdjacencyServicesDamageAdjacencyOperations damageAdjacencyOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess)
    {
        this._characterCaoSongOlOperations = characterCaoSongOlOperations;
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterHuangYueyingOlMouOperations = characterHuangYueyingOlMouOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlcoholSlashSkillReplacementsOperations = coreAlcoholSlashSkillReplacementsOperations;
        this._coreCardUsePaymentsOperations = coreCardUsePaymentsOperations;
        this._coreChainedStateBasicReturnsOperations = coreChainedStateBasicReturnsOperations;
        this._coreCharacterTurnedOverProgramsOperations = coreCharacterTurnedOverProgramsOperations;
        this._coreDamageTargetSkillRoundDrawOperations = coreDamageTargetSkillRoundDrawOperations;
        this._coreDeckEquipmentProgramsOperations = coreDeckEquipmentProgramsOperations;
        this._coreDrawFundedDistinctBasicObserversOperations = coreDrawFundedDistinctBasicObserversOperations;
        this._coreFactionRequestCostRecoveriesOperations = coreFactionRequestCostRecoveriesOperations;
        this._coreHellSoulCardsOperations = coreHellSoulCardsOperations;
        this._coreMantianGuohaiOperations = coreMantianGuohaiOperations;
        this._coreProvenanceClaimObserversOperations = coreProvenanceClaimObserversOperations;
        this._corePublicPileEquipmentSequencesOperations = corePublicPileEquipmentSequencesOperations;
        this._coreRandomDamageTargetSkillsOperations = coreRandomDamageTargetSkillsOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreRequestedDeckBasicOwnershipOperations = coreRequestedDeckBasicOwnershipOperations;
        this._coreShenwuCardsOperations = coreShenwuCardsOperations;
        this._coreTypedRecoveryDamageCursorOperations = coreTypedRecoveryDamageCursorOperations;
        this._coreWenheChaosOperations = coreWenheChaosOperations;
        this._coreWenhePoisonOperations = coreWenhePoisonOperations;
        this._coreWenyiDamageHealthOperations = coreWenyiDamageHealthOperations;
        this._coreHulaoActivityProgramsOperations = coreHulaoActivityProgramsOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeDamageFramesOperations = coreNativeDamageFramesOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreProgramRuntimeHooksOperations = coreProgramRuntimeHooksOperations;
        this._kernelCoreDamageAdjacencyServicesOperations = kernelCoreDamageAdjacencyServicesOperations;
        this._damageAdjacencyOperations = damageAdjacencyOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
    }
}
