#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineActionCategoryGiftsRuntime
{
    private readonly ICoreActionCategoryGiftsCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICoreActionCategoryGiftsCardZonesOperations _cardZonesOperations;
    private readonly ICoreActionCategoryGiftsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreActionCategoryGiftsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreActionCategoryGiftsCoreBoundCardMovementContinuationOperations _coreBoundCardMovementContinuationOperations;
    private readonly ICoreActionCategoryGiftsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreActionCategoryGiftsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreActionCategoryGiftsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreActionCategoryGiftsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreActionCategoryGiftsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreActionCategoryGiftsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreActionCategoryGiftsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineActionCategoryGiftsRuntime(ICoreActionCategoryGiftsCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICoreActionCategoryGiftsCardZonesOperations cardZonesOperations, ICoreActionCategoryGiftsContentRegistryOperations contentRegistryOperations, ICoreActionCategoryGiftsResolutionStackOperations resolutionStackOperations, ICoreActionCategoryGiftsCoreBoundCardMovementContinuationOperations coreBoundCardMovementContinuationOperations, ICoreActionCategoryGiftsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreActionCategoryGiftsCoreRuntimeOperations coreRuntimeOperations, ICoreActionCategoryGiftsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreActionCategoryGiftsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreActionCategoryGiftsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreActionCategoryGiftsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreActionCategoryGiftsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
