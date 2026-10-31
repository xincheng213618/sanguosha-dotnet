#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramRecoveryReceiptsRuntime
{
    private readonly ICoreProgramRecoveryReceiptsCoreTypedRecoveryDamageCursorOperations _coreTypedRecoveryDamageCursorOperations;
    internal CoreGameEngineProgramRecoveryReceiptsRuntime(ICoreProgramRecoveryReceiptsCoreTypedRecoveryDamageCursorOperations coreTypedRecoveryDamageCursorOperations)
    {
        this._coreTypedRecoveryDamageCursorOperations = coreTypedRecoveryDamageCursorOperations;
    }
}
