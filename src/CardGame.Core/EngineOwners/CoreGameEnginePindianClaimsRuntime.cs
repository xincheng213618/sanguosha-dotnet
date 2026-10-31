#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianClaimsRuntime
{
    private readonly ICorePindianClaimsCharacterJianYongBoundaryMobileOperations _characterJianYongBoundaryMobileOperations;
    private readonly ICorePindianClaimsCharacterLuKangWumiaoOperations _characterLuKangWumiaoOperations;
    private readonly ICorePindianClaimsCharacterPangJiOlOperations _characterPangJiOlOperations;
    private readonly ICorePindianClaimsCharacterSunCeBoundaryMobileOperations _characterSunCeBoundaryMobileOperations;
    private readonly ICorePindianClaimsCardZonesOperations _cardZonesOperations;
    private readonly ICorePindianClaimsContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePindianClaimsResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePindianClaimsCorePindianOperations _corePindianOperations;
    private readonly ICorePindianClaimsCorePindianMaximumSlashClaimsOperations _corePindianMaximumSlashClaimsOperations;
    private readonly ICorePindianClaimsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICorePindianClaimsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICorePindianClaimsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICorePindianClaimsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICorePindianClaimsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianClaimsRuntime(ICorePindianClaimsCharacterJianYongBoundaryMobileOperations characterJianYongBoundaryMobileOperations, ICorePindianClaimsCharacterLuKangWumiaoOperations characterLuKangWumiaoOperations, ICorePindianClaimsCharacterPangJiOlOperations characterPangJiOlOperations, ICorePindianClaimsCharacterSunCeBoundaryMobileOperations characterSunCeBoundaryMobileOperations, ICorePindianClaimsCardZonesOperations cardZonesOperations, ICorePindianClaimsContentRegistryOperations contentRegistryOperations, ICorePindianClaimsResolutionStackOperations resolutionStackOperations, ICorePindianClaimsCorePindianOperations corePindianOperations, ICorePindianClaimsCorePindianMaximumSlashClaimsOperations corePindianMaximumSlashClaimsOperations, ICorePindianClaimsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICorePindianClaimsCoreRuntimeOperations coreRuntimeOperations, ICorePindianClaimsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICorePindianClaimsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICorePindianClaimsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterJianYongBoundaryMobileOperations = characterJianYongBoundaryMobileOperations;
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._characterPangJiOlOperations = characterPangJiOlOperations;
        this._characterSunCeBoundaryMobileOperations = characterSunCeBoundaryMobileOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._corePindianOperations = corePindianOperations;
        this._corePindianMaximumSlashClaimsOperations = corePindianMaximumSlashClaimsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
