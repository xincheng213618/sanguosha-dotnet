#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterYuanShaoBoundaryMobileRuntime : IMobileYuanShaoProgramHost, IMobileYuanShaoSelectionQueries
{
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries _coreAdvancedLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineBoundCardMovementContinuationEntries _coreBoundCardMovementContinuationOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramNativeDrawEntries _coreProgramNativeDrawOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CharacterYuanShaoBoundaryMobileRuntime(IKernelCardZoneStoreAccess cardZonesOperations,IKernelFrameStoreAccess resolutionStackOperations,IKernelSkillRuntimeStateAccess skillRuntimeStateOperations,RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries coreAdvancedLifecycleOperations,RuntimeEntries.CoreGameEngineBoundCardMovementContinuationEntries coreBoundCardMovementContinuationOperations,RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineProgramNativeDrawEntries coreProgramNativeDrawOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreBoundCardMovementContinuationOperations = coreBoundCardMovementContinuationOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
