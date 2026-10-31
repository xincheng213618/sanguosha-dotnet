#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineDeckProgramsRuntime : global::CardGame.Core.IDeckProgramHost
{
    private readonly ICoreDeckProgramsCharacterHuangQuanMouOperations _characterHuangQuanMouOperations;
    private readonly ICoreDeckProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreDeckProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreDeckProgramsRandomOperations _randomOperations;
    private readonly ICoreDeckProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDeckProgramsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreDeckProgramsCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreDeckProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreDeckProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreDeckProgramsCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreDeckProgramsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreDeckProgramsCoreNativeSlashUseOperations _coreNativeSlashUseOperations;
    private readonly ICoreDeckProgramsCoreNativeTargetLegalityOperations _coreNativeTargetLegalityOperations;
    private readonly ICoreDeckProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreDeckProgramsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreDeckProgramsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreDeckProgramsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreDeckProgramsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreDeckProgramsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreDeckProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesAccess _coreGameEngineSkillProgramsProgramContributionUsesAccess;
    internal CoreGameEngineDeckProgramsRuntime(ICoreDeckProgramsCharacterHuangQuanMouOperations characterHuangQuanMouOperations, ICoreDeckProgramsCardZonesOperations cardZonesOperations, ICoreDeckProgramsContentRegistryOperations contentRegistryOperations, ICoreDeckProgramsRandomOperations randomOperations, ICoreDeckProgramsResolutionStackOperations resolutionStackOperations, ICoreDeckProgramsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreDeckProgramsCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreDeckProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreDeckProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreDeckProgramsCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreDeckProgramsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreDeckProgramsCoreNativeSlashUseOperations coreNativeSlashUseOperations, ICoreDeckProgramsCoreNativeTargetLegalityOperations coreNativeTargetLegalityOperations, ICoreDeckProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreDeckProgramsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreDeckProgramsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreDeckProgramsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreDeckProgramsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreDeckProgramsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreDeckProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesAccess coreGameEngineSkillProgramsProgramContributionUsesAccess)
    {
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeSlashUseOperations = coreNativeSlashUseOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineSkillProgramsProgramContributionUsesAccess = coreGameEngineSkillProgramsProgramContributionUsesAccess;
    }
}
