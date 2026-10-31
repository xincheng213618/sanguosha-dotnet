#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreDamageObserverCursorQueriesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageAdjacencyServicesEntries _coreDamageAdjacencyServicesOperations;
    internal CoreDamageObserverCursorQueriesRuntime(global::CardGame.Core.RuntimeEntries.CoreDamageAdjacencyServicesEntries coreDamageAdjacencyServicesOperations)
    {
        this._coreDamageAdjacencyServicesOperations = coreDamageAdjacencyServicesOperations;
    }
}
