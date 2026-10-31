#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeDrawAttemptsRuntime
{
    private readonly ICoreNativeDrawAttemptsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreNativeDrawAttemptsNativeDrawAttemptOperations _nativeDrawAttemptOperations;
    internal CoreGameEngineNativeDrawAttemptsRuntime(ICoreNativeDrawAttemptsContentRegistryOperations contentRegistryOperations, ICoreNativeDrawAttemptsNativeDrawAttemptOperations nativeDrawAttemptOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._nativeDrawAttemptOperations = nativeDrawAttemptOperations;
    }
}
