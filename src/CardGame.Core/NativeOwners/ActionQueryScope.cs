#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ActionQueryScope(EngineStates.CoreGameEngineActionQueriesState gameEngineActionQueriesState, bool hasNativeOwner, ActionQuery? previous = null) : IDisposable
{
    public void Dispose()
    {
        if (hasNativeOwner)
            gameEngineActionQueriesState.ReferenceToActionQuery = previous;
    }
}
