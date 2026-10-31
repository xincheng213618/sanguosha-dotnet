#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineProgramDiscardTopPlacementRuntime : global::CardGame.Core.IProgramDiscardTopPlacementHost
{
    private readonly ICoreProgramDiscardTopPlacementCardZonesOperations _cardZonesOperations;
    private readonly ICoreProgramDiscardTopPlacementContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramDiscardTopPlacementResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramDiscardTopPlacementCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICoreProgramDiscardTopPlacementCoreNeighborDiscardOpportunitiesOperations _coreNeighborDiscardOpportunitiesOperations;
    private readonly ICoreProgramDiscardTopPlacementCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramDiscardTopPlacementCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreProgramDiscardTopPlacementCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreProgramDiscardTopPlacementCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreProgramDiscardTopPlacementCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramDiscardTopPlacementRuntime(ICoreProgramDiscardTopPlacementCardZonesOperations cardZonesOperations, ICoreProgramDiscardTopPlacementContentRegistryOperations contentRegistryOperations, ICoreProgramDiscardTopPlacementResolutionStackOperations resolutionStackOperations, ICoreProgramDiscardTopPlacementCoreNationalSkillsOperations coreNationalSkillsOperations, ICoreProgramDiscardTopPlacementCoreNeighborDiscardOpportunitiesOperations coreNeighborDiscardOpportunitiesOperations, ICoreProgramDiscardTopPlacementCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramDiscardTopPlacementCoreRuntimeOperations coreRuntimeOperations, ICoreProgramDiscardTopPlacementCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreProgramDiscardTopPlacementCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreProgramDiscardTopPlacementCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreNeighborDiscardOpportunitiesOperations = coreNeighborDiscardOpportunitiesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
