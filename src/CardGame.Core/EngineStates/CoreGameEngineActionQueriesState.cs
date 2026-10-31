#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineActionQueriesState : Contracts.ICoreGameEngineActionQueriesActionQueryAccess
{
    // A legal-action read is pure. Share its derived answers only until that read
    // returns; no revision cache can survive a command or a skill/seat mutation.
    private NativeOwners.ActionQuery? _actionQuery = default !;
    internal ref NativeOwners.ActionQuery? ReferenceToActionQuery => ref _actionQuery;

    ref NativeOwners.ActionQuery? Contracts.ICoreGameEngineActionQueriesActionQueryAccess._actionQuery => ref _actionQuery;
}
