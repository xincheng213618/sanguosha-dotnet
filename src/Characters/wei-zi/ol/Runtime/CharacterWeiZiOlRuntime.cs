#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterWeiZiOlRuntime : global::CardGame.Core.IWeiZiProgramHost
{
    private readonly ICharacterWeiZiOlCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICharacterWeiZiOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterWeiZiOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterWeiZiOlCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICharacterWeiZiOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterWeiZiOlCoreLiejieSourceDiscardsOperations _coreLiejieSourceDiscardsOperations;
    private readonly ICharacterWeiZiOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterWeiZiOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterWeiZiOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterWeiZiOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterWeiZiOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterWeiZiOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterWeiZiOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterWeiZiOlCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterWeiZiOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterWeiZiOlCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICharacterWeiZiOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterWeiZiOlRuntime(ICharacterWeiZiOlCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICharacterWeiZiOlContentRegistryOperations contentRegistryOperations, ICharacterWeiZiOlResolutionStackOperations resolutionStackOperations, ICharacterWeiZiOlCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICharacterWeiZiOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterWeiZiOlCoreLiejieSourceDiscardsOperations coreLiejieSourceDiscardsOperations, ICharacterWeiZiOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterWeiZiOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterWeiZiOlCoreRuntimeOperations coreRuntimeOperations, ICharacterWeiZiOlCoreMatchLogOperations coreMatchLogOperations, ICharacterWeiZiOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterWeiZiOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterWeiZiOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterWeiZiOlCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterWeiZiOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterWeiZiOlCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICharacterWeiZiOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreLiejieSourceDiscardsOperations = coreLiejieSourceDiscardsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
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
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
