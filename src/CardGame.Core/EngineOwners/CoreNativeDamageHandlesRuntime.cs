#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDamageHandlesRuntime
{
    private readonly ICoreNativeDamageHandlesCharacterZhouQunOlOperations _characterZhouQunOlOperations;
    private readonly ICoreNativeDamageHandlesCardZonesOperations _cardZonesOperations;
    private readonly ICoreNativeDamageHandlesModeRulesOperations _modeRulesOperations;
    private readonly ICoreNativeDamageHandlesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativeDamageHandlesCoreActiveNativeAttackQueriesOperations _coreActiveNativeAttackQueriesOperations;
    private readonly ICoreNativeDamageHandlesCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICoreNativeDamageHandlesCoreProgramAttackAttemptOperations _coreProgramAttackAttemptOperations;
    internal CoreNativeDamageHandlesRuntime(ICoreNativeDamageHandlesCharacterZhouQunOlOperations characterZhouQunOlOperations, ICoreNativeDamageHandlesCardZonesOperations cardZonesOperations, ICoreNativeDamageHandlesModeRulesOperations modeRulesOperations, ICoreNativeDamageHandlesResolutionStackOperations resolutionStackOperations, ICoreNativeDamageHandlesCoreActiveNativeAttackQueriesOperations coreActiveNativeAttackQueriesOperations, ICoreNativeDamageHandlesCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICoreNativeDamageHandlesCoreProgramAttackAttemptOperations coreProgramAttackAttemptOperations)
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
