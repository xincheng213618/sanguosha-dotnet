#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineOwnedExactCountAndNamedTargetsRuntime
{
    private readonly ICoreOwnedExactCountAndNamedTargetsCardZonesOperations _cardZonesOperations;
    private readonly ICoreOwnedExactCountAndNamedTargetsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreOwnedExactCountAndNamedTargetsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreOwnedExactCountAndNamedTargetsCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreOwnedExactCountAndNamedTargetsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreOwnedExactCountAndNamedTargetsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreOwnedExactCountAndNamedTargetsCoreTurnHandCategoryRestrictionsOperations _coreTurnHandCategoryRestrictionsOperations;
    private readonly ICoreOwnedExactCountAndNamedTargetsCoreNativePromptBoundaryOperations _coreNativePromptBoundaryOperations;
    private readonly ICoreOwnedExactCountAndNamedTargetsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineOwnedExactCountAndNamedTargetsRuntime(ICoreOwnedExactCountAndNamedTargetsCardZonesOperations cardZonesOperations, ICoreOwnedExactCountAndNamedTargetsContentRegistryOperations contentRegistryOperations, ICoreOwnedExactCountAndNamedTargetsResolutionStackOperations resolutionStackOperations, ICoreOwnedExactCountAndNamedTargetsCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreOwnedExactCountAndNamedTargetsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreOwnedExactCountAndNamedTargetsCoreRuntimeOperations coreRuntimeOperations, ICoreOwnedExactCountAndNamedTargetsCoreTurnHandCategoryRestrictionsOperations coreTurnHandCategoryRestrictionsOperations, ICoreOwnedExactCountAndNamedTargetsCoreNativePromptBoundaryOperations coreNativePromptBoundaryOperations, ICoreOwnedExactCountAndNamedTargetsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTurnHandCategoryRestrictionsOperations = coreTurnHandCategoryRestrictionsOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
