#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianMaximumSlashClaimsRuntime
{
    private readonly ICorePindianMaximumSlashClaimsCardZonesOperations _cardZonesOperations;
    private readonly ICorePindianMaximumSlashClaimsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePindianMaximumSlashClaimsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePindianMaximumSlashClaimsCorePindianOperations _corePindianOperations;
    private readonly ICorePindianMaximumSlashClaimsCorePindianRandomSelectionsOperations _corePindianRandomSelectionsOperations;
    private readonly ICorePindianMaximumSlashClaimsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICorePindianMaximumSlashClaimsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePindianMaximumSlashClaimsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICorePindianMaximumSlashClaimsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICorePindianMaximumSlashClaimsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianMaximumSlashClaimsRuntime(ICorePindianMaximumSlashClaimsCardZonesOperations cardZonesOperations, ICorePindianMaximumSlashClaimsContentRegistryOperations contentRegistryOperations, ICorePindianMaximumSlashClaimsResolutionStackOperations resolutionStackOperations, ICorePindianMaximumSlashClaimsCorePindianOperations corePindianOperations, ICorePindianMaximumSlashClaimsCorePindianRandomSelectionsOperations corePindianRandomSelectionsOperations, ICorePindianMaximumSlashClaimsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICorePindianMaximumSlashClaimsCoreRuntimeOperations coreRuntimeOperations, ICorePindianMaximumSlashClaimsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICorePindianMaximumSlashClaimsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICorePindianMaximumSlashClaimsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._corePindianOperations = corePindianOperations;
        this._corePindianRandomSelectionsOperations = corePindianRandomSelectionsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
