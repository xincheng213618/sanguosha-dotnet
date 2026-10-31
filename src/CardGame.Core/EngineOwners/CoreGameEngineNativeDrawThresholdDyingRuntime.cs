#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeDrawThresholdDyingRuntime
{
    private readonly ICoreNativeDrawThresholdDyingNativeDrawThresholdOperations _nativeDrawThresholdOperations;
    internal CoreGameEngineNativeDrawThresholdDyingRuntime(ICoreNativeDrawThresholdDyingNativeDrawThresholdOperations nativeDrawThresholdOperations)
    {
        this._nativeDrawThresholdOperations = nativeDrawThresholdOperations;
    }
}
