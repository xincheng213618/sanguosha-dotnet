#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchParticipantSetupRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess _randomOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineConversionStatesEntries _coreConversionStatesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineInitialHandModifiersEntries _coreInitialHandModifiersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineModeRulesEntries _coreModeRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineModeSetupEntries _coreModeSetupOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineModeSetupEquipmentEntries _coreModeSetupEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOpeningCustomizationEntries _coreOpeningCustomizationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchPresentationEntries _coreMatchPresentationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries _coreMatchSetupDefinitionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries _coreNativeAiDecisionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCoreInvariantsEntries _coreNativeCoreInvariantsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly ICoreMatchParticipantSetupCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess _coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICorePhysicalCardPopulationInitialCardCountAccess _corePhysicalCardPopulationInitialCardCountAccess;
    internal CoreMatchParticipantSetupRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelMatchRandomAccess randomOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineConversionStatesEntries coreConversionStatesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineInitialHandModifiersEntries coreInitialHandModifiersOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineModeRulesEntries coreModeRulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineModeSetupEntries coreModeSetupOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineModeSetupEquipmentEntries coreModeSetupEquipmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOpeningCustomizationEntries coreOpeningCustomizationOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreMatchPresentationEntries coreMatchPresentationOperations, global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries coreMatchSetupDefinitionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries coreNativeAiDecisionsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCoreInvariantsEntries coreNativeCoreInvariantsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, ICoreMatchParticipantSetupCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess, global::CardGame.Core.EngineStates.Contracts.ICorePhysicalCardPopulationInitialCardCountAccess corePhysicalCardPopulationInitialCardCountAccess)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._randomOperations = randomOperations;
        this._coreConversionStatesOperations = coreConversionStatesOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
        this._coreInitialHandModifiersOperations = coreInitialHandModifiersOperations;
        this._coreModeRulesOperations = coreModeRulesOperations;
        this._coreModeSetupOperations = coreModeSetupOperations;
        this._coreModeSetupEquipmentOperations = coreModeSetupEquipmentOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreOpeningCustomizationOperations = coreOpeningCustomizationOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreMatchPresentationOperations = coreMatchPresentationOperations;
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._coreNativeCoreInvariantsOperations = coreNativeCoreInvariantsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
        this._coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess = coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess = coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess;
        this._corePhysicalCardPopulationInitialCardCountAccess = corePhysicalCardPopulationInitialCardCountAccess;
    }
}
