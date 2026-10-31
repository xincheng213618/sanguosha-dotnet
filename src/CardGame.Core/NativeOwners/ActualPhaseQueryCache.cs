#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualPhaseQueryCache(global::CardGame.Core.EngineStates.CoreGameEngineActionQueriesState gameEngineActionQueriesState) : IActualPhaseQueryCache
{
    public IActualDiscardPhaseQueryCacheState? Current => gameEngineActionQueriesState.ReferenceToActionQuery;
}
