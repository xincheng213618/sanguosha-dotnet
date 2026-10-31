#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGhostActivityProgramsRuntime : global::CardGame.Core.IGhostActivityProgramHost
{
    private readonly ICoreGhostActivityProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreGhostActivityProgramsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreGhostActivityProgramsRandomOperations _randomOperations;
    private readonly ICoreGhostActivityProgramsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreGhostActivityProgramsCoreNativeLogicalDiscardsOperations _coreNativeLogicalDiscardsOperations;
    private readonly ICoreGhostActivityProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreGhostActivityProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreGhostActivityProgramsCoreTurnHandCategoryRestrictionsOperations _coreTurnHandCategoryRestrictionsOperations;
    private readonly ICoreGhostActivityProgramsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreGhostActivityProgramsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreGhostActivityProgramsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreGhostActivityProgramsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreGhostActivityProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGhostActivityProgramsRuntime(ICoreGhostActivityProgramsCardZonesOperations cardZonesOperations, ICoreGhostActivityProgramsContentRegistryOperations contentRegistryOperations, ICoreGhostActivityProgramsRandomOperations randomOperations, ICoreGhostActivityProgramsResolutionStackOperations resolutionStackOperations, ICoreGhostActivityProgramsCoreNativeLogicalDiscardsOperations coreNativeLogicalDiscardsOperations, ICoreGhostActivityProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreGhostActivityProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreGhostActivityProgramsCoreTurnHandCategoryRestrictionsOperations coreTurnHandCategoryRestrictionsOperations, ICoreGhostActivityProgramsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreGhostActivityProgramsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreGhostActivityProgramsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreGhostActivityProgramsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreGhostActivityProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._randomOperations = randomOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNativeLogicalDiscardsOperations = coreNativeLogicalDiscardsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
