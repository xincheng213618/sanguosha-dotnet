#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterYanJunOlRuntime : global::CardGame.Core.IYanJunProgramHost
{
    private readonly ICharacterYanJunOlCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterYanJunOlCharacterZhaoYanOlOperations _characterZhaoYanOlOperations;
    private readonly ICharacterYanJunOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterYanJunOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterYanJunOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterYanJunOlCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICharacterYanJunOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterYanJunOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterYanJunOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterYanJunOlCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICharacterYanJunOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterYanJunOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterYanJunOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterYanJunOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterYanJunOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterYanJunOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterYanJunOlCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterYanJunOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterYanJunOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterYanJunOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterYanJunOlRuntime(ICharacterYanJunOlCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterYanJunOlCharacterZhaoYanOlOperations characterZhaoYanOlOperations, ICharacterYanJunOlCardZonesOperations cardZonesOperations, ICharacterYanJunOlContentRegistryOperations contentRegistryOperations, ICharacterYanJunOlResolutionStackOperations resolutionStackOperations, ICharacterYanJunOlCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICharacterYanJunOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterYanJunOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterYanJunOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterYanJunOlCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICharacterYanJunOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterYanJunOlCoreRuntimeOperations coreRuntimeOperations, ICharacterYanJunOlCoreMatchLogOperations coreMatchLogOperations, ICharacterYanJunOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterYanJunOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterYanJunOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterYanJunOlCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterYanJunOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterYanJunOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterYanJunOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterZhaoYanOlOperations = characterZhaoYanOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
