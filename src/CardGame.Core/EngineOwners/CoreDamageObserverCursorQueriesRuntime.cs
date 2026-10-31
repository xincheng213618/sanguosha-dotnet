#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreDamageObserverCursorQueriesRuntime
{
    private readonly RuntimeEntries.CoreDamageAdjacencyServicesEntries _coreDamageAdjacencyServicesOperations;
    internal CoreDamageObserverCursorQueriesRuntime(RuntimeEntries.CoreDamageAdjacencyServicesEntries coreDamageAdjacencyServicesOperations)
    {
        this._coreDamageAdjacencyServicesOperations = coreDamageAdjacencyServicesOperations;
    }
}
