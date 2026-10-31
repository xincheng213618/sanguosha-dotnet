#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePersistentPrivateWhirlRuntime
{
    private readonly ICorePersistentPrivateWhirlContentRegistryOperations _contentRegistryOperations;
    internal CoreGameEnginePersistentPrivateWhirlRuntime(ICorePersistentPrivateWhirlContentRegistryOperations contentRegistryOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
    }
}
