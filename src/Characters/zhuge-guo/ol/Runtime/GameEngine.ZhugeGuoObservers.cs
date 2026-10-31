namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref bool _hasIssuedZhugeGuoState => ref _characterZhugeGuoOl.ReferenceToHasIssuedZhugeGuoState;
}
