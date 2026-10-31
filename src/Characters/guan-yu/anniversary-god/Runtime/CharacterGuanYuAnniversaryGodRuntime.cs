#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterGuanYuAnniversaryGodRuntime : global::CardGame.Core.IAnniversaryGuanYuNightmareHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries _coreAttributedMarkerEventsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    internal CharacterGuanYuAnniversaryGodRuntime(global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries coreAttributedMarkerEventsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations)
    {
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreAttributedMarkerEventsOperations = coreAttributedMarkerEventsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
    }
}
