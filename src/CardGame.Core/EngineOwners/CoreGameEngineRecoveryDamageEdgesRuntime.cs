#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRecoveryDamageEdgesRuntime
{
    private readonly ICoreRecoveryDamageEdgesCoreNativeRecoveryDamageOperations _coreNativeRecoveryDamageOperations;
    internal CoreGameEngineRecoveryDamageEdgesRuntime(ICoreRecoveryDamageEdgesCoreNativeRecoveryDamageOperations coreNativeRecoveryDamageOperations)
    {
        this._coreNativeRecoveryDamageOperations = coreNativeRecoveryDamageOperations;
    }
}
