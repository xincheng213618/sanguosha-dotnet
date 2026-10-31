#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeReturnBridgesRuntime
{
    private readonly ICoreNativeReturnBridgesCoreQinglongInsertionOperations _coreQinglongInsertionOperations;
    private readonly ICoreNativeReturnBridgesCoreNativeDyingOperations _coreNativeDyingOperations;
    internal CoreGameEngineNativeReturnBridgesRuntime(ICoreNativeReturnBridgesCoreQinglongInsertionOperations coreQinglongInsertionOperations, ICoreNativeReturnBridgesCoreNativeDyingOperations coreNativeDyingOperations)
    {
        this._coreQinglongInsertionOperations = coreQinglongInsertionOperations;
        this._coreNativeDyingOperations = coreNativeDyingOperations;
    }
}
