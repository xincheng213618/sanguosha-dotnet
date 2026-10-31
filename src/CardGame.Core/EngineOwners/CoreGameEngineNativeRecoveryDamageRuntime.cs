#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeRecoveryDamageRuntime
{
    private readonly ICoreNativeRecoveryDamageNativeHealthRecoveryOperations _nativeHealthRecoveryOperations;
    internal CoreGameEngineNativeRecoveryDamageRuntime(ICoreNativeRecoveryDamageNativeHealthRecoveryOperations nativeHealthRecoveryOperations)
    {
        this._nativeHealthRecoveryOperations = nativeHealthRecoveryOperations;
    }
}
