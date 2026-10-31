namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref long _cardActionSequence => ref _cardActions.ReferenceToCardActionSequence;
}
