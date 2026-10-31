#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDamageHandlesRuntime
{
    private readonly RuntimeEntries.CharacterZhouQunOlEntries _characterZhouQunOlOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    internal CoreNativeDamageHandlesRuntime(RuntimeEntries.CharacterZhouQunOlEntries characterZhouQunOlOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelMatchModeRulesAccess modeRulesOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations)
    {
        this._characterZhouQunOlOperations = characterZhouQunOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
    }
}
