#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterZhouFangOlRuntime : global::CardGame.Core.IZhouFangProgramHost
{
    private readonly ICharacterZhouFangOlCharacterBianFuRenOlOperations _characterBianFuRenOlOperations;
    private readonly ICharacterZhouFangOlCardZonesOperations _cardZonesOperations;
    private readonly ICharacterZhouFangOlContentRegistryOperations _contentRegistryOperations;
    private readonly ICharacterZhouFangOlRandomOperations _randomOperations;
    private readonly ICharacterZhouFangOlResolutionStackOperations _resolutionStackOperations;
    private readonly ICharacterZhouFangOlCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICharacterZhouFangOlCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICharacterZhouFangOlCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICharacterZhouFangOlCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICharacterZhouFangOlCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterZhouFangOlCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICharacterZhouFangOlCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICharacterZhouFangOlCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICharacterZhouFangOlCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICharacterZhouFangOlCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICharacterZhouFangOlCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICharacterZhouFangOlCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICharacterZhouFangOlKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess _coreCommandJournalStorageEventsPendingEventsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterZhouFangOlRuntime(ICharacterZhouFangOlCharacterBianFuRenOlOperations characterBianFuRenOlOperations, ICharacterZhouFangOlCardZonesOperations cardZonesOperations, ICharacterZhouFangOlContentRegistryOperations contentRegistryOperations, ICharacterZhouFangOlRandomOperations randomOperations, ICharacterZhouFangOlResolutionStackOperations resolutionStackOperations, ICharacterZhouFangOlCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICharacterZhouFangOlCoreNationalSkillsOperations coreNationalSkillsOperations, ICharacterZhouFangOlCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICharacterZhouFangOlCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICharacterZhouFangOlCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterZhouFangOlCoreRuntimeOperations coreRuntimeOperations, ICharacterZhouFangOlCoreMatchLogOperations coreMatchLogOperations, ICharacterZhouFangOlCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICharacterZhouFangOlCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICharacterZhouFangOlCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICharacterZhouFangOlCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICharacterZhouFangOlCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICharacterZhouFangOlKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess coreCommandJournalStorageEventsPendingEventsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterBianFuRenOlOperations = characterBianFuRenOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCommandJournalStorageEventsPendingEventsAccess = coreCommandJournalStorageEventsPendingEventsAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
