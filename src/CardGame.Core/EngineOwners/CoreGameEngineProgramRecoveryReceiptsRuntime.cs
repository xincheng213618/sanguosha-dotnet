#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramRecoveryReceiptsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries _coreTypedRecoveryDamageCursorOperations;
    internal CoreGameEngineProgramRecoveryReceiptsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries coreTypedRecoveryDamageCursorOperations)
    {
        this._coreTypedRecoveryDamageCursorOperations = coreTypedRecoveryDamageCursorOperations;
    }
}
