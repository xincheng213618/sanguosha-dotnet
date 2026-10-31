#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CharacterGuanYuAnniversaryGodRuntime : global::CardGame.Core.IAnniversaryGuanYuNightmareHost
{
    private readonly ICharacterGuanYuAnniversaryGodCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICharacterGuanYuAnniversaryGodCoreAttributedMarkerEventsOperations _coreAttributedMarkerEventsOperations;
    private readonly ICharacterGuanYuAnniversaryGodCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICharacterGuanYuAnniversaryGodCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICharacterGuanYuAnniversaryGodCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    internal CharacterGuanYuAnniversaryGodRuntime(ICharacterGuanYuAnniversaryGodCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICharacterGuanYuAnniversaryGodCoreAttributedMarkerEventsOperations coreAttributedMarkerEventsOperations, ICharacterGuanYuAnniversaryGodCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICharacterGuanYuAnniversaryGodCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICharacterGuanYuAnniversaryGodCoreTableSeatQueriesOperations coreTableSeatQueriesOperations)
    {
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreAttributedMarkerEventsOperations = coreAttributedMarkerEventsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
    }
}
