#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreParticipantHandDamageRidesRuntime
{
    private readonly RuntimeEntries.CharacterZhaoYunOlGod2016Entries _characterZhaoYunOlGod2016Operations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreDamageObserverCursorQueriesEntries _coreDamageObserverCursorQueriesOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries _coreRecoveryReplacementDamageRidesOperations;
    private readonly RuntimeEntries.CoreGameEngineRequestedDeckBasicOwnershipEntries _coreRequestedDeckBasicOwnershipOperations;
    private readonly RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries _coreParticipantHandProgramRescueRidesOperations;
    private readonly RuntimeEntries.CoreParticipantHandProgramsEntries _coreParticipantHandProgramsOperations;
    internal CoreParticipantHandDamageRidesRuntime(RuntimeEntries.CharacterZhaoYunOlGod2016Entries characterZhaoYunOlGod2016Operations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, RuntimeEntries.CoreDamageObserverCursorQueriesEntries coreDamageObserverCursorQueriesOperations, RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries coreRecoveryReplacementDamageRidesOperations, RuntimeEntries.CoreGameEngineRequestedDeckBasicOwnershipEntries coreRequestedDeckBasicOwnershipOperations, RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries coreParticipantHandProgramRescueRidesOperations, RuntimeEntries.CoreParticipantHandProgramsEntries coreParticipantHandProgramsOperations)
    {
        this._characterZhaoYunOlGod2016Operations = characterZhaoYunOlGod2016Operations;
        this._cardZonesOperations = cardZonesOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreRecoveryReplacementDamageRidesOperations = coreRecoveryReplacementDamageRidesOperations;
        this._coreRequestedDeckBasicOwnershipOperations = coreRequestedDeckBasicOwnershipOperations;
        this._coreParticipantHandProgramRescueRidesOperations = coreParticipantHandProgramRescueRidesOperations;
        this._coreParticipantHandProgramsOperations = coreParticipantHandProgramsOperations;
    }
}
