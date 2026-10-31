#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreDamageObserverCursorQueriesRuntime
{
    private readonly ICoreDamageObserverCursorQueriesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreDamageObserverCursorQueriesCoreDamageAdjacencyServicesOperations _coreDamageAdjacencyServicesOperations;
    private readonly ICoreDamageObserverCursorQueriesCoreDamageCursorRuntimeOperations _coreDamageCursorRuntimeOperations;
    private readonly ICoreDamageObserverCursorQueriesCoreDyingCursorRuntimeOperations _coreDyingCursorRuntimeOperations;
    internal CoreDamageObserverCursorQueriesRuntime(ICoreDamageObserverCursorQueriesResolutionStackOperations resolutionStackOperations, ICoreDamageObserverCursorQueriesCoreDamageAdjacencyServicesOperations coreDamageAdjacencyServicesOperations, ICoreDamageObserverCursorQueriesCoreDamageCursorRuntimeOperations coreDamageCursorRuntimeOperations, ICoreDamageObserverCursorQueriesCoreDyingCursorRuntimeOperations coreDyingCursorRuntimeOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageAdjacencyServicesOperations = coreDamageAdjacencyServicesOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
    }
}
