#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterRuiJiOlRuntime : global::CardGame.Core.IRuiJiProgramHost
{
    private readonly ICharacterRuiJiOlCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterRuiJiOlCharacterXiaoQiaoMouOlOperations _characterXiaoQiaoMouOlOperations;
    private readonly ICharacterRuiJiOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterRuiJiOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterRuiJiOlRandomOperations _randomOperations;
    private readonly ICharacterRuiJiOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterRuiJiOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterRuiJiOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterRuiJiOlCoreCardUseLifecycleOperations _coreCardUseLifecycleOperations;
    private readonly ICharacterRuiJiOlCoreEquipmentContentOperations _coreEquipmentContentOperations;
    private readonly ICharacterRuiJiOlCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICharacterRuiJiOlCoreProgramCardTargetEffectsOperations _coreProgramCardTargetEffectsOperations;
    private readonly ICharacterRuiJiOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterRuiJiOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterRuiJiOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterRuiJiOlCoreNativeCardUseOperations _coreNativeCardUseOperations;
    private readonly ICharacterRuiJiOlCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICharacterRuiJiOlCoreNativeNullificationOperations _coreNativeNullificationOperations;
    private readonly ICharacterRuiJiOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterRuiJiOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterRuiJiOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterRuiJiOlCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterRuiJiOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterRuiJiOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterRuiJiOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    internal CharacterRuiJiOlRuntime(ICharacterRuiJiOlCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterRuiJiOlCharacterXiaoQiaoMouOlOperations characterXiaoQiaoMouOlOperations, ICharacterRuiJiOlCardZonesOperations cardZonesOperations, ICharacterRuiJiOlContentRegistryOperations contentRegistryOperations, ICharacterRuiJiOlRandomOperations randomOperations, ICharacterRuiJiOlResolutionStackOperations resolutionStackOperations, ICharacterRuiJiOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterRuiJiOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterRuiJiOlCoreCardUseLifecycleOperations coreCardUseLifecycleOperations, ICharacterRuiJiOlCoreEquipmentContentOperations coreEquipmentContentOperations, ICharacterRuiJiOlCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICharacterRuiJiOlCoreProgramCardTargetEffectsOperations coreProgramCardTargetEffectsOperations, ICharacterRuiJiOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterRuiJiOlCoreRuntimeOperations coreRuntimeOperations, ICharacterRuiJiOlCoreMatchLogOperations coreMatchLogOperations, ICharacterRuiJiOlCoreNativeCardUseOperations coreNativeCardUseOperations, ICharacterRuiJiOlCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICharacterRuiJiOlCoreNativeNullificationOperations coreNativeNullificationOperations, ICharacterRuiJiOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterRuiJiOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterRuiJiOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterRuiJiOlCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterRuiJiOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterRuiJiOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterRuiJiOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreEquipmentContentOperations = coreEquipmentContentOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardUseOperations = coreNativeCardUseOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    }
}
