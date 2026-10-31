namespace CardGame.Core;
public sealed partial class GameEngine
{
    // A legal-action read is pure. Share its derived answers only until that read
    // returns; no revision cache can survive a command or a skill/seat mutation.
    private ref global::CardGame.Core.NativeOwners.ActionQuery? _actionQuery => ref _actionQueries.ReferenceToActionQuery;
}
