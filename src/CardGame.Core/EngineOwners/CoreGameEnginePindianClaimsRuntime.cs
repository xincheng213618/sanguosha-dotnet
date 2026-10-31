#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePindianClaimsRuntime
{
    private readonly RuntimeEntries.CharacterJianYongBoundaryMobileEntries _characterJianYongBoundaryMobileOperations;
    private readonly RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly RuntimeEntries.CharacterPangJiOlEntries _characterPangJiOlOperations;
    private readonly RuntimeEntries.CharacterSunCeBoundaryMobileEntries _characterSunCeBoundaryMobileOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianEntries _corePindianOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianMaximumSlashClaimsEntries _corePindianMaximumSlashClaimsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEnginePindianClaimsRuntime(RuntimeEntries.CharacterJianYongBoundaryMobileEntries characterJianYongBoundaryMobileOperations, RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations, RuntimeEntries.CharacterPangJiOlEntries characterPangJiOlOperations, RuntimeEntries.CharacterSunCeBoundaryMobileEntries characterSunCeBoundaryMobileOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEnginePindianEntries corePindianOperations, RuntimeEntries.CoreGameEnginePindianMaximumSlashClaimsEntries corePindianMaximumSlashClaimsOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
