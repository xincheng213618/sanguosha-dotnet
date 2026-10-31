#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActualPhaseQueryCache(EngineStates.CoreGameEngineActionQueriesState gameEngineActionQueriesState) : IActualPhaseQueryCache
{
    public IActualDiscardPhaseQueryCacheState? Current => gameEngineActionQueriesState.ReferenceToActionQuery;
}
