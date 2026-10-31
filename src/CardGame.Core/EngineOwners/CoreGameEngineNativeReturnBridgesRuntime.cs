#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeReturnBridgesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineQinglongInsertionEntries _coreQinglongInsertionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDyingEntries _coreNativeDyingOperations;
    internal CoreGameEngineNativeReturnBridgesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineQinglongInsertionEntries coreQinglongInsertionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDyingEntries coreNativeDyingOperations)
    {
        this._coreQinglongInsertionOperations = coreQinglongInsertionOperations;
        this._coreNativeDyingOperations = coreNativeDyingOperations;
    }
}
