#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ActionQueryScope(global::CardGame.Core.EngineStates.CoreGameEngineActionQueriesState gameEngineActionQueriesState, bool hasNativeOwner, global::CardGame.Core.NativeOwners.ActionQuery? previous = null) : IDisposable
{
    public void Dispose()
    {
        if (hasNativeOwner)
            gameEngineActionQueriesState.ReferenceToActionQuery = previous;
    }
}
