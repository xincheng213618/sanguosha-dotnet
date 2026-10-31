#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePersistentPrivateWhirlRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    internal CoreGameEnginePersistentPrivateWhirlRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
    }
}
