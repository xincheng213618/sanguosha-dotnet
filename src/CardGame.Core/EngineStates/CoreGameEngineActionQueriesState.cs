#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineActionQueriesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess
{
    // A legal-action read is pure. Share its derived answers only until that read
    // returns; no revision cache can survive a command or a skill/seat mutation.
    private global::CardGame.Core.NativeOwners.ActionQuery? _actionQuery = default !;
    internal ref global::CardGame.Core.NativeOwners.ActionQuery? ReferenceToActionQuery => ref _actionQuery;

    ref global::CardGame.Core.NativeOwners.ActionQuery? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess._actionQuery => ref _actionQuery;
}
